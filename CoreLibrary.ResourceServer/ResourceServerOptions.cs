namespace CoreLibrary.ResourceServer
{
    public class ResourceServerOptions
    {
        public string Issuer { get; set; }
        public string CryptionKey { get; set; }
        public int? TokenDurationInDays { get; set; } // The middleware will default to 14 days if this is not provided.
    }
}
