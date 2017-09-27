using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreSystem.ResourceServer.Utils
{
    public static class ControllerExtensions
    {
        public static async Task<bool> IsUserAdminAsync(this Controller controller)
        {
            if (String.IsNullOrEmpty(controller.User.Identity.Name))
                return await Task.FromResult(false);

            return await Task.FromResult(controller.User.Identity.Name == "admin");           
        }
        
        public static List<string> ValidateIncomingModel(this Controller controller)
        {
            if (!controller.ModelState.IsValid)
            {
                List<string> errorMessages = new List<string>();
                foreach (var modelState in controller.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                }

                return errorMessages;
            }

            else return new List<string>();
        }
    }
}
