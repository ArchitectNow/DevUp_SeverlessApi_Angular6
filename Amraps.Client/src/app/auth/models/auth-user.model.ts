export class AuthUser {
  public userId: number;
  public userName: string;
  public displayName: string;
  public roles: string[];

  constructor(data?: any) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property)) {
          (<any>this)[property] = (<any>data)[property];
        }
      }
    }
  }
}
