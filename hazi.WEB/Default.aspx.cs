using hazi.BLL;
using hazi.DAL;
using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                    AdatokUjraToltese(string.Empty);                                       
                }
                else
                {
                    Bejelentesek.Visible = false;
                    Response.Redirect("/Account/Login.aspx");
                }
            }
        }

        public List<UjBejelentes> GetIdoBejelentesek()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (RoleActions.GetRole(User.Identity.Name) == admin)
                {
                    return Bejelentes.GetIdoBejelentesek(admin,"",
                        DateTime.MinValue, DateTime.MinValue);
                }
                else if (RoleActions.GetRole(User.Identity.Name) == normal)
                {
                    return Bejelentes.GetIdoBejelentesek(normal,
                        User.Identity.Name, DateTime.MinValue, DateTime.MinValue);
                }
            }
            return null;
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
                //Admin felhaszbáló által kért bejelentést töröl
                for (int i = 0; i < bejelentesekLista.Rows.Count; i++)
                {
                    if ((bejelentesekLista.Rows[i].FindControl("StatusDDL")as DropDownList).
                        SelectedValue == TorlesStatus.ElfogadottKerelem.ToString())
                    {
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
                } 
                AdatokUjraToltese(sikeresTorles);
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
                        DateTime kezdeti = DateTime.Parse(rowValues["KezdetiDatum"].ToString());
                        DateTime vege = DateTime.Parse(rowValues["VegeDatum"].ToString());
                        int jogid = JogcimBLL.GetIDbyName(rowValues["JogcimNev"].ToString());
                        string username = rowValues["UserName"].ToString();
                        
                        string lastedit = string.Empty;
                        if (rowValues["LastEdit"] != null)
                            lastedit = rowValues["LastEdit"].ToString();

                        JogcimBLL.IdoBejelentesMentes(id, kezdeti, vege, jogid, username, lastedit,
                            TorlesStatus.RegisztraltKerelem.ToString());
                    }
                }
                AdatokUjraToltese(string.Empty);
            }
        }

        private void AdatokUjraToltese(string uzenet)
        {
            bejelentesekLista.DataSource = GetIdoBejelentesek();
            bejelentesekLista.DataBind();

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