export interface Auth {
  id?: number;
  name: string;
  email: string;
  password: string;
  role: string;
  token: string;
  refreshToken: string;
}

export class ConfirmEmail {
  id!: number;
  emailToken!: string;
}
