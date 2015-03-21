using hazi.WEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public enum RegisterUserAs
    {
        Admin,
        NormalUser,
        Jovahagyok
    }

    public class RoleActions
    {
        internal string createUserAs(string uName, string uPass, RegisterUserAs role)
        {
            // Access the application context and create result variables.
            Models.ApplicationDbContext context = new ApplicationDbContext();
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            // Create a RoleStore object by using the ApplicationDbContext object. 
            // The RoleStore is only allowed to contain IdentityRole objects.
            var roleStore = new RoleStore<IdentityRole>(context);

            // Create a RoleManager object that is only allowed to contain IdentityRole objects.
            // When creating the RoleManager object, you pass in (as a parameter) a new RoleStore object. 
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            // Then, you create the "canEdit" role if it doesn't already exist.
            if (!roleMgr.RoleExists(role.ToString()))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = role.ToString() });
            }

            // Create a UserManager object based on the UserStore object and the ApplicationDbContext  
            // object. Note that you can create new objects and use them as parameters in
            // a single line of code, rather than using multiple lines of code, as you did
            // for the RoleManager object.
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var appUser = new ApplicationUser
            {
                UserName = uName
            };
            IdUserResult = userMgr.Create(appUser, uPass);

            if (IdUserResult.Succeeded)
            {
                IdUserResult = userMgr.AddToRole(appUser.Id, role.ToString());
                UserManager mgr = new UserManager();
                IdentityHelper.SignIn(mgr, appUser, isPersistent: false);
            }
            else
                return IdUserResult.Errors.FirstOrDefault();

            return "";
        }

        //Adott user, adott szerepkörhöz való rendelése
        internal static void AddToRole(string name, RegisterUserAs role)
        {
            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;
            Models.ApplicationDbContext context = new ApplicationDbContext();
            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ApplicationUser user = userMgr.FindByName(name);

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            if (!roleMgr.RoleExists(role.ToString()))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = role.ToString() });
            }

            IdUserResult = userMgr.AddToRole(user.Id, role.ToString());
        }

        //szerepkör lekérdezés
        internal static string GetRole(string userName)
        {
            try
            {
                Models.ApplicationDbContext context = new ApplicationDbContext();
                var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                IList<string> roles = userMgr.GetRoles(userMgr.FindByName(userName).Id);
                if (roles.Count != 0)
                    return roles.ElementAt(0);
                else
                    return string.Empty;
            }
            catch (NullReferenceException) { return string.Empty; }
        }


        internal static string ChangeRole(string name, string oldRole, string newRole)
        {
            IdentityResult IdUserResult;
            IdentityResult IdRoleResult;
            Models.ApplicationDbContext context = new ApplicationDbContext();
            
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            if (!roleMgr.RoleExists(newRole.ToString()))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = newRole.ToString() });
            }

            var userMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            

            try
            {
                ApplicationUser user = userMgr.FindByName(name);

                IdUserResult = userMgr.RemoveFromRole(user.Id, oldRole);
                IdUserResult = userMgr.AddToRole(user.Id, newRole);
                return string.Empty;
            }
            catch (Exception) { return "Hiba történt a szerepkör változtatás során"; }
        }
    }
}