﻿using BlackJackHusofication.Business.Helpers;
using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.Business.Mappings;
using BlackJackHusofication.Business.SignalR;
using BlackJackHusofication.Model.Models;
using BlackJackHusofication.Model.Models.Notifications;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlackJackHusofication.Business.BackgroundServices;

public class BjRunnerService : BackgroundService
{
    private readonly IHubContext<BlackJackGameHub, IBlackJackGameClient> _hubContext;
    private readonly ILogger<BjRunnerService> _logger;
    private readonly BjGame _game;
    private readonly BjRoomManager _bjRoomManager;
    private readonly IBlackJackGameClient _allPlayers;

    public BjRunnerService(IServiceProvider services, int roomId)
    {
        _hubContext = services.GetRequiredService<IHubContext<BlackJackGameHub, IBlackJackGameClient>>();
        _logger = services.GetRequiredService<ILogger<BjRunnerService>>();
        _bjRoomManager = services.GetRequiredService<BjRoomManager>();
        _game = _bjRoomManager.CreateRoom($"BJ-{roomId}", roomId);
        _allPlayers = _hubContext.Clients.Group(_game.Name);
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(5_000, stoppingToken); //We wait for 10 seconds in the beginning of the game

        _logger.LogInformation("oyun başladı dostum. Let's gooo");

        try
        {
            //Game Loop
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Hadi lan kekolar bet atın");

                _game.CancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = _game.CancellationTokenSource.Token;

                //We give to the players 10 seconds to bet. 
                var startNewRound = await CheckIfPlayersBetInTime(BjEventType.AcceptingBets, 10, cancellationToken);
                _game.IsAcceptingBets = false;
                if (!startNewRound) continue;

                _game.CancellationTokenSource = new CancellationTokenSource();
                cancellationToken = _game.CancellationTokenSource.Token;
                _logger.LogError("Bet atan bir keko var. Gel de paranı yiyim senin enayi!");

                //We should notify here that a new round is starting
                _game.RoundNo++;
                await _allPlayers.RoundStarted(_game.RoundNo);

                //We should deal cards for all players who has betted and for dealer.
                await DealCardsToPlayersWhoBetted();

                //We should ask for actions. Each action has 30 seconds timeout. If no action is taken, stand is played.
                await AskAllPlayersForActions();

                //Play for dealer
                await PlayForDealer();

                //After all players are asked for actions, we calculate the earnings.
                await CalculateAndDeliverEarnings();

                //Round is over, we should collect the cards, reset the values.
                ResetAfterRoundEnd();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    private async Task CalculateAndDeliverEarnings()
    {
        List<RoundWinningsNotification> results = [];

        //We calculate each player's earnings.
        foreach (var spot in _game.Table.Spots.Where(s => s.BetAmount > 0))
        {
            if (spot.Player is null) continue;

            var earning = BalanceManager.CheckHandsAndDeliverPrizes(_game.Table, spot.Id);

            //Oyuncunun daha önce sonucu döndüyse, sonucu ona eklicez. Aynı oyuncuya birden fazla bildirim atmak istemiyoruz.
            var ntf = results.FirstOrDefault(r => r.PlayerId == spot.Player.Id);
            if (ntf is not null)
            {
                ntf.Earning += earning;
                continue;
            }

            ntf = new() { PlayerId = spot.Player.Id, Earning = earning, Balance = spot.Player.Balance };
            results.Add(ntf);
        }

        //We notify each player about earnings.
        foreach (var result in results)
        {
            await _hubContext.Clients.Client(result.PlayerId).NotifyRoundWinnigs(result);
            await Task.Delay(3000); //render earnings
        }
    }

    private void ResetAfterRoundEnd()
    {
        CollectAllCards();

        if (_game.Table.IsShoeShouldChange)
        {
            List<Card> collectedShoe = [.. _game.Table.PlayedCards, .. _game.Table.Deck, _game.Table.ShufflerCard!];
            _game.Table.Deck = DeckHelper.ShuffleDecks(collectedShoe);
            _game.Table.PlayedCards = [];
            _game.Table.ShufflerCard = null;
            _game.Table.IsShoeShouldChange = false;
        }

        foreach (var spot in _game.Table.Spots.Where(p => p.BetAmount != 0))
        {
            spot.BetAmount = 0;
            spot.Hand = new();
            spot.SplittedHand = null;
        }
        _game.Table.Dealer.Hand = new();

        _allPlayers.UpdateTable( MappingHelper.MapTableObject(_game.Table) );
    }

    private void CollectAllCards()
    {
        _game.Table.PlayedCards.AddRange(_game.Table.Dealer.Hand.Cards);
        _game.Table.Dealer.Hand = new();

        foreach (var spot in _game.Table.Spots.Where(s => s.BetAmount > 0))
        {
            _game.Table.PlayedCards.AddRange(spot.Hand.Cards);
            spot.Hand = new();

            if (spot.SplittedHand is not null)
            {
                _game.Table.PlayedCards.AddRange(spot.SplittedHand.Cards);
                spot.SplittedHand = null;
            }

            spot.BetAmount = 0;
        };
    }

    private async Task<bool> CheckIfPlayersBetInTime(BjEventType eventType, int seconds, CancellationToken cancellationToken)
    {
        _game.IsAcceptingBets = true;
        try
        {
            for (int i = 0; i <= seconds; i++) //each second
            {
                var delayTask = Task.Delay(1_000, cancellationToken);
                var notificationTask = _hubContext.Clients.Group(_game.Name).NotifyCountDown(new CountDownNotification(eventType, seconds - i));
                await Task.WhenAll(delayTask, notificationTask);
                _logger.LogInformation("1 sn geçti. Tüm masalarda bet yok hala. Hadi kekolar bet atın!");
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogInformation(ex.Message);
            _game.CancellationTokenSource = new CancellationTokenSource();

            return true;
        }

        _logger.LogInformation("Süre doldu. Bakalım kimse bet attı mı?");

        //If any player sitting in the table has betted, then start the round
        if (_game.Table.Spots.Any(x => x.BetAmount != 0)) return true;
        return false;
    }

    private async Task DealCardsToPlayersWhoBetted()
    {
        for (int i = 0; i < 2; i++)
        {
            foreach (var spot in _game.Table.Spots.Where(s => s.BetAmount > 0))
            {
                await DealCard(spot.Hand, spot.Id);
            }

            await DealCard(_game.Table.Dealer.Hand, 0); //Zero for dealer.
        }
        //Send Last Game Values To All players.
    }

    private async Task DealCard(Hand hand, int spotNo)
    {
        var dealingAnimation = Task.Delay(1_000);
        var card = _game.Table.Deck[0];
        _game.Table.Deck.Remove(card);

        if (card.CardType == CardType.ShufflerCard)
        {
            _game.Table.IsShoeShouldChange = true; //TODO-HUS buna gerek olmayabilir.
            _game.Table.ShufflerCard = card with { }; //We put aside the shuffler card.
            card = _game.Table.Deck[0]; //We deal a new card
            _game.Table.Deck.Remove(card);
        }

        if (spotNo == 0 && _game.Table.Dealer.Hand.Cards.Count == 1) //For the second card deal of dealer we notify secret card
        {
            _game.Table.Dealer.SecretCard = card with { };
            card = Card.CreateSecretCard();   //we deal secret card for now
        }

        hand.Cards.Add(card);
        CardManager.GetCountOfHandAndUpdateHand(hand);

        await _allPlayers.NotifyCardDeal(new CardDealNotification //We should notify the new card
        {
            NewCard = card,
            SpotNo = spotNo,
            HandValue = hand.HandValue
        });

        await dealingAnimation;
    }

    private async Task AskAllPlayersForActions()
    {
        const int ACTION_TIME = 20;
        var cancellationToken = _game.CancellationTokenSource.Token;

        foreach (var spot in _game.Table.Spots.Where(x => x.BetAmount > 0))
        {
            if (spot.Hand.IsBlackJack) continue; 

            var shouldAskForNormalHand = true;
            while (shouldAskForNormalHand)
            {
                try
                {
                    for (int i = ACTION_TIME; i > 0; i--)
                    {
                        var delayTask = Task.Delay(1_000, cancellationToken);
                        
                        var notificationTask = _allPlayers
                            .NotifyAwaitingCardAction(new AwaitingCardActionNotification { SpotNo = spot.Id, Seconds = i });
                        await Task.WhenAll(delayTask, notificationTask);
                        if (spot.Hand.NextCardAction is not null) break;
                        _logger.LogInformation("1 sn geçti. Henüz bir aksiyon gelmedi! Aksiyon al amık");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation(ex.Message);
                    _game.CancellationTokenSource = new CancellationTokenSource();
                    cancellationToken = _game.CancellationTokenSource.Token;
                }
                //If no action is taken in the time, then stand is played
                spot.Hand.NextCardAction ??= CardAction.Stand;

                shouldAskForNormalHand = await ApplyPlayerAction(spot);
                spot.Hand.NextCardAction = null;
            }

            var shouldAskForSplitHand = spot.SplittedHand is not null;
            while (shouldAskForSplitHand)
            {
                try
                {
                    for (int i = 0; i < ACTION_TIME; i++)
                    {
                        var delayTask = Task.Delay(1_000, cancellationToken);
                        var notificationTask = _hubContext.Clients.Group(_game.Name)
                            .NotifyAwaitingCardAction(new AwaitingCardActionNotification { SpotNo = spot.Id, Seconds = i, IsForSplitHand = true });
                        await Task.WhenAll(delayTask, notificationTask);
                        if (spot.SplittedHand?.NextCardAction is not null) break;
                        _logger.LogInformation("1 sn geçti. Henüz bir aksiyon gelmedi! Aksiyon al amık");
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation(ex.Message);
                    _game.CancellationTokenSource = new CancellationTokenSource();
                    cancellationToken = _game.CancellationTokenSource.Token;
                }

                //If no action is taken in the time, then stand is played
                spot.SplittedHand!.NextCardAction ??= CardAction.Stand;

                shouldAskForSplitHand = await ApplyPlayerAction(spot, true);
                spot.SplittedHand!.NextCardAction = null;
            }

            if (spot.Hand.HandValue > 21) spot.Hand.IsBusted = true;
        }
    }

    private async Task<bool> ApplyPlayerAction(Spot spot, bool isForSplittedHand = false)
    {
        var hand = (isForSplittedHand ? spot.SplittedHand : spot.Hand) ?? throw new Exception("ha ha ha nasıl oldu la bu.");
        return hand.NextCardAction switch
        {
            CardAction.Stand => ApplyStand(),
            CardAction.Hit => await ApplyHit(spot, hand),
            CardAction.Double => await ApplyDouble(spot, hand),
            CardAction.Split => await ApplySplit(spot),
            _ => throw new Exception("La nasıl olur bu!!!")
        };
    }

    private bool ApplyStand()
    {
        return false;
    }

    private async Task<bool> ApplySplit(Spot spot)
    {
        spot.SplittedHand = new Hand();
        var splitCard = spot.Hand.Cards[1];
        spot.Hand.Cards.Remove(splitCard);
        spot.SplittedHand.Cards.Add(splitCard);

        CardManager.GetCountOfHandAndUpdateHand(spot.SplittedHand);
        CardManager.GetCountOfHandAndUpdateHand(spot.Hand);

        BalanceManager.PlayerSplit(spot, _game.Table);

        await DealCard(spot.Hand, spot.Id);
        await DealCard(spot.SplittedHand, spot.Id);
        return true; //TODO-HUs burası false idi. Bir hata mı vardı acaba?
    }

    private async Task<bool> ApplyHit(Spot spot, Hand hand)
    {
        await DealCard(hand, spot.Id);
        return hand.HandValue < 21;
    }

    private async Task<bool> ApplyDouble(Spot spot, Hand hand)
    {
        var cardDealing = DealCard(hand, spot.Id);
        BalanceManager.PlayerDouble(spot, _game.Table);

        await cardDealing;
        return false;
    }

    private async Task PlayForDealer()
    {
        var renderSecretCardTask = Task.Delay(2_000); //render card showing

        //Open dealer's card and notify everyone.
        ArgumentNullException.ThrowIfNull(_game.Table.Dealer.SecretCard);
        _game.Table.Dealer.Hand.Cards[1] = _game.Table.Dealer.SecretCard with { };
        _game.Table.Dealer.SecretCard = null;
        CardManager.GetCountOfHandAndUpdateHand(_game.Table.Dealer.Hand);

        var cardNotificationTask = _allPlayers.NotifySecretCard(new() { SecretCard = _game.Table.Dealer.Hand.Cards[1], HandValue = _game.Table.Dealer.Hand.HandValue });

        await Task.WhenAll(renderSecretCardTask, cardNotificationTask);

        //If everyone who has betted is busted, dealer wins. 
        if (!_game.Table.Spots.Any(x => x.BetAmount > 0 && !x.Hand.IsBusted)
            && !_game.Table.Spots.Any(x => x.BetAmount > 0 && x.SplittedHand is not null && !x.SplittedHand.IsBusted)) return;

        //If everyone who has betted has blackjack, and dealer doesnt, then everyone wins
        if (!_game.Table.Spots.Any(x => x.BetAmount > 0 && !x.Hand.IsBlackJack)
            && !_game.Table.Spots.Any(x => x.BetAmount > 0 && x.SplittedHand is not null && !x.SplittedHand.IsBlackJack)) return;

        //Otherwise dealers opens card and hits until at least 17
        while (_game.Table.Dealer.Hand.HandValue < 17)
        {
            await DealCard(_game.Table.Dealer.Hand, 0);
        }
    }
}