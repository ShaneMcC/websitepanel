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
using System.Collections.Generic;
using System.Data;

using ES = WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class CategoryController
	{
		#region Category routines

		public static DataSet GetWholeCategoriesSet(int userId)
		{
			return EcommerceProvider.GetWholeCategoriesSet(
				ES.SecurityContext.User.UserId,
				userId
			);
		}

		public static int GetCategoriesCount(int userId, int parentId)
		{
			return EcommerceProvider.GetCategoriesCount(
				ES.SecurityContext.User.UserId,
				userId,
				parentId
			);
		}

		public static List<Category> GetCategoriesPaged(int userId, int parentId, int maximumRows, int startRowIndex)
		{
			return ES.ObjectUtils.CreateListFromDataReader<Category>(
				EcommerceProvider.GetCategoriesPaged(
					ES.SecurityContext.User.UserId,
					userId,
					parentId,
					maximumRows,
					startRowIndex
				)
			);
		}

		public static int AddCategory(int userId, string categoryName, string categorySku, int parentId, string shortDescription, string fullDescription)
		{
			SecurityResult result = StorehouseController.CheckAccountNotDemoAndActive();
			//
			if (!result.Success)
				return result.ResultCode;
			//
			return EcommerceProvider.AddCategory(
				ES.SecurityContext.User.UserId,
				userId,
				categoryName,
				categorySku,
				parentId,
				shortDescription,
				fullDescription
			);
		}

		public static Category GetCategory(int userId, int categoryId)
		{
			return ES.ObjectUtils.FillObjectFromDataReader<Category>(
				EcommerceProvider.GetCategory(
					ES.SecurityContext.User.UserId,
					userId,
					categoryId
				)
			);
		}

		public static int UpdateCategory(int userId, int categoryId, string categoryName, string categorySku, int parentId, string shortDescription, string fullDescription)
		{
			SecurityResult result = StorehouseController.CheckAccountNotDemoAndActive();
			//
			if (!result.Success)
				return result.ResultCode;

			return EcommerceProvider.UpdateCategory(
				ES.SecurityContext.User.UserId,
				userId,
				categoryId,
				categoryName,
				categorySku,
				parentId,
				shortDescription,
				fullDescription
			);
		}

		public static int DeleteCategory(int userId, int categoryId)
		{
			SecurityResult result = StorehouseController.CheckAccountNotDemoAndActive();
			//
			if (!result.Success)
				return result.ResultCode;

			return EcommerceProvider.DeleteCategory(
				ES.SecurityContext.User.UserId,
				userId,
				categoryId
			);
		}

		#endregion
	}
}
