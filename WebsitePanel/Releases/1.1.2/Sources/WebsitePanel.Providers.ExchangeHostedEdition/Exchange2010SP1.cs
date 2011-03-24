using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsitePanel.Providers.ExchangeHostedEdition;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using WebsitePanel.Providers.Utils;
using System.Security;
using System.Reflection;
using System.IO;
using Microsoft.Exchange.Data;

namespace WebsitePanel.Providers.ExchangeHostedEdition
{
    public class Exchange2010SP1 : HostingServiceProviderBase, IExchangeHostedEdition
    {
        #region Constants
        private const string ExchangeSnapInName = "Microsoft.Exchange.Management.PowerShell.E2010";
        private const string ExchangeRegistryPath = "SOFTWARE\\Microsoft\\ExchangeServer\\v14\\Setup";
        private const string OrganizationDefaultLocation = "en-us";
        private const string OrganizationManagementGroup = "Organization Management";
        private const string RecipientQuotaPolicyIdentity = "Recipient Quota Policy";
        #endregion

        #region Static constructor
        static Exchange2010SP1()
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveExchangeAssembly);
		}
		#endregion

        public void CreateOrganization(string organizationId, string programId, string offerId, string domain,
            string adminName, string adminEmail, string adminPassword)
        {
            ExchangeLog.LogStart("CreateOrganization");
            ExchangeLog.DebugInfo("organizationId: {0}", organizationId);
            ExchangeLog.DebugInfo("programId: {0}", programId);
            ExchangeLog.DebugInfo("offerId: {0}", offerId);
            ExchangeLog.DebugInfo("domain: {0}", domain);
            ExchangeLog.DebugInfo("adminEmail: {0}", adminEmail);

            bool organizationCreated = false;
            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                #region Create new organization
                ExchangeLog.LogStart("New-Organization");

                Command cmd = new Command("New-Organization");
                cmd.Parameters.Add("Name", organizationId);
                cmd.Parameters.Add("ProgramId", programId);
                cmd.Parameters.Add("OfferId", offerId);
                cmd.Parameters.Add("DomainName", domain);
                cmd.Parameters.Add("Location", OrganizationDefaultLocation);

                // run command and get DN of created organization
                Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);
                string organizationDN = GetResultObjectDN(result);
                ExchangeLog.LogInfo("Organization DN: {0}", organizationDN);
                ExchangeLog.LogEnd("New-Organization");
                organizationCreated = true;
                #endregion

                #region Create administrator mailbox
                ExchangeLog.LogStart("New-Mailbox");

                SecureString secureAdminPassword = SecurityUtils.ConvertToSecureString(adminPassword);
                cmd = new Command("New-Mailbox");
                cmd.Parameters.Add("Organization", organizationId);
                cmd.Parameters.Add("Name", adminName);
                cmd.Parameters.Add("UserPrincipalName", adminEmail);
                cmd.Parameters.Add("Password", secureAdminPassword);

                // run command and get DN of created mailbox
                result = ExecuteShellCommand(runSpace, cmd);
                string adminDN = GetResultObjectDN(result);
                ExchangeLog.LogInfo("Administrator account DN: {0}", adminDN);
                ExchangeLog.LogEnd("New-Mailbox");
                #endregion

                #region Add admin account to "Organization Management" group
                ExchangeLog.LogStart("Add-RoleGroupMember");

                cmd = new Command("Add-RoleGroupMember");
                cmd.Parameters.Add("Identity", GetOrganizationManagementGroupDN(organizationDN));
                cmd.Parameters.Add("Member", adminDN);
                cmd.Parameters.Add("BypassSecurityGroupManagerCheck", true);

                // run command
                ExecuteShellCommand(runSpace, cmd);
                ExchangeLog.LogEnd("Add-RoleGroupMember");
                #endregion

            }
            catch(Exception ex)
            {
                // delete organization if it was created
                if (organizationCreated)
                    DeleteOrganizationInternal(runSpace, organizationId);

                throw ex;
            }
            finally
            {
                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("CreateOrganization");
        }

        public List<ExchangeOrganizationDomain> GetOrganizationDomains(string organizationId)
        {
            // get accepted domains
            ExchangeLog.LogStart("GetOrganizationDomains");
            ExchangeLog.DebugInfo("organizationId: {0}", organizationId);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                // get the list of org domains
                return GetOrganizationDomains(runSpace, organizationId);
            }
            finally
            {
                CloseRunspace(runSpace);
            }
        }

        private List<ExchangeOrganizationDomain> GetOrganizationDomains(Runspace runSpace, string organizationId)
        {
            ExchangeLog.LogStart("Get-AcceptedDomain");

            Command cmd = new Command("Get-AcceptedDomain");
            cmd.Parameters.Add("Organization", organizationId);

            // run command
            Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);

            List<ExchangeOrganizationDomain> domains = new List<ExchangeOrganizationDomain>();
            foreach (PSObject objDomain in result)
            {
                ExchangeOrganizationDomain domain = new ExchangeOrganizationDomain();
                domain.Identity = ObjToString(GetPSObjectProperty(objDomain, "Identity"));
                domain.Name = (string)GetPSObjectProperty(objDomain, "Name");
                domain.IsDefault = (bool)GetPSObjectProperty(objDomain, "Default");
                domains.Add(domain);
            }

            ExchangeLog.LogEnd("Get-AcceptedDomain");
            return domains;
        }

        public void AddOrganizationDomain(string organizationId, string domain)
        {
 	        // add accepted domain
            ExchangeLog.LogStart("AddOrganizationDomain");
            ExchangeLog.DebugInfo("organizationId: {0}", organizationId);
            ExchangeLog.DebugInfo("domain: {0}", domain);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                ExchangeLog.LogStart("New-AcceptedDomain");

                Command cmd = new Command("New-AcceptedDomain");
                cmd.Parameters.Add("Organization", organizationId);
                cmd.Parameters.Add("Name", domain);
                cmd.Parameters.Add("DomainName", domain);

                // run command
                ExecuteShellCommand(runSpace, cmd);
                ExchangeLog.LogEnd("New-AcceptedDomain");
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            // add domain to catch-alls configuration file
            AddCatchAllOrganizationDomain(organizationId, domain);

            ExchangeLog.LogEnd("AddOrganizationDomain");
        }

        public void DeleteOrganizationDomain(string organizationId, string domain)
        {
            // remove accepted domain
            ExchangeLog.LogStart("DeleteOrganizationDomain");
            ExchangeLog.DebugInfo("organizationId: {0}", organizationId);
            ExchangeLog.DebugInfo("domain: {0}", domain);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                ExchangeLog.LogStart("Remove-AcceptedDomain");

                Command cmd = new Command("Remove-AcceptedDomain");
                cmd.Parameters.Add("Identity", String.Format("{0}\\{1}", organizationId, domain));
                cmd.Parameters.Add("Confirm", false);

                // run command
                ExecuteShellCommand(runSpace, cmd);
                ExchangeLog.LogEnd("Remove-AcceptedDomain");
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            // remove domain from catch-alls configuration file
            RemoveCatchAllOrganizationDomain(organizationId, domain);

            ExchangeLog.LogEnd("DeleteOrganizationDomain");
        }

        public ExchangeOrganization GetOrganizationDetails(string organizationId)
        {
 	        // get organization details
            ExchangeLog.LogStart("GetOrganizationDetails");
            ExchangeLog.DebugInfo("organizationId: {0}", organizationId);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                // create organization details object
                ExchangeOrganization org = new ExchangeOrganization { Name = organizationId };

                #region Get organization details
                Collection<PSObject> result = GetOrganizationDetailsInternal(runSpace, organizationId);
                if (result.Count == 0)
                {
                    ExchangeLog.LogWarning("Organization '{0}' was not found", organizationId);
                    return null;
                }

                PSObject objOrg = result[0];
                org.DistinguishedName = (string)GetPSObjectProperty(objOrg, "DistinguishedName");
                org.ServicePlan = (string)GetPSObjectProperty(objOrg, "ServicePlan");
                org.ProgramId = (string)GetPSObjectProperty(objOrg, "ProgramID");
                org.OfferId = (string)GetPSObjectProperty(objOrg, "OfferID");
                #endregion

                #region Get organization quotas
                ExchangeLog.LogStart("Get-RecipientEnforcementProvisioningPolicy");

                Command cmd = new Command("Get-RecipientEnforcementProvisioningPolicy");
                cmd.Parameters.Add("Identity", String.Format("{0}\\{1}", organizationId, RecipientQuotaPolicyIdentity));

                // run command
                result = ExecuteShellCommand(runSpace, cmd);
                if (result.Count == 0)
                    throw new NullReferenceException(String.Format("Recipient quota policy for organization '{0}' was not found", organizationId));

                PSObject objQuota = result[0];
                //ExchangeLog.LogInfo(GetPSObjectProperty(objQuota, "MailboxCountQuota").GetType().ToString());
                org.MailboxCountQuota = ConvertUnlimitedToInt32((Unlimited<Int32>)GetPSObjectProperty(objQuota, "MailboxCountQuota"));
                org.ContactCountQuota = ConvertUnlimitedToInt32((Unlimited<Int32>)GetPSObjectProperty(objQuota, "ContactCountQuota"));
                org.DistributionListCountQuota = ConvertUnlimitedToInt32((Unlimited<Int32>)GetPSObjectProperty(objQuota, "DistributionListCountQuota"));

                ExchangeLog.LogEnd("Get-RecipientEnforcementProvisioningPolicy");
                #endregion

                #region Get organization statistics

                // mailboxes
                ExchangeLog.LogStart("Get-Mailbox");
                cmd = new Command("Get-Mailbox");
                cmd.Parameters.Add("Organization", organizationId);
                org.MailboxCount = ExecuteShellCommand(runSpace, cmd).Count;

                // remove system "DiscoverySearchMailbox" from statistics
                //if (org.MailboxCount > 0)
                //    org.MailboxCount -= 1;

                ExchangeLog.LogEnd("Get-Mailbox");

                // contacts
                ExchangeLog.LogStart("Get-Contact");
                cmd = new Command("Get-Contact");
                cmd.Parameters.Add("Organization", organizationId);
                org.ContactCount = ExecuteShellCommand(runSpace, cmd).Count;
                ExchangeLog.LogEnd("Get-Contact");

                // distribution lists
                ExchangeLog.LogStart("Get-DistributionGroup");
                cmd = new Command("Get-DistributionGroup");
                cmd.Parameters.Add("Organization", organizationId);
                org.DistributionListCount = ExecuteShellCommand(runSpace, cmd).Count;
                ExchangeLog.LogEnd("Get-DistributionGroup");
                #endregion

                #region Get domains
                org.Domains = GetOrganizationDomains(runSpace, organizationId).ToArray();
                #endregion

                #region Administrator e-mail
                ExchangeLog.LogStart("Get-RoleGroupMember");
                cmd = new Command("Get-RoleGroupMember");
                cmd.Parameters.Add("Identity", GetOrganizationManagementGroupDN(org.DistinguishedName));
                result = ExecuteShellCommand(runSpace, cmd);

                if (result.Count > 0)
                {
                    org.AdministratorName = (string)GetPSObjectProperty(result[0], "Name");
                    SmtpAddress adminEmail = (SmtpAddress)GetPSObjectProperty(result[0], "PrimarySmtpAddress");
                    org.AdministratorEmail = (adminEmail != null) ? adminEmail.ToString() : null;
                }
                ExchangeLog.LogEnd("Get-RoleGroupMember");
                #endregion

                ExchangeLog.LogEnd("GetOrganizationDetails");
                return org;
            }
            finally
            {
                CloseRunspace(runSpace);
            }
        }

        public void UpdateOrganizationQuotas(string organizationId, int mailboxesNumber, int contactsNumber, int distributionListsNumber)
        {
 	        // update quotas
            ExchangeLog.LogStart("UpdateOrganizationQuotas");
            ExchangeLog.DebugInfo("organizationId: {0}", organizationId);
            ExchangeLog.DebugInfo("mailboxesNumber: {0}", mailboxesNumber);
            ExchangeLog.DebugInfo("contactsNumber: {0}", contactsNumber);
            ExchangeLog.DebugInfo("distributionListsNumber: {0}", distributionListsNumber);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                ExchangeLog.LogStart("Set-RecipientEnforcementProvisioningPolicy");

                Command cmd = new Command("Set-RecipientEnforcementProvisioningPolicy");
                cmd.Parameters.Add("Identity", String.Format("{0}\\{1}", organizationId, RecipientQuotaPolicyIdentity));
                cmd.Parameters.Add("MailboxCountQuota", ConvertInt32ToUnlimited(mailboxesNumber));
                cmd.Parameters.Add("ContactCountQuota", ConvertInt32ToUnlimited(contactsNumber));
                cmd.Parameters.Add("DistributionListCountQuota", ConvertInt32ToUnlimited(distributionListsNumber));

                // run command
                ExecuteShellCommand(runSpace, cmd);
                ExchangeLog.LogEnd("Set-RecipientEnforcementProvisioningPolicy");
            }
            finally
            {
                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("UpdateOrganizationQuotas");
        }

        public void UpdateOrganizationCatchAllAddress(string organizationId, string catchAllEmail)
        {
 	        // update catch-all address
            SetCatchAllAddress(organizationId, catchAllEmail);
        }

        public void UpdateOrganizationServicePlan(string organizationId, string programId, string offerId)
        {
 	        // update service plan
            ExchangeLog.LogStart("UpdateOrganizationServicePlan");
            ExchangeLog.DebugInfo("organizationId: {0}", organizationId);
            ExchangeLog.DebugInfo("programId: {0}", programId);
            ExchangeLog.DebugInfo("offerId: {0}", offerId);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                ExchangeLog.LogStart("Update-ServicePlan");

                Command cmd = new Command("Update-ServicePlan");
                cmd.Parameters.Add("Identity", organizationId);
                cmd.Parameters.Add("ProgramID", programId);
                cmd.Parameters.Add("OfferID", offerId);

                // run command
                ExecuteShellCommand(runSpace, cmd);
                ExchangeLog.LogEnd("Update-ServicePlan");
            }
            finally
            {
                CloseRunspace(runSpace);
            }
            ExchangeLog.LogEnd("UpdateOrganizationServicePlan");
        }

        public void DeleteOrganization(string organizationId)
        {
            ExchangeLog.LogStart("DeleteOrganization");
            ExchangeLog.DebugInfo("organizationId: {0}", organizationId);

            Runspace runSpace = null;
            try
            {
                runSpace = OpenRunspace();

                // delete
                DeleteOrganizationInternal(runSpace, organizationId);
            }
            finally
            {
                CloseRunspace(runSpace);
            }

            // remove all organization domains from catch-alls configuration file
            RemoveCatchAllOrganization(organizationId);

            ExchangeLog.LogEnd("DeleteOrganization");
        }

        private void DeleteOrganizationInternal(Runspace runSpace, string organizationId)
        {
            // get organization mailboxes
            Command cmd = new Command("Get-Mailbox");
            cmd.Parameters.Add("Organization", organizationId);
            Collection<PSObject> result = ExecuteShellCommand(runSpace, cmd);

            // delete organization mailboxes
            foreach (PSObject objMailbox in result)
            {
                string mailboxDN = (string)GetPSObjectProperty(objMailbox, "DistinguishedName");
                ExchangeLog.LogInfo("Deleting mailbox: {0}", mailboxDN);

                cmd = new Command("Remove-Mailbox");
                cmd.Parameters.Add("Identity", mailboxDN);
                cmd.Parameters.Add("Confirm", false);
                ExecuteShellCommand(runSpace, cmd);
            }

            // delete organization
            ExchangeLog.LogStart("Remove-Organization");
            cmd = new Command("Remove-Organization");
            cmd.Parameters.Add("Identity", organizationId);
            cmd.Parameters.Add("Confirm", false);
            ExecuteShellCommand(runSpace, cmd);
            ExchangeLog.LogEnd("Remove-Organization");
        }

        #region Catch-All methods
        private void SetCatchAllAddress(string organizationId, string catchAllEmail)
        {
            // remove catch all
            if (String.IsNullOrEmpty(catchAllEmail))
                RemoveCatchAllOrganization(organizationId);

            // get all organization domains

            // setup catch-all for organization domains
        }
        
        private void AddCatchAllOrganizationDomain(string organizationId, string domain)
        {

        }

        private void RemoveCatchAllOrganizationDomain(string organizationId, string domain)
        {

        }

        private void RemoveCatchAllOrganization(string organizationId)
        {

        }
        #endregion

        #region Private helpers
        private Collection<PSObject> GetOrganizationDetailsInternal(Runspace runSpace, string organizationId)
        {
            Command cmd = new Command("Get-Organization");
            cmd.Parameters.Add("Identity", organizationId);
            return ExecuteShellCommand(runSpace, cmd);
        }

        private string GetOrganizationManagementGroupDN(string organizationDN)
        {
            return String.Format("CN={0},{1}", OrganizationManagementGroup, organizationDN);
        }
        #endregion

        #region ServiceProvider methods
        public override bool IsInstalled()
        {
            int value = 0;
            RegistryKey root = Registry.LocalMachine;
            RegistryKey rk = root.OpenSubKey(ExchangeRegistryPath);
            if (rk != null)
            {
                value = (int)rk.GetValue("MsiProductMajor", null);
                rk.Close();
            }
            return value == 14;
        }
        #endregion

        #region PowerShell integration
        private static RunspaceConfiguration runspaceConfiguration = null;
        private static string ExchangePath = null;

        internal static string GetExchangePath()
        {
            if (string.IsNullOrEmpty(ExchangePath))
            {
                RegistryKey root = Registry.LocalMachine;
                RegistryKey rk = root.OpenSubKey(ExchangeRegistryPath);
                if (rk != null)
                {
                    string value = (string)rk.GetValue("MsiInstallPath", null);
                    rk.Close();
                    if (!string.IsNullOrEmpty(value))
                        ExchangePath = Path.Combine(value, "bin");
                }
            }
            return ExchangePath;
        }

        private static Assembly ResolveExchangeAssembly(object p, ResolveEventArgs args)
        {
            //Add path for the Exchange 2010 DLLs
            if (args.Name.Contains("Microsoft.Exchange"))
            {
                string exchangePath = GetExchangePath();
                if (string.IsNullOrEmpty(exchangePath))
                    return null;

                string path = Path.Combine(exchangePath, args.Name.Split(',')[0] + ".dll");
                if (!File.Exists(path))
                    return null;

                ExchangeLog.DebugInfo("Resolved assembly: {0}", path);
                return Assembly.LoadFrom(path);
            }
            else
            {
                return null;
            }
        }

        internal virtual Runspace OpenRunspace()
        {
            ExchangeLog.LogStart("OpenRunspace");

            if (runspaceConfiguration == null)
            {
                runspaceConfiguration = RunspaceConfiguration.Create();
                PSSnapInException exception = null;

                PSSnapInInfo info = runspaceConfiguration.AddPSSnapIn(ExchangeSnapInName, out exception);

                if (exception != null)
                {
                    ExchangeLog.LogWarning("SnapIn error", exception);
                }
            }
            Runspace runSpace = RunspaceFactory.CreateRunspace(runspaceConfiguration);
            //AdminSessionADSettings adSettings = SetADSettings();
            runSpace.Open();
            //runSpace.SessionStateProxy.SetVariable("AdminSessionADSettings", adSettings);
            runSpace.SessionStateProxy.SetVariable("ConfirmPreference", "none");
            ExchangeLog.LogEnd("OpenRunspace");
            return runSpace;
        }

        internal void CloseRunspace(Runspace runspace)
        {
            try
            {
                if (runspace != null && runspace.RunspaceStateInfo.State == RunspaceState.Opened)
                {
                    runspace.Close();
                }
            }
            catch (Exception ex)
            {
                ExchangeLog.LogError("Runspace error", ex);
            }
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd)
        {
            object[] errors;
            return ExecuteShellCommand(runSpace, cmd, out errors);
        }

        internal Collection<PSObject> ExecuteShellCommand(Runspace runSpace, Command cmd, out object[] errors)
        {
            ExchangeLog.LogStart("ExecuteShellCommand");
            List<object> errorList = new List<object>();

            ExchangeLog.DebugCommand(cmd);
            Collection<PSObject> results = null;
            // Create a pipeline
            Pipeline pipeLine = runSpace.CreatePipeline();
            using (pipeLine)
            {
                // Add the command
                pipeLine.Commands.Add(cmd);
                // Execute the pipeline and save the objects returned.
                results = pipeLine.Invoke();

                // Log out any errors in the pipeline execution
                // NOTE: These errors are NOT thrown as exceptions! 
                // Be sure to check this to ensure that no errors 
                // happened while executing the command.
                if (pipeLine.Error != null && pipeLine.Error.Count > 0)
                {
                    foreach (object item in pipeLine.Error.ReadToEnd())
                    {
                        errorList.Add(item);
                        string errorMessage = string.Format("Invoke error: {0}", item.ToString());
                        ExchangeLog.LogWarning(errorMessage);
                    }

                    throw new Exception(errorList[0].ToString());
                }
            }
            pipeLine = null;
            errors = errorList.ToArray();
            ExchangeLog.LogEnd("ExecuteShellCommand");
            return results;
        }

        /// <summary>
        /// Returns the distinguished name of the object from the shell execution result
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal string GetResultObjectDN(Collection<PSObject> result)
        {
            ExchangeLog.LogStart("GetResultObjectDN");
            if (result == null)
                throw new ArgumentNullException("result", "Execution result is not specified");

            if (result.Count < 1)
                throw new ArgumentException("Execution result does not contain any object");

            if (result.Count > 1)
                throw new ArgumentException("Execution result contains more than one object");

            PSMemberInfo info = result[0].Members["DistinguishedName"];
            if (info == null)
                throw new ArgumentException("Execution result does not contain DistinguishedName property", "result");

            string ret = info.Value.ToString();
            ExchangeLog.LogEnd("GetResultObjectDN");
            return ret;
        }

        internal object GetPSObjectProperty(PSObject obj, string name)
        {
            PSMemberInfo info = obj.Members[name];
            if (info == null)
                throw new ArgumentException(String.Format("PSObject does not contain '{0}' property", name), "obj");

            return info.Value;
        }
        #endregion

        #region Convert Utils
        internal int ConvertUnlimitedToInt32(Unlimited<Int32> value)
        {
            int ret = 0;
            if (value.IsUnlimited)
            {
                ret = -1;
            }
            else
            {
                ret = value.Value;
            }
            return ret;
        }

        internal Unlimited<int> ConvertInt32ToUnlimited(int value)
        {
            if (value == -1)
                return Unlimited<int>.UnlimitedValue;
            else
            {
                Unlimited<int> ret = new Unlimited<int>();
                ret.Value = value;
                return ret;
            }
        }

        internal string ObjToString(object obj)
        {
            string ret = null;
            if (obj != null)
                ret = obj.ToString();
            return ret;
        }
        #endregion
    }
}
