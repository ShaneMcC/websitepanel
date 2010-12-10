using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

namespace WebsitePanel.Portal.ProviderControls
{
    public partial class Exchange2010SP1_Settings : WebsitePanelControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindSettings(StringDictionary settings)
        {
            programID.Text = settings["programID"];
            offerID.Text = settings["offerID"];
            //location.Text = settings["location"];

            //catchAllPath.Text = settings["catchAllPath"];

            temporaryDomain.Text = settings["temporaryDomain"];
            ecpURL.Text = settings["ecpURL"];
        }

        public void SaveSettings(StringDictionary settings)
        {
            settings["programID"] = programID.Text.Trim();
            settings["offerID"] = offerID.Text.Trim();
            //settings["location"] = location.Text.Trim();

            //settings["catchAllPath"] = catchAllPath.Text.Trim();

            settings["temporaryDomain"] = temporaryDomain.Text.Trim();
            settings["ecpURL"] = ecpURL.Text.Trim();
        }
    }
}