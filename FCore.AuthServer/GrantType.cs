using FCore.Foundations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FCore.AuthServer
{
    /// <summary>
    /// Class representing a grant type whose parameters are passed to the token endpoint.
    /// This is a descriptor only and it is not responsible for processing anything.
    /// </summary>
    public class GrantType
    {
        public string Name { get; set; }
        public string IdentifierName { get; set; }
        public string PasscodeName { get; set; }
        public List<string> OtherRequiredParameters { get; set; }

        public GrantType()
        {
            OtherRequiredParameters = new List<string>();
        }

        public GrantType(string name, string identifierName, string passcodeName, List<string> otherRequiredParameters = null)
        {
            Name = name;
            IdentifierName = identifierName;
            PasscodeName = passcodeName;
            OtherRequiredParameters = (otherRequiredParameters == null) ? new List<string>() : otherRequiredParameters;
        }

        public ManagerResult ValidateTokenRequest(HttpRequest request)
        {
            if (!request.Form.ContainsKey(IdentifierName))
                return new ManagerResult(AuthServerMessages.IdentifierRequired);

            if (!request.Form.ContainsKey(PasscodeName))
                return new ManagerResult(AuthServerMessages.PasscodeRequired);

            foreach(string otherRequiredParams in OtherRequiredParameters)
            {
                if (!request.Form.ContainsKey(otherRequiredParams))
                    return new ManagerResult(AuthServerMessages.OtherParametersRequired);
            }

            return new ManagerResult();
        }

        public List<KeyValuePair<string, string>> ExtractDataFromRequest(HttpRequest request)
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            values.Add(new KeyValuePair<string, string>(IdentifierName, request.Form[IdentifierName].ToString() ?? ""));
            values.Add(new KeyValuePair<string, string>(PasscodeName, request.Form[PasscodeName].ToString() ?? ""));

            OtherRequiredParameters.ForEach(requiredParamName =>
            {
                values.Add(new KeyValuePair<string, string>(requiredParamName, request.Form[requiredParamName].ToString() ?? ""));
            });

            return values;
        }
    }
}
