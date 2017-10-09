export class LoginInfo {
	username: string;
	accessToken: string;
	roles: Array<string>;

	constructor() {
		this.roles = new Array<string>();
	}
}