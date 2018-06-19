using FCore.Foundations;
using FCore.Net.Security;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FCore.AuthServer
{
    /// <summary>
    /// Class representing a grant type whose parameters are passed to the token endpoint.
    /// </summary>
    public interface IGrantTypeProcessor
    {
        // Data properties     
        string IdentifierName { get; set; }
        string PasscodeName { get; set; }
        List<string> OtherRequiredParameters { get; set; }

        /* The following are processor functionality
         */

        /// <summary>
        /// Extracts the other required parameters from the request based on the grant type processor's
        /// required parameters.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<KeyValuePair<string, string>> GetOtherRequiredParametersFromRequest(HttpRequest request);

        /// <summary>
        /// Validates whether all required parameters were supplied in the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ManagerResult ValidateRequest(HttpRequest request);

        /// <summary>
        /// Checks, for example, username-password
        /// </summary>
        /// <param name="identifierValue"></param>
        /// <param name="passcodeValue"></param>
        /// <returns></returns>
        ManagerResult ValidateIdentifier(string identifierValue, string passcodeValue, List<KeyValuePair<string, string>> otherRequiredParamValues);

        /// <summary>
        /// Obtains additional info about the identifier
        /// </summary>
        /// <param name="identifierValue"></param>
        /// <returns></returns>
        List<KeyValuePair<string, string>> ObtainAdditionalClaims(string identifierValue);

        IWebToken GenerateWebTokenObject(TokenIssuerOptions tokenIssuerOptions, string identifierValue, List<KeyValuePair<string, string>> additionalClaims);

        /// <summary>
        /// Responsible for generating the object (containing the access token). It is given the identifierValue so that
        /// any relevant info about the identifier we wish to include in the IAuthServerResponse will be returned.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="identifierValue"></param>
        /// <returns></returns>
        IAuthServerResponse GenerateAuthServerResponse(string accessToken, string identifierValue);
      
        /*
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

        */
    
    }
}
