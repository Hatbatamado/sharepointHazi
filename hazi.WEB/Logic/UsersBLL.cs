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

        /// <summary>
        /// Igaz értéket ad vissza, ha a felhasználót megtalálta az adatbázisban
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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

        /// <summary>
        /// DDL-hez visszaadja a felhasználók nevét egy nullal a lista elején
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Felhasználó profil adatok mentése
        /// </summary>
        /// <param name="username"></param>
        /// <param name="szuletesiDatum"></param>
        /// <param name="vezeto"></param>
        /// <param name="kepUrl"></param>
        internal static void FelhasznaloiAdatokMentese(string username, string szuletesiDatum, string vezeto, string kepUrl)
        {
            using (hazi2Entities db = new hazi2Entities())
            {
                FelhasznaloiProfilok fp = null;
                bool uj = false;
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
                    uj = true;
                }
                fp.SzuletesiDatum = DateTime.Parse(szuletesiDatum);
                fp.Vezeto = vezeto;
                fp.ProfilKepUrl = kepUrl;

                if (uj)
                    db.FelhasznaloiProfiloks.Add(fp);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Felhasználó profil adatok lekérése
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static FelhasznaloiProfilok GetUserProfilData(string user)
        {
            
            using (hazi2Entities db = new hazi2Entities())
            {
                try
                {
                    return (from f in db.FelhasznaloiProfiloks
                            where f.UserName == user
                            select f).Single();
                }
                catch (Exception) { return null; }
            }
        }

        /// <summary>
        /// Felhasználó profil adatok mentése
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="PictureFileUpload"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="username"></param>
        /// <param name="szuldatum"></param>
        /// <param name="vezeto"></param>
        /// <param name="Response"></param>
        /// <param name="redirect"></param>
        public static void ProfilMentes(HttpServerUtility Server,
            System.Web.UI.WebControls.FileUpload PictureFileUpload, System.Web.UI.WebControls.Literal ErrorMessage,
            string username, string szuldatum, string vezeto, HttpResponse Response, string redirect)
        {
            Boolean fileOK = false;
            String path = Server.MapPath("~" + Konstansok.ImagesPath);
            String fileExtension = "";
            if (PictureFileUpload.HasFile)
            {
                fileExtension = System.IO.Path.GetExtension(PictureFileUpload.FileName).ToLower();
                String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
            }
            string FileNev = "";
            if (fileOK)
            {
                try
                {
                    // Save to Images folder.
                    FileNev = username + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Hour
                        + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + fileExtension;
                    PictureFileUpload.PostedFile.SaveAs(path + FileNev);
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = ex.Message;
                }
            }
            else
            {
                ErrorMessage.Text = "Unable to accept file type.";
            }

            if (fileOK)
                FelhasznaloiAdatokMentese(username, szuldatum, vezeto, Konstansok.ImagesPath + FileNev);
            else
                FelhasznaloiAdatokMentese(username, szuldatum, vezeto, FileNev);

            Response.Redirect(redirect);
        }
    }
}