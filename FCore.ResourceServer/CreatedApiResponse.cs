using System;

namespace FCore.ResourceServer
{
    public class CreatedApiResponse<TKey> : ApiResponse
    {
        public Object Data { get; set; }

        public CreatedApiResponse(TKey newResourceId) : base(201)
        {
            Data = new { Id = newResourceId };
        }
    }
}
