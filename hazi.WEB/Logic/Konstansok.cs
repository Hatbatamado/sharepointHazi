using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hazi.WEB.Logic
{
    public class Konstansok
    {
        public const string ReportTemplatesPath = "~/ReportTemplates/";
        public const string ImagesPath = "/Images/";

        public const string RogzitveAlapSzin = "#FFFF00";
        public const string JovahagyvaAlapSzin = "#006400";
        public const string TobbBejelenesAlapszin = "#c9c9ee";
        public const string alapSzin = "lightgreen";
        public const string alapFontSzin = "black";

        public const string RedirectAdminOldal = "/Pages/AdminPage";
        public const string RedirectOsszegzoForm = "/Pages/OsszegzoForm";
        public const string RedirectAccoutManage = "/Account/Manage";
        public const string RedirectAccoutLogin = "/Account/Login";
        public const string RedirectAccoutRegister = "/Account/Register";
        public const string RedirectFooldal = "/";
        public const string RedirectAttekinto = "/Pages/Attekinto";
        public const string RedirectBejelento = "/Pages/Bejelento";

        public static string admin = RegisterUserAs.Admin.ToString();
        public static string normal = RegisterUserAs.NormalUser.ToString();
        public static string jovahagy = RegisterUserAs.Jovahagyok.ToString();
    }
}