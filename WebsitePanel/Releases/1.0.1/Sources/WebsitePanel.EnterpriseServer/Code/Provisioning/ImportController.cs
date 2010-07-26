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

using WebsitePanel.Providers;

namespace WebsitePanel.EnterpriseServer
{
    public class ImportController
    {
        public static List<ServiceProviderItemType> GetImportableItemTypes(int packageId)
        {
            // load all service item types
            List<ServiceProviderItemType> itemTypes = PackageController.GetServiceItemTypes();

            // load package context
            PackageContext cntx = PackageController.GetPackageContext(packageId);

            // build importable items list
            List<ServiceProviderItemType> importableTypes = new List<ServiceProviderItemType>();
            foreach (ServiceProviderItemType itemType in itemTypes)
            {
                if (!itemType.Importable)
                    continue;

                // load group
                ResourceGroupInfo group = ServerController.GetResourceGroup(itemType.GroupId);
                if (cntx.Groups.ContainsKey(group.GroupName))
                    importableTypes.Add(itemType);
            }
            return importableTypes;
        }

        public static List<string> GetImportableItems(int packageId, int itemTypeId)
        {
            List<string> items = new List<string>();

            // check account
            int accountCheck = SecurityContext.CheckAccount(DemandAccount.IsAdmin | DemandAccount.NotDemo);
            if (accountCheck < 0) return items;

            // load item type
            ServiceProviderItemType itemType = PackageController.GetServiceItemType(itemTypeId);

            // load group
            ResourceGroupInfo group = ServerController.GetResourceGroup(itemType.GroupId);

            // get service id
            int serviceId = PackageController.GetPackageServiceId(packageId, group.GroupName);
            if (serviceId == 0)
                return items;

			DataTable dtServiceItems = PackageController.GetServiceItemsDataSet(serviceId).Tables[0];
			DataTable dtPackageItems = PackageController.GetPackageItemsDataSet(packageId).Tables[0];

            // instantiate controller
            IImportController ctrl = null;
            try
            {
                List<string> importableItems = null;
                ctrl = Activator.CreateInstance(Type.GetType(group.GroupController)) as IImportController;
                if (ctrl != null)
                {
                    importableItems = ctrl.GetImportableItems(packageId, itemTypeId, Type.GetType(itemType.TypeName), group);
                }

                foreach (string importableItem in importableItems)
                {

                    // filter items by service
                    bool serviceContains = false;
                    foreach (DataRow dr in dtServiceItems.Rows)
                    {
                        string serviceItemName = (string)dr["ItemName"];
                        int serviceItemTypeId = (int)dr["ItemTypeId"];

                        if (String.Compare(importableItem, serviceItemName, true) == 0
                            && serviceItemTypeId == itemTypeId)
                        {
                            serviceContains = true;
                            break;
                        }
                    }

                    // filter items by package
                    bool packageContains = false;
                    foreach (DataRow dr in dtPackageItems.Rows)
                    {
                        string packageItemName = (string)dr["ItemName"];
                        int packageItemTypeId = (int)dr["ItemTypeId"];

                        if (String.Compare(importableItem, packageItemName, true) == 0
                            && packageItemTypeId == itemTypeId)
                        {
                            packageContains = true;
                            break;
                        }
                    }

                    if (!serviceContains && !packageContains)
                        items.Add(importableItem);
                }
                
            }
            catch { /* do nothing */ }

            return items;
        }

		public static int ImportItems(bool async, string taskId, int packageId, string[] items)
		{
			// check account
			int accountCheck = SecurityContext.CheckAccount(DemandAccount.NotDemo | DemandAccount.IsAdmin);
			if (accountCheck < 0) return accountCheck;


			if (async)
			{
				ImportAsyncWorker worker = new ImportAsyncWorker();
				worker.threadUserId = SecurityContext.User.UserId;
				worker.taskId = taskId;
				worker.packageId = packageId;
				worker.items = items;

				// import
				worker.ImportAsync();

				return 0;
			}
			else
			{
				return ImportItemsInternal(taskId, packageId, items);
			}
		}

        public static int ImportItemsInternal(string taskId, int packageId, string[] items)
        {
			PackageInfo package = PackageController.GetPackage(packageId);

			TaskManager.StartTask(taskId, "IMPORT", "IMPORT", package.PackageName);
			TaskManager.ItemId = packageId;

			TaskManager.IndicatorMaximum = items.Length;
			TaskManager.IndicatorCurrent = 0;

            Dictionary<int, List<string>> groupedItems = new Dictionary<int, List<string>>();

            // sort by groups
            foreach (string item in items)
            {
                string[] itemParts = item.Split('|');
                int itemTypeId = Utils.ParseInt(itemParts[0], 0);
                string itemName = itemParts[1];

                // add to group
                if (!groupedItems.ContainsKey(itemTypeId))
                    groupedItems[itemTypeId] = new List<string>();

                groupedItems[itemTypeId].Add(itemName);
            }

            // import each group
            foreach (int itemTypeId in groupedItems.Keys)
            {
                // load item type
                ServiceProviderItemType itemType = PackageController.GetServiceItemType(itemTypeId);

                // load group
                ResourceGroupInfo group = ServerController.GetResourceGroup(itemType.GroupId);

                // instantiate controller
                IImportController ctrl = null;
                try
                {
                    ctrl = Activator.CreateInstance(Type.GetType(group.GroupController)) as IImportController;
                    if (ctrl != null)
                    {
						foreach (string itemName in groupedItems[itemTypeId])
						{
							TaskManager.Write(String.Format("Import {0} '{1}'",
								itemType.DisplayName, itemName));

							try
							{
								// perform import
								ctrl.ImportItem(packageId, itemTypeId,
									Type.GetType(itemType.TypeName), group, itemName);
							}
							catch (Exception ex)
							{
								TaskManager.WriteError(ex, "Can't import item");
							}

							TaskManager.IndicatorCurrent++;
						}
                    }
                }
                catch { /* do nothing */ }
            }

			TaskManager.CompleteTask();

            return 0;
        }
    }
}
