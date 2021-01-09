export class Emergency {
  Id: number;
  EmergencyTypeId: number;
  Location: string;
  Description: string;
  ReportTime: Date;
  Status: number;
  ModUser: string;

  constructor(type: number, location, description) {
    this.EmergencyTypeId = type;
    this.Location = location;
    this.Description = description;
    this.ReportTime = new Date();
  }
}
