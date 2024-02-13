using AutoMapper;
using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.Model.Models;

namespace BlackJackHusofication.Business.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BjGame, BjGameDto>().ReverseMap(); 
        CreateMap<Table, TableDto>().ReverseMap(); 
    }
}
