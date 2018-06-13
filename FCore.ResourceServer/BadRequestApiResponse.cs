using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCore.ResourceServer
{
    public class BadRequestApiResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; }

        public BadRequestApiResponse(ModelStateDictionary modelState)
        : base(400)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException("ModelState must be invalid", nameof(modelState));
            }

            Errors = modelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
        }

        public BadRequestApiResponse(string appSpecificCode, IEnumerable<string> errors = null) : base(400, appSpecificCode)
        {
            Errors = errors;
        }
    }
}
