using System.Collections.Generic;

namespace FCore.ResourceServer
{
    public class UnauthorizedApiResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; }

        public UnauthorizedApiResponse(string appSpecificCode, IEnumerable<string> errors = null)
            : base(401, appSpecificCode)
        {
            Errors = errors;
        }
    }
}
