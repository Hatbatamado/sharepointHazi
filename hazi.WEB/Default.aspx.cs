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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (User.Identity.IsAuthenticated)
                {
                    
                    Bejelentesek.Visible = true;
                    bejelentesekLista.DataSource = Bejelentes.GetIdoBejelentesek(RoleActions.GetRole(User.Identity.Name),
                                                    User.Identity.Name, DateTime.MinValue, DateTime.MinValue);
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
                //bejelentesekLista.DataSource = Bejelentes.GetIdoBejelentesByFilerJogcim(jogcimFiler.Value,
                //    RoleActions.GetRole(User.Identity.Name), User.Identity.Name);
                //bejelentesekLista.DataBind();
                //MegfeleloMezokMegjelenitese(string.Empty);
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
                    if ((bejelentesekLista.Rows[i].FindControl("StatusDDL")as DropDownList).
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
                    else if ((bejelentesekLista.Rows[i].FindControl("StatusDDL")as DropDownList).
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
                    else if ((bejelentesekLista.Rows[i].FindControl("StatusDDL")as DropDownList).
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
            else if (RoleActions.GetRole(User.Identity.Name) == normal)
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
            else if (RoleActions.GetRole(User.Identity.Name) == normal)
            {
                for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                {
                    bejelentesekLista.Rows[i].FindControl("Remove").Visible = true;
                    bejelentesekLista.Rows[i].FindControl("StatusDDL").Visible = false;
                }
            }

            if (uzenet != string.Empty)
            {
                Master.Uzenet.Visible = true;
                Master.Uzenet.Text = uzenet;
            }
        }
    }
}