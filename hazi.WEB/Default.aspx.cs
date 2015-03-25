﻿using hazi.BLL;
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
                    List<UjBejelentes> lista = ListaAdat(User.Identity.Name);                  

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
                List<UjBejelentes> lista = UjBejelentesBLL.GetIdoBejelentesByFilerJogcim(text);

                GridFeltoles(lista, text, "jogcimFilter");
            }
            else if (Request["__EVENTARGUMENT"] == "FilterByFelhasznalo")
            {
                string text = (bejelentesekLista.HeaderRow.FindControl("usernameFilter") as System.Web.UI.HtmlControls.HtmlInputText).Value;
                List<UjBejelentes> lista;
                if (text != "")
                    lista = UjBejelentesBLL.GetIdoBejelentesByFilerFelhasznalo(text);
                else //ha még van a db-ben olyan bejelentés, aminek nincs "gazdája"
                    lista = ListaAdat(User.Identity.Name);

                GridFeltoles(lista, text, "usernameFilter");
            }
            else if (Request["__EVENTARGUMENT"] == "FilterByLastedit")
            {
                string text = (bejelentesekLista.HeaderRow.FindControl("lasteditFilter") as System.Web.UI.HtmlControls.HtmlInputText).Value;
                List<UjBejelentes> lista;
                if (text != "")
                    lista = UjBejelentesBLL.GetIdoBejelentesByFilerLastEdit(text);
                else //ha még van a db-ben olyan bejelentés, aminek létrehozásakor és utána nem kapott értéket lastedit-re
                    lista = ListaAdat(User.Identity.Name);

                GridFeltoles(lista, text, "lasteditFilter");
            }
            else if (Request["__EVENTARGUMENT"] == "FilterByTorlesStatus")
            {
                string text = (bejelentesekLista.HeaderRow.FindControl("DDLTorles") as DropDownList).SelectedItem.Value;

                List<UjBejelentes> lista;
                if (text != "")
                    lista = UjBejelentesBLL.GetIdoBejelentesByFilerTorlesStatus(text);
                else //ha az összes jelentést látni akarjuk
                    lista = ListaAdat(User.Identity.Name);

                GridFeltoles(lista, text, "DDLTorles");
            }
        }

        private List<UjBejelentes> ListaAdat(string UName)
        {
            List<UjBejelentes> lista = new List<UjBejelentes>();
            if (RoleActions.IsInRole(UName, admin))
                lista = UjBejelentesBLL.GetIdoBejelentesek(admin, UName, DateTime.MinValue, DateTime.MinValue);
            else if (RoleActions.IsInRole(UName, normal))
                lista = UjBejelentesBLL.GetIdoBejelentesek(normal, UName, DateTime.MinValue, DateTime.MinValue);
            else if (RoleActions.IsInRole(UName, jovahagy))
                lista = UjBejelentesBLL.GetIdoBejelentesek(jovahagy, UName, DateTime.MinValue, DateTime.MinValue);

            return lista;
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
            if (RoleActions.IsInRole(User.Identity.Name, admin))
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
                        sikeresTorles = IdoBejelentesBLL.IdoBejelentesTorles(id,
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

                        sikeresTorles = IdoBejelentesBLL.IdoBejelentesTorles(id);
                        if (sikeresTorles != string.Empty) break;
                    }
                    else if ((bejelentesekLista.Rows[i].FindControl("StatusDDL") as DropDownList).
                        SelectedValue == TorlesStatus.Elutasitott.ToString())
                    {
                        //törlési kérelem elutasítása
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = Utility.GetValues(bejelentesekLista.Rows[i]);

                        int id = Convert.ToInt32(rowValues["ID"]);

                        sikeresTorles = IdoBejelentesBLL.TorlesElutasitva(id, TorlesStatus.RegisztraltKerelem.ToString(),
                                                    TorlesStatus.NincsTorlesiKerelem.ToString());
                        if (sikeresTorles != string.Empty) break;
                    }

                }
                bejelentesekLista.DataSource = ListaAdat(User.Identity.Name);
                bejelentesekLista.DataBind();
                MegfeleloMezokMegjelenitese(sikeresTorles);
            }
            else if (RoleActions.IsInRole(User.Identity.Name, normal) || RoleActions.IsInRole(User.Identity.Name, jovahagy))
            {
                //NormalUser törlésre regisztrál bejelentést
                for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                {
                    if ((bejelentesekLista.Rows[i].FindControl("Remove") as CheckBox).Checked)
                    {
                        IOrderedDictionary rowValues = new OrderedDictionary();
                        rowValues = Utility.GetValues(bejelentesekLista.Rows[i]);

                        int id = Convert.ToInt32(rowValues["ID"]);

                        IdoBejelentesBLL.TorlesRegisztracio(id, TorlesStatus.RegisztraltKerelem.ToString());
                    }
                }
                bejelentesekLista.DataSource = ListaAdat(User.Identity.Name);
                bejelentesekLista.DataBind();
                MegfeleloMezokMegjelenitese(string.Empty);
            }
        }

        private void MegfeleloMezokMegjelenitese(string uzenet)
        {
            //ha újból futattjuk és cookie-val bent vagyunk,
            //akkor a User.IsInRole szerint nem vagyunk bent az adott szerepkörben
            //emiatt az egész alkalmazásban lecseréltem
            if (RoleActions.IsInRole(User.Identity.Name, admin))
            {
                if (bejelentesekLista.Rows.Count > 0)
                {
                    for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                    {
                        bejelentesekLista.Rows[i].FindControl("Remove").Visible = false;
                        bejelentesekLista.Rows[i].FindControl("StatusDDL").Visible = true;
                    }
                    BejelentesTorles.Text = "Mentés";
                }
                else
                    BejelentesTorles.Visible = false;
            }
            else if (RoleActions.IsInRole(User.Identity.Name, normal) || RoleActions.IsInRole(User.Identity.Name, jovahagy))
            {
                if (bejelentesekLista.HeaderRow != null)
                {
                    if (bejelentesekLista.Rows.Count != 0)
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

                        BejelentesTorles.Visible = true;
                    }
                    else
                        BejelentesTorles.Visible = false;
                }
                else
                    BejelentesTorles.Visible = false;
            }
            Control DDLTorles = null;
            try
            {
                DDLTorles = bejelentesekLista.HeaderRow.FindControl("DDLTorles");
            }
            catch (NullReferenceException) { }
            if (DDLTorles != null && (bejelentesekLista.HeaderRow.FindControl("DDLTorles") as DropDownList).Items.Count == 0)
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
