export class SimulationLog {
  logType: SimulationLogType;
  message: string;
}

export enum SimulationLogType {
  CardActionLog = 1,
  CardDealLog = 2,
  GameLog = 3,
  DealerActions = 4,
  BalanceLog = 5,
  HusokaLog = 6,
}