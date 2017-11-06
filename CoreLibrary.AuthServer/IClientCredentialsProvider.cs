namespace CoreLibrary.AuthServer
{
    /// <summary>
    /// This interface is created to make a distinction between password and client credentials
    /// for dependency injection.
    /// </summary>
    public interface IClientCredentialsProvider : ICredentialsProvider
    {
    }
}
