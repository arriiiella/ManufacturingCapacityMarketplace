export class MachineCapacityEntry {
  EntryNo: number;
  MachineId: number;
  Date: string;
  StartTime: any;
  Capacity: number;
}

export class SpareCapacities {
  EntryNo: number;
  Location: string;
  Process: string;
  Machine: string;
  Model: string;
  Date: string;
  StartTime: string;
  Capacity: string;
}
