using hazi.DAL;
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
        /// <summary>
        /// Felhasználók kiolvasása beépített db-ből
        /// </summary>
        /// <returns></returns>
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
                user.RoleList.Add(new ListItem { Value = RegisterUserAs.Admin.ToString(), Text = RegisterUserAs.Admin.ToDisplayString() });
                user.RoleList.Add(new ListItem { Value = RegisterUserAs.NormalUser.ToString(), Text = RegisterUserAs.NormalUser.ToDisplayString() });
                user.RoleList.Add(new ListItem { Value = RegisterUserAs.Jovahagyok.ToString(), Text = RegisterUserAs.Jovahagyok.ToDisplayString() });
                user.Role = UserRole(user.Name);
                user.RoleMegjelenes = ((RegisterUserAs)Enum.Parse(typeof(RegisterUserAs), user.Role)).ToDisplayString();
            }

            return users;
        }

        /// <summary>
        /// Felhasználói szerepkörök beállítása
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        public static bool FelhasznaloIsInDB(string username)
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
            foreach (var item in users)
            {
                if (item.Name == username)
                    return true;
            }
            return false;
        }

        public static List<string> GetUserNames()
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

            List<string> usernames = new List<string>();
            usernames.Add(string.Empty); //kötelező választás, null alapértelmezett selected érték
            foreach (var item in users)
            {
                usernames.Add(item.Name);
            }

            return usernames;
        }

        internal static void FelhasznaloiAdatokMentese(string username, string szuletesiDatum, string vezeto, string kepUrl)
        {
            using (hazi2Entities db = new hazi2Entities())
            {
                FelhasznaloiProfilok fp = null;
                try
                {
                    fp = (from u in db.FelhasznaloiProfiloks
                          where u.UserName == username
                          select u).Single();
                }
                catch (Exception) { }
                if (fp == null)
                {
                    fp = new FelhasznaloiProfilok();
                    fp.UserName = username;
                }
                fp.SzuletesiDatum = DateTime.Parse(szuletesiDatum);
                fp.Vezeto = vezeto;
                fp.ProfilKepUrl = kepUrl;

                db.FelhasznaloiProfiloks.Add(fp);

                db.SaveChanges();
            }
        }
    }
}