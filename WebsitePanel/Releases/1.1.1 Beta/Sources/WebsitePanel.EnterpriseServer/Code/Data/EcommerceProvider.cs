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
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Data.SqlClient;

using Microsoft.ApplicationBlocks.Data;

using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	public class EcommerceProvider
	{
		#region Helper Methods

		private static string ConnectionString
		{
			get
			{
				// get the name of connection string
				return ConfigurationManager.ConnectionStrings["EnterpriseServer"].ConnectionString;
			}
		}

		private static string VerifyColumnName(string str)
		{
			return Regex.Replace(str, @"[^\w\. ]", "");
		}

		private static string VerifyColumnValue(string str)
		{
			return str.Replace("'", "''");
		}

		private static DataSet ExecuteLongDataSet(string spName, params SqlParameter[] parameters)
		{
			return ExecuteLongDataSet(spName, CommandType.StoredProcedure, parameters);
		}

		private static DataSet ExecuteLongQueryDataSet(string spName, params SqlParameter[] parameters)
		{
			return ExecuteLongDataSet(spName, CommandType.Text, parameters);
		}

		private static DataSet ExecuteLongDataSet(string commandText, CommandType commandType, params SqlParameter[] parameters)
		{
			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand cmd = new SqlCommand(commandText, conn);
			cmd.CommandType = commandType;
			cmd.CommandTimeout = 300;

			if (parameters != null)
			{
				foreach (SqlParameter prm in parameters)
				{
					cmd.Parameters.Add(prm);
				}
			}

			DataSet ds = new DataSet();
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				da.Fill(ds);
			}
			finally
			{
				if (conn.State == ConnectionState.Open)
					conn.Close();
			}

			return ds;
		}

		private static void ExecuteLongNonQuery(string spName, params SqlParameter[] parameters)
		{
			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand cmd = new SqlCommand(spName, conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.CommandTimeout = 300;

			if (parameters != null)
			{
				foreach (SqlParameter prm in parameters)
				{
					cmd.Parameters.Add(prm);
				}
			}

			try
			{
				conn.Open();
				cmd.ExecuteNonQuery();
			}
			finally
			{
				if (conn.State == ConnectionState.Open)
					conn.Close();
			}
		}

		private static SqlParameter CreateResultParam()
		{
			SqlParameter param = new SqlParameter();
			param.ParameterName = "@Result";
			param.SqlDbType = SqlDbType.Int;
			param.Direction = ParameterDirection.Output;
			return param;
		}

		public static object GetNull(DateTime value)
		{
			if (value == DateTime.MinValue)
				return DBNull.Value;

			return value;
		}
		#endregion

		#region E-Commerce (Service Handlers)

		public static void AddServiceHandlerTextResponse(string serviceId, string contractId, int invoiceId, string dataReceived)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddServiceHandlerTextResponse",
				new SqlParameter("@ServiceID", serviceId),
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@InvoiceID", invoiceId),
				new SqlParameter("@DataReceived", dataReceived)
			);
		}

		public static void UpdateServiceHandlersResponses(int resellerId, string xmlData)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateServiceHandlersResponses",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@XmlData", xmlData)
			);
		}

		public static IDataReader GetServiceHandlersResponsesByReseller(int resellerId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetServiceHandlersResponsesByReseller",
				new SqlParameter("@ResellerID", resellerId)
			);
		}

		#endregion

		#region E-Commerce (System Triggers)

		public static void AddSystemTrigger (int actorId, int ownerId, string triggerHandler, string referenceId, 
            string triggerNamespace, string status)
        {
	        SqlHelper.ExecuteNonQuery (
		        ConnectionString,
		        CommandType.StoredProcedure,
		        "ecAddSystemTrigger",
		        new SqlParameter("@ActorID", actorId),
		        new SqlParameter("@OwnerID", ownerId),
		        new SqlParameter("@TriggerHandler", triggerHandler),
		        new SqlParameter("@ReferenceID", referenceId),
                new SqlParameter("@Namespace", triggerNamespace),
		        new SqlParameter("@Status", status)
	        );
        }

        public static void DeleteSystemTrigger(int actorId, string triggerId)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecDeleteSystemTrigger",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@TriggerID", triggerId)
            );
        }

        public static void UpdateSystemTrigger (int actorId, string triggerId, string triggerHandler, 
            string referenceId, string triggerNamespace, string status)
        {
	        SqlHelper.ExecuteNonQuery (
		        ConnectionString,
		        CommandType.StoredProcedure,
		        "ecUpdateSystemTrigger",
		        new SqlParameter("@ActorID", actorId),
		        new SqlParameter("@TriggerID", triggerId),
		        new SqlParameter("@TriggerHandler", triggerHandler),
		        new SqlParameter("@ReferenceID", referenceId),
                new SqlParameter("@Namespace", triggerNamespace),
		        new SqlParameter("@Status", status)
	        );
        }

        public static IDataReader GetSystemTrigger (int actorId, string referenceId, string triggerNamespace)
        {
	        return SqlHelper.ExecuteReader (
		        ConnectionString,
		        CommandType.StoredProcedure,
		        "ecGetSystemTrigger",
		        new SqlParameter("@ActorID", actorId),
		        new SqlParameter("@ReferenceID", referenceId),
                new SqlParameter("@Namespace", triggerNamespace)
	        );
        }

        #endregion

        #region E-Commerce (Contracts System)

        public static string AddContract(int customerId, int resellerId, string accountName, int status,
            decimal balance, string firstName, string lastName, string email, string companyName, 
            string propertyNames, string propertyValues)
        {
            SqlParameter outParam = new SqlParameter("@ContractID", SqlDbType.NVarChar);
            outParam.Size = 50;
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecAddContract",
                outParam,
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@ResellerID", resellerId),
                new SqlParameter("@AccountName", accountName),
                new SqlParameter("@Status", status),
                new SqlParameter("@Balance", balance),
                new SqlParameter("@FirstName", firstName),
                new SqlParameter("@LastName", lastName),
                new SqlParameter("@Email", email),
                new SqlParameter("@CompanyName", companyName),
                new SqlParameter("@PropertyNames", propertyNames),
                new SqlParameter("@PropertyValues", propertyValues)
            );

            return Convert.ToString(outParam.Value);
        }

		public static bool CheckCustomerContractExists(int customerId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecCheckCustomerContractExists",
				outParam,
				new SqlParameter("@CustomerID", customerId)
			);

			return Convert.ToBoolean(outParam.Value);
		}

        public static int DeleteContract(string contractId)
        {
            SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecDeleteContract",
                new SqlParameter("@ContractID", contractId),
                outParam
            );

            return Convert.ToInt32(outParam.Value);
        }

        public static IDataReader GetContract(string contractId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecGetContract",
                new SqlParameter("@ContractID", contractId)
            );
        }

        public static IDataReader GetCustomerContract(int customerId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecGetCustomerContract",
                new SqlParameter("@CustomerID", customerId)
            );
        }

        public static int UpdateContract(string contractId, int customerId, string accountName, int status, decimal balance, string firstName, string lastName, string email, string companyName, string propertyNames, string propertyValues)
        {
            SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecUpdateContract",
                new SqlParameter("@ContractID", contractId),
                new SqlParameter("@CustomerID", customerId),
                new SqlParameter("@AccountName", accountName),
                new SqlParameter("@Status", status),
                new SqlParameter("@Balance", balance),
                new SqlParameter("@FirstName", firstName),
                new SqlParameter("@LastName", lastName),
                new SqlParameter("@Email", email),
                new SqlParameter("@CompanyName", companyName),
                new SqlParameter("@PropertyNames", propertyNames),
                new SqlParameter("@PropertyValues", propertyValues),
                outParam
            );

            return Convert.ToInt32(outParam.Value);
        }

        #endregion

		#region E-Commerce (Categories)

		public static int AddCategory(int actorId, int userId, string categoryName, string categorySku, int parentId,
			string shortDescription, string fullDescription)
		{
			SqlParameter result = CreateResultParam();

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddCategory",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CategoryName", categoryName),
				new SqlParameter("@CategorySku", categorySku),
				new SqlParameter("@ParentID", parentId),
				new SqlParameter("@ShortDescription", shortDescription),
				new SqlParameter("@FullDescription", fullDescription),
				result
			);

			return Convert.ToInt32(result.Value);
		}

		public static int DeleteCategory(int actorId, int userId, int categoryId)
		{
			SqlParameter result = CreateResultParam();

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecDeleteCategory",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CategoryID", categoryId),
				result
			);

			return Convert.ToInt32(result.Value);
		}

		public static int GetCategoriesCount(int actorId, int userId, int parentId)
		{
			SqlParameter outParam = new SqlParameter("@Count", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCategoriesCount",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ParentID", parentId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetCategoriesPaged(int actorId, int userId, int parentId, 
			int maximumRows, int startRowIndex)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCategoriesPaged",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ParentID", parentId),
				new SqlParameter("@MaximumRows", maximumRows),
				new SqlParameter("@StartRowIndex", startRowIndex)
			);
		}

		public static IDataReader GetCategory(int actorId, int userId, int categoryId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCategory",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CategoryID", categoryId)
			);
		}

		public static IDataReader GetStorefrontCategories(int resellerId, int parentId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetStorefrontCategories",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@ParentID", parentId)
			);
		}

		public static IDataReader GetStorefrontPath(int resellerId, int categoryId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetStorefrontPath",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@CategoryID", categoryId)
			);
		}

		public static IDataReader GetStorefrontCategory(int resellerId, int categoryId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetStorefrontCategory",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@CategoryID", categoryId)
			);
		}

		public static DataSet GetWholeCategoriesSet(int actorId, int userId)
		{
			return SqlHelper.ExecuteDataset(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetWholeCategoriesSet",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId)
			);
		}

		public static int UpdateCategory(int actorId, int userId, int categoryId, string categoryName, string categorySku, int parentId, string shortDescription, string fullDescription)
		{
			SqlParameter result = CreateResultParam();

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateCategory",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CategoryID", categoryId),
				new SqlParameter("@CategoryName", categoryName),
				new SqlParameter("@CategorySku", categorySku),
				new SqlParameter("@ParentID", parentId),
				new SqlParameter("@ShortDescription", shortDescription),
				new SqlParameter("@FullDescription", fullDescription),
				result
			);

			return Convert.ToInt32(result.Value);
		}

		#endregion

		#region E-Commerce (Invoices)

		public static void VoidCustomerInvoice(int actorId, int invoiceId)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecVoidCustomerInvoice",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@InvoiceID", invoiceId)
			);
		}

		public static int UpdateInvoice(int actorId, int invoiceId, 
			string invoiceNumber, DateTime dueDate, decimal total, decimal subTotal, int taxationId, 
			decimal taxAmount, string currency)
		{
			SqlParameter result = CreateResultParam();

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateInvoice",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@InvoiceID", invoiceId),
				new SqlParameter("@InvoiceNumber", invoiceNumber),
				new SqlParameter("@DueDate", dueDate),
				new SqlParameter("@Total", total),
				new SqlParameter("@SubTotal", subTotal),
				new SqlParameter("@TaxationID", taxationId),
				new SqlParameter("@TaxAmount", taxAmount),
				new SqlParameter("@Currency", currency),
				result
			);

			return Convert.ToInt32(result.Value);
		}
		
		#endregion

		#region E-Commerce (Invoice Items)

		public static IDataReader GetCustomerInvoiceItems(int actorId, int invoiceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomerInvoiceItems",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@InvoiceID", invoiceId)
			);
		}

		public static IDataReader GetInvoicesItemsToActivate(int actorId, int resellerId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetInvoicesItemsToActivate",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ResellerID", resellerId)
			);
		}

		public static IDataReader GetInvoicesItemsOverdue(int actorId, int resellerId, DateTime dueDate)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetInvoicesItemsOverdue",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@DateOverdue", dueDate)
			);
		}

		public static int SetInvoiceItemProcessed(int invoiceId, int itemId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecSetInvoiceItemProcessed",
				new SqlParameter("@InvoiceID", invoiceId),
				new SqlParameter("@ItemID", itemId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		#endregion

		#region E-Commerce (Services)

		public static int SetSvcsUsageRecordsClosed(int actorId, int[] serviceIds)
		{
			string xmlSvcs = Common.Utils.Utils.BuildIdentityXmlFromArray(serviceIds, "records", "record");

			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecSetSvcsUsageRecordsClosed",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@XmlSvcs", xmlSvcs),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static DateTime GetServiceSuspendDate(int actorId, int serviceId)
		{
			SqlParameter outParam = new SqlParameter("@SuspendDate", SqlDbType.DateTime);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetServiceSuspendDate",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId),
				outParam
			);

			return Convert.ToDateTime(outParam.Value);
		}

		public static IDataReader GetServicesToInvoice(int actorId, int resellerId, DateTime todayDate, 
			int daysOffset)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetServicesToInvoice",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@TodayDate", todayDate),
				new SqlParameter("@DaysOffset", daysOffset)
			);
		}

		#endregion

		#region E-Commerce (Billing Cycles)

		public static int AddBillingCycle(int actorId, int userId, string cycleName, string billingPeriod, int periodLength)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddBillingCycle",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CycleName", cycleName),
				new SqlParameter("@BillingPeriod", billingPeriod),
				new SqlParameter("@PeriodLength", periodLength),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int UpdateBillingCycle(int actorId, int userId, int cycleId, string cycleName, string billingPeriod, int periodLength)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateBillingCycle",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CycleID", cycleId),
				new SqlParameter("@CycleName", cycleName),
				new SqlParameter("@BillingPeriod", billingPeriod),
				new SqlParameter("@PeriodLength", periodLength),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int DeleteBillingCycle(int actorId, int userId, int cycleId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecDeleteBillingCycle",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CycleID", cycleId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetBillingCycle(int actorId, int userId, int cycleId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetBillingCycle",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CycleID", cycleId)
			);
		}

		public static int GetBillingCyclesCount(int actorId, int userId)
		{
			SqlParameter outParam = new SqlParameter("@Count", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetBillingCyclesCount",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetBillingCyclesPaged(int actorId, int userId, int maximumRows, int startRowIndex)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetBillingCyclesPaged",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@MaximumRows", maximumRows),
				new SqlParameter("@StartRowIndex", startRowIndex)
			);
		}

		public static IDataReader GetBillingCycles(int actorId, int userId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetBillingCycles",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId)
			);
		}

		public static IDataReader GetBillingCyclesFree(int actorId, int userId, string cyclesTakenXml)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetBillingCyclesFree",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@CyclesTakenXml", cyclesTakenXml)
			);
		}

		public static IDataReader GetHostingPlanCycles(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingPlanCycles",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetHostingAddonCycles(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingAddonCycles",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		#endregion

		#region E-Commerce (Hosting Products)

		public static int AddHostingPlan(int actorId, int userId, string planName, string productSku, bool taxInclusive, int planId,
			int userRole, int initialStatus, int domainOption, bool enabled, string planDescription, string planCyclesXml, string planHighlightsXml, string planCategoriesXml)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddHostingPlan",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@PlanName", planName),
				new SqlParameter("@ProductSku", productSku),
				new SqlParameter("@TaxInclusive", taxInclusive),
				new SqlParameter("@PlanID", planId),
				new SqlParameter("@UserRole", userRole),
				new SqlParameter("@InitialStatus", initialStatus),
				new SqlParameter("@DomainOption", domainOption),
				new SqlParameter("@Enabled", enabled),
				new SqlParameter("@PlanDescription", planDescription),
				new SqlParameter("@PlanCyclesXml", planCyclesXml),
				new SqlParameter("@PlanHighlightsXml", planHighlightsXml),
				new SqlParameter("@PlanCategoriesXml", planCategoriesXml),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int AddTopLevelDomain(int actorId, int userId, string topLevelDomain, string productSku, bool taxInclusive,
			int pluginId, bool enabled, bool whoisEnabled, string domainCyclesXml)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddTopLevelDomain",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@TopLevelDomain", topLevelDomain),
				new SqlParameter("@ProductSku", productSku),
				new SqlParameter("@TaxInclusive", taxInclusive),
				new SqlParameter("@PluginID", pluginId),
				new SqlParameter("@Enabled", enabled),
				new SqlParameter("@WhoisEnabled", whoisEnabled),
				new SqlParameter("@DomainCyclesXml", domainCyclesXml),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int AddHostingAddon(int actorId, int userId, string addonName,
			string productSku, bool taxInclusive, bool enabled, int planId, bool recurring, bool dummyAddon, bool countable, string description,
			string addonCyclesXml, string assignedProductsXml)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddHostingAddon",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@AddonName", addonName),
				new SqlParameter("@ProductSku", productSku),
				new SqlParameter("@TaxInclusive", taxInclusive),
				new SqlParameter("@Enabled", enabled),
				new SqlParameter("@PlanID", planId),
				new SqlParameter("@Recurring", recurring),
				new SqlParameter("@DummyAddon", dummyAddon),
				new SqlParameter("@Countable", countable),
				new SqlParameter("@Description", description),
				new SqlParameter("@AddonCyclesXml", addonCyclesXml),
				new SqlParameter("@AssignedProductsXml", assignedProductsXml),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int UpdateHostingAddon(int actorId, int userId, int productId, string addonName, string productSku, bool taxInclusive,
			bool enabled, int planId, bool recurring, bool dummyAddon, bool countable, string description, string addonCyclesXml, string assignedProductsXml)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateHostingAddon",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@AddonName", addonName),
				new SqlParameter("@ProductSku", productSku),
				new SqlParameter("@TaxInclusive", taxInclusive),
				new SqlParameter("@Enabled", enabled),
				new SqlParameter("@PlanID", planId),
				new SqlParameter("@Recurring", recurring),
				new SqlParameter("@DummyAddon", dummyAddon),
				new SqlParameter("@Countable", countable),
				new SqlParameter("@Description", description),
				new SqlParameter("@AddonCyclesXml", addonCyclesXml),
				new SqlParameter("@AssignedProductsXml", assignedProductsXml),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int GetProductsCountByType(int actorId, int userId, int typeId)
		{
			SqlParameter outParam = new SqlParameter("@Count", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetProductsCountByType",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@TypeID", typeId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetProductsPagedByType(int actorId, int userId, int typeId, int maximumRows, int startRowIndex)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetProductsPagedByType",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@TypeID", typeId),
				new SqlParameter("@MaximumRows", maximumRows),
				new SqlParameter("@StartRowIndex", startRowIndex)
			);
		}

		public static IDataReader GetHostingPlansTaken(int actorId, int userId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingPlansTaken",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId)
			);
		}

		public static IDataReader GetHostingAddonsTaken(int actorId, int userId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingAddonsTaken",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId)
			);
		}

		public static IDataReader GetHostingPlan(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingPlan",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetHostingAddon(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingAddon",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetProductHighlights(int resellerId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetProductHighlights",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetProductCategories(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetProductCategories",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetProductCategoriesIds(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetProductCategoriesIds",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static int UpdateHostingPlan(int actorId, int userId, int productId, string planName, string productSku, bool taxInclusive,
			int planId, int userRole, int initialStatus, int domainOption, bool enabled, string planDescription, string planCyclesXml,
			string planHighlightsXml, string planCategoriesXml)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateHostingPlan",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@PlanName", planName),
				new SqlParameter("@ProductSku", productSku),
				new SqlParameter("@TaxInclusive", taxInclusive),
				new SqlParameter("@PlanID", planId),
				new SqlParameter("@UserRole", userRole),
				new SqlParameter("@InitialStatus", initialStatus),
				new SqlParameter("@DomainOption", domainOption),
				new SqlParameter("@Enabled", enabled),
				new SqlParameter("@PlanDescription", planDescription),
				new SqlParameter("@PlanCyclesXml", planCyclesXml),
				new SqlParameter("@PlanHighlightsXml", planHighlightsXml),
				new SqlParameter("@PlanCategoriesXml", planCategoriesXml),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int UpdateTopLevelDomain(int actorId, int userId, int productId, string topLevelDomain,
			string productSku, bool taxInclusive, int pluginId, bool enabled, bool whoisEnabled, string domainCyclesXml)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateTopLevelDomain",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@TopLevelDomain", topLevelDomain),
				new SqlParameter("@ProductSku", productSku),
				new SqlParameter("@TaxInclusive", taxInclusive),
				new SqlParameter("@PluginID", pluginId),
				new SqlParameter("@Enabled", enabled),
				new SqlParameter("@WhoisEnabled", whoisEnabled),
				new SqlParameter("@DomainCyclesXml", domainCyclesXml),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int DeleteProduct(int actorId, int userId, int productId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecDeleteProduct",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetProductsByType(int userId, int typeId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetProductsByType",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@TypeID", typeId)
			);
		}

		public static IDataReader GetAddonProducts(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetAddonProducts",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetAddonProductsIds(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetAddonProductsIds",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetTopLevelDomainsPaged(int actorId, int userId, int maximumRows, int startRowIndex)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetTopLevelDomainsPaged",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@MaximumRows", maximumRows),
				new SqlParameter("@StartRowIndex", startRowIndex)
			);
		}

		public static IDataReader GetTopLevelDomain(int actorId, int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetTopLevelDomain",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetTopLevelDomainCycles(int userId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetTopLevelDomainCycles",
				new SqlParameter("@UserID", userId),
				new SqlParameter("@ProductID", productId)
			);
		}

		#endregion

		#region Ecommerce v 2.1.0

		public static string GetProductTypeControl(int typeId, string controlKey)
		{
			SqlParameter outParam = new SqlParameter("@ControlSrc", SqlDbType.NVarChar);
			outParam.Size = 512;
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetProductTypeControl",
				new SqlParameter("@TypeID", typeId),
				new SqlParameter("@ControlKey", controlKey),
				outParam
			);

			return Convert.ToString(outParam.Value);
		}

		public static IDataReader GetProductType(int typeId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetProductType",
				new SqlParameter("@TypeID", typeId)
			);
		}

		public static IDataReader GetStorefrontProductsByType(int resellerId, int typeId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetStorefrontProductsByType",
				new SqlParameter("@UserID", resellerId),
				new SqlParameter("@TypeID", typeId)
			);
		}

		public static int AddHostingPlanSvc(string contractId, int productId, string planName, int cycleId, string currency)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddHostingPlanSvc",
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@PlanName", planName),
				new SqlParameter("@CycleID", cycleId),
				new SqlParameter("@Currency", currency),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int AddHostingAddonSvc(string contractId, int parentId, int productId, 
			int quantity, string addonName, int cycleId, string currency)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddHostingAddonSvc",
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@ParentID", parentId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@Quantity", quantity),
				new SqlParameter("@AddonName", addonName),
				new SqlParameter("@CycleID", cycleId),
				new SqlParameter("@Currency", currency),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int AddDomainNameSvc(string contractId, int parentId, int productId, string fqdn,
			int cycleId, string currency, string propertyNames, string propertyValues)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddDomainNameSvc",
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@ParentID", parentId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@FQDN", fqdn),
				new SqlParameter("@CycleID", cycleId),
				new SqlParameter("@Currency", currency),
				new SqlParameter("@PropertyNames", propertyNames),
				new SqlParameter("@PropertyValues", propertyValues),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int ChangeHostingPlanSvcCycle(int actorId, int serviceId, int productId,
			int cycleId, string currency)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecChangeHostingPlanSvcCycle",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@CycleID", cycleId),
				new SqlParameter("@Currency", currency),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int UpdateHostingPlanSvc(int actorId, int serviceId, int productId,
			string planName, int status, int planId, int packageId, int userRole, int initialStatus)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateHostingPlanSvc",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@PlanName", planName),
				new SqlParameter("@Status", status),
				new SqlParameter("@PlanID", planId),
				new SqlParameter("@PackageID", packageId),
				new SqlParameter("@UserRole", userRole),
				new SqlParameter("@InitialStatus", initialStatus),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int UpdateHostingAddonSvc(int actorId, int serviceId, int productId, string addonName, 
			int status, int planId, int packageAddonId, bool recurring, bool dummyAddon)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateHostingAddonSvc",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@AddonName", addonName),
				new SqlParameter("@Status", status),
				new SqlParameter("@PlanID", planId),
				new SqlParameter("@PackageAddonID", packageAddonId),
				new SqlParameter("@Recurring", recurring),
				new SqlParameter("@DummyAddon", dummyAddon),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int UpdateDomainNameSvc(int actorId, int serviceId, int productId, int status, 
			int domainId, string fqdn, string propertyNames, string propertyValues)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateDomainNameSvc",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId),
				new SqlParameter("@ProductID", productId),
				new SqlParameter("@Status", status),
				new SqlParameter("@DomainID", domainId),
				new SqlParameter("@FQDN", fqdn),
				new SqlParameter("@PropertyNames", propertyNames),
				new SqlParameter("@PropertyValues", propertyValues),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int AddInvoice(string contractId, DateTime created, DateTime dueDate, 
			int taxationId, decimal totalAmount, decimal subTotalAmount, decimal taxAmount, 
			string xml, string currency)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddInvoice",
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@Created", created),
				new SqlParameter("@DueDate", dueDate),
				new SqlParameter("@TaxationID", taxationId),
				new SqlParameter("@TotalAmount", totalAmount),
				new SqlParameter("@SubTotalAmount", subTotalAmount),
				new SqlParameter("@TaxAmount", taxAmount),
				new SqlParameter("@Xml", xml),
				new SqlParameter("@Currency", currency),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetCustomerService(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomerService",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId)
			);
		}

		public static IDataReader GetHostingPackageSvc(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingPackageSvc",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId)
			);
		}

		public static IDataReader GetHostingAddonSvc(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingAddonSvc",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId)
			);
		}

		public static IDataReader GetDomainNameSvc(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetDomainNameSvc",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId)
			);
		}

		public static IDataReader GetServiceItemType(int serviceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetServiceItemType",
				new SqlParameter("@ServiceID", serviceId)
			);
		}

		public static IDataReader GetSupportedPluginsByGroup(string groupName)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetSupportedPluginsByGroup",
				new SqlParameter("@GroupName", groupName)
			);
		}

		public static IDataReader GetSupportedPlugin(string pluginName, string groupName)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetSupportedPlugin",
				new SqlParameter("@PluginName", pluginName),
				new SqlParameter("@GroupName", groupName)
			);
		}

		public static IDataReader GetSupportedPluginById(int pluginId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetSupportedPluginByID",
				new SqlParameter("@PluginID", pluginId)
			);
		}

		public static IDataReader GetResellerPaymentMethods(int resellerId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetResellerPaymentMethods",
				new SqlParameter("@ResellerID", resellerId)
			);
		}

		public static IDataReader GetResellerPaymentMethod(int resellerId, string methodName)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetResellerPaymentMethod",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@MethodName", methodName)
			);
		}

		public static IDataReader GetResellerPMPlugin(int resellerId, string methodName)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetResellerPMPlugin",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@MethodName", methodName)
			);
		}

		public static int WriteSupportedPluginLog(string contractId, int pluginId, int recordType, string rawData)
		{
			SqlParameter result = CreateResultParam();

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecWriteSupportedPluginLog",
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@PluginID", pluginId),
				new SqlParameter("@RecordType", recordType),
				new SqlParameter("@RawData", rawData),
				result
			);

			return Convert.ToInt32(result.Value);
		}

		public static int AddCustomerPayment(int actorId, string contractId, int invoiceId, string transactionId, 
			decimal total, string currency, string methodName, int statusId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddCustomerPayment",
                new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@InvoiceID", invoiceId),
				new SqlParameter("@TransactionID", transactionId),
				new SqlParameter("@Total", total),
				new SqlParameter("@Currency", currency),
				new SqlParameter("@MethodName", methodName),
				new SqlParameter("@StatusID", statusId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetPaymentMethod(int actorId, int userId, string methodName)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetPaymentMethod",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@MethodName", methodName)
			);
		}

		public static int SetPluginProperties(int actorId, int userId, int pluginId, string xml)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecSetPluginProperties",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@PluginID", pluginId),
				new SqlParameter("@Xml", xml),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int SetPaymentMethod(int actorId, int userId, string methodName, 
			string displayName, int pluginId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecSetPaymentMethod",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@MethodName", methodName),
				new SqlParameter("@DisplayName", displayName),
				new SqlParameter("@PluginID", pluginId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int DeletePaymentMethod(int actorId, int userId, string methodName)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecDeletePaymentMethod",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@MethodName", methodName),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetPluginProperties(int actorId, int userId, int pluginId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetPluginProperties",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@PluginID", pluginId)
			);
		}

		public static int AddTaxation(int actorId, int userId, string country, string state, 
			string description, int typeId, decimal amount, bool active)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddTaxation",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@Country", country),
				new SqlParameter("@State", state),
				new SqlParameter("@Description", description),
				new SqlParameter("@TypeID", typeId),
				new SqlParameter("@Amount", amount),
				new SqlParameter("@Active", active),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static int DeleteTaxation(int actorId, int userId, int taxationId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecDeleteTaxation",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@TaxationID", taxationId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetTaxation(int actorId, int userId, int taxationId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetTaxation",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@TaxationID", taxationId)
			);
		}

		public static IDataReader GetCustomerTaxation(string contractId, string country, string state)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomerTaxation",
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@Country", country),
				new SqlParameter("@State", state)
			);
		}

		public static int GetTaxationsCount(int actorId, int userId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetTaxationsCount",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetTaxationsPaged(int actorId, int userId, int maximumRows, int startRowIndex)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetTaxationsPaged",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@MaximumRows", maximumRows),
				new SqlParameter("@StartRowIndex", startRowIndex)
			);
		}

		public static int UpdateTaxation(int actorId, int userId, int taxationId, string country, string state, string description, int typeId, decimal amount, bool active)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateTaxation",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@TaxationID", taxationId),
				new SqlParameter("@Country", country),
				new SqlParameter("@State", state),
				new SqlParameter("@Description", description),
				new SqlParameter("@TypeID", typeId),
				new SqlParameter("@Amount", amount),
				new SqlParameter("@Active", active),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static void SetPaymentProfile(int actorId, string contractId, string propertyNames, string propertyValues)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecSetPaymentProfile",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@PropertyNames", propertyNames),
				new SqlParameter("@PropertyValues", propertyValues)
			);
		}

		public static IDataReader GetPaymentProfile(int actorId, string contractId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetPaymentProfile",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ContractID", contractId)
			);
		}

		public static int DeletePaymentProfile(int actorId, string contractId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecDeletePaymentProfile",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ContractID", contractId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static bool PaymentProfileExists(int actorId, string contractId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecPaymentProfileExists",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ContractID", contractId),
				outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

        public static IDataReader GetStoreSettings(int resellerId, string settingsName)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecGetStoreSettings",
                new SqlParameter("@ResellerID", resellerId),
                new SqlParameter("@SettingsName", settingsName)
            );
        }

		public static IDataReader GetStoreDefaultSettings(string settingsName)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetStoreDefaultSettings",
				new SqlParameter("@SettingsName", settingsName)
			);
		}

        public static int SetStoreSettings(int actorId, int userId, string settingsName, string xml)
        {
            SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecSetStoreSettings",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@SettingsName", settingsName),
                new SqlParameter("@Xml", xml),
                outParam
            );

            return Convert.ToInt32(outParam.Value);
        }

		public static int AddServiceUsageRecord(int actorId, int serviceId, int svcCycleId, DateTime startDate, DateTime endDate)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecAddServiceUsageRecord",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId),
				new SqlParameter("@SvcCycleID", svcCycleId),
				new SqlParameter("@StartDate", startDate),
				new SqlParameter("@EndDate", endDate),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static bool IsSupportedPluginActive(int actorId, int resellerId, int pluginId)
		{
			SqlParameter outParam = new SqlParameter("@Active", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecIsSupportedPluginActive",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@PluginID", pluginId),
				outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

		public static IDataReader GetStorefrontProductsInCategory(int resellerId, int categoryId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetStorefrontProductsInCategory",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@CategoryID", categoryId)
			);
		}

		public static IDataReader GetStorefrontProduct(int resellerId, int productId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetStorefrontProduct",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@ProductID", productId)
			);
		}

		public static IDataReader GetStorefrontHostingPlanAddons(int resellerId, int planId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetStorefrontHostingPlanAddons",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@PlanID", planId)
			);
		}

		public static int BulkServiceDelete(int actorId, string contractId, string svcsXml)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecBulkServiceDelete",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ContractID", contractId),
				new SqlParameter("@SvcsXml", svcsXml),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static DateTime GetSvcsSuspendDateAligned(int resellerId, string svcsXml, DateTime defaultValue)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.DateTime);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetSvcsSuspendDateAligned",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@SvcsXml", svcsXml),
				new SqlParameter("@DefaultValue", defaultValue),
				outParam
			);

			return Convert.ToDateTime(outParam.Value);
		}

        public static IDataReader GetPaidInvoices(int actorId, int resellerId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "ecGetUnpaidInvoices",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ResellerID", resellerId)
            );
        }

		public static IDataReader GetUnpaidInvoices(int actorId, int resellerId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetUnpaidInvoices",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ResellerID", resellerId)
			);
		}

		public static int GetCustomersPaymentsCount(int actorId, int userId, bool isReseller)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomersPaymentsCount",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@IsReseller", isReseller),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetCustomersPaymentsPaged(int actorId, int userId, bool isReseller, int maximumRows, int startRowIndex)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomersPaymentsPaged",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@IsReseller", isReseller),
				new SqlParameter("@MaximumRows", maximumRows),
				new SqlParameter("@StartRowIndex", startRowIndex)
			);
		}

		public static int DeleteCustomerPayment(int actorId, int paymentId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecDeleteCustomerPayment",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@PaymentID", paymentId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetCustomerPayment(int actorId, int paymentId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomerPayment",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@PaymentID", paymentId)
			);
		}

		public static int UpdateCustomerPayment(int actorId, int paymentId, int invoiceId, string transactionId, 
			decimal total, string currency, string methodName, int pluginId, int statusId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecUpdateCustomerPayment",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@PaymentID", paymentId),
				new SqlParameter("@InvoiceID", invoiceId),
				new SqlParameter("@TransactionID", transactionId),
				new SqlParameter("@Total", total),
				new SqlParameter("@Currency", currency),
				new SqlParameter("@MethodName", methodName),
				new SqlParameter("@PluginID", pluginId),
				new SqlParameter("@StatusID", statusId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetCustomerInvoice(int actorId, int invoiceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomerInvoice",
                new SqlParameter("@ActorID", actorId),
				new SqlParameter("@InvoiceID", invoiceId)
			);
		}

		public static int GetCustomersInvoicesCount(int actorId, int userId, bool isReseller)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomersInvoicesCount",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@IsReseller", isReseller),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetCustomersInvoicesPaged(int actorId, int userId, bool isReseller, 
			int maximumRows, int startRowIndex)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomersInvoicesPaged",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@IsReseller", isReseller),
				new SqlParameter("@MaximumRows", maximumRows),
				new SqlParameter("@StartRowIndex", startRowIndex)
			);
		}

		public static int GetCustomersServicesCount(int actorId, int userId, bool isReseller)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomersServicesCount",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@IsReseller", isReseller),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader GetCustomersServicesPaged(int actorId, int userId, bool isReseller,
			int maximumRows, int startRowIndex)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetCustomersServicesPaged",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@UserID", userId),
				new SqlParameter("@IsReseller", isReseller),
				new SqlParameter("@MaximumRows", maximumRows),
				new SqlParameter("@StartRowIndex", startRowIndex)
			);
		}

		public static IDataReader GetHostingPackageSvcHistory(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingPackageSvcHistory",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId)
			);
		}

		public static IDataReader GetDomainNameSvcHistory(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetDomainNameSvcHistory",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId)
			);
		}

		public static IDataReader GetHostingAddonSvcHistory(int actorId, int serviceId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetHostingAddonSvcHistory",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId)
			);
		}

		public static int DeleteCustomerService(int actorId, int serviceId)
		{
			SqlParameter outParam = new SqlParameter("@Result", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecDeleteCustomerService",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ServiceID", serviceId),
				outParam
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static IDataReader LookupForTransaction(string transactionId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecLookupForTransaction",
				new SqlParameter("@TransactionID", transactionId)
			);
		}

		public static IDataReader GetResellerTopLevelDomain(int resellerId, string tLD)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"ecGetResellerTopLevelDomain",
				new SqlParameter("@ResellerID", resellerId),
				new SqlParameter("@TLD", tLD)
			);
		}

		#endregion
	}
}