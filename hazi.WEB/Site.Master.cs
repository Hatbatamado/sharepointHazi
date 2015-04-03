using hazi.WEB.Logic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace hazi.WEB
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        #region Tulajdonságok
        public Label Uzenet
        {
            get
            {
                return UzenetFelhasznalonak;
            }
            set
            {
                UzenetFelhasznalonak = value;
            }
        }

        public System.Web.UI.HtmlControls.HtmlGenericControl HeaderDiv
        {
            get
            {
                return Header;
            }
            set
            {
                Header = value;
            }
        }

        public Repeater BalMRep
        {
            get
            {
                return BalMenuRepeater;
            }
            set
            {
                BalMenuRepeater = value;
            }
        }

        public Repeater JobbMRep
        {
            get
            {
                return JobbMenuRepeater;
            }
            set
            {
                JobbMenuRepeater = value;
            }
        }

        public System.Web.UI.HtmlControls.HtmlGenericControl LogOffGombDiv
        {
            get
            {
                return logOffButton;
            }
            set
            {
                logOffButton = value;
            }
        }

        public UpdatePanel SiteMasterUpdatePanel
        {
            get
            {
                return MasterMainUpdatePanel;
            }
            set
            {
                MasterMainUpdatePanel = value;
            }
        }
        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    // Set Anti-XSRF token
                    ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                    ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
                }
                else
                {
                    // Validate the Anti-XSRF token
                    if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                        || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                    {
                        //throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                    }
                }
            } catch(Exception)
            {
                Response.Redirect(Konstansok.RedirectAccoutLogin);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HeaderMenu.HeaderMenuBeallitas(logOffButton, BalMenuRepeater, JobbMenuRepeater);
            }
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }
    }

}