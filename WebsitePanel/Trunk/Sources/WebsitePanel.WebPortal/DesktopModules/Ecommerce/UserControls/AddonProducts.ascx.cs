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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal.DesktopModules.Ecommerce.UserControls
{
	public partial class AddonProducts : ecControlBase
	{
		private int[] assignedProducts;
		private bool dataSourceBinded;

		public const string VIEW_STATE_KEY = "__AddonProducts";

		public int[] AssignedProducts
		{
			get
			{
				return SyncDataListSelection();
			}
			set
			{
				SyncProductsSelection(value);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				BindAvailableProducts();
		}

		private void dlProductAddons_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.AlternatingItem:
				case ListItemType.EditItem:
				case ListItemType.SelectedItem:
				case ListItemType.Item:
					Product dataItem = (Product)e.Item.DataItem;

					if (dataItem != null)
					{
						CheckBox chkBox = ecUtils.FindControl<CheckBox>(e.Item, "chkSelected");
						//
						chkBox.Checked = (Array.IndexOf(assignedProducts, dataItem.ProductId) > -1);
					}
					break;
			}
		}

		private int[] SyncDataListSelection()
		{
			List<int> bindIds = new List<int>();

			foreach (DataListItem item in dlProductAddons.Items)
			{
				// obtain checkbox
				CheckBox chkBox = ecUtils.FindControl<CheckBox>(item, "chkSelected");

				if (chkBox != null)
				{
					// user uncheck assigned category - we collect it.
					if (chkBox.Checked)
					{
						bindIds.Add((int)dlProductAddons.DataKeys[item.ItemIndex]);
						continue;
					}
				}
			}

			return bindIds.ToArray();
		}

		private void SyncProductsSelection(int[] value)
		{
			//
			this.assignedProducts = value;
			//
			dlProductAddons.ItemDataBound += new DataListItemEventHandler(dlProductAddons_ItemDataBound);
			//
			if (!dataSourceBinded)
				BindAvailableProducts();
		}

		private void BindAvailableProducts()
		{
			dlProductAddons.DataSource = StorehouseHelper.GetProductsByType(1);
			dlProductAddons.DataBind();

			dataSourceBinded = true;
		}
	}
}