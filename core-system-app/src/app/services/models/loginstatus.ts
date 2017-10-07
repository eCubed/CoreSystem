export class LoginStatus {

	public static readonly LOGGED_OUT = "Logged Out";
	public static readonly LOGGING_IN = "Logging In";
	public static readonly LOGGED_IN = "Logged In";
	public static readonly LOGGING_OUT = "Logging Out";
	public static readonly INVALID_LOGIN = "Invalid Login";
	public static readonly SERVER_ERROR = "Server Error";
	public static readonly SERVER_UNAVAILABLE = "server Unavailable";

	name: string;
	isLoggedIn: boolean;
	isError: boolean;

	constructor(name?: string, isLoggedIn?: boolean, isError?: boolean) {
		this.name = (name != null) ? name : "";
		this.isLoggedIn = (isLoggedIn != null) ? isLoggedIn : false;
		this.isError = (isError != null) ? isError : false;
	}
}
