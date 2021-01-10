import { Emergency } from './Emergency';

export class Team {
  id: number;
  name: string;
  location: string;
  assignedEmergencies: Array<Emergency>;
}
