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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WebsitePanel.Providers.HostedSolution;
using Microsoft.ApplicationBlocks.Data;

namespace WebsitePanel.EnterpriseServer
{
    /// <summary>
    /// Summary description for DataProvider.
    /// </summary>
    public static class DataProvider
    {
        private static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["EnterpriseServer"].ConnectionString;
            }
        }

        private static string ObjectQualifier
        {
            get
            {
                return "";
            }
		}

		#region System Settings

		public static IDataReader GetSystemSettings(string settingsName)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"GetSystemSettings",
				new SqlParameter("@SettingsName", settingsName)
			);
		}

		public static void SetSystemSettings(string settingsName, string xml)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"SetSystemSettings",
				new SqlParameter("@SettingsName", settingsName),
				new SqlParameter("@Xml", xml)
			);
		}

		#endregion

		#region Users
		public static bool CheckUserExists(string username)
        {
            SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
            prmExists.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckUserExists",
                prmExists,
                new SqlParameter("@username", username));

            return Convert.ToBoolean(prmExists.Value);
        }

        public static DataSet GetUsersPaged(int actorId, int userId, string filterColumn, string filterValue,
            int statusId, int roleId, string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUsersPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@roleId", roleId),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows),
                new SqlParameter("@recursive", recursive));
        }

        public static DataSet GetUsersSummary(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUsersSummary",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static DataSet GetUserDomainsPaged(int actorId, int userId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserDomainsPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@filterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@filterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetUsers(int actorId, int ownerId, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUsers",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@OwnerID", ownerId),
                new SqlParameter("@Recursive", recursive));
        }

        public static DataSet GetUserParents(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserParents",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static DataSet GetUserPeers(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserPeers",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static IDataReader GetUserByIdInternally(int userId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserByIdInternally",
                new SqlParameter("@UserID", userId));
        }

        public static IDataReader GetUserByUsernameInternally(string username)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserByUsernameInternally",
                new SqlParameter("@Username", username));
        }

        public static IDataReader GetUserById(int actorId, int userId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserById",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static IDataReader GetUserByUsername(int actorId, string username)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserByUsername",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@Username", username));
        }

        public static int AddUser(int actorId, int ownerId, int roleId, int statusId, bool isDemo,
            bool isPeer, string comments, string username, string password,
            string firstName, string lastName, string email, string secondaryEmail,
            string address, string city, string country, string state, string zip,
            string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail,
            string companyName, bool ecommerceEnabled)
        {
            SqlParameter prmUserId = new SqlParameter("@UserID", SqlDbType.Int);
            prmUserId.Direction = ParameterDirection.Output;

            // add user to WebsitePanel Users table
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddUser",
                prmUserId,
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@OwnerID", ownerId),
                new SqlParameter("@RoleID", roleId),
                new SqlParameter("@StatusId", statusId),
                new SqlParameter("@IsDemo", isDemo),
                new SqlParameter("@IsPeer", isPeer),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@username", username),
                new SqlParameter("@password", password),
                new SqlParameter("@firstName", firstName),
                new SqlParameter("@lastName", lastName),
                new SqlParameter("@email", email),
                new SqlParameter("@secondaryEmail", secondaryEmail),
                new SqlParameter("@address", address),
                new SqlParameter("@city", city),
                new SqlParameter("@country", country),
                new SqlParameter("@state", state),
                new SqlParameter("@zip", zip),
                new SqlParameter("@primaryPhone", primaryPhone),
                new SqlParameter("@secondaryPhone", secondaryPhone),
                new SqlParameter("@fax", fax),
                new SqlParameter("@instantMessenger", instantMessenger),
                new SqlParameter("@htmlMail", htmlMail),
				new SqlParameter("@CompanyName", companyName),
				new SqlParameter("@EcommerceEnabled", ecommerceEnabled));

            return Convert.ToInt32(prmUserId.Value);
        }

        public static void UpdateUser(int actorId, int userId, int roleId, int statusId, bool isDemo,
            bool isPeer, string comments, string firstName, string lastName, string email, string secondaryEmail,
            string address, string city, string country, string state, string zip,
            string primaryPhone, string secondaryPhone, string fax, string instantMessenger, bool htmlMail,
			string companyName, bool ecommerceEnabled)
        {
            // update user
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateUser",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@RoleID", roleId),
                new SqlParameter("@StatusId", statusId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@IsDemo", isDemo),
                new SqlParameter("@IsPeer", isPeer),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@firstName", firstName),
                new SqlParameter("@lastName", lastName),
                new SqlParameter("@email", email),
                new SqlParameter("@secondaryEmail", secondaryEmail),
                new SqlParameter("@address", address),
                new SqlParameter("@city", city),
                new SqlParameter("@country", country),
                new SqlParameter("@state", state),
                new SqlParameter("@zip", zip),
                new SqlParameter("@primaryPhone", primaryPhone),
                new SqlParameter("@secondaryPhone", secondaryPhone),
                new SqlParameter("@fax", fax),
                new SqlParameter("@instantMessenger", instantMessenger),
                new SqlParameter("@htmlMail", htmlMail),
				new SqlParameter("@CompanyName", companyName),
				new SqlParameter("@EcommerceEnabled", ecommerceEnabled));
        }

        public static void DeleteUser(int actorId, int userId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteUser",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static void ChangeUserPassword(int actorId, int userId, string password)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "ChangeUserPassword",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@password", password));
        }

        #endregion

        #region User Settings
        public static IDataReader GetUserSettings(int actorId, int userId, string settingsName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserSettings",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@SettingsName", settingsName));
        }
        public static void UpdateUserSettings(int actorId, int userId, string settingsName, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateUserSettings",
                new SqlParameter("@UserID", userId),
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@SettingsName", settingsName),
                new SqlParameter("@Xml", xml));
        }
        #endregion

        #region Servers
        public static DataSet GetAllServers(int actorId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAllServers",
                new SqlParameter("@actorId", actorId));
        }
        public static DataSet GetServers(int actorId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServers",
                new SqlParameter("@actorId", actorId));
        }

        public static IDataReader GetServer(int actorId, int serverId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServer",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId));
        }

        public static IDataReader GetServerShortDetails(int serverId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServerShortDetails",
                new SqlParameter("@ServerID", serverId));
        }

        public static IDataReader GetServerByName(int actorId, string serverName)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServerByName",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerName", serverName));
        }

        public static IDataReader GetServerInternal(int serverId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServerInternal",
                new SqlParameter("@ServerID", serverId));
        }

        public static int AddServer(string serverName, string serverUrl,
            string password, string comments, bool virtualServer, string instantDomainAlias,
            int primaryGroupId, bool adEnabled, string adRootDomain, string adUsername, string adPassword,
            string adAuthenticationType)
        {
            SqlParameter prmServerId = new SqlParameter("@ServerID", SqlDbType.Int);
            prmServerId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddServer",
                prmServerId,
                new SqlParameter("@ServerName", serverName),
                new SqlParameter("@ServerUrl", serverUrl),
                new SqlParameter("@Password", password),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@VirtualServer", virtualServer),
                new SqlParameter("@InstantDomainAlias", instantDomainAlias),
                new SqlParameter("@PrimaryGroupId", primaryGroupId),
                new SqlParameter("@AdEnabled", adEnabled),
                new SqlParameter("@AdRootDomain", adRootDomain),
                new SqlParameter("@AdUsername", adUsername),
                new SqlParameter("@AdPassword", adPassword),
                new SqlParameter("@AdAuthenticationType", adAuthenticationType));

            return Convert.ToInt32(prmServerId.Value);
        }

        public static void UpdateServer(int serverId, string serverName, string serverUrl,
            string password, string comments, string instantDomainAlias,
            int primaryGroupId, bool adEnabled, string adRootDomain, string adUsername, string adPassword,
            string adAuthenticationType)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateServer",
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@ServerName", serverName),
                new SqlParameter("@ServerUrl", serverUrl),
                new SqlParameter("@Password", password),
                new SqlParameter("@Comments", comments),
                new SqlParameter("@InstantDomainAlias", instantDomainAlias),
                new SqlParameter("@PrimaryGroupId", primaryGroupId),
                new SqlParameter("@AdEnabled", adEnabled),
                new SqlParameter("@AdRootDomain", adRootDomain),
                new SqlParameter("@AdUsername", adUsername),
                new SqlParameter("@AdPassword", adPassword),
                new SqlParameter("@AdAuthenticationType", adAuthenticationType));

        }

        public static int DeleteServer(int serverId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteServer",
                prmResult,
                new SqlParameter("@ServerID", serverId));

            return Convert.ToInt32(prmResult.Value);
        }
        #endregion

        #region Virtual Servers
        public static DataSet GetVirtualServers(int actorId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetVirtualServers",
                new SqlParameter("@actorId", actorId));
        }

        public static DataSet GetAvailableVirtualServices(int actorId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAvailableVirtualServices",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId));
        }

        public static DataSet GetVirtualServices(int actorId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetVirtualServices",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId));
        }

        public static void AddVirtualServices(int serverId, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddVirtualServices",
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@xml", xml));
        }

        public static void DeleteVirtualServices(int serverId, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteVirtualServices",
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@xml", xml));
        }

        public static void UpdateVirtualGroups(int serverId, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateVirtualGroups",
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@xml", xml));
        }
        #endregion

        #region Providers

        // Providers methods

        public static DataSet GetProviders()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetProviders");
        }

        public static DataSet GetGroupProviders(int groupId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetGroupProviders",
                new SqlParameter("@groupId", groupId));
        }

        public static IDataReader GetProvider(int providerId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetProvider",
                new SqlParameter("@ProviderID", providerId));
        }

        public static IDataReader GetProviderByServiceID(int serviceId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetProviderByServiceID",
                new SqlParameter("@ServiceID", serviceId));
        }

        #endregion

        #region IPAddresses
        public static IDataReader GetIPAddress(int ipAddressId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetIPAddress",
                new SqlParameter("@AddressID", ipAddressId));
        }

        public static IDataReader GetIPAddresses(int actorId, int poolId, int serverId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetIPAddresses",
                                        new SqlParameter("@ActorId", actorId),
                                        new SqlParameter("@PoolId", poolId),
                                        new SqlParameter("@ServerId", serverId));
            return reader;
        }

        public static IDataReader GetIPAddressesPaged(int actorId, int poolId, int serverId,
            string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetIPAddressesPaged",
                                        new SqlParameter("@ActorId", actorId),
                                        new SqlParameter("@PoolId", poolId),
                                        new SqlParameter("@ServerId", serverId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@startRow", startRow),
                                        new SqlParameter("@maximumRows", maximumRows));
            return reader;
        }

        public static int AddIPAddress(int poolId, int serverId, string externalIP, string internalIP,
            string subnetMask, string defaultGateway, string comments)
        {
            SqlParameter prmAddresId = new SqlParameter("@AddressID", SqlDbType.Int);
            prmAddresId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddIPAddress",
                prmAddresId,
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@externalIP", externalIP),
                new SqlParameter("@internalIP", internalIP),
                new SqlParameter("@PoolId", poolId),
                new SqlParameter("@SubnetMask", subnetMask),
                new SqlParameter("@DefaultGateway", defaultGateway),
                new SqlParameter("@Comments", comments));

            return Convert.ToInt32(prmAddresId.Value);
        }

        public static void UpdateIPAddress(int addressId, int poolId, int serverId,
            string externalIP, string internalIP, string subnetMask, string defaultGateway, string comments)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateIPAddress",
                new SqlParameter("@AddressID", addressId),
                new SqlParameter("@externalIP", externalIP),
                new SqlParameter("@internalIP", internalIP),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@PoolId", poolId),
                new SqlParameter("@SubnetMask", subnetMask),
                new SqlParameter("@DefaultGateway", defaultGateway),
                new SqlParameter("@Comments", comments));
        }

        public static void UpdateIPAddresses(string xmlIds, int poolId, int serverId,
            string subnetMask, string defaultGateway, string comments)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateIPAddresses",
                new SqlParameter("@Xml", xmlIds),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@PoolId", poolId),
                new SqlParameter("@SubnetMask", subnetMask),
                new SqlParameter("@DefaultGateway", defaultGateway),
                new SqlParameter("@Comments", comments));
        }

        public static int DeleteIPAddress(int ipAddressId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteIPAddress",
                prmResult,
                new SqlParameter("@AddressID", ipAddressId));

            return Convert.ToInt32(prmResult.Value);
        }



        #endregion

        #region Clusters
        public static IDataReader GetClusters(int actorId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetClusters",
                new SqlParameter("@actorId", actorId));
        }

        public static int AddCluster(string clusterName)
        {
            SqlParameter prmId = new SqlParameter("@ClusterID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddCluster",
                prmId,
                new SqlParameter("@ClusterName", clusterName));

            return Convert.ToInt32(prmId.Value);
        }

        public static void DeleteCluster(int clusterId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteCluster",
                new SqlParameter("@ClusterId", clusterId));
        }

        #endregion

        #region Global DNS records
        public static DataSet GetDnsRecordsByService(int actorId, int serviceId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsByService",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceId", serviceId));
        }

        public static DataSet GetDnsRecordsByServer(int actorId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsByServer",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerId", serverId));
        }

        public static DataSet GetDnsRecordsByPackage(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsByPackage",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageId", packageId));
        }

        public static DataSet GetDnsRecordsByGroup(int groupId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsByGroup",
                new SqlParameter("@GroupId", groupId));
        }

        public static DataSet GetDnsRecordsTotal(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecordsTotal",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId));
        }

        public static IDataReader GetDnsRecord(int actorId, int recordId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDnsRecord",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@RecordId", recordId));
        }

        public static void AddDnsRecord(int actorId, int serviceId, int serverId, int packageId, string recordType,
            string recordName, string recordData, int mxPriority, int ipAddressId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddDnsRecord",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceId", serviceId),
                new SqlParameter("@ServerId", serverId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@RecordType", recordType),
                new SqlParameter("@RecordName", recordName),
                new SqlParameter("@RecordData", recordData),
                new SqlParameter("@MXPriority", mxPriority),
                new SqlParameter("@IpAddressId", ipAddressId));
        }

        public static void UpdateDnsRecord(int actorId, int recordId, string recordType,
            string recordName, string recordData, int mxPriority, int ipAddressId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateDnsRecord",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@RecordId", recordId),
                new SqlParameter("@RecordType", recordType),
                new SqlParameter("@RecordName", recordName),
                new SqlParameter("@RecordData", recordData),
                new SqlParameter("@MXPriority", mxPriority),
                new SqlParameter("@IpAddressId", ipAddressId));
        }

        public static void DeleteDnsRecord(int actorId, int recordId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteDnsRecord",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@RecordId", recordId));
        }
        #endregion

        #region Domains
        public static DataSet GetDomains(int actorId, int packageId, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomains",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@Recursive", recursive));
        }

        public static DataSet GetResellerDomains(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetResellerDomains",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId));
        }

        public static DataSet GetDomainsPaged(int actorId, int packageId, int serverId, bool recursive, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomainsPaged",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@recursive", recursive),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@StartRow", startRow),
                new SqlParameter("@MaximumRows", maximumRows));
        }

        public static IDataReader GetDomain(int actorId, int domainId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomain",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@domainId", domainId));
        }

        public static IDataReader GetDomainByName(int actorId, string domainName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetDomainByName",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@domainName", domainName));
        }

        public static int CheckDomain(int packageId, string domainName)
        {
            SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckDomain",
                prmId,
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@domainName", domainName));

            return Convert.ToInt32(prmId.Value);
        }

        public static int AddDomain(int actorId, int packageId, int zoneItemId, string domainName,
            bool hostingAllowed, int webSiteId, int mailDomainId, bool isSubDomain, bool isInstantAlias, bool isDomainPointer)
        {
            SqlParameter prmId = new SqlParameter("@DomainID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddDomain",
                prmId,
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@ZoneItemId", zoneItemId),
                new SqlParameter("@DomainName", domainName),
                new SqlParameter("@HostingAllowed", hostingAllowed),
                new SqlParameter("@WebSiteId", webSiteId),
                new SqlParameter("@MailDomainId", mailDomainId),
                new SqlParameter("@IsSubDomain", isSubDomain),
                new SqlParameter("@IsInstantAlias", isInstantAlias),
                new SqlParameter("@IsDomainPointer", isDomainPointer));

            return Convert.ToInt32(prmId.Value);
        }

        public static void UpdateDomain(int actorId, int domainId, int zoneItemId,
            bool hostingAllowed, int webSiteId, int mailDomainId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateDomain",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@DomainId", domainId),
                new SqlParameter("@ZoneItemId", zoneItemId),
                new SqlParameter("@HostingAllowed", hostingAllowed),
                new SqlParameter("@WebSiteId", webSiteId),
                new SqlParameter("@MailDomainId", mailDomainId));
        }

        public static void DeleteDomain(int actorId, int domainId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteDomain",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@DomainId", domainId));
        }
        #endregion

        #region Services
        public static IDataReader GetServicesByServerId(int actorId, int serverId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServicesByServerID",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId));
        }

        public static IDataReader GetServicesByServerIdGroupName(int actorId, int serverId, string groupName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServicesByServerIdGroupName",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@GroupName", groupName));
        }

        public static DataSet GetRawServicesByServerId(int actorId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetRawServicesByServerID",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServerID", serverId));
        }

        public static DataSet GetServicesByGroupId(int actorId, int groupId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServicesByGroupID",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@groupId", groupId));
        }

        public static DataSet GetServicesByGroupName(int actorId, string groupName)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServicesByGroupName",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@GroupName", groupName));
        }

        public static IDataReader GetService(int actorId, int serviceId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetService",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceID", serviceId));
        }

        public static int AddService(int serverId, int providerId, string serviceName, int serviceQuotaValue,
            int clusterId, string comments)
        {
            SqlParameter prmServiceId = new SqlParameter("@ServiceID", SqlDbType.Int);
            prmServiceId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddService",
                prmServiceId,
                new SqlParameter("@ServerID", serverId),
                new SqlParameter("@ProviderID", providerId),
                new SqlParameter("@ServiceName", serviceName),
                new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
                new SqlParameter("@ClusterId", clusterId),
                new SqlParameter("@comments", comments));

            return Convert.ToInt32(prmServiceId.Value);
        }

        public static void UpdateService(int serviceId, string serviceName, int serviceQuotaValue,
            int clusterId, string comments)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateService",
                new SqlParameter("@ServiceName", serviceName),
                new SqlParameter("@ServiceID", serviceId),
                new SqlParameter("@ServiceQuotaValue", serviceQuotaValue),
                new SqlParameter("@ClusterId", clusterId),
                new SqlParameter("@Comments", comments));
        }

        public static int DeleteService(int serviceId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteService",
                prmResult,
                new SqlParameter("@ServiceID", serviceId));

            return Convert.ToInt32(prmResult.Value);
        }

        public static IDataReader GetServiceProperties(int actorId, int serviceId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceProperties",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceID", serviceId));
        }

        public static void UpdateServiceProperties(int serviceId, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateServiceProperties",
                new SqlParameter("@ServiceId", serviceId),
                new SqlParameter("@Xml", xml));
        }

        public static IDataReader GetResourceGroup(int groupId)
        {
            return SqlHelper.ExecuteReader(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetResourceGroup",
                new SqlParameter("@groupId", groupId));
        }

        public static DataSet GetResourceGroups()
        {
            return SqlHelper.ExecuteDataset(ConnectionString,
                CommandType.StoredProcedure,
                ObjectQualifier + "GetResourceGroups");
        }
        #endregion

        #region Service Items
        public static DataSet GetServiceItems(int actorId, int packageId, string groupName, string itemTypeName, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItems",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@GroupName", groupName),
                new SqlParameter("@ItemTypeName", itemTypeName),
                new SqlParameter("@Recursive", recursive));

        }

        public static DataSet GetServiceItemsPaged(int actorId, int packageId, string groupName, string itemTypeName,
            int serverId, bool recursive, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@groupName", groupName),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@itemTypeName", itemTypeName),
                new SqlParameter("@recursive", recursive),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetSearchableServiceItemTypes()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSearchableServiceItemTypes");
        }

        public static DataSet GetServiceItemsByService(int actorId, int serviceId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsByService",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ServiceID", serviceId));
        }

        public static int GetServiceItemsCount(string typeName, string groupName, int serviceId)
        {
            SqlParameter prmTotalNumber = new SqlParameter("@TotalNumber", SqlDbType.Int);
            prmTotalNumber.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsCount",
                prmTotalNumber,
                new SqlParameter("@itemTypeName", typeName),
                new SqlParameter("@groupName", groupName),
                new SqlParameter("@serviceId", serviceId));

            // read identity
            return Convert.ToInt32(prmTotalNumber.Value);
        }

        public static DataSet GetServiceItemsForStatistics(int actorId, int serviceId, int packageId,
            bool calculateDiskspace, bool calculateBandwidth, bool suspendable, bool disposable)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsForStatistics",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ServiceID", serviceId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@calculateDiskspace", calculateDiskspace),
                new SqlParameter("@calculateBandwidth", calculateBandwidth),
                new SqlParameter("@suspendable", suspendable),
                new SqlParameter("@disposable", disposable));
        }

        public static DataSet GetServiceItemsByPackage(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsByPackage",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet GetServiceItem(int actorId, int itemId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItem",
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@actorId", actorId));
        }

        public static bool CheckServiceItemExists(int serviceId, string itemName, string itemTypeName)
        {
            SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
            prmExists.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckServiceItemExistsInService",
                prmExists,
                new SqlParameter("@serviceId", serviceId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@itemTypeName", itemTypeName));

            return Convert.ToBoolean(prmExists.Value);
        }

        public static bool CheckServiceItemExists(string itemName, string groupName, string itemTypeName)
        {
            SqlParameter prmExists = new SqlParameter("@Exists", SqlDbType.Bit);
            prmExists.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CheckServiceItemExists",
                prmExists,
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@groupName", groupName),
                new SqlParameter("@itemTypeName", itemTypeName));

            return Convert.ToBoolean(prmExists.Value);
        }

        public static DataSet GetServiceItemByName(int actorId, int packageId, string groupName,
            string itemName, string itemTypeName)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemByName",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@itemTypeName", itemTypeName),
                new SqlParameter("@groupName", groupName));
        }

        public static DataSet GetServiceItemsByName(int actorId, int packageId, string itemName)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetServiceItemsByName",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@itemName", itemName));
        }

        public static int AddServiceItem(int actorId, int serviceId, int packageId, string itemName,
            string itemTypeName, string xmlProperties)
        {
            // add item
            SqlParameter prmItemId = new SqlParameter("@ItemID", SqlDbType.Int);
            prmItemId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddServiceItem",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@ServiceID", serviceId),
                new SqlParameter("@ItemName", itemName),
                new SqlParameter("@ItemTypeName", itemTypeName),
                new SqlParameter("@xmlProperties", xmlProperties),
                new SqlParameter("@CreatedDate", DateTime.Now),
                prmItemId);

            return Convert.ToInt32(prmItemId.Value);
        }

        public static void UpdateServiceItem(int actorId, int itemId, string itemName, string xmlProperties)
        {
            // update item
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateServiceItem",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ItemName", itemName),
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@XmlProperties", xmlProperties));
        }

        public static void DeleteServiceItem(int actorId, int itemId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteServiceItem",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ItemID", itemId));
        }

        public static void MoveServiceItem(int actorId, int itemId, int destinationServiceId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "MoveServiceItem",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@DestinationServiceID", destinationServiceId));
        }

        public static int GetPackageServiceId(int actorId, int packageId, string groupName)
        {
            SqlParameter prmServiceId = new SqlParameter("@ServiceID", SqlDbType.Int);
            prmServiceId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageServiceID",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@groupName", groupName),
                prmServiceId);

            return Convert.ToInt32(prmServiceId.Value);
        }

        public static void UpdatePackageDiskSpace(int packageId, string xml)
        {
            ExecuteLongNonQuery(
                ObjectQualifier + "UpdatePackageDiskSpace",
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@xml", xml));
        }

        public static void UpdatePackageBandwidth(int packageId, string xml)
        {
            ExecuteLongNonQuery(
                ObjectQualifier + "UpdatePackageBandwidth",
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@xml", xml));
        }

        public static DateTime GetPackageBandwidthUpdate(int packageId)
        {
            SqlParameter prmUpdateDate = new SqlParameter("@UpdateDate", SqlDbType.DateTime);
            prmUpdateDate.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageBandwidthUpdate",
                prmUpdateDate,
                new SqlParameter("@packageId", packageId));

            return (prmUpdateDate.Value != DBNull.Value) ? Convert.ToDateTime(prmUpdateDate.Value) : DateTime.MinValue;
        }

        public static void UpdatePackageBandwidthUpdate(int packageId, DateTime updateDate)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackageBandwidthUpdate",
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@updateDate", updateDate));
        }

        public static IDataReader GetServiceItemType(int itemTypeId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetServiceItemType",
                new SqlParameter("@ItemTypeID", itemTypeId)
            );
        }

        public static IDataReader GetServiceItemTypes()
        {
	        return SqlHelper.ExecuteReader (
		        ConnectionString,
		        CommandType.StoredProcedure,
		        "GetServiceItemTypes"
	        );
        }
        #endregion

        #region Plans
        // Plans methods
        public static DataSet GetHostingPlans(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetHostingPlans",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static DataSet GetHostingAddons(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetHostingAddons",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static DataSet GetUserAvailableHostingPlans(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserAvailableHostingPlans",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static DataSet GetUserAvailableHostingAddons(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetUserAvailableHostingAddons",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId));
        }

        public static IDataReader GetHostingPlan(int actorId, int planId)
        {
            return (IDataReader)SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetHostingPlan",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PlanId", planId));
        }

        public static DataSet GetHostingPlanQuotas(int actorId, int packageId, int planId, int serverId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetHostingPlanQuotas",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@serverId", serverId));
        }

        public static int AddHostingPlan(int actorId, int userId, int packageId, string planName,
            string planDescription, bool available, int serverId, decimal setupPrice, decimal recurringPrice,
            int recurrenceUnit, int recurrenceLength, bool isAddon, string quotasXml)
        {
            SqlParameter prmPlanId = new SqlParameter("@PlanID", SqlDbType.Int);
            prmPlanId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddHostingPlan",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@planName", planName),
                new SqlParameter("@planDescription", planDescription),
                new SqlParameter("@available", available),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@setupPrice", setupPrice),
                new SqlParameter("@recurringPrice", recurringPrice),
                new SqlParameter("@recurrenceUnit", recurrenceUnit),
                new SqlParameter("@recurrenceLength", recurrenceLength),
                new SqlParameter("@isAddon", isAddon),
                new SqlParameter("@quotasXml", quotasXml),
                prmPlanId);

            // read identity
            return Convert.ToInt32(prmPlanId.Value);
        }

        public static DataSet UpdateHostingPlan(int actorId, int planId, int packageId, int serverId, string planName,
            string planDescription, bool available, decimal setupPrice, decimal recurringPrice,
            int recurrenceUnit, int recurrenceLength, string quotasXml)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateHostingPlan",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@planName", planName),
                new SqlParameter("@planDescription", planDescription),
                new SqlParameter("@available", available),
                new SqlParameter("@setupPrice", setupPrice),
                new SqlParameter("@recurringPrice", recurringPrice),
                new SqlParameter("@recurrenceUnit", recurrenceUnit),
                new SqlParameter("@recurrenceLength", recurrenceLength),
                new SqlParameter("@quotasXml", quotasXml));
        }

        public static int CopyHostingPlan(int planId, int userId, int packageId)
        {
            SqlParameter prmPlanId = new SqlParameter("@DestinationPlanID", SqlDbType.Int);
            prmPlanId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "CopyHostingPlan",
                new SqlParameter("@SourcePlanID", planId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@PackageID", packageId),
                prmPlanId);

            return Convert.ToInt32(prmPlanId.Value);
        }

        public static int DeleteHostingPlan(int actorId, int planId)
        {
            SqlParameter prmResult = new SqlParameter("@Result", SqlDbType.Int);
            prmResult.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteHostingPlan",
                prmResult,
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PlanId", planId));

            return Convert.ToInt32(prmResult.Value);
        }
        #endregion

        #region Packages

        // Packages
        public static DataSet GetMyPackages(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetMyPackages",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static DataSet GetPackages(int actorId, int userId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackages",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId));
        }

        public static DataSet GetNestedPackagesSummary(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetNestedPackagesSummary",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet SearchServiceItemsPaged(int actorId, int userId, int itemTypeId, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "SearchServiceItemsPaged",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@itemTypeId", itemTypeId),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetPackagesPaged(int actorId, int userId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackagesPaged",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetNestedPackagesPaged(int actorId, int packageId, string filterColumn, string filterValue,
            int statusId, int planId, int serverId, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetNestedPackagesPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@serverId", serverId),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetPackagePackages(int actorId, int packageId, bool recursive)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackagePackages",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@recursive", recursive));
        }

        public static IDataReader GetPackage(int actorId, int packageId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackage",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet GetPackageQuotas(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageQuotas",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet GetPackageQuotasForEdit(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageQuotasForEdit",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static DataSet AddPackage(int actorId, out int packageId, int userId, int planId, string packageName,
            string packageComments, int statusId, DateTime purchaseDate)
        {
            SqlParameter prmPackageId = new SqlParameter("@PackageID", SqlDbType.Int);
            prmPackageId.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddPackage",
                prmPackageId,
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@packageName", packageName),
                new SqlParameter("@packageComments", packageComments),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@purchaseDate", purchaseDate));

            // read identity
            packageId = Convert.ToInt32(prmPackageId.Value);

            return ds;
        }

        public static DataSet UpdatePackage(int actorId, int packageId, int planId, string packageName,
            string packageComments, int statusId, DateTime purchaseDate,
            bool overrideQuotas, string quotasXml)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackage",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@packageName", packageName),
                new SqlParameter("@packageComments", packageComments),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@purchaseDate", purchaseDate),
                new SqlParameter("@overrideQuotas", overrideQuotas),
                new SqlParameter("@quotasXml", quotasXml));
        }

        public static void UpdatePackageName(int actorId, int packageId, string packageName,
            string packageComments)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackageName",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@packageName", packageName),
                new SqlParameter("@packageComments", packageComments));
        }

        public static void DeletePackage(int actorId, int packageId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeletePackage",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        // Package Add-ons
        public static DataSet GetPackageAddons(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageAddons",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId));
        }

        public static IDataReader GetPackageAddon(int actorId, int packageAddonId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageAddon",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageAddonID", packageAddonId));
        }

        public static DataSet AddPackageAddon(int actorId, out int addonId, int packageId, int planId, int quantity,
            int statusId, DateTime purchaseDate, string comments)
        {
            SqlParameter prmPackageAddonId = new SqlParameter("@PackageAddonID", SqlDbType.Int);
            prmPackageAddonId.Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddPackageAddon",
                prmPackageAddonId,
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@PurchaseDate", purchaseDate),
                new SqlParameter("@Comments", comments));

            // read identity
            addonId = Convert.ToInt32(prmPackageAddonId.Value);

            return ds;
        }

        public static DataSet UpdatePackageAddon(int actorId, int packageAddonId, int planId, int quantity,
            int statusId, DateTime purchaseDate, string comments)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackageAddon",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageAddonID", packageAddonId),
                new SqlParameter("@planId", planId),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@PurchaseDate", purchaseDate),
                new SqlParameter("@Comments", comments));
        }

        public static void DeletePackageAddon(int actorId, int packageAddonId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeletePackageAddon",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageAddonID", packageAddonId));
        }

        #endregion

        #region Packages Settings
        public static IDataReader GetPackageSettings(int actorId, int packageId, string settingsName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageSettings",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@SettingsName", settingsName));
        }
        public static void UpdatePackageSettings(int actorId, int packageId, string settingsName, string xml)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdatePackageSettings",
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@SettingsName", settingsName),
                new SqlParameter("@Xml", xml));
        }
        #endregion

        #region Quotas
        public static IDataReader GetProviderServiceQuota(int providerId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetProviderServiceQuota",
                new SqlParameter("@providerId", providerId));
        }

        public static IDataReader GetPackageQuota(int actorId, int packageId, string quotaName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetPackageQuota",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@QuotaName", quotaName));
        }
        #endregion

        #region Log
        public static void AddAuditLogRecord(string recordId, int severityId,
            int userId, string username, int packageId, int itemId, string itemName, DateTime startDate, DateTime finishDate, string sourceName,
            string taskName, string executionLog)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddAuditLogRecord",
                new SqlParameter("@recordId", recordId),
                new SqlParameter("@severityId", severityId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@username", username),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@ItemId", itemId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@startDate", startDate),
                new SqlParameter("@finishDate", finishDate),
                new SqlParameter("@sourceName", sourceName),
                new SqlParameter("@taskName", taskName),
                new SqlParameter("@executionLog", executionLog));
        }

        public static DataSet GetAuditLogRecordsPaged(int actorId, int userId, int packageId, int itemId, string itemName, DateTime startDate, DateTime endDate,
            int severityId, string sourceName, string taskName, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAuditLogRecordsPaged",
                new SqlParameter("@ActorId", actorId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@PackageID", packageId),
                new SqlParameter("@itemId", itemId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@severityId", severityId),
                new SqlParameter("@sourceName", sourceName),
                new SqlParameter("@taskName", taskName),
                new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetAuditLogSources()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAuditLogSources");
        }

        public static DataSet GetAuditLogTasks(string sourceName)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAuditLogTasks",
                new SqlParameter("@sourceName", sourceName));
        }

        public static IDataReader GetAuditLogRecord(string recordId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetAuditLogRecord",
                new SqlParameter("@recordId", recordId));
        }

        public static void DeleteAuditLogRecords(int actorId, int userId, int itemId, string itemName, DateTime startDate, DateTime endDate,
            int severityId, string sourceName, string taskName)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteAuditLogRecords",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@itemId", itemId),
                new SqlParameter("@itemName", itemName),
                new SqlParameter("@startDate", startDate),
                new SqlParameter("@endDate", endDate),
                new SqlParameter("@severityId", severityId),
                new SqlParameter("@sourceName", sourceName),
                new SqlParameter("@taskName", taskName));
        }

        public static void DeleteAuditLogRecordsComplete()
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteAuditLogRecordsComplete");
        }

        #endregion

        #region Reports
        public static DataSet GetPackagesBandwidthPaged(int actorId, int userId, int packageId,
            DateTime startDate, DateTime endDate, string sortColumn,
            int startRow, int maximumRows)
        {
            return ExecuteLongDataSet(
                ObjectQualifier + "GetPackagesBandwidthPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate),
                new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetPackagesDiskspacePaged(int actorId, int userId, int packageId, string sortColumn,
            int startRow, int maximumRows)
        {
            return ExecuteLongDataSet(
                ObjectQualifier + "GetPackagesDiskspacePaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@sortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetPackageBandwidth(int actorId, int packageId, DateTime startDate, DateTime endDate)
        {
            return ExecuteLongDataSet(
                ObjectQualifier + "GetPackageBandwidth",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageId", packageId),
                new SqlParameter("@StartDate", startDate),
                new SqlParameter("@EndDate", endDate));
        }

        public static DataSet GetPackageDiskspace(int actorId, int packageId)
        {
            return ExecuteLongDataSet(
                ObjectQualifier + "GetPackageDiskspace",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@PackageId", packageId));
        }

        #endregion

        #region Scheduler
        public static IDataReader GetScheduleTasks(int actorId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleTasks",
                new SqlParameter("@actorId", actorId));
        }

        public static IDataReader GetScheduleTask(int actorId, string taskId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleTask",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@taskId", taskId));
        }

        public static DataSet GetSchedules(int actorId, int packageId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSchedules",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId));
        }

        public static DataSet GetSchedulesPaged(int actorId, int packageId, bool recursive,
            string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSchedulesPaged",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@recursive", recursive),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@startRow", startRow),
                new SqlParameter("@maximumRows", maximumRows));
        }

        public static DataSet GetSchedule(int actorId, int scheduleId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetSchedule",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId));
        }
        public static IDataReader GetScheduleInternal(int scheduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleInternal",
                new SqlParameter("@scheduleId", scheduleId));
        }
        public static DataSet GetNextSchedule()
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetNextSchedule");
        }
        public static IDataReader GetScheduleParameters(int actorId, string taskId, int scheduleId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleParameters",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@scheduleId", scheduleId));
        }

		/// <summary>
		/// Loads view configuration for the task with specified id.
		/// </summary>
		/// <param name="taskId">Task id which points to task for which view configuration will be loaded.</param>
		/// <returns>View configuration for the task with supplied id.</returns>
		public static IDataReader GetScheduleTaskViewConfigurations(string taskId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
			                               ObjectQualifier + "GetScheduleTaskViewConfigurations",
			                               new SqlParameter("@taskId", taskId));
		}

        public static int AddSchedule(int actorId, string taskId, int packageId,
            string scheduleName, string scheduleTypeId, int interval,
            DateTime fromTime, DateTime toTime, DateTime startTime,
            DateTime nextRun, bool enabled, string priorityId, int historiesNumber,
            int maxExecutionTime, int weekMonthDay, string xmlParameters)
        {
            SqlParameter prmId = new SqlParameter("@ScheduleID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddSchedule",
                prmId,
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@packageId", packageId),
                new SqlParameter("@scheduleName", scheduleName),
                new SqlParameter("@scheduleTypeId", scheduleTypeId),
                new SqlParameter("@interval", interval),
                new SqlParameter("@fromTime", fromTime),
                new SqlParameter("@toTime", toTime),
                new SqlParameter("@startTime", startTime),
                new SqlParameter("@nextRun", nextRun),
                new SqlParameter("@enabled", enabled),
                new SqlParameter("@priorityId", priorityId),
                new SqlParameter("@historiesNumber", historiesNumber),
                new SqlParameter("@maxExecutionTime", maxExecutionTime),
                new SqlParameter("@weekMonthDay", weekMonthDay),
                new SqlParameter("@xmlParameters", xmlParameters));

            // read identity
            return Convert.ToInt32(prmId.Value);
        }
        public static void UpdateSchedule(int actorId, int scheduleId, string taskId,
            string scheduleName, string scheduleTypeId, int interval,
            DateTime fromTime, DateTime toTime, DateTime startTime,
            DateTime lastRun, DateTime nextRun, bool enabled, string priorityId, int historiesNumber,
            int maxExecutionTime, int weekMonthDay, string xmlParameters)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateSchedule",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId),
                new SqlParameter("@taskId", taskId),
                new SqlParameter("@scheduleName", scheduleName),
                new SqlParameter("@scheduleTypeId", scheduleTypeId),
                new SqlParameter("@interval", interval),
                new SqlParameter("@fromTime", fromTime),
                new SqlParameter("@toTime", toTime),
                new SqlParameter("@startTime", startTime),
                new SqlParameter("@lastRun", (lastRun == DateTime.MinValue) ? DBNull.Value : (object)lastRun),
                new SqlParameter("@nextRun", nextRun),
                new SqlParameter("@enabled", enabled),
                new SqlParameter("@priorityId", priorityId),
                new SqlParameter("@historiesNumber", historiesNumber),
                new SqlParameter("@maxExecutionTime", maxExecutionTime),
                new SqlParameter("@weekMonthDay", weekMonthDay),
                new SqlParameter("@xmlParameters", xmlParameters));
        }
        public static void DeleteSchedule(int actorId, int scheduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteSchedule",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId));
        }

        public static DataSet GetScheduleHistories(int actorId, int scheduleId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleHistories",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId));
        }
        public static IDataReader GetScheduleHistory(int actorId, int scheduleHistoryId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetScheduleHistory",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleHistoryId", scheduleHistoryId));
        }
        public static int AddScheduleHistory(int actorId, int scheduleId,
            DateTime startTime, DateTime finishTime, string statusId, string executionLog)
        {
            SqlParameter prmId = new SqlParameter("@ScheduleHistoryID", SqlDbType.Int);
            prmId.Direction = ParameterDirection.Output;

            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddScheduleHistory",
                prmId,
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId),
                new SqlParameter("@startTime", (startTime == DateTime.MinValue) ? DBNull.Value : (object)startTime),
                new SqlParameter("@finishTime", (finishTime == DateTime.MinValue) ? DBNull.Value : (object)finishTime),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@executionLog", executionLog));

            // read identity
            return Convert.ToInt32(prmId.Value);
        }
        public static void UpdateScheduleHistory(int actorId, int scheduleHistoryId,
            DateTime startTime, DateTime finishTime, string statusId, string executionLog)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "UpdateScheduleHistory",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleHistoryId", scheduleHistoryId),
                new SqlParameter("@startTime", (startTime == DateTime.MinValue) ? DBNull.Value : (object)startTime),
                new SqlParameter("@finishTime", (finishTime == DateTime.MinValue) ? DBNull.Value : (object)finishTime),
                new SqlParameter("@statusId", statusId),
                new SqlParameter("@executionLog", executionLog));
        }
        public static void DeleteScheduleHistories(int actorId, int scheduleId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteScheduleHistories",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@scheduleId", scheduleId));
        }
        #endregion

        #region Comments
        public static DataSet GetComments(int actorId, int userId, string itemTypeId, int itemId)
        {
            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "GetComments",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@itemTypeId", itemTypeId),
                new SqlParameter("@itemId", itemId));
        }

        public static void AddComment(int actorId, string itemTypeId, int itemId,
            string commentText, int severityId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "AddComment",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@itemTypeId", itemTypeId),
                new SqlParameter("@itemId", itemId),
                new SqlParameter("@commentText", commentText),
                new SqlParameter("@severityId", severityId));
        }

        public static void DeleteComment(int actorId, int commentId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                ObjectQualifier + "DeleteComment",
                new SqlParameter("@actorId", actorId),
                new SqlParameter("@commentId", commentId));
        }
        #endregion

        #region Helper Methods
        private static string VerifyColumnName(string str)
        {
            if (str == null)
                str = "";
            return Regex.Replace(str, @"[^\w\. ]", "");
        }

        private static string VerifyColumnValue(string str)
        {
            return String.IsNullOrEmpty(str) ? str : str.Replace("'", "''");
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
        #endregion

		#region Exchange Server
        

		public static int AddExchangeAccount(int itemId, int accountType, string accountName,
            string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
            string mailboxManagerActions, string samAccountName, string accountPassword)
		{
			SqlParameter outParam = new SqlParameter("@AccountID", SqlDbType.Int);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"AddExchangeAccount",
				outParam,
				new SqlParameter("@ItemID", itemId),
				new SqlParameter("@AccountType", accountType),
				new SqlParameter("@AccountName", accountName),
				new SqlParameter("@DisplayName", displayName),
				new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress),
				new SqlParameter("@MailEnabledPublicFolder", mailEnabledPublicFolder),
                new SqlParameter("@MailboxManagerActions", mailboxManagerActions),
                new SqlParameter("@SamAccountName", samAccountName),
                new SqlParameter("@AccountPassword", accountPassword)
			);

			return Convert.ToInt32(outParam.Value);
		}

		public static void AddExchangeAccountEmailAddress(int accountId, string emailAddress)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"AddExchangeAccountEmailAddress",
				new SqlParameter("@AccountID", accountId),
				new SqlParameter("@EmailAddress", emailAddress)
			);
		}

		public static void AddExchangeOrganization(int itemId, string organizationId)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"AddExchangeOrganization",
				new SqlParameter("@ItemID", itemId),
				new SqlParameter("@OrganizationID", organizationId)
			);
		}

		public static void AddExchangeOrganizationDomain(int itemId, int domainId, bool isHost)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"AddExchangeOrganizationDomain",
				new SqlParameter("@ItemID", itemId),
				new SqlParameter("@DomainID", domainId),
				new SqlParameter("@IsHost", isHost)
			);
		}

		public static IDataReader GetExchangeOrganizationStatistics(int itemId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"GetExchangeOrganizationStatistics",
				new SqlParameter("@ItemID", itemId)
			);
		}

        public static void DeleteUserEmailAddresses(int accountId, string primaryAddress)
        {
            SqlHelper.ExecuteNonQuery(
                ConnectionString,
                CommandType.StoredProcedure,
                "DeleteUserEmailAddresses",                
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@PrimaryEmailAddress", primaryAddress)
            );
        }
		
        public static void DeleteExchangeAccount(int itemId, int accountId)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"DeleteExchangeAccount",
				new SqlParameter("@ItemID", itemId),
				new SqlParameter("@AccountID", accountId)
			);
		}

		public static void DeleteExchangeAccountEmailAddress(int accountId, string emailAddress)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"DeleteExchangeAccountEmailAddress",
				new SqlParameter("@AccountID", accountId),
				new SqlParameter("@EmailAddress", emailAddress)
			);
		}

		public static void DeleteExchangeOrganization(int itemId)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"DeleteExchangeOrganization",
				new SqlParameter("@ItemID", itemId)
			);
		}

		public static void DeleteExchangeOrganizationDomain(int itemId, int domainId)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"DeleteExchangeOrganizationDomain",
				new SqlParameter("@ItemId", itemId),
				new SqlParameter("@DomainID", domainId)
			);
		}

		public static bool ExchangeAccountEmailAddressExists(string emailAddress)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ExchangeAccountEmailAddressExists",
				new SqlParameter("@EmailAddress", emailAddress),
				outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

        public static bool ExchangeOrganizationDomainExists(int domainId)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ExchangeOrganizationDomainExists",
				new SqlParameter("@DomainID", domainId),
				outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

		public static bool ExchangeOrganizationExists(string organizationId)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ExchangeOrganizationExists",
				new SqlParameter("@OrganizationID", organizationId),
				outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

		public static bool ExchangeAccountExists(string accountName)
		{
			SqlParameter outParam = new SqlParameter("@Exists", SqlDbType.Bit);
			outParam.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"ExchangeAccountExists",
				new SqlParameter("@AccountName", accountName),
				outParam
			);

			return Convert.ToBoolean(outParam.Value);
		}

		public static void UpdateExchangeAccount(int accountId, string accountName, ExchangeAccountType accountType,
            string displayName, string primaryEmailAddress, bool mailEnabledPublicFolder,
            string mailboxManagerActions, string samAccountName, string accountPassword)
		{
			SqlHelper.ExecuteNonQuery(
				ConnectionString,
				CommandType.StoredProcedure,
				"UpdateExchangeAccount",
				new SqlParameter("@AccountID", accountId),
				new SqlParameter("@AccountName", accountName),
				new SqlParameter("@DisplayName", displayName),
                new SqlParameter("@AccountType", (int)accountType),
				new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress),
				new SqlParameter("@MailEnabledPublicFolder", mailEnabledPublicFolder),
                new SqlParameter("@MailboxManagerActions", mailboxManagerActions),
                new SqlParameter("@Password", string.IsNullOrEmpty(accountPassword) ? (object)DBNull.Value : (object)accountPassword),
                new SqlParameter("@SamAccountName", samAccountName)

			);
		}

		public static IDataReader GetExchangeAccount(int itemId, int accountId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"GetExchangeAccount",
				new SqlParameter("@ItemID", itemId),
				new SqlParameter("@AccountID", accountId)
			);
		}

		public static IDataReader GetExchangeAccountEmailAddresses(int accountId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"GetExchangeAccountEmailAddresses",
				new SqlParameter("@AccountID", accountId)
			);
		}

		public static IDataReader GetExchangeOrganizationDomains(int itemId)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"GetExchangeOrganizationDomains",
				new SqlParameter("@ItemID", itemId)
			);
		}

        
        public static IDataReader GetExchangeAccounts(int itemId, int accountType)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"GetExchangeAccounts",
				new SqlParameter("@ItemID", itemId),
				new SqlParameter("@AccountType", accountType)
			);
		}

        public static IDataReader GetExchangeMailboxes(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetExchangeMailboxes",
                new SqlParameter("@ItemID", itemId)
            );
        }

		public static DataSet GetExchangeAccountsPaged(int actorId, int itemId, string accountTypes,
                string filterColumn, string filterValue, string sortColumn, int startRow, int maximumRows)
		{
            // check input parameters
            string[] types = accountTypes.Split(',');
            for (int i = 0; i < types.Length; i++)
            {
                try
                {
                    int type = Int32.Parse(types[i]);
                }
                catch
                {
                    throw new ArgumentException("Wrong patameter", "accountTypes");
                }
            }

            string searchTypes = String.Join(",", types);

			return SqlHelper.ExecuteDataset(
				ConnectionString,
				CommandType.StoredProcedure,
				"GetExchangeAccountsPaged",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ItemID", itemId),
                new SqlParameter("@AccountTypes", searchTypes),
				new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
				new SqlParameter("@StartRow", startRow),
				new SqlParameter("@MaximumRows", maximumRows)
			);
		}

		public static IDataReader SearchExchangeAccounts(int actorId, int itemId, bool includeMailboxes,
                bool includeContacts, bool includeDistributionLists, bool includeRooms, bool includeEquipment,
                string filterColumn, string filterValue, string sortColumn)
		{
			return SqlHelper.ExecuteReader(
				ConnectionString,
				CommandType.StoredProcedure,
				"SearchExchangeAccounts",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@ItemID", itemId),
				new SqlParameter("@IncludeMailboxes", includeMailboxes),
				new SqlParameter("@IncludeContacts", includeContacts),
				new SqlParameter("@IncludeDistributionLists", includeDistributionLists),
                new SqlParameter("@IncludeRooms", includeRooms),
                new SqlParameter("@IncludeEquipment", includeEquipment),
				new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
				new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
				new SqlParameter("@SortColumn", VerifyColumnName(sortColumn))
			);
		}

        public static IDataReader SearchExchangeAccount(int actorId, int accountType, string primaryEmailAddress)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "SearchExchangeAccount",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@AccountType", accountType),
                new SqlParameter("@PrimaryEmailAddress", primaryEmailAddress)
            );
        }

		#endregion

        #region Organizations

        public static void DeleteOrganizationUser(int itemId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeleteOrganizationUsers", new SqlParameter("@ItemID", itemId));
        }
        
        public static int GetItemIdByOrganizationId(string id)
        {            
            object obj =SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetItemIdByOrganizationId",
                                    new SqlParameter("@OrganizationId", id));

            return (obj == null || DBNull.Value == obj) ? 0 : (int)obj;
            
        }
        
        public static IDataReader GetOrganizationStatistics(int itemId)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOrganizationStatistics",
                new SqlParameter("@ItemID", itemId)
            );
        }        

        public static IDataReader SearchOrganizationAccounts(int actorId, int itemId,
                string filterColumn, string filterValue, string sortColumn, bool includeMailboxes)
        {
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "SearchOrganizationAccounts",
                new SqlParameter("@ActorID", actorId),
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                new SqlParameter("@IncludeMailboxes", includeMailboxes)
            );
        }

        #endregion

        #region CRM
            
        public static int GetCRMUsersCount(int itemId, string name, string email)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    GetFilterSqlParam("@Name", name),
                    GetFilterSqlParam("@Email", email),
                };

            return (int) SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetCRMUsersCount", sqlParams);

        }
        
        private static SqlParameter GetFilterSqlParam(string paramName, string value)
        {
            if (string.IsNullOrEmpty(value))
                return new SqlParameter(paramName, DBNull.Value);

            return new SqlParameter(paramName, value); 
        }

        public static IDataReader GetCrmUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count )
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),                    
                    GetFilterSqlParam("@Name", name),
                    GetFilterSqlParam("@Email", email),                    
                    new SqlParameter("@StartRow", startRow),
                    new SqlParameter("Count", count)
                };

            
            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetCRMUsers", sqlParams);
        }

        public static IDataReader GetCRMOrganizationUsers(int itemId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "GetCRMOrganizationUsers",
                                           new SqlParameter[] {new SqlParameter("@ItemID", itemId)});
        }

        public static void CreateCRMUser(int itemId, Guid crmId, Guid businessUnitId)
        {
            SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "InsertCRMUser",
                                    new SqlParameter[]
                                        {
                                            new SqlParameter("@ItemID", itemId),
                                            new SqlParameter("@CrmUserID", crmId),
                                            new SqlParameter("@BusinessUnitId", businessUnitId)
                                        });
                
        }


        public static IDataReader GetCrmUser(int itemId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "GetCRMUser",
                                    new SqlParameter[]
                                        {
                                            new SqlParameter("@AccountID", itemId)
                                        });
            return reader;
        
        }

        public static int GetCrmUserCount(int itemId)
        {
            return (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetOrganizationCRMUserCount", 
                new SqlParameter[] 
                { new SqlParameter("@ItemID",itemId)});
        }

        public static void DeleteCrmOrganization(int organizationId)
        {
            SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "DeleteCRMOrganization",
                                    new SqlParameter[] { new SqlParameter("@ItemID", organizationId) });
        }
        
        #endregion 

        #region VPS - Virtual Private Servers

        public static IDataReader GetVirtualMachinesPaged(int actorId, int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetVirtualMachinesPaged",
                                        new SqlParameter("@ActorID", actorId),
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@StartRow", startRow),
                                        new SqlParameter("@MaximumRows", maximumRows),
                                        new SqlParameter("@Recursive", recursive));
            return reader;
        }
        #endregion

        #region VPS - External Network

        public static IDataReader GetUnallottedIPAddresses(int packageId, int serviceId, int poolId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetUnallottedIPAddresses",
                                        new SqlParameter("@PackageId", packageId),
                                        new SqlParameter("@ServiceId", serviceId),
                                        new SqlParameter("@PoolId", poolId));
        }


        public static void AllocatePackageIPAddresses(int packageId, string xml)
        {
            SqlParameter[] param = new[]
                                       {
                                           new SqlParameter("@PackageID", packageId),
                                           new SqlParameter("@xml", xml)
                                       };

            ExecuteLongNonQuery("AllocatePackageIPAddresses", param);
        }

        public static IDataReader GetPackageIPAddresses(int packageId, int poolId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows, bool recursive)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetPackageIPAddresses",
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@PoolId", poolId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@startRow", startRow),
                                        new SqlParameter("@maximumRows", maximumRows),
                                        new SqlParameter("@Recursive", recursive));
            return reader;
        }


        public static void DeallocatePackageIPAddress(int id)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "DeallocatePackageIPAddress",
                                      new SqlParameter("@PackageAddressID", id));
        }
        #endregion

        #region VPS - Private Network

        public static IDataReader GetPackagePrivateIPAddressesPaged(int packageId, string filterColumn, string filterValue,
            string sortColumn, int startRow, int maximumRows)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetPackagePrivateIPAddressesPaged",
                                        new SqlParameter("@PackageID", packageId),
                                        new SqlParameter("@FilterColumn", VerifyColumnName(filterColumn)),
                                        new SqlParameter("@FilterValue", VerifyColumnValue(filterValue)),
                                        new SqlParameter("@SortColumn", VerifyColumnName(sortColumn)),
                                        new SqlParameter("@startRow", startRow),
                                        new SqlParameter("@maximumRows", maximumRows));
            return reader;
        }

        public static IDataReader GetPackagePrivateIPAddresses(int packageId)
        {
            IDataReader reader = SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                     "GetPackagePrivateIPAddresses",
                                        new SqlParameter("@PackageID", packageId));
            return reader;
        }
        #endregion

        #region VPS - External Network Adapter
        public static IDataReader GetPackageUnassignedIPAddresses(int actorId, int packageId, int poolId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                "GetPackageUnassignedIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@PackageID", packageId),
                                new SqlParameter("@PoolId", poolId));
        }

        public static IDataReader GetPackageIPAddress(int packageAddressId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                "GetPackageIPAddress",
                                new SqlParameter("@PackageAddressId", packageAddressId));
        }

        public static IDataReader GetItemIPAddresses(int actorId, int itemId, int poolId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                "GetItemIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PoolID", poolId));
        }

        public static int AddItemIPAddress(int actorId, int itemId, int packageAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "AddItemIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PackageAddressID", packageAddressId));
        }

        public static int SetItemPrimaryIPAddress(int actorId, int itemId, int packageAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "SetItemPrimaryIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PackageAddressID", packageAddressId));
        }

        public static int DeleteItemIPAddress(int actorId, int itemId, int packageAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "DeleteItemIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PackageAddressID", packageAddressId));
        }

        public static int DeleteItemIPAddresses(int actorId, int itemId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "DeleteItemIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId));
        }
        #endregion

        #region VPS - Private Network Adapter
        public static IDataReader GetItemPrivateIPAddresses(int actorId, int itemId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
                                "GetItemPrivateIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId));
        }

        public static int AddItemPrivateIPAddress(int actorId, int itemId, string ipAddress)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "AddItemPrivateIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@IPAddress", ipAddress));
        }

        public static int SetItemPrivatePrimaryIPAddress(int actorId, int itemId, int privateAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "SetItemPrivatePrimaryIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PrivateAddressID", privateAddressId));
        }

        public static int DeleteItemPrivateIPAddress(int actorId, int itemId, int privateAddressId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "DeleteItemPrivateIPAddress",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId),
                                new SqlParameter("@PrivateAddressID", privateAddressId));
        }

        public static int DeleteItemPrivateIPAddresses(int actorId, int itemId)
        {
            return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
                                "DeleteItemPrivateIPAddresses",
                                new SqlParameter("@ActorID", actorId),
                                new SqlParameter("@ItemID", itemId));
        }
        #endregion

        #region BlackBerry
        
        public static void AddBlackBerryUser(int accountId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "AddBlackBerryUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId)
                                          });
        }
     
   
        public static bool CheckBlackBerryUserExists(int accountId)
        {
            int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckBlackBerryUserExists",
                                    new SqlParameter("@AccountID", accountId));
            return res > 0;
        }


        public static IDataReader GetBlackBerryUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),                    
                    GetFilterSqlParam("@Name", name),
                    GetFilterSqlParam("@Email", email),                    
                    new SqlParameter("@StartRow", startRow),
                    new SqlParameter("Count", count)
                };


            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetBlackBerryUsers", sqlParams);
        }

        public static int GetBlackBerryUsersCount(int itemId, string name, string email)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                                           {
                                               new SqlParameter("@ItemID", itemId),
                                               GetFilterSqlParam("@Name", name),
                                               GetFilterSqlParam("@Email", email),
                                           };

            return
                (int)
                SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetBlackBerryUsersCount", sqlParams);
        }

        public static void DeleteBlackBerryUser(int accountId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "DeleteBlackBerryUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId)
                                          });
            
        }

        #endregion

        #region OCS

        public static void AddOCSUser(int accountId, string instanceId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "AddOCSUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId),
                                              new SqlParameter("@InstanceID", instanceId)
                                          });
        }


        public static bool CheckOCSUserExists(int accountId)
        {
            int res = (int)SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "CheckOCSUserExists",
                                    new SqlParameter("@AccountID", accountId));
            return res > 0;
        }


        public static IDataReader GetOCSUsers(int itemId, string sortColumn, string sortDirection, string name, string email, int startRow, int count)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@SortColumn", sortColumn),
                    new SqlParameter("@SortDirection", sortDirection),                    
                    GetFilterSqlParam("@Name", name),
                    GetFilterSqlParam("@Email", email),                    
                    new SqlParameter("@StartRow", startRow),
                    new SqlParameter("Count", count)
                };


            return SqlHelper.ExecuteReader(
                ConnectionString,
                CommandType.StoredProcedure,
                "GetOCSUsers", sqlParams);
        }

        public static int GetOCSUsersCount(int itemId, string name, string email)
        {
            SqlParameter[] sqlParams = new SqlParameter[]
                                           {
                                               new SqlParameter("@ItemID", itemId),
                                               GetFilterSqlParam("@Name", name),
                                               GetFilterSqlParam("@Email", email),
                                           };

            return
                (int)
                SqlHelper.ExecuteScalar(ConnectionString, CommandType.StoredProcedure, "GetOCSUsersCount", sqlParams);
        }

        public static void DeleteOCSUser(string instanceId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "DeleteOCSUser",
                                      new[]
                                          {                                              
                                              new SqlParameter("@InstanceId", instanceId)
                                          });

        }

        public static string GetOCSUserInstanceID(int accountId)
        {
            return (string)SqlHelper.ExecuteScalar(ConnectionString,
                                      CommandType.StoredProcedure,
                                      "GetInstanceID",
                                      new[]
                                          {                                              
                                              new SqlParameter("@AccountID", accountId)
                                          });
        }

        #endregion

		#region SSL
		public static int AddSSLRequest(int actorId, int packageId, int siteID, int userID, string friendlyname, string hostname, string csr, int csrLength, string distinguishedName, bool isRenewal, int previousID)
		{
			SqlParameter prmId = new SqlParameter("@SSLID", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "AddSSLRequest", prmId,
				new SqlParameter("@ActorId", actorId),
				new SqlParameter("@PackageId", packageId),
				new SqlParameter("@UserID", userID),
				new SqlParameter("@WebSiteID", siteID),
				new SqlParameter("@FriendlyName", friendlyname),
				new SqlParameter("@HostName", hostname),
				new SqlParameter("@CSR", csr),
				new SqlParameter("@CSRLength", csrLength),
				new SqlParameter("@DistinguishedName", distinguishedName),
				new SqlParameter("@IsRenewal", isRenewal),
				new SqlParameter("@PreviousId", previousID)
				);
			return Convert.ToInt32(prmId.Value);

		}

		public static void CompleteSSLRequest(int actorId, int packageId, int id, string certificate, string distinguishedName, string serialNumber, byte[] hash, DateTime validFrom, DateTime expiryDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "CompleteSSLRequest",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@PackageID", packageId),
				new SqlParameter("@ID", id),
				new SqlParameter("@DistinguishedName", distinguishedName),
				new SqlParameter("@Certificate", certificate),
				new SqlParameter("@SerialNumber", serialNumber),
				new SqlParameter("@Hash", Convert.ToBase64String(hash)),
				new SqlParameter("@ValidFrom", validFrom),
				new SqlParameter("@ExpiryDate", expiryDate));

		}

		public static void AddPFX(int actorId, int packageId, int siteID, int userID, string hostname, string friendlyName, string distinguishedName, int csrLength, string serialNumber, DateTime validFrom, DateTime expiryDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "AddPFX",
				new SqlParameter("@ActorId", actorId),
				new SqlParameter("@PackageId", packageId),
				new SqlParameter("@UserID", userID),
				new SqlParameter("@WebSiteID", siteID),
				new SqlParameter("@FriendlyName", friendlyName),
				new SqlParameter("@HostName", hostname),
				new SqlParameter("@CSRLength", csrLength),
				new SqlParameter("@DistinguishedName", distinguishedName),
				new SqlParameter("@SerialNumber", serialNumber),
				new SqlParameter("@ValidFrom", validFrom),
				new SqlParameter("@ExpiryDate", expiryDate));

		}

		public static DataSet GetSSL(int actorId, int packageId, int id)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "GetSSL",
				new SqlParameter("@SSLID", id));

		}

		public static DataSet GetCertificatesForSite(int actorId, int packageId, int siteId)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "GetCertificatesForSite",
				new SqlParameter("@ActorId", actorId),
				new SqlParameter("@PackageId", packageId),
				new SqlParameter("@websiteid", siteId));

		}

		public static DataSet GetPendingCertificates(int actorId, int packageId, int id, bool recursive)
		{
			return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "GetPendingSSLForWebsite",
				new SqlParameter("@ActorId", actorId),
				new SqlParameter("@PackageId", packageId),
				new SqlParameter("@websiteid", id),
				new SqlParameter("@Recursive", recursive));

		}

		public static IDataReader GetSSLCertificateByID(int actorId, int id)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "GetSSLCertificateByID",
				new SqlParameter("@ActorId", actorId),
				new SqlParameter("@ID", id));
		}

		public static int CheckSSL(int siteID, bool renewal)
		{
			SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Int);
			prmId.Direction = ParameterDirection.Output;

			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "CheckSSL",
				prmId,
				new SqlParameter("@siteID", siteID),
				new SqlParameter("@Renewal", renewal));

			return Convert.ToInt32(prmId.Value);
		}

		public static IDataReader GetSiteCert(int actorId, int siteID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "GetSSLCertificateByID",
				new SqlParameter("@ActorId", actorId),
				new SqlParameter("@ID", siteID));
		}

		public static void DeleteCertificate(int actorId, int packageId, int id)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "DeleteCertificate",
				new SqlParameter("@ActorID", actorId),
				new SqlParameter("@PackageID", packageId),
				new SqlParameter("@id", id));
		}

		public static bool CheckSSLExistsForWebsite(int siteId)
		{
			SqlParameter prmId = new SqlParameter("@Result", SqlDbType.Bit);
			prmId.Direction = ParameterDirection.Output;
			SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure,
				ObjectQualifier + "CheckSSLExistsForWebsite", prmId,
				new SqlParameter("@siteID", siteId),
				new SqlParameter("@SerialNumber", ""));
			return Convert.ToBoolean(prmId.Value);
		}
		#endregion
    }
}
