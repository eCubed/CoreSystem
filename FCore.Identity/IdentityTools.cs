using FCore.Foundations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FCore.Identity
{
    public static class IdentityTools
    {
        /// <summary>
        /// Creates a user if not already created (uniqueness is the username). It will only add a role to the user
        /// if the role already exists.
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="userManager"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="roles"></param>
        /// <param name="resolveRoles"></param>
        /// <param name="fillCustomUserProperties"></param>
        /// <returns></returns>
        public static async Task<ManagerResult<TUser>> CreateUserAsync<TUser, TRole, TKey>(UserManager<TUser> userManager, RoleManager<TRole> roleManager,
            string username, string password, string email, List<string> roles = null, Action<TUser> setCustomUserProperties = null)
            where TUser : IdentityUser<TKey>, new()
            where TRole: IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            TUser user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                user = new TUser();
                user.UserName = username;
                user.Email = email;

                if (setCustomUserProperties != null)
                    setCustomUserProperties.Invoke(user);

                var createRes = await userManager.CreateAsync(user, password);

                if (createRes.Succeeded)
                {
                    if ((roles != null) && (roles.Count > 0))
                    {
                        roles.ForEach(roleName =>
                        {
                            if (roleManager.RoleExistsAsync(roleName).Result)
                            {
                                userManager.AddToRoleAsync(user, roleName).Wait();
                            }
                        });
                    }
                }
                else
                {
                    return new ManagerResult<TUser>(createRes.Errors.Select(e => e.Description).ToArray());
                }
            }

            // We actually do nothing if the user has already been created. It's not considered an error.
            return new ManagerResult<TUser>(user);
        }

        public static async Task<ManagerResult<TUser>> CreateUserAsync<TUser, TRole, TKey>(UserManager<TUser> userManager, RoleManager<TRole> roleManager,
            string username, string password, string email, string role, Action<TUser> setCustomUserProperties = null)
            where TUser : IdentityUser<TKey>, new()
            where TRole : IdentityRole<TKey>
            where TKey : IEquatable<TKey>
        {
            return await CreateUserAsync<TUser, TRole, TKey>(userManager, roleManager, username, password, email, 
                new List<string> { role }, setCustomUserProperties);
        }

        /// <summary>
        /// Creates a role if not already created, uniqueness by role name
        /// </summary>
        /// <typeparam name="TRole"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="roleManager"></param>
        /// <param name="roleName"></param>
        /// <param name="setCustomRoleProperties"></param>
        /// <returns></returns>
        public static async Task<ManagerResult<TRole>> CreateRoleAsync<TRole, TKey>(RoleManager<TRole> roleManager, string roleName, 
            Action<TRole> setCustomRoleProperties = null)
            where TRole : IdentityRole<TKey>, new()
            where TKey : IEquatable<TKey>
        {
            TRole role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                role = new TRole();

                if (setCustomRoleProperties != null)
                {
                    setCustomRoleProperties.Invoke(role);
                }

                var createRes = await roleManager.CreateAsync(role);

                if (!createRes.Succeeded)
                    return new ManagerResult<TRole>(createRes.Errors.Select(e => e.Description).ToArray());
            }

            return new ManagerResult<TRole>(role);
        }
        
    }
}
