using System.Collections.Generic;

namespace FCore.ResourceServer
{
    public class InternalServerErrorApiResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; }

        public InternalServerErrorApiResponse(string appSpecificCode, IEnumerable<string> errors = null)
            : base(500, appSpecificCode)
        {
            Errors = errors;
        }
    }
}
