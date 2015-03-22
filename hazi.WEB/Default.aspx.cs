using hazi.BLL;
using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB
{
    public partial class _Default : Page
    {
        private string admin = RegisterUserAs.Admin.ToString();
        private string normal = RegisterUserAs.NormalUser.ToString();
        private string jovahagy = RegisterUserAs.Jovahagyok.ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {

                    Bejelentesek.Visible = true;
                    List<UjBejelentes> lista = Bejelentes.GetIdoBejelentesek(RoleActions.GetRole(User.Identity.Name),
                                                    User.Identity.Name, DateTime.MinValue, DateTime.MinValue);

                    bejelentesekLista.DataSource = lista;
                    bejelentesekLista.DataBind();
                    MegfeleloMezokMegjelenitese(string.Empty);
                }
                else
                {
                    Bejelentesek.Visible = false;
                    Response.Redirect("/Account/Login.aspx");
                }
            }
            if (Request["__EVENTARGUMENT"] == "FilterByJogcim")
            {
                string text = (bejelentesekLista.HeaderRow.FindControl("jogcimFilter") as System.Web.UI.HtmlControls.HtmlInputText).Value;
                List<UjBejelentes> lista = Bejelentes.GetIdoBejelentesByFilerJogcim(text);

                GridFeltoles(lista, text, "jogcimFilter");
            }
            else if (Request["__EVENTARGUMENT"] == "FilterByFelhasznalo")
            {
                string text = (bejelentesekLista.HeaderRow.FindControl("usernameFilter") as System.Web.UI.HtmlControls.HtmlInputText).Value;
                List<UjBejelentes> lista;
                if (text != "")
                    lista = Bejelentes.GetIdoBejelentesByFilerFelhasznalo(text);
                else //ha még van a db-ben olyan bejelentés, aminek nincs "gazdája"
                    lista = Bejelentes.GetIdoBejelentesek(RoleActions.GetRole(User.Identity.Name),
                                                    User.Identity.Name, DateTime.MinValue, DateTime.MinValue);

                GridFeltoles(lista, text, "usernameFilter");
            }
            else if (Request["__EVENTARGUMENT"] == "FilterByLastedit")
            {
                string text = (bejelentesekLista.HeaderRow.FindControl("lasteditFilter") as System.Web.UI.HtmlControls.HtmlInputText).Value;
                List<UjBejelentes> lista;
                if (text != "")
                    lista = Bejelentes.GetIdoBejelentesByFilerLastEdit(text);
                else //ha még van a db-ben olyan bejelentés, aminek létrehozásakor és utána nem kapott értéket lastedit-re
                    lista = Bejelentes.GetIdoBejelentesek(RoleActions.GetRole(User.Identity.Name),
                                                    User.Identity.Name, DateTime.MinValue, DateTime.MinValue);

                GridFeltoles(lista, text, "lasteditFilter");
            }
            else if (Request["__EVENTARGUMENT"] == "FilterByTorlesStatus")
            {
                string text = (bejelentesekLista.HeaderRow.FindControl("DDLTorles") as DropDownList).SelectedItem.Value;

                List<UjBejelentes> lista;
                if (text != "")
                    lista = Bejelentes.GetIdoBejelentesByFilerTorlesStatus(text);
                else //ha az összes jelentést látni akarjuk
                    lista = Bejelentes.GetIdoBejelentesek(RoleActions.GetRole(User.Identity.Name),
                                                    User.Identity.Name, DateTime.MinValue, DateTime.MinValue);

                GridFeltoles(lista, text, "DDLTorles");
            }
        }

        private void GridFeltoles(List<UjBejelentes> lista, string txt, string control)
        {
            //null értéknél nem sikerült rájönni, hogyan tűntessem el az oldalról a 2 gombot,
            //így maradt ez az empty datás verzió
            bejelentesekLista.DataSource = lista;
            bejelentesekLista.DataBind();

            MegfeleloMezokMegjelenitese(string.Empty);
            if (control != "DDLTorles")
            {
                (bejelentesekLista.HeaderRow.FindControl(control) as System.Web.UI.HtmlControls.HtmlInputText).Value = txt;

                TextBoxFocus(control);
            }
            else if (control == "DDLTorles")
            {
                (bejelentesekLista.HeaderRow.FindControl("DDLTorles") as DropDownList).SelectedValue = txt;
            }
        }

        protected void Ujbejelentes_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Pages/Bejelento");
        }

        protected void BejelentesTorles_Click(object sender, EventArgs e)
        {
            //ha már volt hiba üzenet, azt rejtsük el
            Master.Uzenet.Visible = false;

            //Admin nem tud egy bejelentés törlési kérelmét átállítani regisztrálta
            //így ha véletlen mégis benyomná egy admin, nem fog történni semmi
            if (RoleActions.GetRole(User.Identity.Name) == admin)
            {
                string sikeresTorles = string.Empty;

                for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                {
                    if ((bejelentesekLista.Rows[i].FindControl("StatusDDL") as DropDownList).
                        SelectedValue == TorlesStatus.ElfogadottKerelem.ToString())
                    {
                        //Admin felhaszbáló által kért bejelentést töröl
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = Utility.GetValues(bejelentesekLista.Rows[i]);

                        int id = Convert.ToInt32(rowValues["ID"]);
                        sikeresTorles = JogcimBLL.IdoBejelentesTorles(id,
                                                    TorlesStatus.RegisztraltKerelem.ToString());
                        if (sikeresTorles != string.Empty) break;
                    }
                    else if ((bejelentesekLista.Rows[i].FindControl("StatusDDL") as DropDownList).
                        SelectedValue == TorlesStatus.Torles.ToString())
                    {
                        //mivel adminként bármelyik bejelentést törölhetjük,
                        //nem csak a felhasználó által kért bejelentéseket, ezért van erre a részre szükség
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = Utility.GetValues(bejelentesekLista.Rows[i]);

                        int id = Convert.ToInt32(rowValues["ID"]);

                        sikeresTorles = JogcimBLL.IdoBejelentesTorles(id);
                        if (sikeresTorles != string.Empty) break;
                    }
                    else if ((bejelentesekLista.Rows[i].FindControl("StatusDDL") as DropDownList).
                        SelectedValue == TorlesStatus.Elutasitott.ToString())
                    {
                        //törlési kérelem elutasítása
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = Utility.GetValues(bejelentesekLista.Rows[i]);

                        int id = Convert.ToInt32(rowValues["ID"]);

                        sikeresTorles = JogcimBLL.TorlesElutasitva(id, TorlesStatus.RegisztraltKerelem.ToString(),
                                                    TorlesStatus.NincsTorlesiKerelem.ToString());
                        if (sikeresTorles != string.Empty) break;
                    }

                }
                bejelentesekLista.DataSource = Bejelentes.GetIdoBejelentesek(RoleActions.GetRole(User.Identity.Name),
                                                    User.Identity.Name, DateTime.MinValue, DateTime.MinValue);
                bejelentesekLista.DataBind();
                MegfeleloMezokMegjelenitese(sikeresTorles);
            }
            else if (RoleActions.GetRole(User.Identity.Name) == normal || RoleActions.GetRole(User.Identity.Name) == jovahagy)
            {
                //NormalUser törlésre regisztrál bejelentést
                for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                {
                    if ((bejelentesekLista.Rows[i].FindControl("Remove") as CheckBox).Checked)
                    {
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = Utility.GetValues(bejelentesekLista.Rows[i]);

                        int id = Convert.ToInt32(rowValues["ID"]);

                        JogcimBLL.TorlesRegisztracio(id, TorlesStatus.RegisztraltKerelem.ToString());
                    }
                }
                bejelentesekLista.DataSource = Bejelentes.GetIdoBejelentesek(RoleActions.GetRole(User.Identity.Name),
                                                    User.Identity.Name, DateTime.MinValue, DateTime.MinValue);
                bejelentesekLista.DataBind();
                MegfeleloMezokMegjelenitese(string.Empty);
            }
        }

        private void MegfeleloMezokMegjelenitese(string uzenet)
        {
            //ha újból futattjuk és cookie-val bent vagyunk,
            //akkor a User.IsInRole szerint nem vagyunk bent az adott szerepkörben
            //emiatt az egész alkalmazásban lecseréltem
            if (RoleActions.GetRole(User.Identity.Name) == admin)
            {
                for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                {
                    bejelentesekLista.Rows[i].FindControl("Remove").Visible = false;
                    bejelentesekLista.Rows[i].FindControl("StatusDDL").Visible = true;
                }
                BejelentesTorles.Text = "Mentés";
            }
            else if (RoleActions.GetRole(User.Identity.Name) == normal || RoleActions.GetRole(User.Identity.Name) == jovahagy)
            {
                for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                {
                    bejelentesekLista.Rows[i].FindControl("Remove").Visible = true;
                    bejelentesekLista.Rows[i].FindControl("StatusDDL").Visible = false;
                }
                (bejelentesekLista.HeaderRow.FindControl("DDLTorles") as DropDownList).Visible = false;
                (bejelentesekLista.HeaderRow.FindControl("lasteditFilter") as System.Web.UI.HtmlControls.HtmlInputText).Visible = false;
                (bejelentesekLista.HeaderRow.FindControl("usernameFilter") as System.Web.UI.HtmlControls.HtmlInputText).Visible = false;
                (bejelentesekLista.HeaderRow.FindControl("jogcimFilter") as System.Web.UI.HtmlControls.HtmlInputText).Visible = false;
            }
            if ((bejelentesekLista.HeaderRow.FindControl("DDLTorles") as DropDownList).Items.Count == 0)
            {
                DropDownList ddlTorles = (bejelentesekLista.HeaderRow.FindControl("DDLTorles") as DropDownList);
                ddlTorles.Items.Add(new ListItem());
                ddlTorles.Items.Add(new ListItem() { Text = "Nincs törlési kérelem", Value = TorlesStatus.NincsTorlesiKerelem.ToString() });
                ddlTorles.Items.Add(new ListItem() { Text = "Regisztrált kérelem", Value = TorlesStatus.RegisztraltKerelem.ToString() });
            }

            if (uzenet != string.Empty)
            {
                Master.Uzenet.Visible = true;
                Master.Uzenet.Text = uzenet;
            }
        }

        private void TextBoxFocus(string control)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "tmp2", "var t2 =     document.getElementById('MainContent_bejelentesekLista_" + control + "');" +
            "t2.focus();t2.value = t2.value;", true);
        }

        protected void Vissza_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }
    }
}