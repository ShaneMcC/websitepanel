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
using System.Collections.Generic;
using System.Text;

using WebsitePanel.Portal;
using WebsitePanel.Ecommerce.EnterpriseServer;

namespace WebsitePanel.Ecommerce.Portal
{
	public class CategoryHelper
	{
		public static DataSet GetWholeCategoriesSet()
		{
			return EC.Services.Storehouse.GetWholeCategoriesSet(
				PanelSecurity.SelectedUserId
			);
		}

		public static int GetCategoriesCount()
		{
            return EC.Services.Storehouse.GetCategoriesCount(
				PanelSecurity.SelectedUserId,
				ecPanelRequest.CategoryId
			);
		}

		public static Category[] GetCategoriesPaged(int maximumRows, int startRowIndex)
		{
            return EC.Services.Storehouse.GetCategoriesPaged(
				PanelSecurity.SelectedUserId,
				ecPanelRequest.CategoryId,
				maximumRows,
				startRowIndex
			);
		}

		public static int AddCategory(string categoryName, string categorySku, 
			int parentId, string shortDescription, string fullDescription)
		{
            return EC.Services.Storehouse.AddCategory(
				PanelSecurity.SelectedUserId,
				categoryName,
				categorySku,
				parentId,
				shortDescription,
				fullDescription
			);
		}

		public static int DeleteCategory(int categoryId)
		{
            return EC.Services.Storehouse.DeleteCategory(
				PanelSecurity.SelectedUserId,
				categoryId
			);
		}

		public static int UpdateCategory(int categoryId, string categoryName, 
			string categorySku, int parentId, string shortDescription, string fullDescription)
		{
            return EC.Services.Storehouse.UpdateCategory(
				PanelSecurity.SelectedUserId,
				categoryId,
				categoryName,
				categorySku,
				parentId,
				shortDescription,
				fullDescription
			);
		}

		public static Category GetCategory(int categoryId)
		{
            return EC.Services.Storehouse.GetCategory(
				PanelSecurity.SelectedUserId,
				categoryId
			);
		}

		public static void BuildCategoriesIndent(DataView dv)
		{
			string levelIdent = "...";

			// create visual identation
			foreach (DataRow row in dv.Table.Rows)
			{
				int level = (int)row["Level"];
				StringBuilder text = new StringBuilder((string)row["CategoryName"]);
				text.Insert(0, levelIdent, level);

				row["CategoryName"] = text.ToString();
			}
		}

		public static DataView BuildCategoriesIndent(DataSet ds)
		{
			DataView dv = GetCategoriesTreeView(ds);
			BuildCategoriesIndent(dv);
			return dv;
		}

		public static DataView GetCategoriesTreeView(DataSet ds)
		{
			return GetCategoriesTreeView(ds.Tables[0]);
		}

		public static DataView GetCategoriesTreeView(DataView dv)
		{
			return GetCategoriesTreeView(dv.Table);
		}

		public static DataView GetCategoriesTreeView(DataTable dt)
		{
			CategoryTreeViewSorting cts = new CategoryTreeViewSorting(dt);

			cts.Sort();

			return cts.SortedView;
		}
	}
}

public class CategoryTreeViewSorting
{
	private DataTable _originalDt;
	private DataTable _resultDt;
	private bool _sorted;

	public DataView SortedView
	{
		get
		{
			if (!_sorted)
				Sort();
			return _resultDt.DefaultView;
		}
	}

	public DataTable SortedTable
	{
		get
		{
			if (!_sorted)
				Sort();
			return _resultDt;
		}
	}

	public CategoryTreeViewSorting(DataView dv)
		: this(dv.Table)
	{
	}

	public CategoryTreeViewSorting(DataSet ds)
		: this(ds.Tables[0])
	{
	}

	public CategoryTreeViewSorting(DataTable dt)
	{
		_originalDt = dt;
		// copy table schema to the result table
		_resultDt = _originalDt.Clone();
		_sorted = false;
	}

	public void Sort()
	{
		_originalDt.DefaultView.Sort = "ParentID,CategoryName";
		DataRelation r1 = new DataRelation("r1", _originalDt.Columns["CategoryID"], _originalDt.Columns["ParentID"]);
		_originalDt.ChildRelations.Add(r1);

		DataRow[] rootRows = _originalDt.Select("ISNULL(ParentID, 0) = 0");

		foreach (DataRow row in rootRows)
			Copy(row);

		_sorted = true;
	}

	private void Copy(DataRow leaf)
	{
		_resultDt.ImportRow(leaf);

		DataRow[] childs = leaf.GetChildRows("r1");

		foreach (DataRow child in childs)
		{
			DataRow[] subChilds = child.GetChildRows("r1");

			if (subChilds.Length > 0)
				Copy(child);
			else
				_resultDt.ImportRow(child);
		}
	}
}