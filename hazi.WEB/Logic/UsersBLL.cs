using hazi.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace hazi.WEB.Logic
{
    public class UsersBLL
    {
        public static List<Users> UserList()
        {
            List<Users> users = new List<Users>();
            using (var db = new ApplicationDbContext())
            {
                 users = (from b in db.Users
                        select new Users
                        {
                            Name = b.UserName,
                        }).ToList();
            }

            foreach (Users user in users)
            {
                user.RoleList = new List<ListItem>();
                user.RoleList.Add(new ListItem { Value = RegisterUserAs.Admin.ToString(), Text = RegisterUserAs.Admin.ToString() });
                user.RoleList.Add(new ListItem { Value = RegisterUserAs.NormalUser.ToString(), Text = RegisterUserAs.NormalUser.ToString() });
                user.RoleList.Add(new ListItem { Value = RegisterUserAs.Jovahagyok.ToString(), Text = RegisterUserAs.Jovahagyok.ToString() });
                user.Role = UserRole(user.Name);
            }

            return users;
        }

        private static string UserRole(string name)
        {
            string admin = RegisterUserAs.Admin.ToString();
            string normal = RegisterUserAs.NormalUser.ToString();
            string jovahagy = RegisterUserAs.Jovahagyok.ToString();
            if (RoleActions.IsInRole(name, admin))
                return admin;
            else if (RoleActions.IsInRole(name, normal))
                return normal;
            else if (RoleActions.IsInRole(name, jovahagy))
                return jovahagy;
            else return string.Empty;
        }
    }
}