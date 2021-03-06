﻿using FCore.Foundations;
using FCore.Net.Security;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FCore.AuthServer
{
    public abstract class GrantTypeProcessorBase : IGrantTypeProcessor
    {
        public string IdentifierName { get; set; }
        public string PasscodeName { get; set; }
        public List<string> OtherRequiredParameters { get; set; }

        public GrantTypeProcessorBase()
        {
            OtherRequiredParameters = new List<string>();
        }

        public GrantTypeProcessorBase(string identifierName, string passcodeName, List<string> otherRequiredParameters)
        {
            IdentifierName = identifierName;
            PasscodeName = passcodeName;
            OtherRequiredParameters = otherRequiredParameters;
        }

        public ManagerResult ValidateRequest(HttpRequest request)
        {
            if (!request.Form.ContainsKey(IdentifierName))
                return new ManagerResult(AuthServerMessages.IdentifierRequired);

            if (!request.Form.ContainsKey(PasscodeName))
                return new ManagerResult(AuthServerMessages.PasscodeRequired);

            foreach (string otherRequiredParams in OtherRequiredParameters)
            {
                if (!request.Form.ContainsKey(otherRequiredParams))
                    return new ManagerResult(AuthServerMessages.OtherParametersRequired);
            }

            return new ManagerResult();
        }

        public List<KeyValuePair<string, string>> GetOtherRequiredParametersFromRequest(HttpRequest request)
        {
            List<KeyValuePair<string, string>> otherRequiredParamValues = new List<KeyValuePair<string, string>>();

            OtherRequiredParameters.ForEach(requiredParamName =>
            {
                if (request.Form.ContainsKey(requiredParamName))
                {
                    otherRequiredParamValues.Add(new KeyValuePair<string, string>(requiredParamName, request.Form[requiredParamName]));
                }
            });

            return otherRequiredParamValues;
        }

        public abstract IWebToken GenerateWebTokenObject(TokenIssuerOptions tokenIssuerOptions, string identifierValue, List<KeyValuePair<string, string>> additionalClaims);

        public abstract List<KeyValuePair<string, string>> ObtainAdditionalClaims(string identifierValue);

        public abstract ManagerResult ValidateIdentifier(string identifierValue, string passcodeValue, List<KeyValuePair<string, string>> otherRequiredParamValues);

        public abstract IAuthServerResponse GenerateAuthServerResponse(string accessToken, string identifierValue);
    }
}
