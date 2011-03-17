using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers.Virtualization
{
    public class VMInfo : ServiceProviderItem
    {
        [Persistent]
        public  int VmId {get; set; }

        [Persistent]
        public Guid VmGuid { get; set; }

        [Persistent]
        public Guid TemplateId { get; set; }
        [Persistent]
        public string TemplateName { get; set; }
        [Persistent]
        public string CurrentTaskId { get; set; }
        [Persistent]
        public string VmPath { get; set; }
        [Persistent]
        public string ProductKey { get; set; }
        [Persistent]
        public string HostName { get; set; }
        [Persistent]
        public string ComputerName { get; set; }
        [Persistent]
        public string Owner { get; set; }
        [Persistent]
        public string AdminUserName { get; set; }
        [Persistent]
        public string AdminPassword { get; set; }
        public string JoinDomain { get; set; }
        public string JoinDomainUserName { get; set; }
        public string JoinDomainPassword { get; set; }
        public bool MergeAnswerFile { get; set; }
        [Persistent]
        public int CPUCount { get; set; }
        [Persistent]
        public int CPUUtilization { get; set; }
        public int PerfCPUUtilization { get; set; }

        [Persistent]
        public int Memory { get; set; }

        public int ProcessMemory { get; set; }

        [Persistent]
        public bool NumLockEnabled { get; set; }
        [Persistent]
        public bool DvdDriver { get; set; }
        [Persistent]
        public int HddSize { get; set; }
        [Persistent]
        public int SnapshotsNumber { get; set; }
        [Persistent]
        public bool BootFromCD { get; set; }

        public LogicalDisk[] HddLogicalDisks { get; set; }

        [Persistent]
        public bool ExternalNetworkEnabled { get; set; }
        [Persistent]
        public string ExternalNicMacAddress { get; set; }
        [Persistent]
        public string ExternalVirtualNetwork { get; set; }
        [Persistent]
        public string ExternalNetworkLocation { get; set; }

        [Persistent]
        public bool PrivateNetworkEnabled { get; set; }
        [Persistent]
        public string PrivateNicMacAddress { get; set; }
        [Persistent]
        public string PrivateVirtualNetwork { get; set; }
        [Persistent]
        public ushort PrivateVLanID { get; set; }
        [Persistent]
        public string PrivateNetworkLocation { get; set; }

        [Persistent]
        public bool ManagementNetworkEnabled { get; set; }
        [Persistent]
        public string ManagementNicMacAddress { get; set; }

        [Persistent]
        public VirtualMachineProvisioningStatus ProvisioningStatus { get; set; }
        [Persistent]
        public VMComputerSystemStateInfo State { get; set; }

        public string ModifiedTime { get; set; }

        // User configuration
        [Persistent]
        public bool StartTurnOffAllowed { get; set; }
        [Persistent]
        public bool PauseResumeAllowed { get; set; }
        [Persistent]
        public bool RebootAllowed { get; set; }
        [Persistent]
        public bool ResetAllowed { get; set; }
        [Persistent]
        public bool ReinstallAllowed { get; set; }

        [Persistent]
        public ConcreteJob CurrentJob { get; set; }

        public string exMessage { get; set; }

        public string logMessage { get; set; }
    }
}
