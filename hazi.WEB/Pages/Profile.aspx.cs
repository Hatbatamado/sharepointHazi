using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB.Pages
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    string Username = Request.QueryString["UserName"];
                    FelhasznaloiProfilok fp = UsersBLL.GetUserProfilData(Username);
                    if (fp == null)
                    {
                        Master.Uzenet.Visible = true;
                        Master.Uzenet.Text = "A keresett felhasználói profil oldal nem található! Vagy a tulajdonosa még nem töltötte ki a profil adatait.";
                        contentDiv.Visible = false;
                    }
                    else
                    {
                        UserNameLabel.Text = fp.UserName;
                        if (fp.ProfilKepUrl != "")
                            ProfilePictureImg.ImageUrl = fp.ProfilKepUrl;
                        else
                            ProfilePictureImg.ImageUrl = Konstansok.ImagesPath + "logo.jpg";
                        BirthdayLabel.Text = fp.SzuletesiDatum.ToString("yyyy.MM.dd");
                        ManagerLabel.Text = fp.Vezeto;
                    }
                }
                else
                    Error404.HibaDobas(Response);
            }
        }
    }
}