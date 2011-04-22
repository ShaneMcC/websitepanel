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

﻿using System;
using System.Collections.Generic;
using System.Text;
using WebsitePanel.Providers.Utils;
using System.Management;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using WebsitePanel.Server.Utils;
using System.Linq;

using Vds = Microsoft.Storage.Vds;
using System.Configuration;

using WebsitePanel.Providers.Virtualization;
using WebsitePanel.Providers.VirtualizationForPC.SVMMService;
using WebsitePanel.Providers.VirtualizationForPC.MonitoringWebService;

using System.ServiceModel;
using System.ServiceModel.Description;

namespace WebsitePanel.Providers.VirtualizationForPC
{

	public class WSPVirtualMachineManagementServiceClient : VirtualMachineManagementServiceClient, IDisposable
	{
		public WSPVirtualMachineManagementServiceClient()
		{
		}

		public WSPVirtualMachineManagementServiceClient(string endpointConfigurationName) :
			base(endpointConfigurationName)
		{
		}

		public WSPVirtualMachineManagementServiceClient(string endpointConfigurationName, string remoteAddress) :
			base(endpointConfigurationName, remoteAddress)
		{
		}

		public WSPVirtualMachineManagementServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
			base(endpointConfigurationName, remoteAddress)
		{
		}

		public WSPVirtualMachineManagementServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
			base(binding, remoteAddress)
		{
		}

		public void Dispose()
		{
			if ((this.State == CommunicationState.Opened || this.State == CommunicationState.Opening))
				this.Close();
		}
	}

	public class WSPMonitoringServiceClient : MonitoringServiceClient, IDisposable
	{
		public WSPMonitoringServiceClient()
		{
		}

		public WSPMonitoringServiceClient(string endpointConfigurationName) :
			base(endpointConfigurationName)
		{
		}

		public WSPMonitoringServiceClient(string endpointConfigurationName, string remoteAddress) :
			base(endpointConfigurationName, remoteAddress)
		{
		}

		public WSPMonitoringServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
			base(endpointConfigurationName, remoteAddress)
		{
		}

		public WSPMonitoringServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
			base(binding, remoteAddress)
		{
		}

		public void Dispose()
		{
			if ((this.State == CommunicationState.Opened || this.State == CommunicationState.Opening))
				this.Close();
		}
	}

	public class HyperVForPC : HostingServiceProviderBase, IVirtualizationServerForPC
	{
		#region Constants
		private const string CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG = "WebsitePanel.HyperV.UseDiskPartClearReadOnlyFlag";
		private const string WMI_VIRTUALIZATION_NAMESPACE = @"root\virtualization";
		private const string WMI_CIMV2_NAMESPACE = @"root\cimv2";

		private const int SWITCH_PORTS_NUMBER = 1024;
		private const string LIBRARY_INDEX_FILE_NAME = "index.xml";
		private const string EXTERNAL_NETWORK_ADAPTER_NAME = "External Network Adapter";
		private const string PRIVATE_NETWORK_ADAPTER_NAME = "Private Network Adapter";
		private const string MANAGEMENT_NETWORK_ADAPTER_NAME = "Management Network Adapter";

		private const string KVP_RAM_SUMMARY_KEY = "VM-RAM-Summary";
		private const string KVP_HDD_SUMMARY_KEY = "VM-HDD-Summary";
		private const Int64 Size1G = 0x40000000;
		private const Int64 Size1M = 0x100000;

		private const int ByteToGbByte = 1073741824;

		#endregion

		private static Dictionary<string, HostInfo> HostinfoByVMName = new Dictionary<string, HostInfo>();

		#region Provider Settings
		/// <summary>
		/// Gets server name from the provider's settings.
		/// </summary>
		protected string ServerNameSettings
		{
			get { return ProviderSettings["ServerName"]; }
		}

		/// <summary>
		/// Gets an action that should take place automatically when starting a virtual machine.
		/// </summary>
		public int AutomaticStartActionSettings
		{
			get { return ProviderSettings.GetInt("StartAction"); }
		}

		protected string MonitoringServerNameSettings
		{
			get { return ProviderSettings["MonitoringServerName"]; }
		}


		/// <summary>
		/// Gets startup delay that should occur automatically when starting a virtual machine.
		/// </summary>
		public int AutomaticStartupDelaySettings
		{
			get { return ProviderSettings.GetInt("StartupDelay"); }
		}

		/// <summary>
		/// Gets an action that should take place automatically when stopping a virtual machine.
		/// </summary>
		public int AutomaticStopActionSettings
		{
			get { return ProviderSettings.GetInt("StopAction"); }
		}

		/// <summary>
		/// Gets a recorvery action that should take place when recovering a virtual machine (Restart only).
		/// </summary>
		public int AutomaticRecoveryActionSettings
		{
			get { return 1 /* Restart */; }
		}

		/// <summary>
		/// Gets CPU reserve setting.
		/// </summary>
		public int CpuReserveSettings
		{
			get { return ProviderSettings.GetInt("CpuReserve"); }
		}

		/// <summary>
		/// Gets CPU limit setting.
		/// </summary>
		public int CpuLimitSettings
		{
			get { return ProviderSettings.GetInt("CpuLimit"); }
		}

		/// <summary>
		/// Gets CPU weight setting.
		/// </summary>
		public int CpuWeightSettings
		{
			get { return ProviderSettings.GetInt("CpuWeight"); }
		}

		/// <summary>
		/// Gets server type (cluster only).
		/// </summary>
		public string ServerType
		{
			get { return ProviderSettings["ServerType"]; }
		}

		//Hyper-V Cloud
		/// <summary>
		/// Gets a DDCTK endpoint URL of System Center Virtual Machine Manager service to connect to.
		/// </summary>
		protected string SCVMMServer
		{
			get { return ProviderSettings[VMForPCSettingsName.SCVMMServer.ToString()]; }
		}

		/// <summary>
		/// Gets a principal name being used to connect to the System Center Virtual Manager DDCTK endpoint.
		/// </summary>
		protected string SCVMMPrincipalName
		{
			get { return ProviderSettings[VMForPCSettingsName.SCVMMPrincipalName.ToString()]; }
		}

		/// <summary>
		/// Gets a DDCTK endpoint URL of System Center Operations Manager service to connect to.
		/// </summary>
		protected string SCOMServer
		{
			get { return ProviderSettings[VMForPCSettingsName.SCOMServer.ToString()]; }
		}

		/// <summary>
		/// Gets a principal name being used to connect to the System Center Operations Manager DDCTK endpoint.
		/// </summary>
		protected string SCOMPrincipalName
		{
			get { return ProviderSettings[VMForPCSettingsName.SCOMPrincipalName.ToString()]; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected string VPServer
		{
			get { return ProviderSettings[VMForPCSettingsName.VPServer.ToString()]; }
		}

		/// <summary>
		/// Gets a DDCTK endpoint URL of System Center Data Protection Manager service to connect to.
		/// </summary>
		protected string SCDPMServer
		{
			get { return ProviderSettings[VMForPCSettingsName.SCDPMServer.ToString()]; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected string SCDPMEndPoint
		{
			get { return ProviderSettings[VMForPCSettingsName.SCDPMEndPoint.ToString()]; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected string SCCMServer
		{
			get { return ProviderSettings[VMForPCSettingsName.SCCMServer.ToString()]; }
		}

		/// <summary>
		/// Gets a DDCTK endpoint URL of SCCM service to connect to.
		/// </summary>
		protected string SCCMEndPoint
		{
			get { return ProviderSettings[VMForPCSettingsName.SCCMEndPoint.ToString()]; }
		}

		/// <summary>
		/// Gets storage endpoint URL.
		/// </summary>
		protected string StorageEndPoint
		{
			get { return ProviderSettings[VMForPCSettingsName.StorageEndPoint.ToString()]; }
		}

		/// <summary>
		/// Gets an endpoint URL to core service.
		/// </summary>
		protected string CoreSvcEndpoint
		{
			get { return ProviderSettings["CoreSvcEndpoint"]; }
		}

		/// <summary>
		/// Gets a configured path to virtual machines library
		/// </summary>
		protected string LibraryPath
		{
			get { return ProviderSettings["LibraryPath"]; }
		}
		#endregion

		#region Fields
		private Wmi _wmi = null;

		/// <summary>
		/// Gets an instance of wrapper for interaction with Windows Management Instrumentation (lazy initialization).
		/// </summary>
		private Wmi wmi
		{
			get
			{
				if (_wmi == null)
					_wmi = new Wmi(ServerNameSettings, WMI_VIRTUALIZATION_NAMESPACE);
				return _wmi;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Ctor.
		/// </summary>
		public HyperVForPC()
		{
		}
		#endregion

		#region Virtual Machines
		/// <summary>
		/// Gets a virtual machine information by its identificator.
		/// </summary>
		/// <param name="vmId">Virtual machine id, represented by Guid (for example "4664215D-D195-4E35-BB6F-BFC1F17666EB").</param>
		/// <returns>Virtual machine information, such as Name, HostName, Id, State, count of CPUs assigned, 
		/// CreationTime, ComputerName, Owner, Domain name (if assigned), CPU utilization, CPU performance utilization, RAM</returns>
		public VMInfo GetVirtualMachine(string vmId)
		{
			VMInfo vm = new VMInfo();
			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					VirtualMachineInfo vminfo = client.GetVirtualMachineByName(vmId);

					if (vminfo == null)
						throw new InvalidDataException(String.Format("GetVirtualMachineByName for VM {0} return NULL value.", vmId));

					vm.logMessage = String.Format("Current state VM {0} is {1}.", vminfo.Name, vminfo.Status);

					vm.Name = vminfo.Name;
					vm.HostName = vminfo.HostName;
					vm.VmGuid = vminfo.Id;
					vm.State = (Virtualization.VMComputerSystemStateInfo)vminfo.Status;
					vm.CPUCount = vminfo.CPUCount;
					vm.CreatedDate = vminfo.CreationTime;
					vm.ComputerName = vminfo.ComputerName;
					vm.Owner = vminfo.Owner;
					vm.JoinDomain = (vminfo.VMHost == null ? string.Empty : vminfo.VMHost.DomainName);
					vm.CPUUtilization = vminfo.CPUUtilization;
					vm.PerfCPUUtilization = vminfo.perfCPUUtilization;
					vm.ModifiedTime = "00:00:00";
					vm.Memory = vminfo.Memory;
					vm.ProcessMemory = vminfo.Memory;

					if ((vminfo.VirtualHardDisks != null)
					 && (vminfo.VirtualHardDisks.Length > 0))
					{
						vm.HddLogicalDisks = new LogicalDisk[vminfo.VirtualHardDisks.Length];
						for (int i = 0; i < vminfo.VirtualHardDisks.Length; i++)
						{
							vm.HddLogicalDisks[i] = new LogicalDisk();
							var d = vminfo.VirtualHardDisks[i];
							if (d != null)
							{
								vm.HddLogicalDisks[i].Size = (int)(d.MaximumSize / ByteToGbByte);
								vm.HddLogicalDisks[i].FreeSpace = (int)(((long)d.MaximumSize - vminfo.VirtualHardDisks[i].Size) / ByteToGbByte);
								vm.HddLogicalDisks[i].DriveLetter = d.Name;
							}
						}
					}

					vm.ProvisioningStatus = VirtualMachineProvisioningStatus.OK;
				}
			}
			catch (Exception ex)
			{
				vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;
				vm.State = Virtualization.VMComputerSystemStateInfo.CreationFailed;
				vm.exMessage = String.Format("{0} \n {1}", ex.Message, ex.StackTrace);
				//
				Log.WriteError(ex);
			}

			return vm;
		}

		public VirtualMachine GetVirtualMachineEx(string vmId)
		{
			ManagementObject objVm = wmi.GetWmiObject("msvm_ComputerSystem", "Name = '{0}'", vmId);
			if (objVm == null)
				return null;

			// general settings
			VirtualMachine vm = CreateVirtualMachineFromWmiObject(objVm);

			// CPU
			ManagementObject objCpu = wmi.GetWmiObject("Msvm_ProcessorSettingData", "InstanceID Like 'Microsoft:{0}%'", vmId);
			vm.CpuCores = Convert.ToInt32(objCpu["VirtualQuantity"]);

			// RAM
			ManagementObject objRam = wmi.GetWmiObject("Msvm_MemorySettingData", "InstanceID Like 'Microsoft:{0}%'", vmId);
			vm.RamSize = Convert.ToInt32(objRam["VirtualQuantity"]);

			// other settings
			ManagementObject objSettings = GetVirtualMachineSettingsObject(vmId);

			// BIOS (num lock)
			vm.NumLockEnabled = Convert.ToBoolean(objSettings["BIOSNumLock"]);

			// BIOS (boot order)
			// BootOrder = 0 - Boot from floppy, 1 - Boot from CD, 2 - Boot from disk, 3 - PXE Boot 
			UInt16[] bootOrder = (UInt16[])objSettings["BootOrder"];
			vm.BootFromCD = (bootOrder[0] == 1);

			// DVD drive
			ManagementObject objDvd = wmi.GetWmiObject(
				"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Synthetic DVD Drive'"
				+ " and InstanceID Like 'Microsoft:{0}%'", vmId);
			vm.DvdDriveInstalled = (objDvd != null);

			// HDD
			ManagementObject objVhd = wmi.GetWmiObject(
				"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Virtual Hard Disk'"
				+ " and InstanceID like 'Microsoft:{0}%'", vmId);

			if (objVhd != null)
			{
				vm.VirtualHardDrivePath = ((string[])objVhd["Connection"])[0];

				// get VHD size
				WebsitePanel.Providers.Virtualization.VirtualHardDiskInfo vhdInfo = GetVirtualHardDiskInfo(vm.VirtualHardDrivePath);
				if (vhdInfo != null)
					vm.HddSize = Convert.ToInt32(vhdInfo.MaxInternalSize / Size1G);
			}

			// network adapters
			List<VirtualMachineNetworkAdapter> nics = new List<VirtualMachineNetworkAdapter>();
			ManagementObject objVM = GetVirtualMachineObject(vmId);

			// synthetic adapters
			foreach (ManagementObject objNic in wmi.GetWmiObjects("Msvm_SyntheticEthernetPortSettingData", "InstanceID like 'Microsoft:{0}%'", vmId))
				nics.Add(new VirtualMachineNetworkAdapter() { Name = (string)objNic["ElementName"], MacAddress = (string)objNic["Address"] });

			// legacy adapters
			foreach (ManagementObject objNic in wmi.GetWmiObjects("Msvm_EmulatedEthernetPortSettingData", "InstanceID like 'Microsoft:{0}%'", vmId))
				nics.Add(new VirtualMachineNetworkAdapter() { Name = (string)objNic["ElementName"], MacAddress = (string)objNic["Address"] });

			vm.Adapters = nics.ToArray();

			return vm;
		}

		/// <summary>
		/// Gets list of virtual machines on the server
		/// </summary>
		/// <returns></returns>
		public List<VirtualMachine> GetVirtualMachines()
		{
			List<VirtualMachine> vms = new List<VirtualMachine>();

			ManagementObjectCollection objVms = wmi.ExecuteWmiQuery("select * from msvm_ComputerSystem where Name <> ElementName");
			foreach (ManagementObject objVm in objVms)
				vms.Add(CreateVirtualMachineFromWmiObject(objVm));

			return vms;
		}

		/// <summary>
		/// Gets a thumbnail image for the virtual machine (Wrapper).
		/// </summary>
		/// <param name="vmId">Virtual machine id, represented by Guid (for example "4664215D-D195-4E35-BB6F-BFC1F17666EB").</param>
		/// <param name="size">Size of the thumbnail being requested</param>
		/// <returns>Array of bytes representing the virtual machine thumbnail image requested.</returns>
		public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
		{
			return GetTumbnailFromSummaryInformation(vmId, size);
		}

		/// <summary>
		/// Gets a thumbnail image for the virtual machine (Implementation).
		/// </summary>
		/// <param name="vmId">Virtual machine id, represented by Guid (for example "4664215D-D195-4E35-BB6F-BFC1F17666EB").</param>
		/// <param name="size">Size of the thumbnail being requested</param>
		/// <returns>Array of bytes representing the virtual machine thumbnail image requested.</returns>
		private byte[] GetTumbnailFromSummaryInformation(string vmName, ThumbnailSize size)
		{
			int width = 80;
			int height = 60;

			if (size == ThumbnailSize.Medium160x120)
			{
				width = 160;
				height = 120;
			}
			else if (size == ThumbnailSize.Large320x240)
			{
				width = 320;
				height = 240;
			}

			lock (HostinfoByVMName)
			{
				if (!HostinfoByVMName.ContainsKey(vmName))
				{
					using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
					{
						VirtualMachineInfo vminfo = client.GetVirtualMachineByName(vmName);
						if (vminfo != null)
						{
							HostInfo host = client.GetHostById(vminfo.HostId);

							HostinfoByVMName.Add(vmName, host);
						}
					}
				}
			}

			HostInfo hostInfo = null;

			HostinfoByVMName.TryGetValue(vmName, out hostInfo);

			byte[] imgData = null;

			if (hostInfo != null)
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					try
					{
						imgData = client.GetVirtualSystemThumbnailImage(width, height, vmName, hostInfo.ComputerName);
					}
					catch (Exception ex)
					{
						imgData = null;
						//
						Log.WriteError(ex);
					}
				}
			}

			// Create new bitmap
			if (imgData == null)
			{
				using (Bitmap bmp = new Bitmap(width, height))
				{
					Graphics g = Graphics.FromImage(bmp);
					SolidBrush brush = new SolidBrush(Color.LightGray);
					g.FillRectangle(brush, 0, 0, width, height);

					using (MemoryStream stream = new MemoryStream())
					{
						bmp.Save(stream, ImageFormat.Png);
						imgData = stream.ToArray();
					}
				}
			}

			return imgData;
		}

		public VMInfo CreateVirtualMachine(VMInfo vm)
		{
			// Evaluate paths
			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					TemplateInfo selTemplate = client.GetTemplateById(vm.TemplateId);

					HostInfo hostInfo = null;

					if (ServerType.Equals("cluster"))
					{
						HostClusterInfo selCluster = client.GetHostClusterByName(ServerNameSettings);

						if (selCluster.Nodes != null)
						{
							foreach (HostInfo curr in selCluster.Nodes)
							{
								if (curr.AvailableForPlacement)
								{
									hostInfo = curr;
									break;
								}
							}
						}
					}

					if (hostInfo == null)
					{
						try
						{
							hostInfo = client.GetHostByName(String.IsNullOrWhiteSpace(ServerNameSettings)
								? selTemplate.HostName : ServerNameSettings);
						}
						catch (Exception ex)
						{
							hostInfo = null;
							//
							Log.WriteError(ex);
						}
					}

					selTemplate.CPUCount = Convert.ToByte(vm.CPUCount);
					selTemplate.Memory = vm.Memory;
					selTemplate.TotalVHDCapacity = (ulong)(vm.HddSize * ByteToGbByte);

					if (!vm.DvdDriver)
					{
						selTemplate.VirtualDVDDrives = null;
					}

					try
					{
						GuestOSProfileInfo gos = new GuestOSProfileInfo();

						VirtualMachineInfo newVM = client.NewVirtualMachineFromTemplate(selTemplate
															 , vm.Name
															 , null
															 , selTemplate.Owner
															 , hostInfo
															 , hostInfo.VMPaths[0]
															 , null
															 , gos
															 , vm.AdminUserName
															 , vm.AdminPassword
															 , selTemplate.SysprepScript
															 , new Guid(vm.CurrentTaskId)
															 , vm.ComputerName
															 , null
															 , vm.JoinDomain
															 , vm.JoinDomainUserName
															 , vm.MergeAnswerFile
															 , selTemplate.OperatingSystem
															 , selTemplate.GuiRunOnceCommands
															 , vm.JoinDomainPassword
															 , (String.IsNullOrWhiteSpace(vm.JoinDomain) ?
																   selTemplate.JoinWorkgroup : null)
															 , null
															 , (selTemplate.ProductKeyHasValue ? null : vm.ProductKey)
															 , null);


						vm.VmGuid = newVM.Id;
						vm.TemplateName = selTemplate.Name;
						vm.ComputerName = newVM.ComputerName;
						vm.State = (Virtualization.VMComputerSystemStateInfo)newVM.Status;
						vm.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;

						vm.CurrentJob = new ConcreteJob();
						vm.CurrentJob.Id = vm.Name;
						vm.CurrentJob.JobState = ConcreteJobState.Running;
						vm.CurrentJob.TargetState = Virtualization.VMComputerSystemStateInfo.PowerOff;

						// Warning: 5 seconds thread sleep
						System.Threading.Thread.Sleep(5000);

						VirtualMachineInfo vmWait = client.GetVirtualMachineByName(vm.Name);

						while (vmWait.Status == SVMMService.VMComputerSystemStateInfo.UnderCreation)
						{
							System.Threading.Thread.Sleep(30000);
							vmWait = client.GetVirtualMachineByName(vm.Name);
						}

						if (vmWait.Status != SVMMService.VMComputerSystemStateInfo.CreationFailed)
						{
							if ((vmWait.Status != SVMMService.VMComputerSystemStateInfo.PowerOff)
							 && (vmWait.Status != SVMMService.VMComputerSystemStateInfo.Stored))
							{
								client.ShutdownVirtualMachine(vmWait.Id);

								while (vmWait.Status != SVMMService.VMComputerSystemStateInfo.Stored
									&& vmWait.Status != SVMMService.VMComputerSystemStateInfo.PowerOff
									&& vmWait.Status != SVMMService.VMComputerSystemStateInfo.UpdateFailed)
								{
									System.Threading.Thread.Sleep(5000);
									vmWait = client.GetVirtualMachineByName(vmWait.Name);
								}

							}

							ConfigureCreatedVMNetworkAdapters(vm);
						}
					}
					catch (Exception ex)
					{
						vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;
						throw;
					}
				}
			}
			catch (Exception ex)
			{
				vm.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;
				// TODO: Possibly we should avoid exposing such detailed exceptions to the end-user
				vm.exMessage = ex.Message;
				// Log the exception occured
				Log.WriteError(ex);
			}
			//
			return vm;
		}

		public VMInfo CreateVMFromVM(string sourceName, VMInfo vmTemplate, Guid taskGuid)
		{
			string paramCreate = String.Empty;
			//
			var steps = new StringBuilder();

			try
			{
				steps.AppendLine("Start Connect to ScVMM (new VirtualMachineManagementServiceClient)");
				//
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					steps.AppendLine("Connected to ScVMM");
					//
					#region Hardware profiles
					steps.AppendLine("Start select Hardware Profle (GetHardwareProfles())");
					HardwareProfileInfo[] hProfiles = client.GetHardwareProfles();

					if (hProfiles == null || hProfiles.Length == 0)
						throw new Exception("No hardware profile found can't continue.");
					steps.AppendLine("Hardware Profle selected");
					#endregion

					steps.AppendLine("Start Get template VM info (GetVirtualMachineByName())");
					//
					VirtualMachineInfo sourceVM = client.GetVirtualMachineByName(sourceName);
					//
					steps.AppendLine("Done Get template VM info");
					//
					if (sourceVM.Status == SVMMService.VMComputerSystemStateInfo.CreationFailed
						&& sourceVM.Status == SVMMService.VMComputerSystemStateInfo.CustomizationFailed
						&& sourceVM.Status == SVMMService.VMComputerSystemStateInfo.UpdateFailed
						&& sourceVM.Status == SVMMService.VMComputerSystemStateInfo.Deleting
						&& sourceVM.Status == SVMMService.VMComputerSystemStateInfo.TemplateCreationFailed
						&& sourceVM.Status == SVMMService.VMComputerSystemStateInfo.UnderCreation
						&& sourceVM.Status == SVMMService.VMComputerSystemStateInfo.UnderTemplateCreation
						&& sourceVM.Status == SVMMService.VMComputerSystemStateInfo.UnderUpdate)
					{
						throw new Exception(String.Format("Creation Failed. Template state = {0}.", sourceVM.Status));
					}

					if (sourceVM.Status != SVMMService.VMComputerSystemStateInfo.PowerOff
						&& sourceVM.Status != SVMMService.VMComputerSystemStateInfo.Stored
						&& sourceVM.Status != SVMMService.VMComputerSystemStateInfo.Saved)
					{
						//
						steps.AppendLine("Template VM Stopping (ShutdownVirtualMachine())");
						//
						client.ShutdownVirtualMachine(sourceVM.Id);

						while (sourceVM.Status != SVMMService.VMComputerSystemStateInfo.Stored
							&& sourceVM.Status != SVMMService.VMComputerSystemStateInfo.PowerOff
							&& sourceVM.Status != SVMMService.VMComputerSystemStateInfo.UpdateFailed)
						{
							System.Threading.Thread.Sleep(5000);
							sourceVM = client.GetVirtualMachineByName(sourceName);
						}

						if (sourceVM.Status == WebsitePanel.Providers.VirtualizationForPC.SVMMService.VMComputerSystemStateInfo.UpdateFailed)
						{
							throw new Exception(String.Format("Creation Failed. Template not stoped. Current state = {0}", sourceVM.Status));
						}
						//
						steps.AppendLine("Template VM Stoped");
					}

					#region Library
					steps.AppendLine("Start Select library (GetLibraryServers())");
					LibraryServerInfo[] arrLi = client.GetLibraryServers();
					if (arrLi.Length == 0)
						throw new InvalidOperationException("Get library servers returns empty list");

					LibraryServerInfo li = null;

					foreach (var cur in arrLi)
					{
						if (LibraryPath.ToLower().Contains(cur.ComputerName.ToLower()))
						{
							li = cur;
							break;
						}
					}

					if (li == null)
						throw new Exception(string.Format("Library server for share {0} not found.", LibraryPath));

					if (li.Status != ComputerStateInfo.Responding && li.Status != ComputerStateInfo.Pending && li.Status != ComputerStateInfo.Updating)
						throw new InvalidOperationException(string.Format("Library server {0} in invalid state {1}", li.ComputerName, li.Status));

					steps.AppendLine("Library selected");
					#endregion

					paramCreate = String.Format("Params: SourceVM ID: {0}\n New VM Name: {1}\n Owner Name : {2}\n Library : {3}\n Liblary Path:{4}\n Hardware profile:{5}\n"
						, sourceVM.Id, vmTemplate.Name, sourceVM.Owner, (li != null ? li.Name : "unknown"), LibraryPath, hProfiles[0].Name);

					steps.AppendLine("Start Create VM (NewVirtualMachineFromVM())");

					taskGuid = (((taskGuid == null) || (taskGuid == Guid.Empty)) ? Guid.NewGuid() : taskGuid);

					#region GetMovement params
					HostInfo hostInfo = null;
					if (ServerType.Equals("cluster"))
					{
						steps.AppendLine("Start Get Host by Rating (GetVMHostRatingsByCluster())");
						var ratings = client.GetVMHostRatingsByCluster(sourceVM.Id, true, ServerNameSettings).OrderByDescending(item => item.Rating).ToList();
						if (ratings.Count == 0)
							throw new InvalidOperationException("got empty ratings list");

						hostInfo = ratings.ToArray()[0].VMHost;
						steps.AppendLine("Done Get Host by Rating");
					}
					else
					{
						steps.AppendLine("Start Get Host (GetHostByName())");
						hostInfo = client.GetHostByName(ServerNameSettings);
						steps.AppendLine("Done Get Host");
					}
					#endregion
					//
					steps.AppendLine("start NewVirtualMachineFromVM");
					//
					VirtualMachineInfo newVM = client.NewVirtualMachineFromVM(sourceVM.Id
																			, vmTemplate.Name
																			, string.Format("Clone of {0}", sourceVM.Name)
																			, sourceVM.Owner
																			, li
																			, LibraryPath
																			, hProfiles[0]
																			, taskGuid);
					//
					steps.AppendLine("end NewVirtualMachineFromVM");
					//
					steps.AppendFormat("start MoveVirtualMachine {0} to {1} - {2}", newVM.Name, hostInfo.ComputerName, hostInfo.VMPaths[0]).AppendLine();
					//
					client.MoveVirtualMachine(newVM.Id, hostInfo.Id, hostInfo.VMPaths[0], false, true, false, taskGuid);
					//
					steps.AppendLine("end MoveVirtualMachine");

					vmTemplate.VmGuid = newVM.Id;
					vmTemplate.ComputerName = newVM.ComputerName;
					vmTemplate.State = (Virtualization.VMComputerSystemStateInfo)newVM.Status;
					vmTemplate.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;
					//
					steps.AppendLine("VM created");
				}
			}
			catch (System.TimeoutException ex)
			{
				vmTemplate.ProvisioningStatus = VirtualMachineProvisioningStatus.InProgress;
			}
			catch (Exception ex)
			{
				vmTemplate.ProvisioningStatus = VirtualMachineProvisioningStatus.Error;
				// TO-DO: Possibly we should avoid exposing such detailed exceptions to the end-user
				vmTemplate.exMessage = ex.Message + "\n";
				//
				Log.WriteError(ex);
			}
			// 
			vmTemplate.logMessage = paramCreate + steps;

			//// Переносим виртуалку
			//Providers.Virtualization.VMComputerSystemStateInfo state = vmTemplate.State;

			//while (state == Providers.Virtualization.VMComputerSystemStateInfo.UnderCreation)
			//{
			//    System.Threading.Thread.Sleep(10000);
			//    VMInfo stateVmInfo = GetVirtualMachine(vmTemplate.Name);
			//    state = stateVmInfo.State;
			//}

			//if ((state == Providers.Virtualization.VMComputerSystemStateInfo.PowerOff)
			//    || (state == Providers.Virtualization.VMComputerSystemStateInfo.Stored)
			//    || (state == Providers.Virtualization.VMComputerSystemStateInfo.Saved)
			//    || String.IsNullOrEmpty(vmTemplate.exMessage))
			//{
			//    vmTemplate = MoveVM(vmTemplate);
			//}

			return vmTemplate;
		}

		public VMInfo MoveVM(VMInfo vmForMove)
		{
			var steps = new StringBuilder().AppendLine("MoveVM");
			//
			string paramsMove = String.Empty;
			try
			{
				steps.AppendLine("Start Connect to ScVNMM (new VirtualMachineManagementServiceClient)");
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					steps.AppendLine("Connected to ScVNMM");
					//
					steps.AppendLine("Start Get source VM info (GetVirtualMachineByName() )");
					//
					VirtualMachineInfo sourceVM = client.GetVirtualMachineByName(vmForMove.Name);
					//
					steps.AppendLine("Done Get source VM info");

					HostInfo hostInfo = null;

					if (ServerType.Equals("cluster"))
					{
						steps.AppendLine("Start Get Cluster (GetHostClusterByName())");
						//
						HostClusterInfo selCluster = client.GetHostClusterByName(ServerNameSettings);
						//
						steps.AppendLine("Done Get Cluster (GetHostClusterByName())");
						steps.AppendLine("Start Get Host by Rating (GetVMHostRatingsByCluster())");
						//
						VMHostRatingInfo overHost = client.GetVMHostRatingsByCluster(sourceVM.Id, true, ServerNameSettings)
							   .OrderByDescending(item => item.Rating).ToArray()[0];

						if (overHost != null)
						{
							hostInfo = overHost.VMHost;
						}
						//
						steps.AppendLine("Done Get Host by Rating");
					}
					else
					{
						steps.AppendLine("Start Get Host (GetHostByName())");
						hostInfo = client.GetHostByName(ServerNameSettings);
						steps.AppendLine("Done Get Host");
					}

					if (hostInfo == null)
					{
						throw new Exception("Host not found.");
					}

					paramsMove = String.Format("VM Id: {0}\n Host Id: {1}\n VM Path: {2}\n", sourceVM.Id, hostInfo.Id, hostInfo.VMPaths[0]);
					//
					steps.AppendLine("Start Move VM (MoveVirtualMachine)");
					//
					client.MoveVirtualMachine(sourceVM.Id, hostInfo.Id, hostInfo.VMPaths[0], false, true, true, null);
					//
					steps.AppendLine("Done Move VM (MoveVirtualMachine)");
				}
			}
			catch (Exception ex)
			{
				// TO-DO: Possibly we should avoid exposing such detailed exceptions to the end-user
				vmForMove.exMessage = vmForMove.exMessage + "\n MoveVM \n" + ex.Message;
				//
				Log.WriteError(ex);
			}

			vmForMove.logMessage = vmForMove.logMessage + steps + paramsMove;
			return vmForMove;
		}

		public VMInfo UpdateVirtualMachine(VMInfo vm)
		{
			string vmId = vm.Name;

			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{

				VirtualMachineInfo vmi = client.GetVirtualMachineByName(vm.Name);

				#region This code is intended to update a VM settings only in Host mode but it does not support Cluster mode
				// get VM object
				ManagementObject objVM = GetVirtualMachineObject(vmId);

				//// update general settings
				//UpdateVirtualMachineGeneralSettings(vmId, objVM,
				//    vm.CpuCores,
				//    vm.RamSize,
				//    vm.BootFromCD,
				//    vm.NumLockEnabled);

				// check DVD drive
				ManagementObject objDvdDrive = wmi.GetWmiObject(
					"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Synthetic DVD Drive'"
						+ " and InstanceID like 'Microsoft:{0}%' and Address = 0", vmId);

				if (vm.DvdDriver && objDvdDrive == null)
					AddVirtualMachineDvdDrive(vmId, objVM);
				else if (!vm.DvdDriver && objDvdDrive != null)
					RemoveVirtualMachineResources(objVM, objDvdDrive);

				// External NIC
				if (!vm.ExternalNetworkEnabled
					&& !String.IsNullOrEmpty(vm.ExternalNicMacAddress))
				{
					// delete adapter
					//                DeleteNetworkAdapter(objVM, vm.ExternalNicMacAddress);

					// reset MAC
					vm.ExternalNicMacAddress = null;
				}
				else if (vm.ExternalNetworkEnabled
					&& !String.IsNullOrEmpty(vm.ExternalNicMacAddress))
				{
					// add external adapter
					//AddNetworkAdapter(objVM, vm.ExternalSwitchId, vm.Name, vm.ExternalNicMacAddress, EXTERNAL_NETWORK_ADAPTER_NAME, vm.LegacyNetworkAdapter);
				}


				// Private NIC
				if (!vm.PrivateNetworkEnabled
					&& !String.IsNullOrEmpty(vm.PrivateNicMacAddress))
				{
					// delete adapter
					//                DeleteNetworkAdapter(objVM, vm.PrivateNicMacAddress);

					// reset MAC
					vm.PrivateNicMacAddress = null;
				}
				else if (vm.PrivateNetworkEnabled
					&& !String.IsNullOrEmpty(vm.PrivateNicMacAddress))
				{
					// add private adapter
					//AddNetworkAdapter(objVM, vm.PrivateSwitchId, vm.Name, vm.PrivateNicMacAddress, PRIVATE_NETWORK_ADAPTER_NAME, vm.LegacyNetworkAdapter);
				}
				#endregion
			}

			return vm;
		}

		private void AddVirtualMachineDvdDrive(string vmId, ManagementObject objVM)
		{
			// load IDE 1 controller
			ManagementObject objIDE1 = wmi.GetWmiObject(
				"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Emulated IDE Controller'"
				+ " and InstanceID Like 'Microsoft:{0}%' and Address = 1", vmId);

			// load default hard disk drive
			ManagementObject objDefaultDvd = wmi.GetWmiObject(
				"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Synthetic DVD Drive'"
					+ " and InstanceID like '%Default'");
			ManagementObject objDvd = (ManagementObject)objDefaultDvd.Clone();
			objDvd["Parent"] = objIDE1.Path;
			objDvd["Address"] = 0;

			// add DVD drive to VM resources
			AddVirtualMachineResources(objVM, objDvd);
		}

		public void ConfigureCreatedVMNetworkAdapters(VMInfo vmInfo)
		{
			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{
				VirtualMachineInfo vm = client.GetVirtualMachineByName(vmInfo.Name);

				if (vm.Status != SVMMService.VMComputerSystemStateInfo.PowerOff && vm.Status != SVMMService.VMComputerSystemStateInfo.Stored)
				{
					throw new ApplicationException("Virtual machine should has status PowerOff to configure network adapters");
				}
				// Remove exists Network adapters
				VirtualNetworkAdapterInfo[] existsNetworkAdapters = vm.VirtualNetworkAdapters;
				foreach (VirtualNetworkAdapterInfo adapter in existsNetworkAdapters)
					client.RemoveVirtualNetworkAdapter(adapter, false, new Guid(vmInfo.CurrentTaskId));

				// Add external network
				if (vmInfo.ExternalNetworkEnabled)
				{
					client.NewVMVirtualNetworkAdapter(
						vm.Id,
						string.IsNullOrEmpty(vmInfo.ExternalNicMacAddress) ? MacAddressHelper.GetNewMacAddress() : vmInfo.ExternalNicMacAddress,
						EthernetAddressTypeInfo.Static,
						vmInfo.ExternalVirtualNetwork);
				}

				// Add private network
				if (vmInfo.PrivateNetworkEnabled)
				{
					VirtualNetworkAdapterInfo adapter = client.NewVMVirtualNetworkAdapter(
						vm.Id,
						string.IsNullOrEmpty(vmInfo.PrivateNicMacAddress) ? MacAddressHelper.GetNewMacAddress() : vmInfo.PrivateNicMacAddress,
						EthernetAddressTypeInfo.Static,
						vmInfo.PrivateVirtualNetwork
						);

					// Update vLanID
					client.SetVirtualNetworkAdapter(
						adapter,
						null,
						null,
						null,
						null,
						null,
						true,
						vmInfo.PrivateVLanID,
						false,
						new Guid(vmInfo.CurrentTaskId)
						);
				}
			}
		}

		public Virtualization.VirtualNetworkInfo[] GetVirtualNetworkByHostName(string hostName)
		{
			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{
				HostInfo host = client.GetHostByName(hostName);
				return GetVirtualNetworkByHostInfo(host);
			}

		}

		public Virtualization.VirtualNetworkInfo[] GetVirtualNetworkByHostInfo(HostInfo hostInfo)
		{
			List<Virtualization.VirtualNetworkInfo> result = new List<Virtualization.VirtualNetworkInfo>();

			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					VirtualizationForPC.SVMMService.VirtualNetworkInfo[] networks = client.GetVirtualNetworkByHost(hostInfo);
					foreach (var item in networks)
					{
						result.Add(
							new Virtualization.VirtualNetworkInfo
							{
								BoundToVMHost = item.BoundToVMHost,
								DefaultGatewayAddress = item.DefaultGatewayAddress,
								Description = item.Description,
								DNSServers = item.DNSServers,
								EnablingIPAddress = item.EnablingIPAddress,
								HighlyAvailable = item.HighlyAvailable,
								HostBoundVlanId = item.HostBoundVlanId,
								Id = item.Id,
								Name = item.Name,
								NetworkAddress = item.NetworkAddress,
								NetworkMask = item.NetworkMask,
								Tag = item.Tag,
								VMHost = item.VMHost.ComputerName,
								VMHostId = item.VMHostId,
								WINServers = item.WINServers
							});
					}
				}
			}
			catch (Exception ex)
			{
			}

			return result.ToArray();

		}

		private void AddNetworkAdapter(ManagementObject objVm, string switchId, string portName, string macAddress, string adapterName, bool legacyAdapter)
		{
			string nicClassName = GetNetworkAdapterClassName(legacyAdapter);

			string vmId = (string)objVm["Name"];

			// check if already exists
			ManagementObject objNic = wmi.GetWmiObject(
				nicClassName, "InstanceID like 'Microsoft:{0}%' and Address = '{1}'", vmId, macAddress);

			if (objNic != null)
				return; // exists - exit

			portName = String.Format("{0} - {1}",
				portName, (adapterName == EXTERNAL_NETWORK_ADAPTER_NAME) ? "External" : "Private");

			// Network service
			ManagementObject objNetworkSvc = GetVirtualSwitchManagementService();

			// default NIC
			ManagementObject objDefaultNic = wmi.GetWmiObject(nicClassName, "InstanceID like '%Default'");

			// find switch
			ManagementObject objSwitch = wmi.GetWmiObject("msvm_VirtualSwitch", "Name = '{0}'", switchId);

			// create switch port
			ManagementBaseObject inParams = objNetworkSvc.GetMethodParameters("CreateSwitchPort");
			inParams["VirtualSwitch"] = objSwitch;
			inParams["Name"] = portName;
			inParams["FriendlyName"] = portName;
			inParams["ScopeOfResidence"] = "";

			// invoke method
			ManagementBaseObject outParams = objNetworkSvc.InvokeMethod("CreateSwitchPort", inParams, null);

			// process output parameters
			ReturnCode code = (ReturnCode)Convert.ToInt32(outParams["ReturnValue"]);
			if (code == ReturnCode.OK)
			{
				// created port
				ManagementObject objPort = wmi.GetWmiObjectByPath((string)outParams["CreatedSwitchPort"]);

				// create NIC
				ManagementObject objExtNic = (ManagementObject)objDefaultNic.Clone();
				objExtNic["Connection"] = new string[] { objPort.Path.Path };

				if (!String.IsNullOrEmpty(macAddress))
				{
					objExtNic["StaticMacAddress"] = true;
					objExtNic["Address"] = macAddress;
				}
				else
				{
					objExtNic["StaticMacAddress"] = false;
				}
				objExtNic["ElementName"] = adapterName;

				if (!legacyAdapter)
					objExtNic["VirtualSystemIdentifiers"] = new string[] { Guid.NewGuid().ToString("B") };

				// add NIC
				ManagementObject objCreatedExtNic = AddVirtualMachineResources(objVm, objExtNic);
			}
		}

		private string GetNetworkAdapterClassName(bool legacy)
		{
			return legacy ? "Msvm_EmulatedEthernetPortSettingData" : "Msvm_SyntheticEthernetPortSettingData";
		}

		private ManagementObject AddVirtualMachineResources(ManagementObject objVm, ManagementObject resource)
		{
			if (resource == null)
				return resource;

			// request management service
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			// add resources
			string txtResource = resource.GetText(TextFormat.CimDtd20);
			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("AddVirtualSystemResources");
			inParams["TargetSystem"] = objVm;
			inParams["ResourceSettingData"] = new string[] { txtResource };
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("AddVirtualSystemResources", inParams, null);
			JobResult result = CreateJobResultFromWmiMethodResults(outParams);

			if (result.ReturnValue == ReturnCode.OK)
			{
				string[] wmiPaths = (string[])outParams["NewResources"];
				return wmi.GetWmiObjectByPath(wmiPaths[0]);
			}
			else if (result.ReturnValue == ReturnCode.JobStarted)
			{
				if (JobCompleted(result.Job))
				{
					string[] wmiPaths = (string[])outParams["NewResources"];
					return wmi.GetWmiObjectByPath(wmiPaths[0]);
				}
				else
				{
					throw new Exception("Cannot add virtual machine resources");
				}
			}
			else
			{
				throw new Exception("Cannot add virtual machine resources: " + txtResource);
			}
		}

		private JobResult RemoveVirtualMachineResources(ManagementObject objVm, ManagementObject resource)
		{
			if (resource == null)
				return null;

			// request management service
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			// remove resources
			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("RemoveVirtualSystemResources");
			inParams["TargetSystem"] = objVm;
			inParams["ResourceSettingData"] = new string[] { resource.Path.Path };
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("RemoveVirtualSystemResources", inParams, null);
			JobResult result = CreateJobResultFromWmiMethodResults(outParams);
			if (result.ReturnValue == ReturnCode.OK)
			{
				return result;
			}
			else if (result.ReturnValue == ReturnCode.JobStarted)
			{
				if (!JobCompleted(result.Job))
				{
					throw new Exception("Cannot remove virtual machine resources");
				}
			}
			else
			{
				throw new Exception("Cannot remove virtual machine resources: " + resource.Path.Path);
			}

			return result;
		}

		public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
		{
			// target computer
			JobResult ret = new JobResult();
			ret.Job = new ConcreteJob();
			ret.Job.Id = vmId;

			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{

					VirtualMachineInfo vm = client.GetVirtualMachineByName(vmId);

					switch (newState)
					{
						case VirtualMachineRequestedState.Start:
							{
								client.StartVirtualMachine(vm.Id);
								ret.Job.TargetState = Virtualization.VMComputerSystemStateInfo.Running;
								break;
							}
						case VirtualMachineRequestedState.Resume:
							{
								client.ResumeVirtualMachine(vm.Id);
								ret.Job.TargetState = Virtualization.VMComputerSystemStateInfo.Running;
								break;
							}
						case VirtualMachineRequestedState.Pause:
							{
								client.PauseVirtualMachine(vm.Id);
								ret.Job.TargetState = Virtualization.VMComputerSystemStateInfo.Paused;
								break;
							}
						case VirtualMachineRequestedState.ShutDown:
							{
								client.ShutdownVirtualMachine(vm.Id);
								ret.Job.TargetState = Virtualization.VMComputerSystemStateInfo.Stored;
								break;
							}
						case VirtualMachineRequestedState.TurnOff:
							{
								client.StopVirtualMachine(vm.Id);
								ret.Job.TargetState = Virtualization.VMComputerSystemStateInfo.PowerOff;
								break;
							}
						default:
							{
								break;
							}
					}

					ret.Job.JobState = ConcreteJobState.Running;
					ret.Job.Caption = newState.ToString();
					ret.ReturnValue = ReturnCode.JobStarted;
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Could not change virtual machine state", ex);
				//
				ret.Job.JobState = ConcreteJobState.Exception;
				ret.Job.Caption = newState.ToString();
				ret.ReturnValue = ReturnCode.Failed;
			}

			return ret;
		}

		public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
		{
			ReturnCode ret = ReturnCode.JobStarted;
			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{

					VirtualMachineInfo vm = client.GetVirtualMachineByName(vmId);

					client.ShutdownVirtualMachine(vm.Id);

					ret = ReturnCode.OK;
				}
			}
			catch (Exception ex)
			{
				Log.WriteError("Could not shut down virtual machine", ex);
				//
				ret = ReturnCode.Failed;
			}

			return ret;
		}

		public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
		{
			List<ConcreteJob> jobs = new List<ConcreteJob>();

			ManagementBaseObject objSummary = GetVirtualMachineSummaryInformation(
				vmId, SummaryInformationRequest.AsynchronousTasks);
			ManagementBaseObject[] objJobs = (ManagementBaseObject[])objSummary["AsynchronousTasks"];

			if (objJobs != null)
			{
				foreach (ManagementBaseObject objJob in objJobs)
					jobs.Add(CreateJobFromWmiObject(objJob));
			}

			return jobs;
		}

		public JobResult RenameVirtualMachine(string vmId, string name)
		{
			// load virtual machine
			ManagementObject objVm = GetVirtualMachineObject(vmId);

			// load machine settings
			ManagementObject objVmSettings = GetVirtualMachineSettingsObject(vmId);

			// rename machine
			objVmSettings["ElementName"] = name;

			// save
			ManagementObject objVmsvc = GetVirtualSystemManagementService();
			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("ModifyVirtualSystem");
			inParams["ComputerSystem"] = objVm.Path.Path;
			inParams["SystemSettingData"] = objVmSettings.GetText(TextFormat.CimDtd20);
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("ModifyVirtualSystem", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}

		public JobResult DeleteVirtualMachine(string vmId)
		{
			// check state

			VMInfo vm = GetVirtualMachine(vmId);

			JobResult ret = new JobResult();
			ret.Job = new ConcreteJob();
			ret.Job.Id = vmId;
			ret.Job.JobState = ConcreteJobState.Completed;
			ret.ReturnValue = ReturnCode.OK;

			if (vm.VmGuid == Guid.Empty)
			{
				return ret;
			}

			// The virtual computer system must be in the powered off or saved state prior to calling this method.
			if (vm.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.Saved
				|| vm.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.PowerOff
				|| vm.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.CreationFailed
				|| vm.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.Stored
				|| vm.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.IncompleteVMConfig)
			{
				// delete network adapters and ports
				try
				{
					using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
					{

						if (vm.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.PowerOff)
						{
							DeleteNetworkAdapters(vm.VmGuid);

							System.Threading.Thread.Sleep(5000);
						}

						client.DeleteVirtualMachine(vm.VmGuid);

						ret.Job.Caption = "Delete VM Done";
						ret.Job.JobState = ConcreteJobState.Running;
						ret.Job.TargetState = Virtualization.VMComputerSystemStateInfo.Deleting;
						ret.ReturnValue = ReturnCode.JobStarted;
					}
				}
				catch (Exception ex)
				{
					ret.Job.Caption = ex.Message;
					ret.Job.Description = ex.StackTrace;
					ret.Job.JobState = ConcreteJobState.Exception;
					ret.ReturnValue = ReturnCode.Failed;
				}

				return ret;
			}
			else
			{
				throw new Exception("The virtual computer system must be in the powered off or saved state prior to calling Destroy method.");
			}
		}

		private void DeleteNetworkAdapters(Guid objVM)
		{
			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{
				VirtualNetworkAdapterInfo[] adapters = client.GetVirtualNetworkAdaptersByVM(objVM);

				if (adapters != null)
				{
					foreach (VirtualNetworkAdapterInfo item in adapters)
					{
						DeleteNetworkAdapter(item, false);
					}
				}
			}
		}

		private void DeleteNetworkAdapter(VirtualNetworkAdapterInfo objVM, bool runAsunc)
		{
			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{
				client.RemoveVirtualNetworkAdapter(objVM, runAsunc, null);
			}
		}

		private void DeleteNetworkAdapter(ManagementObject objVM, ManagementObject objNic)
		{
			if (objNic == null)
				return;

			// delete corresponding switch port
			string[] conn = (string[])objNic["Connection"];
			if (conn != null && conn.Length > 0)
				DeleteSwitchPort(conn[0]);

			// delete adapter
			RemoveVirtualMachineResources(objVM, objNic);
		}

		private void DeleteSwitchPort(string portPath)
		{
			// Network service
			ManagementObject objNetworkSvc = GetVirtualSwitchManagementService();

			// create switch port
			ManagementBaseObject inParams = objNetworkSvc.GetMethodParameters("DeleteSwitchPort");
			inParams["SwitchPort"] = portPath;

			// invoke method
			objNetworkSvc.InvokeMethod("DeleteSwitchPort", inParams, null);
		}
		#endregion

		#region Snapshots
		public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
		{
			// get all VM setting objects

			List<VirtualMachineSnapshot> ret = new List<VirtualMachineSnapshot>();

			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					VMCheckpointInfo[] chkPtnList = client.GetVirtualMachineByName(vmId).VMCheckpoints;

					if (chkPtnList != null)
					{
						foreach (VMCheckpointInfo curr in chkPtnList)
						{
							ret.Add(new VirtualMachineSnapshot()
							{
								Created = curr.AddedTime
							,
								Id = curr.Id.ToString()
							,
								Name = curr.Name
							,
								CheckPointId = curr.CheckpointID
							,
								ParentId = curr.ParentCheckpointID
							});
						}
					}
				}
			}
			catch (Exception e)
			{
			}

			return ret;
		}

		public VirtualMachineSnapshot GetSnapshot(string snapshotId)
		{
			// load snapshot
			ManagementObject objSnapshot = GetSnapshotObject(snapshotId);
			return CreateSnapshotFromWmiObject(objSnapshot);
		}

		public JobResult CreateSnapshot(string vmId)
		{
			JobResult ret = new JobResult();

			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					ret.Job = new ConcreteJob();
					ret.Job.Id = vmId;
					ret.Job.JobState = ConcreteJobState.Starting;
					ret.ReturnValue = ReturnCode.JobStarted;

					VirtualMachineInfo vm = client.GetVirtualMachineByName(vmId);

					client.NewVirtualMachineCheckpoint(vm.Id, String.Format("{0} - {1}", vm.Name, DateTime.Now), String.Empty);
				}
			}
			catch (TimeoutException ext)
			{
				ret.ReturnValue = ReturnCode.JobStarted;
			}
			catch (Exception ex)
			{
				ret.Job.ErrorDescription = ex.Message;
				ret.Job.JobState = ConcreteJobState.Exception;
				ret.ReturnValue = ReturnCode.Failed;
			}

			return ret;
		}

		public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
		{
			// load virtual machine
			ManagementObject objVm = GetVirtualMachineObject(vmId);

			// load snapshot
			ManagementObject objSnapshot = GetSnapshotObject(snapshotId);

			// rename snapshot
			objSnapshot["ElementName"] = name;

			// save
			ManagementObject objVmsvc = GetVirtualSystemManagementService();
			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("ModifyVirtualSystem");
			inParams["ComputerSystem"] = objVm.Path.Path;
			inParams["SystemSettingData"] = objSnapshot.GetText(TextFormat.CimDtd20);
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("ModifyVirtualSystem", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}

		public JobResult ApplySnapshot(string vmId, string snapshotId)
		{
			JobResult ret = new JobResult();
			bool error = false;
			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					ret.Job = new ConcreteJob();
					ret.Job.Id = vmId;
					ret.Job.JobState = ConcreteJobState.Starting;
					ret.ReturnValue = ReturnCode.JobStarted;

					client.RestoreVirtualMachineCheckpoint(snapshotId);
				}
			}
			catch (TimeoutException ext)
			{
				error = true;
				ret.ReturnValue = ReturnCode.JobStarted;
			}
			catch (Exception ex)
			{
				error = true;
				ret.Job.ErrorDescription = ex.Message;
				ret.Job.JobState = ConcreteJobState.Exception;
				ret.ReturnValue = ReturnCode.Failed;
			}

			if (!error)
			{
				ret.ReturnValue = ReturnCode.OK;
			}

			return ret;
		}

		public JobResult DeleteSnapshot(string vmId, string snapshotId)
		{
			JobResult ret = new JobResult();

			try
			{
				using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
				{
					ret.Job = new ConcreteJob();
					ret.Job.Id = vmId;
					ret.Job.JobState = ConcreteJobState.Starting;
					ret.ReturnValue = ReturnCode.JobStarted;

					VirtualMachineInfo vm = client.GetVirtualMachineByName(vmId);

					client.DeleteVirtualMachineCheckpoint(snapshotId);
				}
			}
			catch (Exception ex)
			{
				ret.Job.ErrorDescription = ex.Message;
				ret.Job.JobState = ConcreteJobState.Exception;
				ret.ReturnValue = ReturnCode.Failed;
			}

			return ret;
		}

		public JobResult DeleteSnapshotSubtree(string snapshotId)
		{
			// get VM management service
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			// load snapshot object
			ManagementObject objSnapshot = GetSnapshotObject(snapshotId);

			// get method params
			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("RemoveVirtualSystemSnapshotTree");
			inParams["SnapshotSettingData"] = objSnapshot.Path.Path;

			// invoke method
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("RemoveVirtualSystemSnapshotTree", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}

		public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
		{
			//            ManagementBaseObject objSummary = GetSnapshotSummaryInformation(snapshotId, (SummaryInformationRequest)size);

			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{

				VirtualMachineInfo vminfo = client.GetVirtualMachineByName(snapshotId);
				HostInfo host = client.GetHostById(vminfo.HostId);

				return GetTumbnailFromSummaryInformation(vminfo.Name, size);
			}
		}
		#endregion

		#region DVD operations
		public string GetInsertedDVD(string vmId)
		{
			// find CD/DVD disk
			ManagementObject objDvd = wmi.GetWmiObject(
				"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Virtual CD/DVD Disk'"
					+ " and InstanceID Like 'Microsoft:{0}%'", vmId);

			if (objDvd == null)
				return null;

			string[] path = (string[])objDvd["Connection"];
			if (path != null && path.Length > 0)
				return path[0];

			return null;
		}

		public JobResult InsertDVD(string vmId, string isoPath)
		{
			isoPath = FileUtils.EvaluateSystemVariables(isoPath);

			// find DVD drive
			ManagementObject objDvdDrive = wmi.GetWmiObject(
				"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Synthetic DVD Drive'"
					+ " and InstanceID Like 'Microsoft:{0}%'", vmId);

			// create CD/DVD disk
			ManagementObject objDefaultDVD = wmi.GetWmiObject(
				"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Virtual CD/DVD Disk'"
					+ " and InstanceID like '%Default'");
			ManagementObject objDvd = (ManagementObject)objDefaultDVD.Clone();
			objDvd["Parent"] = objDvdDrive.Path;
			objDvd["Connection"] = new string[] { isoPath };

			// get VM service
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			// get method
			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("AddVirtualSystemResources");
			inParams["TargetSystem"] = GetVirtualMachineObject(vmId);
			inParams["ResourceSettingData"] = new string[] { objDvd.GetText(TextFormat.CimDtd20) };

			// execute method
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("AddVirtualSystemResources", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}

		public JobResult EjectDVD(string vmId)
		{
			// find CD/DVD disk
			ManagementObject objDvd = wmi.GetWmiObject(
				"Msvm_ResourceAllocationSettingData", "ResourceSubType = 'Microsoft Virtual CD/DVD Disk'"
					+ " and InstanceID Like 'Microsoft:{0}%'", vmId);

			// get VM service
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			// get method
			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("RemoveVirtualSystemResources");
			inParams["TargetSystem"] = GetVirtualMachineObject(vmId);
			inParams["ResourceSettingData"] = new object[] { objDvd.Path.Path };

			// execute method
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("RemoveVirtualSystemResources", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}
		#endregion

		#region Virtual Switches
		public List<VirtualSwitch> GetSwitches()
		{
			List<VirtualSwitch> switches = new List<VirtualSwitch>();

			// load wmi objects
			ManagementObjectCollection objSwitches = wmi.GetWmiObjects("msvm_VirtualSwitch");
			foreach (ManagementObject objSwitch in objSwitches)
				switches.Add(CreateSwitchFromWmiObject(objSwitch));

			return switches;
		}

		public List<VirtualSwitch> GetExternalSwitches(string computerName)
		{
			Wmi cwmi = new Wmi(computerName, WMI_VIRTUALIZATION_NAMESPACE);

			Dictionary<string, string> switches = new Dictionary<string, string>();
			List<VirtualSwitch> list = new List<VirtualSwitch>();

			// load external adapters
			Dictionary<string, string> adapters = new Dictionary<string, string>();
			ManagementObjectCollection objAdapters = cwmi.GetWmiObjects("Msvm_ExternalEthernetPort");
			foreach (ManagementObject objAdapter in objAdapters)
				adapters.Add((string)objAdapter["DeviceID"], "1");

			// get active connections
			ManagementObjectCollection objConnections = cwmi.GetWmiObjects("Msvm_ActiveConnection");
			foreach (ManagementObject objConnection in objConnections)
			{
				// check LAN andpoint
				ManagementObject objLanEndpoint = new ManagementObject(new ManagementPath((string)objConnection["Dependent"]));
				string endpointName = (string)objLanEndpoint["Name"];

				if (!endpointName.StartsWith("/DEVICE/"))
					continue;

				endpointName = endpointName.Substring(8);

				if (adapters.ContainsKey(endpointName))
				{
					// get switch port
					ManagementObject objPort = new ManagementObject(new ManagementPath((string)objConnection["Antecedent"]));
					string switchId = (string)objPort["SystemName"];
					if (switches.ContainsKey(switchId))
						continue;

					// add info about switch
					ManagementObject objSwitch = cwmi.GetRelatedWmiObject(objPort, "Msvm_VirtualSwitch");
					switches.Add(switchId, (string)objSwitch["ElementName"]);
				}
			}

			foreach (string switchId in switches.Keys)
			{
				VirtualSwitch sw = new VirtualSwitch();
				sw.SwitchId = switchId;
				sw.Name = switches[switchId];
				list.Add(sw);
			}

			return list;
		}

		public bool SwitchExists(string switchId)
		{
			ManagementObject objSwitch = wmi.GetWmiObject("msvm_VirtualSwitch", "Name = '{0}'", switchId);
			return (objSwitch != null);
		}

		public VirtualSwitch CreateSwitch(string name)
		{
			// generate ID for new virtual switch
			string id = Guid.NewGuid().ToString();

			// get switch management object
			ManagementObject objNetworkSvc = GetVirtualSwitchManagementService();

			ManagementBaseObject inParams = objNetworkSvc.GetMethodParameters("CreateSwitch");
			inParams["Name"] = id;
			inParams["FriendlyName"] = name;
			inParams["NumLearnableAddresses"] = SWITCH_PORTS_NUMBER;

			// invoke method
			ManagementBaseObject outParams = objNetworkSvc.InvokeMethod("CreateSwitch", inParams, null);

			// process output parameters
			ManagementObject objSwitch = wmi.GetWmiObjectByPath((string)outParams["CreatedVirtualSwitch"]);
			return CreateSwitchFromWmiObject(objSwitch);
		}

		public ReturnCode DeleteSwitch(string switchId)
		{
			// find requested switch
			ManagementObject objSwitch = wmi.GetWmiObject("msvm_VirtualSwitch", "Name = '{0}'", switchId);

			if (objSwitch == null)
				throw new Exception("Virtual switch with the specified ID was not found.");

			// get switch management object
			ManagementObject objNetworkSvc = GetVirtualSwitchManagementService();

			// get method params
			ManagementBaseObject inParams = objNetworkSvc.GetMethodParameters("DeleteSwitch");
			inParams["VirtualSwitch"] = objSwitch.Path.Path;

			ManagementBaseObject outParams = (ManagementBaseObject)objNetworkSvc.InvokeMethod("DeleteSwitch", inParams, null);
			return (ReturnCode)Convert.ToInt32(outParams["ReturnValue"]);
		}
		#endregion

		#region Library
		public LibraryItem[] GetLibraryItems(string path)
		{
			path = Path.Combine(FileUtils.EvaluateSystemVariables(path), LIBRARY_INDEX_FILE_NAME);

			// convert to UNC if it is a remote computer
			path = ConvertToUNC(path);

			if (!File.Exists(path))
			{
				Log.WriteWarning("The folder does not contain 'index.xml' file: {0}", path);
				return null;
			}

			// create list
			List<LibraryItem> items = new List<LibraryItem>();

			// load xml
			XmlDocument xml = new XmlDocument();
			xml.Load(path);

			XmlNodeList nodeItems = xml.SelectNodes("/items/item");

			if (nodeItems.Count == 0)
				Log.WriteWarning("index.xml found, but contains 0 items: {0}", path);

			foreach (XmlNode nodeItem in nodeItems)
			{
				LibraryItem item = new LibraryItem();
				item.Path = nodeItem.Attributes["path"].Value;

				// optional attributes
				if (nodeItem.Attributes["diskSize"] != null)
					item.DiskSize = Int32.Parse(nodeItem.Attributes["diskSize"].Value);

				if (nodeItem.Attributes["legacyNetworkAdapter"] != null)
					item.LegacyNetworkAdapter = Boolean.Parse(nodeItem.Attributes["legacyNetworkAdapter"].Value);

				item.ProcessVolume = 0; // process (extend and sysprep) 1st volume by default
				if (nodeItem.Attributes["processVolume"] != null)
					item.ProcessVolume = Int32.Parse(nodeItem.Attributes["processVolume"].Value);

				if (nodeItem.Attributes["remoteDesktop"] != null)
					item.RemoteDesktop = Boolean.Parse(nodeItem.Attributes["remoteDesktop"].Value);

				// inner nodes
				item.Name = nodeItem.SelectSingleNode("name").InnerText;
				item.Description = nodeItem.SelectSingleNode("description").InnerText;

				// sysprep files
				XmlNodeList nodesSyspep = nodeItem.SelectNodes("provisioning/sysprep");
				List<string> sysprepFiles = new List<string>();
				foreach (XmlNode nodeSyspep in nodesSyspep)
				{
					if (nodeSyspep.Attributes["file"] != null)
						sysprepFiles.Add(nodeSyspep.Attributes["file"].Value);
				}
				item.SysprepFiles = sysprepFiles.ToArray();

				// vmconfig
				XmlNode nodeVmConfig = nodeItem.SelectSingleNode("provisioning/vmconfig");
				if (nodeVmConfig != null)
				{
					if (nodeVmConfig.Attributes["computerName"] != null)
						item.ProvisionComputerName = Boolean.Parse(nodeVmConfig.Attributes["computerName"].Value);

					if (nodeVmConfig.Attributes["administratorPassword"] != null)
						item.ProvisionAdministratorPassword = Boolean.Parse(nodeVmConfig.Attributes["administratorPassword"].Value);

					if (nodeVmConfig.Attributes["networkAdapters"] != null)
						item.ProvisionNetworkAdapters = Boolean.Parse(nodeVmConfig.Attributes["networkAdapters"].Value);
				}

				items.Add(item);
			}

			return items.ToArray();
		}

		public LibraryItem[] GetOSLibraryItems()
		{
			List<LibraryItem> items = new List<LibraryItem>();

			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{
				TemplateInfo[] ti = client.GetTemplates();

				for (int i = 0; i < ti.Length; i++)
				{
					LibraryItem newItem = new LibraryItem();

					newItem.Description = ti[i].OperatingSystem.Name;
					newItem.Name = ti[i].Name;
					newItem.Path = ti[i].Id.ToString();
					newItem.ProcessVolume = ti[i].CPUCount;
					newItem.ProvisionAdministratorPassword = ti[i].AdminPasswordhasValue;
					newItem.ProvisionComputerName = true;
					newItem.ProvisionNetworkAdapters = (ti[i].VirtualNetworkAdapters.Length > 0);
					newItem.LegacyNetworkAdapter = (ti[i].NetworkUtilization > 0);

					items.Add(newItem);
				}
			}
			return items.ToArray();
		}

		public LibraryItem[] GetClusters()
		{
			List<LibraryItem> items = new List<LibraryItem>();

			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{

				if (client.State != CommunicationState.Opened)
				{
					client.Open();
				}

				HostClusterInfo[] ci = client.GetHostClusters();

				if (ci == null || ci.Length == 0)
				{
					throw new Exception("Clusters is not found.");
				}

				for (int i = 0; i < ci.Length; i++)
				{
					LibraryItem newItem = new LibraryItem();

					HostClusterInfo hostInfo = ci[i];

					newItem.Description = hostInfo.Description;
					newItem.Name = hostInfo.Name;
					newItem.Path = hostInfo.Id.ToString();

					//TODO нужно думать
					newItem.ProcessVolume = hostInfo.AvailableStorageNode.CoresPerCPU;
					newItem.ProvisionComputerName = true;

					// Get host's networks
					newItem.Networks = GetVirtualNetworkByHostInfo(hostInfo.AvailableStorageNode);

					items.Add(newItem);
				}

				client.Close();
			}

			return items.ToArray();
		}

		public LibraryItem[] GetHosts()
		{
			List<LibraryItem> items = new List<LibraryItem>();

			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{
				if (client.State != CommunicationState.Opened)
				{
					client.Open();
				}

				HostInfo[] ti = null;

				try
				{
					ti = client.GetHosts();
				}
				catch (Exception ex)
				{
					throw new Exception("GetHost Failed", ex);
				}

				if (ti == null || ti.Length == 0)
				{
					throw new Exception("Hosts is not found.");
				}

				for (int i = 0; i < ti.Length; i++)
				{
					LibraryItem newItem = new LibraryItem();

					HostInfo hostInfo = ti[i];

					newItem.Description = hostInfo.Description;
					newItem.Name = hostInfo.ComputerName;
					newItem.Path = hostInfo.Id.ToString();
					newItem.ProcessVolume = hostInfo.CoresPerCPU;
					newItem.ProvisionComputerName = true;

					// Get host's networks
					newItem.Networks = GetVirtualNetworkByHostInfo(hostInfo);

					items.Add(newItem);
				}
			}
			return items.ToArray();
		}

		private string ConvertToUNC(string path)
		{
			if (String.IsNullOrEmpty(ServerNameSettings)
				|| path.StartsWith(@"\\"))
				return path;

			return String.Format(@"\\{0}\{1}", ServerNameSettings, path.Replace(":", "$"));
		}
		#endregion

		#region KVP
		public List<KvpExchangeDataItem> GetKVPItems(string vmId)
		{
			return GetKVPItems(vmId, "GuestExchangeItems");
		}

		public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
		{
			return GetKVPItems(vmId, "GuestIntrinsicExchangeItems");
		}

		private List<KvpExchangeDataItem> GetKVPItems(string vmId, string exchangeItemsName)
		{
			List<KvpExchangeDataItem> pairs = new List<KvpExchangeDataItem>();

			// load VM
			ManagementObject objVm = GetVirtualMachineObject(vmId);

			ManagementObject objKvpExchange = null;

			try
			{
				objKvpExchange = wmi.GetRelatedWmiObject(objVm, "msvm_KvpExchangeComponent");
			}
			catch
			{
				// TODO
				// add logging...

				return pairs;
			}

			// return XML pairs
			string[] xmlPairs = (string[])objKvpExchange[exchangeItemsName];

			if (xmlPairs == null)
				return pairs;

			// join all pairs
			StringBuilder sb = new StringBuilder();
			sb.Append("<result>");
			foreach (string xmlPair in xmlPairs)
				sb.Append(xmlPair);
			sb.Append("</result>");

			// parse pairs
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(sb.ToString());

			foreach (XmlNode nodeName in doc.SelectNodes("/result/INSTANCE/PROPERTY[@NAME='Name']/VALUE"))
			{
				string name = nodeName.InnerText;
				string data = nodeName.ParentNode.ParentNode.SelectSingleNode("PROPERTY[@NAME='Data']/VALUE").InnerText;
				pairs.Add(new KvpExchangeDataItem(name, data));
			}

			return pairs;
		}

		public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
		{
			// get KVP management object
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			// create KVP items array
			string[] wmiItems = new string[items.Length];

			for (int i = 0; i < items.Length; i++)
			{
				ManagementClass clsKvp = wmi.GetWmiClass("Msvm_KvpExchangeDataItem");
				ManagementObject objKvp = clsKvp.CreateInstance();
				objKvp["Name"] = items[i].Name;
				objKvp["Data"] = items[i].Data;
				objKvp["Source"] = 0;

				// convert to WMI format
				wmiItems[i] = objKvp.GetText(TextFormat.CimDtd20);
			}

			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("AddKvpItems");
			inParams["TargetSystem"] = GetVirtualMachineObject(vmId);
			inParams["DataItems"] = wmiItems;

			// invoke method
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("AddKvpItems", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}

		public JobResult RemoveKVPItems(string vmId, string[] itemNames)
		{
			// get KVP management object
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			// delete items one by one
			for (int i = 0; i < itemNames.Length; i++)
			{
				ManagementClass clsKvp = wmi.GetWmiClass("Msvm_KvpExchangeDataItem");
				ManagementObject objKvp = clsKvp.CreateInstance();
				objKvp["Name"] = itemNames[i];
				objKvp["Data"] = "";
				objKvp["Source"] = 0;

				// convert to WMI format
				string wmiItem = objKvp.GetText(TextFormat.CimDtd20);

				// call method
				ManagementBaseObject inParams = objVmsvc.GetMethodParameters("RemoveKvpItems");
				inParams["TargetSystem"] = GetVirtualMachineObject(vmId);
				inParams["DataItems"] = new string[] { wmiItem };

				// invoke method
				objVmsvc.InvokeMethod("RemoveKvpItems", inParams, null);
			}
			return null;
		}

		public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
		{
			// get KVP management object
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			// create KVP items array
			string[] wmiItems = new string[items.Length];

			for (int i = 0; i < items.Length; i++)
			{
				ManagementClass clsKvp = wmi.GetWmiClass("Msvm_KvpExchangeDataItem");
				ManagementObject objKvp = clsKvp.CreateInstance();
				objKvp["Name"] = items[i].Name;
				objKvp["Data"] = items[i].Data;
				objKvp["Source"] = 0;

				// convert to WMI format
				wmiItems[i] = objKvp.GetText(TextFormat.CimDtd20);
			}

			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("ModifyKvpItems");
			inParams["TargetSystem"] = GetVirtualMachineObject(vmId);
			inParams["DataItems"] = wmiItems;

			// invoke method
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("ModifyKvpItems", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}
		#endregion

		#region Storage
		public WebsitePanel.Providers.Virtualization.VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
		{
			ManagementObject objImgSvc = GetImageManagementService();

			// get method params
			ManagementBaseObject inParams = objImgSvc.GetMethodParameters("GetVirtualHardDiskInfo");
			inParams["Path"] = FileUtils.EvaluateSystemVariables(vhdPath);

			// execute method
			ManagementBaseObject outParams = (ManagementBaseObject)objImgSvc.InvokeMethod("GetVirtualHardDiskInfo", inParams, null);
			ReturnCode result = (ReturnCode)Convert.ToInt32(outParams["ReturnValue"]);
			if (result == ReturnCode.OK)
			{
				// create XML
				string xml = (string)outParams["Info"];
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(xml);

				// read properties
				WebsitePanel.Providers.Virtualization.VirtualHardDiskInfo vhd = new WebsitePanel.Providers.Virtualization.VirtualHardDiskInfo();
				vhd.DiskType = (VirtualHardDiskType)Enum.Parse(typeof(VirtualHardDiskType), GetPropertyValue("Type", doc), true);
				vhd.FileSize = Int64.Parse(GetPropertyValue("FileSize", doc));
				vhd.InSavedState = Boolean.Parse(GetPropertyValue("InSavedState", doc));
				vhd.InUse = Boolean.Parse(GetPropertyValue("InUse", doc));
				vhd.MaxInternalSize = Int64.Parse(GetPropertyValue("MaxInternalSize", doc));
				vhd.ParentPath = GetPropertyValue("ParentPath", doc);
				return vhd;
			}
			return null;
		}

		private string GetPropertyValue(string propertyName, XmlDocument doc)
		{
			string xpath = string.Format(@"//PROPERTY[@NAME = '{0}']/VALUE/child::text()", propertyName);
			XmlNode node = doc.SelectSingleNode(xpath);
			return node != null ? node.Value : null;
		}

		public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
		{
			ManagementObject objImgSvc = GetImageManagementService();

			// get method params
			ManagementBaseObject inParams = objImgSvc.GetMethodParameters("Mount");
			inParams["Path"] = FileUtils.EvaluateSystemVariables(vhdPath);

			ManagementBaseObject outParams = (ManagementBaseObject)objImgSvc.InvokeMethod("Mount", inParams, null);
			JobResult result = CreateJobResultFromWmiMethodResults(outParams);

			// load storage job
			if (result.ReturnValue != ReturnCode.JobStarted)
				throw new Exception("Failed to start Mount job with the following error: " + result.ReturnValue); ;

			ManagementObject objJob = wmi.GetWmiObject("msvm_StorageJob", "InstanceID = '{0}'", result.Job.Id);

			if (!JobCompleted(result.Job))
				throw new Exception("Failed to complete Mount job with the following error: " + result.Job.ErrorDescription);

			try
			{
				List<string> volumes = new List<string>();

				// load output data
				ManagementObject objImage = wmi.GetRelatedWmiObject(objJob, "Msvm_MountedStorageImage");

				int pathId = Convert.ToInt32(objImage["PathId"]);
				int portNumber = Convert.ToInt32(objImage["PortNumber"]);
				int targetId = Convert.ToInt32(objImage["TargetId"]);
				int lun = Convert.ToInt32(objImage["Lun"]);

				string diskAddress = String.Format("Port{0}Path{1}Target{2}Lun{3}", portNumber, pathId, targetId, lun);

				Log.WriteInfo("Disk address: " + diskAddress);

				// find mounted disk using VDS
				Vds.Advanced.AdvancedDisk advancedDisk = null;
				Vds.Pack diskPack = null;

				// first attempt
				System.Threading.Thread.Sleep(3000);
				Log.WriteInfo("Trying to find mounted disk - first attempt");
				FindVdsDisk(diskAddress, out advancedDisk, out diskPack);

				// second attempt
				if (advancedDisk == null)
				{
					System.Threading.Thread.Sleep(20000);
					Log.WriteInfo("Trying to find mounted disk - second attempt");
					FindVdsDisk(diskAddress, out advancedDisk, out diskPack);
				}

				if (advancedDisk == null)
					throw new Exception("Could not find mounted disk");

				// check if DiskPart must be used to bring disk online and clear read-only flag
				bool useDiskPartToClearReadOnly = false;
				if (ConfigurationManager.AppSettings[CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG] != null)
					useDiskPartToClearReadOnly = Boolean.Parse(ConfigurationManager.AppSettings[CONFIG_USE_DISKPART_TO_CLEAR_READONLY_FLAG]);

				// determine disk index for DiskPart
				Wmi cimv2 = new Wmi(ServerNameSettings, WMI_CIMV2_NAMESPACE);
				ManagementObject objDisk = cimv2.GetWmiObject("win32_diskdrive",
					"Model='Msft Virtual Disk SCSI Disk Device' and ScsiTargetID={0} and ScsiLogicalUnit={1} and scsiPort={2}",
					targetId, lun, portNumber);

				if (useDiskPartToClearReadOnly)
				{
					// *** Clear Read-Only and bring disk online with DiskPart ***
					Log.WriteInfo("Clearing disk Read-only flag and bringing disk online");

					if (objDisk != null)
					{
						// disk found
						// run DiskPart
						string diskPartResult = RunDiskPart(String.Format(@"select disk {0}
attributes disk clear readonly
online disk
exit", Convert.ToInt32(objDisk["Index"])));

						Log.WriteInfo("DiskPart Result: " + diskPartResult);
					}
				}
				else
				{
					// *** Clear Read-Only and bring disk online with VDS ***
					// clear Read-Only
					if ((advancedDisk.Flags & Vds.DiskFlags.ReadOnly) == Vds.DiskFlags.ReadOnly)
					{
						Log.WriteInfo("Clearing disk Read-only flag");
						advancedDisk.ClearFlags(Vds.DiskFlags.ReadOnly);
						while ((advancedDisk.Flags & Vds.DiskFlags.ReadOnly) == Vds.DiskFlags.ReadOnly)
						{
							System.Threading.Thread.Sleep(100);
							advancedDisk.Refresh();
						}
					}

					// bring disk ONLINE
					if (advancedDisk.Status == Vds.DiskStatus.Offline)
					{
						Log.WriteInfo("Bringing disk online");
						advancedDisk.Online();
						while (advancedDisk.Status == Vds.DiskStatus.Offline)
						{
							System.Threading.Thread.Sleep(100);
							advancedDisk.Refresh();
						}
					}
				}

				// small pause after getting disk online
				System.Threading.Thread.Sleep(3000);

				// get disk again
				FindVdsDisk(diskAddress, out advancedDisk, out diskPack);

				// find volumes using VDS
				Log.WriteInfo("Querying disk volumes with VDS");
				foreach (Vds.Volume volume in diskPack.Volumes)
				{
					string letter = volume.DriveLetter.ToString();
					if (letter != "")
						volumes.Add(letter);
				}

				// find volumes using WMI
				if (volumes.Count == 0 && objDisk != null)
				{
					Log.WriteInfo("Querying disk volumes with WMI");
					foreach (ManagementObject objPartition in objDisk.GetRelated("Win32_DiskPartition"))
					{
						foreach (ManagementObject objVolume in objPartition.GetRelated("Win32_LogicalDisk"))
						{
							volumes.Add(objVolume["Name"].ToString().TrimEnd(':'));
						}
					}
				}

				Log.WriteInfo("Volumes found: " + volumes.Count);

				// info object
				MountedDiskInfo info = new MountedDiskInfo();
				info.DiskAddress = diskAddress;
				info.DiskVolumes = volumes.ToArray();
				return info;
			}
			catch (Exception ex)
			{
				// unmount disk
				UnmountVirtualHardDisk(vhdPath);

				// throw error
				throw ex;
			}
		}

		private void FindVdsDisk(string diskAddress, out Vds.Advanced.AdvancedDisk advancedDisk, out Vds.Pack diskPack)
		{
			advancedDisk = null;
			diskPack = null;

			Vds.ServiceLoader serviceLoader = new Vds.ServiceLoader();
			Vds.Service vds = serviceLoader.LoadService(ServerNameSettings);
			vds.WaitForServiceReady();

			foreach (Vds.Disk disk in vds.UnallocatedDisks)
			{
				if (disk.DiskAddress == diskAddress)
				{
					advancedDisk = (Vds.Advanced.AdvancedDisk)disk;
					break;
				}
			}

			if (advancedDisk == null)
			{
				vds.HardwareProvider = false;
				vds.SoftwareProvider = true;

				foreach (Vds.SoftwareProvider provider in vds.Providers)
					foreach (Vds.Pack pack in provider.Packs)
						foreach (Vds.Disk disk in pack.Disks)
							if (disk.DiskAddress == diskAddress)
							{
								diskPack = pack;
								advancedDisk = (Vds.Advanced.AdvancedDisk)disk;
								break;
							}
			}
		}

		public ReturnCode UnmountVirtualHardDisk(string vhdPath)
		{
			ManagementObject objImgSvc = GetImageManagementService();

			// get method params
			ManagementBaseObject inParams = objImgSvc.GetMethodParameters("Unmount");
			inParams["Path"] = FileUtils.EvaluateSystemVariables(vhdPath);

			ManagementBaseObject outParams = (ManagementBaseObject)objImgSvc.InvokeMethod("Unmount", inParams, null);
			return (ReturnCode)Convert.ToInt32(outParams["ReturnValue"]);
		}

		public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
		{
			const UInt64 Size1G = 0x40000000;

			ManagementObject objImgSvc = GetImageManagementService();

			// get method params
			ManagementBaseObject inParams = objImgSvc.GetMethodParameters("ExpandVirtualHardDisk");
			inParams["Path"] = FileUtils.EvaluateSystemVariables(vhdPath);
			inParams["MaxInternalSize"] = sizeGB * Size1G;

			ManagementBaseObject outParams = (ManagementBaseObject)objImgSvc.InvokeMethod("ExpandVirtualHardDisk", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}

		public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
		{
			sourcePath = FileUtils.EvaluateSystemVariables(sourcePath);
			destinationPath = FileUtils.EvaluateSystemVariables(destinationPath);

			// check source file
			if (!FileExists(sourcePath))
				throw new Exception("Source VHD cannot be found: " + sourcePath);

			// check destination folder
			string destFolder = Path.GetDirectoryName(destinationPath);
			if (!DirectoryExists(destFolder))
				CreateFolder(destFolder);

			ManagementObject objImgSvc = GetImageManagementService();

			// get method params
			ManagementBaseObject inParams = objImgSvc.GetMethodParameters("ConvertVirtualHardDisk");
			inParams["SourcePath"] = sourcePath;
			inParams["DestinationPath"] = destinationPath;
			inParams["Type"] = (UInt16)diskType;

			ManagementBaseObject outParams = (ManagementBaseObject)objImgSvc.InvokeMethod("ConvertVirtualHardDisk", inParams, null);
			return CreateJobResultFromWmiMethodResults(outParams);
		}

		public void DeleteRemoteFile(string path)
		{
			if (DirectoryExists(path))
				DeleteFolder(path); // WMI way
			else if (FileExists(path))
				DeleteFile(path); // WMI way
		}

		public void ExpandDiskVolume(string diskAddress, string volumeName)
		{
			// find mounted disk using VDS
			Vds.Advanced.AdvancedDisk advancedDisk = null;
			Vds.Pack diskPack = null;

			FindVdsDisk(diskAddress, out advancedDisk, out diskPack);

			if (advancedDisk == null)
				throw new Exception("Could not find mounted disk");

			// find volume
			Vds.Volume diskVolume = null;
			foreach (Vds.Volume volume in diskPack.Volumes)
			{
				if (volume.DriveLetter.ToString() == volumeName)
				{
					diskVolume = volume;
					break;
				}
			}

			if (diskVolume == null)
				throw new Exception("Could not find disk volume: " + volumeName);

			// determine maximum available space
			ulong oneMegabyte = 1048576;
			ulong freeSpace = 0;
			foreach (Vds.DiskExtent extent in advancedDisk.Extents)
			{
				if (extent.Type != Microsoft.Storage.Vds.DiskExtentType.Free)
					continue;

				if (extent.Size > oneMegabyte)
					freeSpace += extent.Size;
			}

			if (freeSpace == 0)
				return;

			// input disk
			Vds.InputDisk inputDisk = new Vds.InputDisk();
			foreach (Vds.VolumePlex plex in diskVolume.Plexes)
			{
				inputDisk.DiskId = advancedDisk.Id;
				inputDisk.Size = freeSpace;
				inputDisk.PlexId = plex.Id;

				foreach (Vds.DiskExtent extent in plex.Extents)
					inputDisk.MemberIndex = extent.MemberIndex;

				break;
			}

			// extend volume
			Vds.Async extendEvent = diskVolume.BeginExtend(new Vds.InputDisk[] { inputDisk }, null, null);
			while (!extendEvent.IsCompleted)
				System.Threading.Thread.Sleep(100);
			diskVolume.EndExtend(extendEvent);
		}

		// obsolete and currently is not used
		private string RunDiskPart(string script)
		{
			// create temp script file name
			string localPath = Path.Combine(GetTempRemoteFolder(), Guid.NewGuid().ToString("N"));

			// save script to remote temp file
			string remotePath = ConvertToUNC(localPath);
			File.AppendAllText(remotePath, script);

			// run diskpart
			ExecuteRemoteProcess("DiskPart /s " + localPath);

			// delete temp script
			try
			{
				File.Delete(remotePath);
			}
			catch
			{
				// TODO
			}

			return "";
		}

		public string ReadRemoteFile(string path)
		{
			// temp file name on "system" drive available through hidden share
			string tempPath = Path.Combine(GetTempRemoteFolder(), Guid.NewGuid().ToString("N"));

			Log.WriteInfo("Read remote file: " + path);
			Log.WriteInfo("Local file temp path: " + tempPath);

			// copy remote file to temp file (WMI)
			if (!CopyFile(path, tempPath))
				return null;

			// read content of temp file
			string remoteTempPath = ConvertToUNC(tempPath);
			Log.WriteInfo("Remote file temp path: " + remoteTempPath);

			string content = File.ReadAllText(remoteTempPath);

			// delete temp file (WMI)
			DeleteFile(tempPath);

			return content;
		}

		public void WriteRemoteFile(string path, string content)
		{
			// temp file name on "system" drive available through hidden share
			string tempPath = Path.Combine(GetTempRemoteFolder(), Guid.NewGuid().ToString("N"));

			// write to temp file
			string remoteTempPath = ConvertToUNC(tempPath);
			File.WriteAllText(remoteTempPath, content);

			// delete file (WMI)
			if (FileExists(path))
				DeleteFile(path);

			// copy (WMI)
			CopyFile(tempPath, path);

			// delete temp file (WMI)
			DeleteFile(tempPath);
		}
		#endregion

		#region Jobs
		public ConcreteJob GetJob(string jobId)
		{
			ManagementObject objJob = wmi.GetWmiObject("CIM_ConcreteJob", "InstanceID = '{0}'", jobId);
			return CreateJobFromWmiObject(objJob);
		}

		public List<ConcreteJob> GetAllJobs()
		{
			List<ConcreteJob> jobs = new List<ConcreteJob>();

			ManagementObjectCollection objJobs = wmi.GetWmiObjects("CIM_ConcreteJob");
			foreach (ManagementObject objJob in objJobs)
				jobs.Add(CreateJobFromWmiObject(objJob));

			return jobs;
		}

		public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
		{
			ManagementObject objJob = GetJobWmiObject(jobId);

			// get method
			ManagementBaseObject inParams = objJob.GetMethodParameters("RequestStateChange");
			inParams["RequestedState"] = (Int32)newState;

			// invoke method
			ManagementBaseObject outParams = objJob.InvokeMethod("RequestStateChange", inParams, null);
			return (ChangeJobStateReturnCode)Convert.ToInt32(outParams["ReturnValue"]);
		}

		#endregion

		#region Configuration
		public int GetProcessorCoresNumber(string templateId)
		{
			int ret = 0;

			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{
				TemplateInfo selTemplate = client.GetTemplateById(new Guid(templateId));
				ret = selTemplate.CPUMax;
			}
			return ret;
		}
		#endregion

		#region IHostingServiceProvier methods
		public override string[] Install()
		{
			List<string> messages = new List<string>();

			// TODO

			return messages.ToArray();
		}

		public override bool IsInstalled()
		{
			// check if Hyper-V role is installed and available for management
			//Wmi root = new Wmi(ServerNameSettings, "root");
			//ManagementObject objNamespace = root.GetWmiObject("__NAMESPACE", "name = 'virtualization'");
			//return (objNamespace != null);
			return true;
		}

		public override void ChangeServiceItemsState(ServiceProviderItem[] items, bool enabled)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is VirtualMachine)
				{
					// start/stop virtual machine
					VirtualMachine vm = item as VirtualMachine;
					ChangeVirtualMachineServiceItemState(vm, enabled);
				}
			}
		}

		public override void DeleteServiceItems(ServiceProviderItem[] items)
		{
			foreach (ServiceProviderItem item in items)
			{
				if (item is VirtualMachine)
				{
					// delete virtual machine
					VirtualMachine vm = item as VirtualMachine;
					DeleteVirtualMachineServiceItem(vm);
				}
				else if (item is VirtualSwitch)
				{
					// delete switch
					VirtualSwitch vs = item as VirtualSwitch;
					DeleteVirtualSwitchServiceItem(vs);
				}
			}
		}

		private void ChangeVirtualMachineServiceItemState(VirtualMachine vm, bool started)
		{
			try
			{
				VMInfo vps = GetVirtualMachine(vm.VirtualMachineId);
				JobResult result = null;

				if (vps == null)
				{
					Log.WriteWarning(String.Format("Virtual machine '{0}' object with ID '{1}' was not found. Change state operation aborted.",
						vm.Name, vm.VirtualMachineId));
					return;
				}

				#region Start
				if (started &&
					(vps.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.PowerOff
					|| vps.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.Paused
					|| vps.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.Saved))
				{
					VirtualMachineRequestedState state = VirtualMachineRequestedState.Start;
					if (vps.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.Paused)
					{
						state = VirtualMachineRequestedState.Resume;
					}

					result = ChangeVirtualMachineState(vm.VirtualMachineId, state);

					// check result
					if (result.ReturnValue != ReturnCode.JobStarted)
					{
						Log.WriteWarning(String.Format("Cannot {0} '{1}' virtual machine: {2}",
							state, vm.Name, result.ReturnValue));
						return;
					}

					// wait for completion
					if (!JobCompleted(result.Job))
					{
						Log.WriteWarning(String.Format("Cannot complete {0} '{1}' of virtual machine: {1}",
							state, vm.Name, result.Job.ErrorDescription));
						return;
					}
				}
				#endregion

				#region Stop
				else if (!started &&
					(vps.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.Starting
					|| vps.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.Paused))
				{
					if (vps.State == WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.Running)
					{
						// try to shutdown the system
						ReturnCode code = ShutDownVirtualMachine(vm.VirtualMachineId, true, "Virtual Machine has been suspended from WebsitePanel");
						if (code == ReturnCode.OK)
							return;
					}

					// turn off
					VirtualMachineRequestedState state = VirtualMachineRequestedState.TurnOff;
					result = ChangeVirtualMachineState(vm.VirtualMachineId, state);

					// check result
					if (result.ReturnValue != ReturnCode.JobStarted)
					{
						Log.WriteWarning(String.Format("Cannot {0} '{1}' virtual machine: {2}",
							state, vm.Name, result.ReturnValue));
						return;
					}

					// wait for completion
					if (!JobCompleted(result.Job))
					{
						Log.WriteWarning(String.Format("Cannot complete {0} '{1}' of virtual machine: {1}",
							state, vm.Name, result.Job.ErrorDescription));
						return;
					}
				}
				#endregion
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error {0} Virtual Machine '{1}'",
					started ? "starting" : "turning off",
					vm.Name), ex);
			}
		}

		private void DeleteVirtualMachineServiceItem(VirtualMachine vm)
		{
			try
			{
				JobResult result = null;
				VMInfo vps = GetVirtualMachine(vm.VirtualMachineId);

				if (vps == null)
				{
					Log.WriteWarning(String.Format("Virtual machine '{0}' object with ID '{1}' was not found. Delete operation aborted.",
						vm.Name, vm.VirtualMachineId));
					return;
				}

				#region Turn off (if required)
				if (vps.State != WebsitePanel.Providers.Virtualization.VMComputerSystemStateInfo.PowerOff)
				{
					result = ChangeVirtualMachineState(vm.VirtualMachineId, VirtualMachineRequestedState.TurnOff);
					// check result
					if (result.ReturnValue != ReturnCode.JobStarted)
					{
						Log.WriteWarning(String.Format("Cannot Turn off '{0}' virtual machine before deletion: {1}",
							vm.Name, result.ReturnValue));
						return;
					}

					// wait for completion
					if (!JobCompleted(result.Job))
					{
						Log.WriteWarning(String.Format("Cannot complete Turn off '{0}' of virtual machine before deletion: {1}",
							vm.Name, result.Job.ErrorDescription));
						return;
					}
				}
				#endregion

				#region Delete virtual machine
				result = DeleteVirtualMachine(vm.VirtualMachineId);

				// check result
				if (result.ReturnValue != ReturnCode.JobStarted)
				{
					Log.WriteWarning(String.Format("Cannot delete '{0}' virtual machine: {1}",
						vm.Name, result.ReturnValue));
					return;
				}

				// wait for completion
				if (!JobCompleted(result.Job))
				{
					Log.WriteWarning(String.Format("Cannot complete deletion of '{0}' virtual machine: {1}",
						vm.Name, result.Job.ErrorDescription));
					return;
				}
				#endregion

				#region Delete virtual machine
				try
				{
					DeleteFile(vm.RootFolderPath);
				}
				catch (Exception ex)
				{
					Log.WriteError(String.Format("Cannot delete virtual machine folder '{0}'",
						vm.RootFolderPath), ex);
				}
				#endregion

			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error deleting Virtual Machine '{0}'", vm.Name), ex);
			}
		}

		private void DeleteVirtualSwitchServiceItem(VirtualSwitch vs)
		{
			try
			{
				// delete virtual switch
				DeleteSwitch(vs.SwitchId);
			}
			catch (Exception ex)
			{
				Log.WriteError(String.Format("Error deleting Virtual Switch '{0}'", vs.Name), ex);
			}
		}
		#endregion

		#region Private Methods
		private JobResult CreateJobResultFromWmiMethodResults(ManagementBaseObject outParams)
		{
			JobResult result = new JobResult();

			// return value
			result.ReturnValue = (ReturnCode)Convert.ToInt32(outParams["ReturnValue"]);

			// try getting job details job
			try
			{
				ManagementBaseObject objJob = wmi.GetWmiObjectByPath((string)outParams["Job"]);
				if (objJob != null && objJob.Properties.Count > 0)
				{
					result.Job = CreateJobFromWmiObject(objJob);
				}
			}
			catch { /* dumb */ }

			return result;
		}

		private ManagementObject GetJobWmiObject(string id)
		{
			return wmi.GetWmiObject("msvm_ConcreteJob", "InstanceID = '{0}'", id);
		}

		private ManagementObject GetVirtualSystemManagementService()
		{
			return wmi.GetWmiObject("msvm_VirtualSystemManagementService");
		}

		private ManagementObject GetVirtualSwitchManagementService()
		{
			return wmi.GetWmiObject("msvm_VirtualSwitchManagementService");
		}

		private ManagementObject GetImageManagementService()
		{
			return wmi.GetWmiObject("msvm_ImageManagementService");
		}

		private ManagementObject GetVirtualMachineObject(string vmId)
		{
			return wmi.GetWmiObject("msvm_ComputerSystem", "Name = '{0}'", vmId);
		}

		private ManagementObject GetSnapshotObject(string snapshotId)
		{
			return wmi.GetWmiObject("Msvm_VirtualSystemSettingData", "InstanceID = '{0}'", snapshotId);
		}

		private VirtualMachine CreateVirtualMachineFromWmiObject(ManagementObject objVm)
		{
			using (WSPVirtualMachineManagementServiceClient client = GetVMMSClient())
			{

				if (objVm == null || objVm.Properties.Count == 0)
					return null;

				VirtualMachine vm = new VirtualMachine();
				vm.VirtualMachineId = (string)objVm["Name"];
				vm.Name = (string)objVm["ElementName"];
				vm.State = (VirtualMachineState)Convert.ToInt32(objVm["EnabledState"]);
				vm.Uptime = Convert.ToInt64(objVm["OnTimeInMilliseconds"]);
				return vm;
			}
		}

		//private VMInfo CreateVMFromWmiObject(VirtualMachineInfo objVm)
		//{
		//    VMInfo vm = new VMInfo();

		//    vm.Name = objVm.Name;
		//    vm.HostName = objVm.HostName;
		//    vm.VmGuid = objVm.Id;
		//    vm.State = (Virtualization.VMComputerSystemStateInfo)objVm.Status;
		//    vm.CPUCount = objVm.CPUCount;
		//    vm.CreatedDate = objVm.CreationTime;
		//    vm.ComputerName  = objVm.ComputerName;
		//    vm.Owner = objVm.Owner;
		//    vm.JoinDomain = objVm.VMHost.DomainName;
		//    vm.CPUUtilization = objVm.CPUUtilization;
		//    vm.PerfCPUUtilization = objVm.perfCPUUtilization;
		//    vm.ModifiedTime = "00:00:00";
		//    vm.Memory = objVm.Memory;
		//    vm.ProcessMemory = objVm.Memory;

		//    if (objVm.VirtualHardDisks != null)
		//    {
		//        vm.HddLogicalDisks = new LogicalDisk[objVm.VirtualHardDisks.Length];
		//        for (int i = 0; i < objVm.VirtualHardDisks.Length; i++)
		//        {
		//            vm.HddLogicalDisks[i] = new LogicalDisk();
		//            vm.HddLogicalDisks[i].Size = (int)(objVm.VirtualHardDisks[i].MaximumSize / ByteToGbByte);
		//            vm.HddLogicalDisks[i].FreeSpace = (int)(((long)objVm.VirtualHardDisks[i].MaximumSize - objVm.VirtualHardDisks[i].Size) / ByteToGbByte);
		//            vm.HddLogicalDisks[i].DriveLetter = objVm.VirtualHardDisks[i].Name;
		//        }
		//    }

		//    vm.ProvisioningStatus = VirtualMachineProvisioningStatus.OK;

		//    return vm;
		//}

		private VirtualMachineSnapshot CreateSnapshotFromWmiObject(ManagementBaseObject objSnapshot)
		{
			if (objSnapshot == null || objSnapshot.Properties.Count == 0)
				return null;

			VirtualMachineSnapshot snapshot = new VirtualMachineSnapshot();
			snapshot.Id = (string)objSnapshot["InstanceID"];
			snapshot.Name = (string)objSnapshot["ElementName"];

			string parentId = (string)objSnapshot["Parent"];
			if (!String.IsNullOrEmpty(parentId))
			{
				int idx = parentId.IndexOf("Microsoft:");
				snapshot.ParentId = parentId.Substring(idx, parentId.Length - idx - 1);
			}
			snapshot.Created = wmi.ToDateTime((string)objSnapshot["CreationTime"]);

			return snapshot;
		}

		private VirtualSwitch CreateSwitchFromWmiObject(ManagementObject objSwitch)
		{
			if (objSwitch == null || objSwitch.Properties.Count == 0)
				return null;

			VirtualSwitch sw = new VirtualSwitch();
			sw.SwitchId = (string)objSwitch["Name"];
			sw.Name = (string)objSwitch["ElementName"];
			return sw;
		}

		private ConcreteJob CreateJobFromWmiObject(ManagementBaseObject objJob)
		{
			if (objJob == null || objJob.Properties.Count == 0)
				return null;

			ConcreteJob job = new ConcreteJob();
			job.Id = (string)objJob["InstanceID"];
			job.JobState = (ConcreteJobState)Convert.ToInt32(objJob["JobState"]);
			job.Caption = (string)objJob["Caption"];
			job.Description = (string)objJob["Description"];
			job.StartTime = wmi.ToDateTime((string)objJob["StartTime"]);
			// TODO proper parsing of WMI time spans, e.g. 00000000000001.325247:000
			job.ElapsedTime = DateTime.Now; //wmi.ToDateTime((string)objJob["ElapsedTime"]);
			job.ErrorCode = Convert.ToInt32(objJob["ErrorCode"]);
			job.ErrorDescription = (string)objJob["ErrorDescription"];
			job.PercentComplete = Convert.ToInt32(objJob["PercentComplete"]);
			return job;
		}

		private ManagementBaseObject GetSnapshotSummaryInformation(
			string snapshotId,
			SummaryInformationRequest requestedInformation)
		{
			// find VM settings object
			ManagementObject objVmSetting = GetSnapshotObject(snapshotId);

			// get summary
			return GetSummaryInformation(objVmSetting, requestedInformation);
		}

		private ManagementBaseObject GetVirtualMachineSummaryInformation(
			string vmId,
			params SummaryInformationRequest[] requestedInformation)
		{
			// find VM settings object
			ManagementObject objVmSetting = GetVirtualMachineSettingsObject(vmId);

			// get summary
			return GetSummaryInformation(objVmSetting, requestedInformation);
		}

		private ManagementBaseObject GetSummaryInformation(
			ManagementObject objVmSetting, params SummaryInformationRequest[] requestedInformation)
		{
			if (requestedInformation == null || requestedInformation.Length == 0)
				throw new ArgumentNullException("requestedInformation");

			// get management service
			ManagementObject objVmsvc = GetVirtualSystemManagementService();

			uint[] reqif = new uint[requestedInformation.Length];
			for (int i = 0; i < requestedInformation.Length; i++)
				reqif[i] = (uint)requestedInformation[i];

			// get method params
			ManagementBaseObject inParams = objVmsvc.GetMethodParameters("GetSummaryInformation");
			inParams["SettingData"] = new ManagementObject[] { objVmSetting };
			inParams["RequestedInformation"] = reqif;

			// invoke method
			ManagementBaseObject outParams = objVmsvc.InvokeMethod("GetSummaryInformation", inParams, null);
			return ((ManagementBaseObject[])outParams["SummaryInformation"])[0];
		}

		private ManagementObject GetVirtualMachineSettingsObject(string vmId)
		{
			return wmi.GetWmiObject("msvm_VirtualSystemSettingData", "InstanceID Like 'Microsoft:{0}%'", vmId);
		}

		private bool JobCompleted(ConcreteJob job)
		{
			bool jobCompleted = true;

			while (job.JobState == ConcreteJobState.Starting ||
				job.JobState == ConcreteJobState.Running)
			{
				System.Threading.Thread.Sleep(200);
				job = GetJob(job.Id);
			}

			if (job.JobState != ConcreteJobState.Completed)
			{
				jobCompleted = false;
			}

			return jobCompleted;
		}
		#endregion

		#region Remote File Methods
		public bool FileExists(string path)
		{
			Log.WriteInfo("Check remote file exists: " + path);

			if (path.StartsWith(@"\\")) // network share
				return File.Exists(path);
			else
			{
				Wmi cimv2 = new Wmi(ServerNameSettings, WMI_CIMV2_NAMESPACE);
				ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", path.Replace("\\", "\\\\"));
				return (objFile != null);
			}
		}

		public bool DirectoryExists(string path)
		{
			if (path.StartsWith(@"\\")) // network share
				return Directory.Exists(path);
			else
			{
				Wmi cimv2 = new Wmi(ServerNameSettings, WMI_CIMV2_NAMESPACE);
				ManagementObject objDir = cimv2.GetWmiObject("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));
				return (objDir != null);
			}
		}

		public bool CopyFile(string sourceFileName, string destinationFileName)
		{
			Log.WriteInfo("Copy file - source: " + sourceFileName);
			Log.WriteInfo("Copy file - destination: " + destinationFileName);

			if (sourceFileName.StartsWith(@"\\")) // network share
			{
				if (!File.Exists(sourceFileName))
					return false;

				File.Copy(sourceFileName, destinationFileName);
			}
			else
			{
				if (!FileExists(sourceFileName))
					return false;

				// copy using WMI
				Wmi cimv2 = new Wmi(ServerNameSettings, WMI_CIMV2_NAMESPACE);
				ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", sourceFileName.Replace("\\", "\\\\"));
				if (objFile == null)
					throw new Exception("Source file does not exists: " + sourceFileName);

				objFile.InvokeMethod("Copy", new object[] { destinationFileName });
			}
			return true;
		}

		public void DeleteFile(string path)
		{
			if (path.StartsWith(@"\\"))
			{
				// network share
				File.Delete(path);
			}
			else
			{
				// delete file using WMI
				Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");
				ManagementObject objFile = cimv2.GetWmiObject("CIM_Datafile", "Name='{0}'", path.Replace("\\", "\\\\"));
				objFile.InvokeMethod("Delete", null);
			}
		}

		public void DeleteFolder(string path)
		{
			if (path.StartsWith(@"\\"))
			{
				// network share
				try
				{
					FileUtils.DeleteFile(path);
				}
				catch { /* just skip */ }
				FileUtils.DeleteFile(path);
			}
			else
			{
				// local folder
				// delete sub folders first
				ManagementObjectCollection objSubFolders = GetSubFolders(path);
				foreach (ManagementObject objSubFolder in objSubFolders)
					DeleteFolder(objSubFolder["Name"].ToString());

				// delete this folder itself
				Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");
				ManagementObject objFolder = cimv2.GetWmiObject("Win32_Directory", "Name='{0}'", path.Replace("\\", "\\\\"));
				objFolder.InvokeMethod("Delete", null);
			}
		}

		private ManagementObjectCollection GetSubFolders(string path)
		{
			if (path.EndsWith("\\"))
				path = path.Substring(0, path.Length - 1);

			Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");

			return cimv2.ExecuteWmiQuery("Associators of {Win32_Directory.Name='"
				+ path + "'} "
				+ "Where AssocClass = Win32_Subdirectory "
				+ "ResultRole = PartComponent");
		}

		public void CreateFolder(string path)
		{
			ExecuteRemoteProcess(String.Format("cmd.exe /c md \"{0}\"", path));
		}

		public void ExecuteRemoteProcess(string command)
		{
			Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");
			ManagementClass objProcess = cimv2.GetWmiClass("Win32_Process");

			// run process
			object[] methodArgs = { command, null, null, 0 };
			objProcess.InvokeMethod("Create", methodArgs);

			// process ID
			int processId = Convert.ToInt32(methodArgs[3]);

			// wait until finished
			// Create event query to be notified within 1 second of 
			// a change in a service
			WqlEventQuery query =
				new WqlEventQuery("__InstanceDeletionEvent",
				new TimeSpan(0, 0, 1),
				"TargetInstance isa \"Win32_Process\"");

			// Initialize an event watcher and subscribe to events 
			// that match this query
			ManagementEventWatcher watcher = new ManagementEventWatcher(cimv2.GetScope(), query);
			// times out watcher.WaitForNextEvent in 20 seconds
			watcher.Options.Timeout = new TimeSpan(0, 0, 20);

			// Block until the next event occurs 
			// Note: this can be done in a loop if waiting for 
			//        more than one occurrence
			while (true)
			{
				ManagementBaseObject e = null;

				try
				{
					// wait untill next process finish
					e = watcher.WaitForNextEvent();
				}
				catch
				{
					// nothing has been finished in timeout period
					return; // exit
				}

				// check process id
				int pid = Convert.ToInt32(((ManagementBaseObject)e["TargetInstance"])["ProcessID"]);
				if (pid == processId)
				{
					//Cancel the subscription
					watcher.Stop();

					// exit
					return;
				}
			}
		}

		public string GetTempRemoteFolder()
		{
			Wmi cimv2 = new Wmi(ServerNameSettings, "root\\cimv2");
			ManagementObject objOS = cimv2.GetWmiObject("win32_OperatingSystem");
			string sysPath = (string)objOS["SystemDirectory"];

			// remove trailing slash
			if (sysPath.EndsWith("\\"))
				sysPath = sysPath.Substring(0, sysPath.Length - 1);

			sysPath = sysPath.Substring(0, sysPath.LastIndexOf("\\") + 1) + "Temp";

			return sysPath;
		}
		#endregion

		#region Hyper-V Cloud

		public bool CheckServerState(VMForPCSettingsName control, string connString, string connName)
		{
			bool ret = false;

			try
			{
				switch (control)
				{
					case VMForPCSettingsName.SCVMMServer:
						{
							if (!String.IsNullOrWhiteSpace(connString)
								&& !String.IsNullOrWhiteSpace(connName))
							{
								EndpointAddress endPointAddress = GetEndPointAddress(connString, connName);

								using (VirtualMachineManagementServiceClient check = new VirtualMachineManagementServiceClient(new WSHttpBinding("WSHttpBinding_IVirtualMachineManagementService"), endPointAddress))
								{
									check.Open();
									ret = true;
									check.Close();
								}
							}
							break;
						}
					case VMForPCSettingsName.SCOMServer:
						{
							if (!String.IsNullOrWhiteSpace(connString)
								&& !String.IsNullOrWhiteSpace(connName))
							{
								EndpointAddress endPointAddress = GetEndPointAddress(connString, connName);

								using (MonitoringServiceClient checkMonitoring = new MonitoringServiceClient(new WSHttpBinding("WSHttpBinding_IMonitoringService"), endPointAddress))
								{
									checkMonitoring.Open();
									ret = true;
									checkMonitoring.Close();
								}
							}
							break;
						}
				}
			}
			catch (Exception ex)
			{
				//
				Log.WriteError("Could not check server state", ex);
				//
				ret = false;
				//
				throw;
			}
			return ret;
		}

		#endregion Hyper-V Cloud

		#region Monitoring
		/// <summary>
		/// Get device Events
		/// </summary>
		/// <param name="serviceName">serviceName</param>
		/// <param name="displayName">displayName</param>
		/// <returns></returns>
		public List<MonitoredObjectEvent> GetDeviceEvents(string serviceName, string displayName)
		{
			List<MonitoredObjectEvent> monitoredObjectEventCollection = new List<MonitoredObjectEvent>();
			using (WSPVirtualMachineManagementServiceClient context = GetVMMSClient())
			{
				VirtualMachineInfo vmi = context.GetVirtualMachineByName(displayName);
				if (vmi != null)
				{
					using (WSPMonitoringServiceClient client = GetMonitoringServiceClient())
					{
						MonitoredObject monitoringObject = client.GetMonitoredObjectByDisplayName(vmi.HostName, vmi.ComputerName);
						foreach (var item in monitoringObject.Events)
						{
							monitoredObjectEventCollection.Add(
								new MonitoredObjectEvent
								{
									Category = item.Category,
									Decription = item.Decription,
									EventData = item.EventData,
									Level = item.Level,
									Number = item.Number,
									TimeGenerated = item.TimeGenerated
								});
						}
					}
				}
			}

			return monitoredObjectEventCollection;
		}

		public List<MonitoredObjectAlert> GetMonitoringAlerts(string serviceName, string virtualMachineName)
		{
			List<MonitoredObjectAlert> result = new List<MonitoredObjectAlert>();
			using (WSPVirtualMachineManagementServiceClient context = GetVMMSClient())
			{
				VirtualMachineInfo vmi = context.GetVirtualMachineByName(virtualMachineName);
				if (vmi != null)
				{
					using (WSPMonitoringServiceClient client = GetMonitoringServiceClient())
					{
						MonitoredObject mo = client.GetMonitoredObjectByDisplayName(vmi.HostName, vmi.ComputerName);

						//                        Alert[] alerts = client.GetMonitoringAlertsByObjectDisplayName(serviceName, GetComputerNameByVMName(virtualMachineName));
						Alert[] alerts = mo.Alerts;
						foreach (var item in alerts)
						{
							result.Add(
								new MonitoredObjectAlert
								{
									Created = item.Created,
									Description = item.Description,
									Name = item.Name,
									ResolutionState = item.ResolutionState,
									Severity = item.Severity,
									Source = item.Source
								});
						}
					}
				}
			}
			return result;
		}

		public List<Virtualization.PerformanceDataValue> GetPerfomanceValue(string VmName, PerformanceType perf, DateTime startPeriod, DateTime endPeriod)
		{
			List<Virtualization.PerformanceDataValue> ret = new List<Virtualization.PerformanceDataValue>();

			/* This test code */
			//Random random = new Random((int)DateTime.Now.Ticks);

			//TimeSpan count = (endPeriod - startPeriod);

			//for (int pointIndex = 0; pointIndex < 20; pointIndex++)
			//{
			//    ret.Add(new Virtualization.PerformanceDataValue() { SampleValue = random.Next(1, 99), TimeSampled = DateTime.Now });
			//}

			//return ret;


			using (WSPMonitoringServiceClient client = GetMonitoringServiceClient())
			{
				client.Open();

				PerformanceData[] pdOneVM = null;

				switch (perf)
				{
					case PerformanceType.Processor:
						pdOneVM = client.GetSingleVMHyperVCPUCounters(MonitoringServerNameSettings, VmName);
						break;
					case PerformanceType.Network:
						pdOneVM = client.GetSingleVMHyperVVirtualNetwork(MonitoringServerNameSettings, VmName);
						break;
					case PerformanceType.Memory:
						pdOneVM = client.GetSingleVMHyperVGuestMemoryPagesAllocated(MonitoringServerNameSettings, VmName);
						break;
					//case PerformanceType.DiskIO:
					//    break;

				}

				if ((pdOneVM != null) && (pdOneVM.Length > 0))
				{
					WebsitePanel.Providers.VirtualizationForPC.MonitoringWebService.PerformanceDataValue[] retData =
						client.GetMonitoringPerformanceValues(MonitoringServerNameSettings, pdOneVM[0], startPeriod, endPeriod);

					int index = 1;

					if (retData.Length > 100)
					{
						index = (int)Math.Ceiling(((double)retData.Length) / 100);
					}

					for (int i = 0; i < retData.Length; i = i + index)
					{
						WebsitePanel.Providers.VirtualizationForPC.MonitoringWebService.PerformanceDataValue curr = retData[i];

						ret.Add(new Virtualization.PerformanceDataValue()
						{
							SampleValue = curr.SampleValue
							,
							TimeAdded = curr.TimeAdded
							,
							TimeSampled = curr.TimeSampled
							,
							ExtensionData = curr.ExtensionData
						});
					}
				}

				client.Close();
			}

			return ret;

			/* This test code */
			//Random random = new Random((int)DateTime.Now.Ticks);

			//TimeSpan count = (endPeriod - startPeriod);

			//for (int pointIndex = 0; pointIndex < 20; pointIndex++)
			//{
			//    ret.Add(new Virtualization.PerformanceDataValue() { SampleValue = random.Next(1, 99) });
			//}

			//return ret;
		}

		private static Dictionary<string, string> computerNameByVMName = new Dictionary<string, string>();

		private string GetComputerNameByVMName(string virtualMachineName)
		{
			string result;
			if (!computerNameByVMName.TryGetValue(virtualMachineName, out result))
			{
				using (WSPVirtualMachineManagementServiceClient context = GetVMMSClient())
				{
					VirtualMachineInfo vmInfo = context.GetVirtualMachineByName(virtualMachineName);
					computerNameByVMName[virtualMachineName] = result = (vmInfo != null) ? vmInfo.ComputerName : string.Empty;
				}
			}
			return result;
		}
		private EndpointAddress GetEndPointAddress(string connString, string connName)
		{
			bool UseSPN = true;

			if (!Boolean.TryParse(ConfigurationManager.AppSettings["UseSPN"], out UseSPN))
			{
				UseSPN = false;
			}

			EndpointAddress endPointAddress = null;

			if (UseSPN)
			{
				endPointAddress = new EndpointAddress(new Uri(connString)
								 , EndpointIdentity.CreateSpnIdentity(connName));
			}
			else
			{
				endPointAddress = new EndpointAddress(new Uri(connString)
								 , EndpointIdentity.CreateUpnIdentity(connName));

			}

			return endPointAddress;
		}

		#endregion

		#region Procxy

		public WSPVirtualMachineManagementServiceClient GetVMMSClient()
		{
			WSPVirtualMachineManagementServiceClient ret;
			try
			{
				if (!String.IsNullOrWhiteSpace(SCVMMServer)
					&& !String.IsNullOrWhiteSpace(SCVMMPrincipalName))
				{
					EndpointAddress endPointAddress = GetEndPointAddress(SCVMMServer, SCVMMPrincipalName);

					ret = new WSPVirtualMachineManagementServiceClient(new WSHttpBinding("WSHttpBinding_IVirtualMachineManagementService"), endPointAddress);

					VersionInfo ver = new VersionInfo();
				}
				else
				{
					throw new Exception("SCVMMServer or SCVMMPrincipalName is empty");
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return ret;
		}

		public WSPMonitoringServiceClient GetMonitoringServiceClient()
		{
			WSPMonitoringServiceClient ret;

			try
			{
				if (!String.IsNullOrWhiteSpace(SCOMServer)
					&& !String.IsNullOrWhiteSpace(SCOMPrincipalName))
				{
					EndpointAddress endPointAddress = GetEndPointAddress(SCOMServer, SCOMPrincipalName);

					ret = new WSPMonitoringServiceClient(new WSHttpBinding("WSHttpBinding_IMonitoringService"), endPointAddress);
				}
				else
				{
					throw new Exception("MonitoringServer or MonitoringPrincipalName is empty");
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			return ret;
		}

		#endregion Proxy

	}
}
