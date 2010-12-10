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

using WebsitePanel.Portal;

namespace WebsitePanel.Ecommerce.Portal.UserControls
{
	public partial class ProductCategories : ecControlBase
	{
		private bool dataSourceBinded;
		private int[] assignedCategories;

		public int[] AssignedCategories
		{
			get
			{
				return SyncDataListSelection();
			}
			set 
			{
				SyncProductSelection(value);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
				BindAvailableCategories();
		}

		private void _dlCategories_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.AlternatingItem:
				case ListItemType.EditItem:
				case ListItemType.SelectedItem:
				case ListItemType.Item:
					DataRowView drView = (DataRowView)e.Item.DataItem;

					if (drView != null)
					{
						CheckBox chkBox = ecUtils.FindControl<CheckBox>(e.Item, "chkSelected");
						// lookup category in selection
						chkBox.Checked = (Array.IndexOf(assignedCategories, drView["CategoryID"]) > -1);
					}
					break;
			}
		}

		private int[] SyncDataListSelection()
		{
			List<int> bindIds = new List<int>();

			foreach (DataListItem item in _dlCategories.Items)
			{
				// obtain checkbox
				CheckBox chkBox = ecUtils.FindControl<CheckBox>(item, "chkSelected");

				if (chkBox != null)
				{
					// user uncheck assigned category - we collect it.
					if (chkBox.Checked)
					{
						bindIds.Add((int)_dlCategories.DataKeys[item.ItemIndex]);
						continue;
					}
				}
			}

			return bindIds.ToArray();
		}

		private void SyncProductSelection(int[] assignedCats)
		{
			// store categories
			this.assignedCategories = assignedCats;
			// DataList event hadlers
			_dlCategories.ItemDataBound += new DataListItemEventHandler(_dlCategories_ItemDataBound);
			// bind categories workaround for data sync
			if (!dataSourceBinded)
				BindAvailableCategories();
		}

		private void BindAvailableCategories()
		{
			_dlCategories.DataSource = CategoryHelper.GetWholeCategoriesSet();
			_dlCategories.DataBind();
			dataSourceBinded = true;
		}
	}
}