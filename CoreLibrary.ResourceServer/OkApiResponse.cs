namespace CoreLibrary.ResourceServer
{
    public class OkApiResponse<T> : ApiResponse
    {
        public T Data { get; }

        public OkApiResponse(T data) : base(200)
        {
            Data = data;
        }
    }
}
