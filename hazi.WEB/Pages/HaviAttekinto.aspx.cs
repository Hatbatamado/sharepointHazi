using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using hazi.WEB.Models;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace hazi.WEB.Pages
{
    public partial class HaviAttekinto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                    Error404.HibaDobas(Response);
                else
                {
                    Feltoltes(DateTime.Now);
                    honapLabel.Text = DateTime.Now.Year + "/" + DateTime.Now.Month;
                }
            }
            if (Request["__EVENTARGUMENT"] == "TextChangedJobbra")
            {
                string[] seged = honapLabel.Text.Split('/');
                int honap = Convert.ToInt32(seged[1]);
                int ev = Convert.ToInt32(seged[0]);
                if (honap == 12)
                {
                    ev++;
                    honap = 1;
                }
                else
                    honap++;
                DateTime date = new DateTime(ev, honap, 1);

                honapLabel.Text = date.Year + "/" + date.Month;

                Feltoltes(date);
            }
            else if (Request["__EVENTARGUMENT"] == "TextChangedBalra")
            {
                string[] seged = honapLabel.Text.Split('/');
                int honap = Convert.ToInt32(seged[1]);
                int ev = Convert.ToInt32(seged[0]);
                if (honap == 1)
                {
                    ev--;
                    honap = 12;
                }
                else
                    honap--;
                DateTime date = new DateTime(ev, honap, 1);

                honapLabel.Text = date.Year + "/" + date.Month;

                Feltoltes(date);
            }
        }

        private void Feltoltes(DateTime date)
        {
            bool vezeto = UsersBLL.IsManager(User.Identity.Name);
            if (vezeto || RoleActions.IsInRole(HttpContext.Current.User.Identity.Name, RegisterUserAs.Admin.ToString()) ||
                    RoleActions.IsInRole(HttpContext.Current.User.Identity.Name, RegisterUserAs.Jovahagyok.ToDisplayString()))
                HaviMegjelenites(vezeto, date);
            else
                Error404.HibaDobas(Response);
        }

        private void HaviMegjelenites(bool vezeto, DateTime date)
        {
            Jelmagyarazat();

            List<HiearchiaOsszeg> lista = new List<HiearchiaOsszeg>();
            List<string> userlista = new List<string>();
            if (RoleActions.IsInRole(HttpContext.Current.User.Identity.Name, RegisterUserAs.Admin.ToString()) ||
                    RoleActions.IsInRole(HttpContext.Current.User.Identity.Name, RegisterUserAs.Jovahagyok.ToDisplayString()))
            {
                userlista = UsersBLL.GetUserNames();
                userlista.RemoveAt(0);
            }
            else
                userlista.Add(User.Identity.Name);

            while (userlista.Count > 0)
                JovahagyBLL.GetUserNumbersByManager(userlista[0], lista, userlista);
            
            lista = lista.OrderBy(o => o.UserCount).ToList();

            List<HaviAttekintoElem> hv = new List<HaviAttekintoElem>();
            while (lista.Count > 0)
                hv.Add(JovahagyBLL.HiearchiaElem(lista[lista.Count - 1].UserName, lista));

            HAVS havs = new HAVS(hv, date);

            KulsoRepeater.DataSource = HAVS.HvRep;
            KulsoRepeater.DataBind();

            List<Napokszama> napok = new List<Napokszama>();
            for (int i = 1; i <= 31; i++)
            {
                napok.Add(new Napokszama() { napokSzama = i });
            }
            napokSzama.DataSource = napok;
            napokSzama.DataBind();
        }

        private void Jelmagyarazat()
        {
            List<JelMagy> jelmagy = UjJogcimBLL.GetJelMagy();
            JelmagyarazatRepeater.DataSource = jelmagy;
            JelmagyarazatRepeater.DataBind();
        }

        protected void RangLinkB_Command(object sender, CommandEventArgs e)
        {
            List<HaviAttekintoViewModel> tempList = new List<HaviAttekintoViewModel>();
            if (e.CommandName == "+")
                Kinyitas(tempList, e.CommandArgument.ToString());
            else if (e.CommandName == "-")
                Becsukas(tempList, e.CommandArgument.ToString());
        }

        private void Becsukas(List<HaviAttekintoViewModel> tempList, string userNev)
        {
            int i = IndexOfUser(0, tempList, userNev);

            List<string> UsersList = new List<string>();
            if (i < HAVS.HvRep.Count)
            {
                tempList[tempList.Count - 1].RangVezeto = '+';
                UsersBLL.GetUsersByManager(tempList[tempList.Count - 1].Nev, UsersList);
            }
            foreach (var item in HAVS.HvRep.ToList())
            {
                foreach (var itemL in UsersList)
                {
                    if (item.Nev == itemL)
                        HAVS.HvRep.Remove(item);
                }
            }
            ListaEsRepeaterFeltoltes(tempList, i);
        }

        private void Kinyitas(List<HaviAttekintoViewModel> tempList, string userNev)
        {
            int i = IndexOfUser(0, tempList, userNev);

            if (i < HAVS.HvRep.Count)
            {
                tempList[tempList.Count - 1].RangVezeto = '-';
                List<HaviAttekintoElem> hv = new List<HaviAttekintoElem>();
                hv.Add(JovahagyBLL.HiearchiaElem(HAVS.HvRep[i].Nev, null));

                foreach (var item in hv)
                {
                    if (item.UsersLista.Count != 0)
                    {
                        int j = 0;
                        while (j < item.UsersLista.Count)
                        {
                            string[] seged = honapLabel.Text.Split('/');
                            int ev = Convert.ToInt32(seged[0]);
                            int honap = Convert.ToInt32(seged[1]);
                            if (item.UsersLista[j].UsersLista.Count != 0)
                            {
                                
                                tempList.Add(new HaviAttekintoViewModel(ev, honap, item.UsersLista[j].UserName)
                                {
                                    Nev = item.UsersLista[j].UserName,
                                    RangVezeto = '+'
                                });
                            }
                            else
                                tempList.Add(new HaviAttekintoViewModel(ev, honap, item.UsersLista[j].UserName)
                                {
                                    Nev = item.UsersLista[j].UserName,
                                    RangNormal = 'o'
                                });
                            j++;
                        }
                    }
                }
                ListaEsRepeaterFeltoltes(tempList, i);
            }
        }

        private int IndexOfUser(int i, List<HaviAttekintoViewModel> tempList, string userNev)
        {
            while (i < HAVS.HvRep.Count)
            {
                tempList.Add(HAVS.HvRep[i]);
                if (HAVS.HvRep[i].Nev != userNev)
                    i++;
                else
                    break;
            }

            return i;
        }

        private void ListaEsRepeaterFeltoltes(List<HaviAttekintoViewModel> tempList, int i)
        {
            i++;
            while (i < HAVS.HvRep.Count)
            {
                tempList.Add(HAVS.HvRep[i++]);
            }

            HAVS.HvRep = tempList;
            KulsoRepeater.DataSource = tempList;
            KulsoRepeater.DataBind();
        }

        protected void BelsoRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string szin = (e.Item.DataItem as HaviAttekintoElem).Szin;
            string fontcolor = "";
            if (szin == "" || szin == null)
            {
                szin = Konstansok.alapSzin;
                fontcolor = Konstansok.alapSzin;
            }
            else
                fontcolor = Konstansok.alapFontSzin;
            HtmlGenericControl divRog = (HtmlGenericControl)(e.Item.FindControl("bejelentesKocka"));

            if (divRog != null)
                divRog.Attributes["style"] += ("background:" + szin + "; color: " + fontcolor + ";)"); 
        }

        protected void JelmagyarazatRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlGenericControl div = (HtmlGenericControl)(e.Item.FindControl("jelSzin"));
            string szin = (e.Item.DataItem as JelMagy).Szin;
            if (div != null)
                div.Attributes["style"] += ("background:" + szin + "; color: " + Konstansok.alapFontSzin + ";)");
        }
    }
}