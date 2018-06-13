namespace FCore.ResourceServer
{
    public class NoContentApiResponse<TKey> : ApiResponse
    {
        public object Data { get; set; }

        public NoContentApiResponse(TKey id) : base(204)
        {
            Data = new { Id = id };
        }
    }
}
