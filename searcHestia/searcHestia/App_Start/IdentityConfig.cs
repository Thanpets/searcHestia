using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using searcHestia.Models;
using Microsoft.AspNet.Identity.Owin;

namespace searcHestia.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new SearchestiaContext());
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<RoleManager<ApplicationRole>>((options, context) =>
                new RoleManager<ApplicationRole>(
                    new RoleStore<ApplicationRole>(context.Get<SearchestiaContext>())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });

            
            //////// CreateRoles //////// 
            string[] roleNames = { "Admin", "Renter", "Owner" };

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new SearchestiaContext()));
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    roleResult = roleManager.Create(new IdentityRole(roleName));
                }
            }
        }
    }
}