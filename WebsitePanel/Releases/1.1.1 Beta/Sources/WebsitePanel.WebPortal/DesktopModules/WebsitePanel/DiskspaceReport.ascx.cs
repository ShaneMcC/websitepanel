// Copyright (c) 2010, SMB SAAS Systems Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Security.Application;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal
{
    public partial class DiskspaceReport : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            // set display preferences
            gvReport.PageSize = UsersHelper.GetDisplayItemsPerPage();        
        }

        protected void odsReport_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Row.DataItem;
            if (dr != null)
            {
                if ((int)dr["UsagePercentage"] > 100)
                    e.Row.CssClass = "NormalBold";
            }
        }

        public string GetAllocatedValue(int val)
        {
            return (val == -1) ? GetLocalizedString("Unlimited.Text") : val.ToString();
        }

        private void ExportReport()
        {
            // build HTML
            DataTable dtRecords = new ReportsHelper().GetPackagesDiskspacePaged(-1, Int32.MaxValue, 0, "");

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<title>Disk Space Report").Append("</title>");
            sb.Append("</head>");
            sb.Append("<body><table border=\"1\">");

            sb.Append("<tr>");
            sb.Append("<th>Hosting Space</th>");
            sb.Append("<th>Allocated, MB</th>");
            sb.Append("<th>Used, MB</th>");
            sb.Append("<th>Usage, %</th>");
            sb.Append("<th>Username</th>");
            sb.Append("<th>User E-mail</th>");
            sb.Append("</tr>");

            foreach (DataRow dr in dtRecords.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td>").Append(dr["PackageName"]).Append("</td>");
                sb.Append("<td>").Append(dr["QuotaValue"]).Append("</td>");
                sb.Append("<td>").Append(dr["Diskspace"]).Append("</td>");
                sb.Append("<td>").Append(dr["UsagePercentage"]).Append("</td>");
                sb.Append("<td>").Append(dr["Username"]).Append("</td>");
                sb.Append("<td>").Append(dr["Email"]).Append("</td>");
                sb.Append("</tr>");
            }

            sb.Append("</table></body>");
            sb.Append("</html>");

            string fileName = "DiskspaceReport.htm";

            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.ContentType = "application/octet-stream";

            Response.Write(sb.ToString());

            Response.End();
        }

        protected void btnExportReport_Click(object sender, EventArgs e)
        {
            ExportReport();
        }
    }
}