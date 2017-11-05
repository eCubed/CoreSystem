namespace CoreLibrary.AuthServer
{
    /// <summary>
    /// This interface represents the incoming request body of a token endpoint. Note that we do not
    /// specify specific parameters such as username, password, client_id, or client_secret. Instead,
    /// we will instantiate specific implementations of IAuthServerRequest that will represent the actual
    /// identifier-passCode duo from the actual request.
    /// </summary>
    public interface IAuthServerRequest
    {
        string GrantType { get; set; }
    }
}
