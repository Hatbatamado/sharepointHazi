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
        
        hibak hiba = hibak.nincsHiba;
        
        public int? Id
        {
            get
            {
                return (int?)ViewState["Id"];
            }
            set
            {
                ViewState["Id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToInt32(HiddenField.Value) != 0)
                BejelentoForm.Visible = false;
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
            if (Request.QueryString["ID"] != null && Request["__EVENTARGUMENT"] == null)
            {
                Bejelento.DropDownListFeltoltes(DropDownList1);
                Bejelento.AlapAdatokFeltoltese(ora1, ora2, perc1, perc2);

                BejelentoForm.Visible = true;
                Tab.Visible = false;
                TabLinkek.Visible = false;

                Bejelentes.UjBejelentes = false;
                Id = Int32.Parse(Request.QueryString["ID"]);
                IdoBejelentes ib;
                if (RoleActions.GetRole(User.Identity.Name) == RegisterUserAs.Admin.ToString())
                    ib = JogcimBLL.GetIdoBejelentesById(Id.Value, true, string.Empty);
                else
                    ib = JogcimBLL.GetIdoBejelentesById(Id.Value, false, User.Identity.Name);
                if (ib == null)
                {
                    Bejelento.HibaUzenetFelhasznalonak(hibak.IbNincsDBben, new Label());
                    ElemekElrejtese();
                }
                else
                    Bejelento.AdatokFeltolteseIdAlapjan(ib, datepicker, ora1, ora2, perc1, perc2, DropDownList1);
            }
            else if (Request["__EVENTARGUMENT"] == "Mentes")
            {
                //Form létrehozási részbe így nem fut le, de innét nem is hívható meg a mentés, mert validátorok így nem futnak le
                return;
            }
            else if (Request["__EVENTARGUMENT"] == "FilterByJogcim")
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
            else if (RoleActions.GetRole(User.Identity.Name) == normal || RoleActions.GetRole(User.Identity.Name) == jovahagy)
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

        private void ElemekElrejtese()
        {
            Label5.Visible = false;
            datepicker.Visible = false;
            Label1.Visible = false;
            ora1.Visible = false;
            Label6.Visible = false;
            perc1.Visible = false;
            Label2.Visible = false;
            ora2.Visible = false;
            Label7.Visible = false;
            perc2.Visible = false;
            Label3.Visible = false;
            DropDownList1.Visible = false;
            save.Visible = false;

            cancel.Text = "Vissza";
        }

        #region validatorok
        //dátum validátor
        protected void CustomValidatorDatum_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hiba == hibak.nincsHiba)
            {
                DateTime datumKezdeti = new DateTime();
                //Kiválasztott dátum dátum-e
                try
                {
                    datumKezdeti = DateTime.Parse(datepicker.Text);
                    Bejelentes.Kezdeti = datumKezdeti;
                    Bejelentes.Vege = datumKezdeti;
                }
                catch (Exception)
                {
                    hiba = hibak.HibasDatum;
                    args.IsValid = false;
                    return;
                }

                //Kiválasztott dátum régebbi-e a mai dátumnál (csak új bejelentés esetén)
                if (Bejelentes.UjBejelentes && Bejelento.IdoVizsgalat(vizsgalat.CsakDatum, DateTime.Now) >
                    Bejelento.IdoVizsgalat(vizsgalat.CsakDatum, datumKezdeti))
                {
                    hiba = hibak.KezdetiDatumRegebbiMainal;
                    args.IsValid = false;
                }
            }
        }

        //folyamat kezdeti időpont validátor
        protected void CustomValidatorIdopont1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hiba == hibak.nincsHiba)
            {
                string seged;

                //Folyamat kezdeti óra és perc helyes-e
                seged = Bejelento.DateTimeTosringMegfeleloModra(Bejelentes.Kezdeti);
                seged += " " + ora1.SelectedItem.Text + ":" + perc1.SelectedItem.Text + ":00";
                try
                {
                    Bejelentes.Kezdeti = DateTime.Parse(seged);
                }
                catch (Exception)
                {
                    hiba = hibak.HibasKezdetiIdo;
                    args.IsValid = false;
                    return;
                }

                //Folyamat kezdete régebbi-e a mostaninál (csak új bejelentés esetén)
                if (Bejelentes.UjBejelentes && Bejelento.IdoVizsgalat(vizsgalat.MasodpercNulla, DateTime.Now) > Bejelentes.Kezdeti)
                {
                    hiba = hibak.HibasKezdetiErtekek;
                    args.IsValid = false;
                }
            }
        }

        //folyamat vége időpont validátor
        protected void CustomValidatorIdopont2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hiba == hibak.nincsHiba)
            {
                string seged;

                //Folyamat vége óra és perc helyes-e
                seged = Bejelento.DateTimeTosringMegfeleloModra(Bejelentes.Vege);
                seged += " " + ora2.SelectedItem.Text + ":" + perc2.SelectedItem.Text + ":00";
                try
                {
                    Bejelentes.Vege = DateTime.Parse(seged);
                }
                catch (Exception)
                {
                    hiba = hibak.HibasVegeIdo;
                    args.IsValid = false;
                    return;
                }

                //Folyamat vége régebbi-e a mostaninál (csak új bejelentés esetén)
                if (Bejelentes.UjBejelentes && Bejelento.IdoVizsgalat(vizsgalat.MasodpercNulla, DateTime.Now) > Bejelentes.Vege)
                {
                    hiba = hibak.HibasVegeErtekek;
                    args.IsValid = false;
                }
            }
        }
        #endregion

        private void Mentes()
        {
            //ha már volt előzőleg, egy hiba azt rejtsük el
            Master.Uzenet.Visible = false;
            //ha már volt egy sikeres mentésünk és most újabb lesz, azt rejtsük el
            mentesLabel.Visible = false;

            if (Page.IsValid)
            {
                //maradék hiba ellenőrzése
                hiba = Bejelento.Hibakereses();

                //hiba esetén a felhasználó értesítése
                if (hiba != hibak.nincsHiba)
                    Bejelento.HibaUzenetFelhasznalonak(hiba, Master.Uzenet);
                else
                {
                    //db-be mentés
                    if (Bejelentes.Kezdeti != null && Bejelentes.Vege != null)
                    {
                        Bejelento.Mentes(Id, DropDownList1, Master.Uzenet, mentesLabel);
                    }
                    //ennek sose szabadna lefutnia
                    else
                        Bejelento.HibaUzenetFelhasznalonak(hibak.Ismeretlen, Master.Uzenet);
                }
            }
            else
                Bejelento.HibaUzenetFelhasznalonak(hiba, Master.Uzenet);
        }

        protected void save_Click(object sender, EventArgs e)
        {
            Mentes();
            ElemekElrejtese();
            TabLinkek.Visible = true;
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }
    }
}