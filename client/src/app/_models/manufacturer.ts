export class Manufacturer {
  Id: number = 0;
  Name: string = '';
  IndustryId: number = 0;
  AddressId: number = 0;
  VatRegistrationNo: string = '';
  BillingAddressId: number = 0;
}

export class Machine {
  Id: number;
  Name: string;
  ModelNo: string;
  Capicity: string;
  SetupTime: string;
  CostPerUnit: string;
  Process: string;
}

export class Capacity {
  Id: number;
  Date: Date;
  StartTime: string;
  Capacity: string;
}

export class CapacityInfo {
  Id: number;
  Location: string;
  Process: string;
  Machine: string;
  Model: string;
  StartTime: string;
  Capacity: string;
}
