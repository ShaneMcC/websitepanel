// Material sourced from the bluePortal project (http://blueportal.codeplex.com).
// Licensed under the Microsoft Public License (available at http://www.opensource.org/licenses/ms-pl.html).

using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CSSFriendly
{
    public class LoginStatusAdapter : System.Web.UI.WebControls.Adapters.WebControlAdapter
    {
        private WebControlAdapterExtender _extender = null;
        private WebControlAdapterExtender Extender
        {
            get
            {
                if (((_extender == null) && (Control != null)) ||
                    ((_extender != null) && (Control != _extender.AdaptedControl)))
                {
                    _extender = new WebControlAdapterExtender(Control);
                }

                System.Diagnostics.Debug.Assert(_extender != null, "CSS Friendly adapters internal error", "Null extender instance");
                return _extender;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Extender.AdapterEnabled)
            {
                RegisterScripts();
            }
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                //  The LoginStatus is very simple INPUT or A tag so we don't wrap it with an being/end tag (e.g., no DIV wraps it).
            }
            else
            {
                base.RenderBeginTag(writer);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                //  The LoginStatus is very simple INPUT or A tag so we don't wrap it with an being/end tag (e.g., no DIV wraps it).
            }
            else
            {
                base.RenderEndTag(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (Extender.AdapterEnabled)
            {
                LoginStatus loginStatus = Control as LoginStatus;
                if (loginStatus != null)
                {
                    string className = (!String.IsNullOrEmpty(loginStatus.CssClass)) ? ("AspNet-LoginStatus " + loginStatus.CssClass) : "AspNet-LoginStatus";

                    if (Membership.GetUser() == null)
                    {
                        if (!String.IsNullOrEmpty(loginStatus.LoginImageUrl))
                        {
                            Control ctl = loginStatus.FindControl("ctl03");
                            if (ctl != null)
                            {
                                writer.WriteBeginTag("input");
                                writer.WriteAttribute("id", loginStatus.ClientID);
                                writer.WriteAttribute("type", "image");
                                writer.WriteAttribute("name", ctl.UniqueID);
                                writer.WriteAttribute("title", loginStatus.ToolTip);
                                writer.WriteAttribute("class", className);
                                writer.WriteAttribute("src", loginStatus.ResolveClientUrl(loginStatus.LoginImageUrl));
                                writer.WriteAttribute("alt", loginStatus.LoginText);
                                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                                Page.ClientScript.RegisterForEventValidation(ctl.UniqueID);
                            }
                        } 
                        else
                        {
                            Control ctl = loginStatus.FindControl("ctl02");
                            if (ctl != null)
                            {
                                writer.WriteBeginTag("a");
                                writer.WriteAttribute("id", loginStatus.ClientID);
                                writer.WriteAttribute("title", loginStatus.ToolTip);
                                writer.WriteAttribute("class", className);
                                writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(loginStatus.FindControl("ctl02"), ""));
                                writer.Write(HtmlTextWriter.TagRightChar);
                                writer.Write(loginStatus.LoginText);
                                writer.WriteEndTag("a");
                                Page.ClientScript.RegisterForEventValidation(ctl.UniqueID);
                            }
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(loginStatus.LogoutImageUrl))
                        {
                            Control ctl = loginStatus.FindControl("ctl01");
                            if (ctl != null)
                            {
                                writer.WriteBeginTag("input");
                                writer.WriteAttribute("id", loginStatus.ClientID);
                                writer.WriteAttribute("type", "image");
                                writer.WriteAttribute("name", ctl.UniqueID);
                                writer.WriteAttribute("title", loginStatus.ToolTip);
                                writer.WriteAttribute("class", className);
                                writer.WriteAttribute("src", loginStatus.ResolveClientUrl(loginStatus.LogoutImageUrl));
                                writer.WriteAttribute("alt", loginStatus.LogoutText);
                                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                                Page.ClientScript.RegisterForEventValidation(ctl.UniqueID);
                            }
                        }
                        else
                        {
                            Control ctl = loginStatus.FindControl("ctl00");
                            if (ctl != null)
                            {
                                writer.WriteBeginTag("a");
                                writer.WriteAttribute("id", loginStatus.ClientID);
                                writer.WriteAttribute("title", loginStatus.ToolTip);
                                writer.WriteAttribute("class", className);
                                writer.WriteAttribute("href", Page.ClientScript.GetPostBackClientHyperlink(loginStatus.FindControl("ctl00"), ""));
                                writer.Write(HtmlTextWriter.TagRightChar);
                                writer.Write(loginStatus.LogoutText);
                                writer.WriteEndTag("a");
                                Page.ClientScript.RegisterForEventValidation(ctl.UniqueID);
                            }
                        }
                    }
                }
            }
            else
            {
                base.RenderContents(writer);
            }
        }

        /// ///////////////////////////////////////////////////////////////////////////////
        /// PRIVATE        

        private void RegisterScripts()
        {
        }
    }
}
