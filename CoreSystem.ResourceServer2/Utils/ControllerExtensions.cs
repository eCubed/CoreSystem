using CoreSystem.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreSystem.ResourceServer2.Utils
{
    public static class ControllerExtensions
    {
        public static async Task<bool> IsUserAdminAsync(this Controller controller, UserManager<CoreSystemUser> UM)
        {
            if (String.IsNullOrEmpty(controller.User.Identity.Name))
                return false;

            CoreSystemUser user = await UM.FindByNameAsync(controller.User.Identity.Name);

            if (user == null)
                return false;

            return await UM.IsInRoleAsync(user, RoleNames.Administrator);
        }

        public static async Task<AuthenticatedInfo> ResolveAuthenticatedEntitiesAsync(this Controller controller, CoreSystemDbContext context,
            UserManager<CoreSystemUser> userManager)
        {
            string username = controller.Request.HttpContext.User?.Identity?.Name ?? "";
            CoreSystemUser user = await userManager.FindByNameAsync(username);

            int userId = user?.Id ?? 0;

            return new AuthenticatedInfo
            {
                UserId = userId
            };
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
