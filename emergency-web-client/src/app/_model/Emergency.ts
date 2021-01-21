export class Emergency {
  Id: number;
  emergencyTypeId: number;
  location: string;
  description: string;
  reportTime: Date;
  status: number;
  modUser: string;
  identifier: string;

  constructor(type: number, location, description, identifier) {
    this.emergencyTypeId = type;
    this.location = location;
    this.description = description;
    this.reportTime = new Date();
    this.identifier = identifier;
  }
}
