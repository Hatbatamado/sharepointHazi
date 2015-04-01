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
                    Frissites();
                }
                else
                {
                    Bejelentesek.Visible = false;
                    Response.Redirect("/Account/Login.aspx");
                }
            }
            if (Request["__EVENTARGUMENT"] == "FilterTextChanged")
            {
                List<UjBejelentes> lista;
                if (DDLSzures.SelectedValue == "Jogcímre")
                {
                    lista = new List<UjBejelentes>();
                    if (FilterBox.Text != "")
                        lista = UjBejelentesBLL.GetIdoBejelentesByFilerJogcim(FilterBox.Text);
                    else
                        lista = ListaAdat(User.Identity.Name);                  
                    GridFeltoles(lista);
                }
                else if (DDLSzures.SelectedValue == "Felhasználóra")
                {
                    if (FilterBox.Text != "")
                        lista = UjBejelentesBLL.GetIdoBejelentesByFilerFelhasznalo(FilterBox.Text);
                    else
                        lista = ListaAdat(User.Identity.Name);
                    GridFeltoles(lista);
                }
                else if (DDLSzures.SelectedValue == "Utolsó módosítóra")
                {
                    if (FilterBox.Text != "")
                        lista = UjBejelentesBLL.GetIdoBejelentesByFilerLastEdit(FilterBox.Text);
                    else
                        lista = ListaAdat(User.Identity.Name);
                    GridFeltoles(lista);
                }
                else if (DDLSzures.SelectedValue == "Státuszra")
                {
                    if (FilterBox.Text != "")
                    {
                        //ékezet eltávolítása, mivel db-ben is úgy van
                        string seged = FilterBox.Text;
                        byte[] temp;
                        temp = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(seged);
                        seged = System.Text.Encoding.UTF8.GetString(temp);
                        lista = UjBejelentesBLL.GetIdoBejelentesByFilerStatus(seged);
                    }                        
                    else
                        lista = ListaAdat(User.Identity.Name);
                    GridFeltoles(lista);
                }
            }
            else if (Request["__EVENTARGUMENT"] == "TabFrissites")
            {
                Frissites();
            }
        }

        /// <summary>
        /// GV feltöltése / újratöltése tab váltásnál
        /// </summary>
        private void Frissites()
        {
            List<UjBejelentes> lista = ListaAdat(User.Identity.Name);

            bejelentesekLista.DataSource = lista;
            bejelentesekLista.DataBind();
            MegfeleloMezokMegjelenitese(string.Empty);
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

        private void GridFeltoles(List<UjBejelentes> lista)
        {
            //null értéknél nem sikerült rájönni, hogyan tűntessem el az oldalról a 2 gombot,
            //így maradt ez az empty datás verzió
            bejelentesekLista.DataSource = lista;
            bejelentesekLista.DataBind();

            if (lista.Count > 0)
            {
                MegfeleloMezokMegjelenitese(string.Empty);
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
                                                    TorlesStatus.BejelentettKerelem.ToString());
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

                        sikeresTorles = IdoBejelentesBLL.TorlesElutasitva(id, TorlesStatus.BejelentettKerelem.ToString(),
                                                    TorlesStatus.Inaktiv.ToString());
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

                        IdoBejelentesBLL.TorlesRegisztracio(id, TorlesStatus.BejelentettKerelem.ToString());
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
                ddlTorles.Items.Add(new ListItem() {
                    Text = TorlesStatus.Inaktiv.ToDisplayString(), Value = TorlesStatus.Inaktiv.ToString() });
                ddlTorles.Items.Add(new ListItem() {
                    Text = TorlesStatus.BejelentettKerelem.ToDisplayString(), Value = TorlesStatus.BejelentettKerelem.ToString() });
            }

            if (uzenet != string.Empty)
            {
                Master.Uzenet.Visible = true;
                Master.Uzenet.Text = uzenet;
            }
        }

        protected void Vissza_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Default.aspx");
        }

        protected void bejelentesekLista_DataBound(object sender, EventArgs e)
        {
            if (bejelentesekLista.Rows.Count > 0 && RoleActions.IsInRole(User.Identity.Name, admin))
            {
                SzuroDiv.Visible = true;
                DDLSzures.Items.Clear(); //újraírjuk az egészet, így nem lesz benne dupla elem
                DDLSzures.Items.Add(new ListItem());
                DDLSzures.Items.Add("Jogcímre");
                DDLSzures.Items.Add("Felhasználóra");
                DDLSzures.Items.Add("Utolsó módosítóra");
                DDLSzures.Items.Add("Státuszra");
            }
            else
                SzuroDiv.Visible = false;
        }

        protected void DDLSzures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as DropDownList).SelectedValue != "")
            {
                Warning.Visible = true;
                Warning.Text = "A magyar dupla karaktereket ki kell írni a megfelelő működéshez!." +
                    "(pl. \"sz\" esetén csak \"s\"-re nem adja ki a találatokat)";
                Warning.ForeColor = System.Drawing.Color.DarkBlue;
            }
            else
                Warning.Visible = false;
        }
    }
}
