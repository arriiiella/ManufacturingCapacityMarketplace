export class Orders {
  OrderNo: number;
  CustomerId: number;
  CustomerName: string;
  OrderDate: Date;
  ManufacturerId: Number;
  ManufacturerName: string;
  OrderedByName: string;
  Fulfilled: boolean;
  OrderLine: OrderLine[];
}

export class OrderLine {
  OrderNo: number;
  LineNo: number;
  LocationId: number;
  MachineId: number;
  Capacity: number;
  CostPerUnit: number;
  LineAmount: number;
  CapacityEntryNo: number;
}
