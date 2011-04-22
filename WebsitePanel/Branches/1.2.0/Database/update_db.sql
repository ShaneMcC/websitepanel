USE [${install.database}]
GO

-- update database version
DECLARE @build_version nvarchar(10), @build_date datetime
SET @build_version = N'${release.version}'
SET @build_date = '${release.date}T00:00:00' -- ISO 8601 Format (YYYY-MM-DDTHH:MM:SS)

IF NOT EXISTS (SELECT * FROM [dbo].[Versions] WHERE [DatabaseVersion] = @build_version)
BEGIN
	INSERT [dbo].[Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
END
GO

-- Helicon APE's quota
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'Web.Htaccess')
BEGIN
	INSERT [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (344, 2, 9, N'Web.Htaccess', N'htaccess', 1, 0, NULL)
END
GO

-- Change quota order for MySQL 4 Quotas
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [GroupID] = 6 And [QuotaOrder] = 4)
BEGIN
	UPDATE [dbo].[Quotas] SET [QuotaOrder] = 4 WHERE [GroupID] = 6 And [QuotaOrder] = 3
END
GO
-- Check for new MySQL 4 quota (MaxDatabaseSize)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL4.MaxDatabaseSize')
BEGIN
	-- Add new MySQL 4 quotas if not exists
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (103, 6, 3, N'MySQL4.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL)
END
GO
-- Check for new MySQL 4 quota (Restore)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL4.Restore')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (104, 6, 5, N'MySQL4.Restore', N'Database Restores', 1, 0, NULL)
END
GO
-- Check for new MySQL 4 quota (Truncate)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL4.Truncate')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (105, 6, 6, N'MySQL4.Truncate', N'Database Truncate', 1, 0, NULL)
END
GO

-- Change quota order for MySQL 5 Quotas
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [GroupID] = 11 And [QuotaOrder] = 4)
BEGIN
	UPDATE [dbo].[Quotas] SET [QuotaOrder] = 4 WHERE [GroupID] = 11 And [QuotaOrder] = 3
END
GO
-- Add new MySQL 5 quota if not exists (MaxDatabaseSize)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL5.MaxDatabaseSize')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (106, 11, 3, N'MySQL5.MaxDatabaseSize', N'Max Database Size', 3, 0, NULL)
END
GO
-- Add new MySQL 5 quota if not exists (Restore)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL5.Restore')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (107, 11, 5, N'MySQL5.Restore', N'Database Restores', 1, 0, NULL)
END
GO
-- Add new MySQL 5 quota if not exists (Truncate)
IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'MySQL5.Truncate')
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (108, 11, 6, N'MySQL5.Truncate', N'Database Truncate', 1, 0, NULL)
END
GO

-- Notify Overused Databases Scheduled Task
IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTasks] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES')
BEGIN
	INSERT [dbo].[ScheduleTasks] ([TaskID], [TaskType], [RoleID]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WebsitePanel.EnterpriseServer.NotifyOverusedDatabasesTask, WebsitePanel.EnterpriseServer', 2)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskViewConfiguration] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ConfigurationID] = N'ASP_NET')
BEGIN
	INSERT [dbo].[ScheduleTaskViewConfiguration] ([TaskID], [ConfigurationID], [Environment], [Description]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'ASP_NET', N'ASP.NET', N'~/DesktopModules/WebsitePanel/ScheduleTaskControls/NotifyOverusedDatabases.ascx')
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'MSSQL_OVERUSED')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'MSSQL_OVERUSED', N'Boolean', N'true', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'MYSQL_OVERUSED')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'MYSQL_OVERUSED', N'Boolean', N'true', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'SEND_WARNING_EMAIL')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'SEND_WARNING_EMAIL', N'Boolean', N'true', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'SEND_OVERUSED_EMAIL')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'SEND_OVERUSED_EMAIL', N'Boolean', N'true', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_MAIL_BCC')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_BCC', N'String', N'', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_MAIL_BODY')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_BODY', N'String', N'', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_MAIL_FROM')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_FROM', N'String', N'', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_MAIL_SUBJECT')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_MAIL_SUBJECT', N'String', N'', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'OVERUSED_USAGE_THRESHOLD')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'OVERUSED_USAGE_THRESHOLD', N'String', N'100', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_MAIL_BCC')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_BCC', N'String', N'', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_MAIL_BODY')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_BODY', N'String', N'', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_MAIL_FROM')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_FROM', N'String', N'', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_MAIL_SUBJECT')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_MAIL_SUBJECT', N'String', N'', 1)
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[ScheduleTaskParameters] WHERE [TaskID] = N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES' AND [ParameterID] = N'WARNING_USAGE_THRESHOLD')
BEGIN
	INSERT [dbo].[ScheduleTaskParameters] ([TaskID], [ParameterID], [DataTypeID], [DefaultValue], [ParameterOrder]) VALUES (N'SCHEDULE_TASK_NOTIFY_OVERUSED_DATABASES', N'WARNING_USAGE_THRESHOLD', N'String', N'80', 1)
END
GO

-- Fix resource groups order
IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'OS' AND [GroupOrder] = 1)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 1 WHERE [GroupName] = N'OS'
END 
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Web' AND [GroupOrder] = 2)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 2 WHERE [GroupName] = N'Web'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'FTP' AND [GroupOrder] = 3)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 3 WHERE [GroupName] = N'FTP'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Mail' AND [GroupOrder] = 4)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 4 WHERE [GroupName] = N'Mail'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Exchange' AND [GroupOrder] = 5)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 5 WHERE [GroupName] = N'Exchange'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Hosted Organizations' AND [GroupOrder] = 6)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 6 WHERE [GroupName] = N'Hosted Organizations'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'ExchangeHostedEdition' AND [GroupOrder] = 7)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 7 WHERE [GroupName] = N'ExchangeHostedEdition'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'MsSQL2000' AND [GroupOrder] = 8)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 8 WHERE [GroupName] = N'MsSQL2000'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'MsSQL2005' AND [GroupOrder] = 9)
BEGIN
UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 9 WHERE [GroupName] = N'MsSQL2005'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'MsSQL2008' AND [GroupOrder] = 10)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 10 WHERE [GroupName] = N'MsSQL2008'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'MySQL4' AND [GroupOrder] = 11)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 11 WHERE [GroupName] = N'MySQL4'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'MySQL5' AND [GroupOrder] = 12)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 12 WHERE [GroupName] = N'MySQL5'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'SharePoint' AND [GroupOrder] = 13)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 13 WHERE [GroupName] = N'SharePoint'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Hosted SharePoint' AND [GroupOrder] = 14)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 14 WHERE [GroupName] = N'Hosted SharePoint'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Hosted CRM' AND [GroupOrder] = 15)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 15 WHERE [GroupName] = N'Hosted CRM'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'DNS' AND [GroupOrder] = 16)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 16 WHERE [GroupName] = N'DNS'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'Statistics' AND [GroupOrder] = 17)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 17 WHERE [GroupName] = N'Statistics'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'VPS' AND [GroupOrder] = 18)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 18 WHERE [GroupName] = N'VPS'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'BlackBerry' AND [GroupOrder] = 19)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 19 WHERE [GroupName] = N'BlackBerry'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'OCS' AND [GroupOrder] = 20)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 20 WHERE [GroupName] = N'OCS'
END
GO

-- Update resource groups ordering info
IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'BlackBerry' AND [GroupOrder] = 20)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 20 WHERE [GroupName] = N'BlackBerry'
END
GO

-- Update resource groups ordering info
IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'OCS' AND [GroupOrder] = 21)
BEGIN
	UPDATE [dbo].[ResourceGroups] SET [GroupOrder] = 21 WHERE [GroupName] = N'OCS'
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[ResourceGroups] WHERE [GroupName] = N'VPSForPC' AND [GroupID] = 40)
BEGIN
	INSERT INTO [dbo].[ResourceGroups] ([GroupID], [GroupName], [GroupOrder], [GroupController]) VALUES (40, N'VPSForPC', 19, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Providers] WHERE [ProviderName] = N'HyperVForPC' AND [ProviderID] = 400)
BEGIN
	INSERT INTO [dbo].[Providers] ([ProviderID], [GroupID], [ProviderName], [DisplayName], [ProviderType], [EditorControl], [DisableAutoDiscovery]) VALUES (400, 40, N'HyperVForPC', N'Microsoft Hyper-V For Private Cloud', N'WebsitePanel.Providers.VirtualizationForPC.HyperVForPC, WebsitePanel.Providers.VirtualizationForPC.HyperVForPC', N'HyperVForPrivateCloud', 1)
END
GO

-- Add VMInfo service item type
IF NOT EXISTS(SELECT * FROM [dbo].[ServiceItemTypes] WHERE [DisplayName] = N'VMInfo' AND [ItemTypeID] = 35)
BEGIN
	INSERT INTO [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (35, 40, N'VMInfo', N'WebsitePanel.Providers.Virtualization.VMInfo, WebsitePanel.Providers.Base', 1, 0, 0, 1, 1, 1, 0, 0)
END
GO

-- Add VirtualSwitch service item type
IF NOT EXISTS(SELECT * FROM [dbo].[ServiceItemTypes] WHERE [DisplayName] = N'VirtualSwitch' AND [ItemTypeID] = 36)
BEGIN
	INSERT INTO [dbo].[ServiceItemTypes] ([ItemTypeID], [GroupID], [DisplayName], [TypeName], [TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable], [Backupable]) VALUES (36, 40, N'VirtualSwitch', N'WebsitePanel.Providers.Virtualization.VirtualSwitch, WebsitePanel.Providers.Base', 2, 0, 0, 1, 1, 1, 0, 0)
END
GO

-- Add hosting plan quotas
IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.ServersNumber' AND [QuotaID] = 345)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (345, 40, 1, N'VPSForPC.ServersNumber', N'Number of VPS', 2, 0, 35)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.ManagingAllowed' AND [QuotaID] = 346)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (346, 40, 2, N'VPSForPC.ManagingAllowed', N'Allow user to create VPS', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.CpuNumber' AND [QuotaID] = 347)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (347, 40, 3, N'VPSForPC.CpuNumber', N'Number of CPU cores', 3, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.BootCdAllowed' AND [QuotaID] = 348)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (348, 40, 7, N'VPSForPC.BootCdAllowed', N'Boot from CD allowed', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.BootCdEnabled' AND [QuotaID] = 349)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (349, 40, 7, N'VPSForPC.BootCdEnabled', N'Boot from CD', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.Ram' AND [QuotaID] = 350)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (350, 40, 4, N'VPSForPC.Ram', N'RAM size, MB', 2, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.Hdd' AND [QuotaID] = 351)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (351, 40, 5, N'VPSForPC.Hdd', N'Hard Drive size, GB', 2, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.DvdEnabled' AND [QuotaID] = 352)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (352, 40, 6, N'VPSForPC.DvdEnabled', N'DVD drive', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.ExternalNetworkEnabled' AND [QuotaID] = 353)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (353, 40, 10, N'VPSForPC.ExternalNetworkEnabled', N'External Network', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.ExternalIPAddressesNumber' AND [QuotaID] = 354)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (354, 40, 11, N'VPSForPC.ExternalIPAddressesNumber', N'Number of External IP addresses', 2, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.PrivateNetworkEnabled' AND [QuotaID] = 355)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (355, 40, 13, N'VPSForPC.PrivateNetworkEnabled', N'Private Network', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.PrivateIPAddressesNumber' AND [QuotaID] = 356)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (356, 40, 14, N'VPSForPC.PrivateIPAddressesNumber', N'Number of Private IP addresses per VPS', 3, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.SnapshotsNumber' AND [QuotaID] = 357)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (357, 40, 9, N'VPSForPC.SnapshotsNumber', N'Number of Snaphots', 3, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.StartShutdownAllowed' AND [QuotaID] = 358)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (358, 40, 15, N'VPSForPC.StartShutdownAllowed', N'Allow user to Start, Turn off and Shutdown VPS', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.PauseResumeAllowed' AND [QuotaID] = 359)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (359, 40, 16, N'VPSForPC.PauseResumeAllowed', N'Allow user to Pause, Resume VPS', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.RebootAllowed' AND [QuotaID] = 360)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (360, 40, 17, N'VPSForPC.RebootAllowed', N'Allow user to Reboot VPS', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.ResetAlowed' AND [QuotaID] = 361)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (361, 40, 18, N'VPSForPC.ResetAlowed', N'Allow user to Reset VPS', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.ReinstallAllowed' AND [QuotaID] = 362)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (362, 40, 19, N'VPSForPC.ReinstallAllowed', N'Allow user to Re-install VPS', 1, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Quotas] WHERE [QuotaName] = N'VPSForPC.Bandwidth' AND [QuotaID] = 363)
BEGIN
	INSERT INTO [dbo].[Quotas] ([QuotaID], [GroupID], [QuotaOrder], [QuotaName], [QuotaDescription], [QuotaTypeID], [ServiceQuota], [ItemTypeID]) VALUES (363, 40, 12, N'VPSForPC.Bandwidth', N'Monthly bandwidth, GB', 2, 0, NULL)
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [Name] = N'AdditionalParams' and Object_ID = Object_ID(N'Users'))
BEGIN
	ALTER TABLE [dbo].[Users] ADD [AdditionalParams] [nvarchar] (max) COLLATE Latin1_General_CI_AS NULL
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVirtualMachinesPagedForPC]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[GetVirtualMachinesPagedForPC]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetVirtualMachinesPagedForPC]
(
	@ActorID int,
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Recursive bit
)
AS


-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
SI.ItemTypeID = 35 -- VPS
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'SI.ItemName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(SI.ItemID) FROM Packages AS P
INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
WHERE ' + @condition + '

DECLARE @Items AS TABLE
(
	ItemID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		SI.ItemID
	FROM Packages AS P
	INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
	INNER JOIN Users AS U ON P.UserID = U.UserID
	LEFT OUTER JOIN (
		SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
		INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
		WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
	) AS EIP ON SI.ItemID = EIP.ItemID
	LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
	WHERE ' + @condition + '
)

INSERT INTO @Items
SELECT ItemID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	SI.ItemID,
	SI.ItemName,
	SI.PackageID,
	P.PackageName,
	P.UserID,
	U.Username,

	EIP.ExternalIP,
	PIP.IPAddress
FROM @Items AS TSI
INNER JOIN ServiceItems AS SI ON TSI.ItemID = SI.ItemID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
'

--print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit',
@PackageID, @StartRow, @MaximumRows, @Recursive

RETURN
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserByUsername]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[GetUserByUsername]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserByUsername]
(
	@ActorID int,
	@Username nvarchar(50)
)
AS

	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		CASE WHEN dbo.CanGetUserPassword(@ActorID, UserID) = 1 THEN U.Password
		ELSE '' END AS Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.Username = @Username
	AND dbo.CanGetUserDetails(@ActorID, UserID) = 1 -- actor user rights

	RETURN
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserById]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[GetUserById]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserById]
(
	@ActorID int,
	@UserID int
)
AS
	-- user can retrieve his own account, his users accounts
	-- and his reseller account (without pasword)
	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		CASE WHEN dbo.CanGetUserPassword(@ActorID, @UserID) = 1 THEN U.Password
		ELSE '' END AS Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.UserID = @UserID
	AND dbo.CanGetUserDetails(@ActorID, @UserID) = 1 -- actor user rights

	RETURN
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateUser]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[UpdateUser]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateUser]
(
	@ActorID int,
	@UserID int,
	@RoleID int,
	@StatusID int,
	@IsDemo bit,
	@IsPeer bit,
	@Comments ntext,
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@Email nvarchar(255),
	@SecondaryEmail nvarchar(255),
	@Address nvarchar(200),
	@City nvarchar(50),
	@State nvarchar(50),
	@Country nvarchar(50),
	@Zip varchar(20),
	@PrimaryPhone varchar(30),
	@SecondaryPhone varchar(30),
	@Fax varchar(30),
	@InstantMessenger nvarchar(200),
	@HtmlMail bit,
	@CompanyName nvarchar(100),
	@EcommerceEnabled BIT,
	@AdditionalParams NVARCHAR(max)
)
AS

	-- check actor rights
	IF dbo.CanUpdateUserDetails(@ActorID, @UserID) = 0
	BEGIN
		RETURN
	END

	UPDATE Users SET 
		RoleID = @RoleID,
		StatusID = @StatusID,
		Changed = GetDate(),
		IsDemo = @IsDemo,
		IsPeer = @IsPeer,
		Comments = @Comments,
		FirstName = @FirstName,
		LastName = @LastName,
		Email = @Email,
		SecondaryEmail = @SecondaryEmail,
		Address = @Address,
		City = @City,
		State = @State,
		Country = @Country,
		Zip = @Zip,
		PrimaryPhone = @PrimaryPhone,
		SecondaryPhone = @SecondaryPhone,
		Fax = @Fax,
		InstantMessenger = @InstantMessenger,
		HtmlMail = @HtmlMail,
		CompanyName = @CompanyName,
		EcommerceEnabled = @EcommerceEnabled,
		[AdditionalParams] = @AdditionalParams
	WHERE UserID = @UserID

	RETURN
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserByIdInternally]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[GetUserByIdInternally]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserByIdInternally]
(
	@UserID int
)
AS
	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		U.Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.UserID = @UserID

	RETURN
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetUserByUsernameInternally]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[GetUserByUsernameInternally]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetUserByUsernameInternally]
(
	@Username nvarchar(50)
)
AS
	SELECT
		U.UserID,
		U.RoleID,
		U.StatusID,
		U.OwnerID,
		U.Created,
		U.Changed,
		U.IsDemo,
		U.Comments,
		U.IsPeer,
		U.Username,
		U.Password,
		U.FirstName,
		U.LastName,
		U.Email,
		U.SecondaryEmail,
		U.Address,
		U.City,
		U.State,
		U.Country,
		U.Zip,
		U.PrimaryPhone,
		U.SecondaryPhone,
		U.Fax,
		U.InstantMessenger,
		U.HtmlMail,
		U.CompanyName,
		U.EcommerceEnabled,
		U.[AdditionalParams]
	FROM Users AS U
	WHERE U.Username = @Username

	RETURN
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CalculateQuotaUsage]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	DROP FUNCTION [dbo].[CalculateQuotaUsage]
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[CalculateQuotaUsage]
(
	@PackageID int,
	@QuotaID int
)
RETURNS int
AS
	BEGIN

		DECLARE @QuotaTypeID int
		SELECT @QuotaTypeID = QuotaTypeID FROM Quotas
		WHERE QuotaID = @QuotaID

		IF @QuotaTypeID <> 2
			RETURN 0

		DECLARE @Result int

		IF @QuotaID = 52 -- diskspace
			SET @Result = dbo.CalculatePackageDiskspace(@PackageID)
		ELSE IF @QuotaID = 51 -- bandwidth
			SET @Result = dbo.CalculatePackageBandwidth(@PackageID)
		ELSE IF @QuotaID = 53 -- domains
			SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
				INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
				WHERE IsSubDomain = 0 AND IsInstantAlias = 0 AND IsDomainPointer = 0 AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 54 -- sub-domains
			SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
				INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
				WHERE IsSubDomain = 1 AND IsInstantAlias = 0 AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 220 -- domain pointers
			SET @Result = (SELECT COUNT(D.DomainID) FROM PackagesTreeCache AS PT
				INNER JOIN Domains AS D ON D.PackageID = PT.PackageID
				WHERE IsDomainPointer = 1 AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 71 -- scheduled tasks
			SET @Result = (SELECT COUNT(S.ScheduleID) FROM PackagesTreeCache AS PT
				INNER JOIN Schedule AS S ON S.PackageID = PT.PackageID
				WHERE PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 305 -- RAM of VPS
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'RamSize' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 306 -- HDD of VPS
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 309 -- External IP addresses of VPS
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 3)
		ELSE IF @QuotaID = 100 -- Dedicated Web IP addresses
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 2)
		ELSE IF @QuotaID = 349 -- RAM of VPSforPc
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'RamSize' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 350 -- HDD of VPSforPc
			SET @Result = (SELECT SUM(CAST(SIP.PropertyValue AS int)) FROM ServiceItemProperties AS SIP
							INNER JOIN ServiceItems AS SI ON SIP.ItemID = SI.ItemID
							INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID
							WHERE SIP.PropertyName = 'HddSize' AND PT.ParentPackageID = @PackageID)
		ELSE IF @QuotaID = 352 -- External IP addresses of VPSforPc
			SET @Result = (SELECT COUNT(PIP.PackageAddressID) FROM PackageIPAddresses AS PIP
							INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
							INNER JOIN PackagesTreeCache AS PT ON PIP.PackageID = PT.PackageID
							WHERE PT.ParentPackageID = @PackageID AND IP.PoolID = 3)
		ELSE IF @QuotaID = 319 -- BB Users
			SET @Result = (SELECT COUNT(ea.AccountID) 
								FROM 
									ExchangeAccounts ea 
								INNER JOIN 
									BlackBerryUsers bu 
								ON 
									ea.AccountID = bu.AccountID
								INNER JOIN 
									ServiceItems  si 
								ON 
									ea.ItemID = si.ItemID
								INNER JOIN 
									PackagesTreeCache pt ON si.PackageID = pt.PackageID
								WHERE 
									pt.ParentPackageID = @PackageID)
		ELSE
			SET @Result = (SELECT COUNT(SI.ItemID) FROM Quotas AS Q
			INNER JOIN ServiceItems AS SI ON SI.ItemTypeID = Q.ItemTypeID
			INNER JOIN PackagesTreeCache AS PT ON SI.PackageID = PT.PackageID AND PT.ParentPackageID = @PackageID
			WHERE Q.QuotaID = @QuotaID)

		RETURN @Result
	END
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetVirtualMachinesPagedForPC]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[GetVirtualMachinesPagedForPC]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetVirtualMachinesPagedForPC]
(
	@ActorID int,
	@PackageID int,
	@FilterColumn nvarchar(50) = '',
	@FilterValue nvarchar(50) = '',
	@SortColumn nvarchar(50),
	@StartRow int,
	@MaximumRows int,
	@Recursive bit
)
AS


-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR('You are not allowed to access this package', 16, 1)
	RETURN
END

-- start
DECLARE @condition nvarchar(700)
SET @condition = '
SI.ItemTypeID = 35 -- VPS
AND ((@Recursive = 0 AND P.PackageID = @PackageID)
OR (@Recursive = 1 AND dbo.CheckPackageParent(@PackageID, P.PackageID) = 1))
'

IF @FilterColumn <> '' AND @FilterColumn IS NOT NULL
AND @FilterValue <> '' AND @FilterValue IS NOT NULL
SET @condition = @condition + ' AND ' + @FilterColumn + ' LIKE ''' + @FilterValue + ''''

IF @SortColumn IS NULL OR @SortColumn = ''
SET @SortColumn = 'SI.ItemName ASC'

DECLARE @sql nvarchar(3500)

set @sql = '
SELECT COUNT(SI.ItemID) FROM Packages AS P
INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
WHERE ' + @condition + '

DECLARE @Items AS TABLE
(
	ItemID int
);

WITH TempItems AS (
	SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortColumn + ') as Row,
		SI.ItemID
	FROM Packages AS P
	INNER JOIN ServiceItems AS SI ON P.PackageID = SI.PackageID
	INNER JOIN Users AS U ON P.UserID = U.UserID
	LEFT OUTER JOIN (
		SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
		INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
		WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
	) AS EIP ON SI.ItemID = EIP.ItemID
	LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
	WHERE ' + @condition + '
)

INSERT INTO @Items
SELECT ItemID FROM TempItems
WHERE TempItems.Row BETWEEN @StartRow + 1 and @StartRow + @MaximumRows

SELECT
	SI.ItemID,
	SI.ItemName,
	SI.PackageID,
	P.PackageName,
	P.UserID,
	U.Username,

	EIP.ExternalIP,
	PIP.IPAddress
FROM @Items AS TSI
INNER JOIN ServiceItems AS SI ON TSI.ItemID = SI.ItemID
INNER JOIN Packages AS P ON SI.PackageID = P.PackageID
INNER JOIN Users AS U ON P.UserID = U.UserID
LEFT OUTER JOIN (
	SELECT PIP.ItemID, IP.ExternalIP FROM PackageIPAddresses AS PIP
	INNER JOIN IPAddresses AS IP ON PIP.AddressID = IP.AddressID
	WHERE PIP.IsPrimary = 1 AND IP.PoolID = 3 -- external IP addresses
) AS EIP ON SI.ItemID = EIP.ItemID
LEFT OUTER JOIN PrivateIPAddresses AS PIP ON PIP.ItemID = SI.ItemID AND PIP.IsPrimary = 1
'

--print @sql

exec sp_executesql @sql, N'@PackageID int, @StartRow int, @MaximumRows int, @Recursive bit',
@PackageID, @StartRow, @MaximumRows, @Recursive

RETURN
GO

IF EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName] = N'WebPolicy' AND [PropertyName] = N'PublishingProfile')
BEGIN
	DELETE FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName] = N'WebPolicy' AND [PropertyName] = N'PublishingProfile'
END
GO

INSERT INTO [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES(1, N'WebPolicy', N'PublishingProfile', N'<?xml version="1.0" encoding="utf-8"?>
<publishData>
<ad:if test="#WebSite.WebDeploySitePublishingEnabled#">
	<publishProfile
		profileName="#WebSite.Name# - Web Deploy"
		publishMethod="MSDeploy"
		publishUrl="#WebSite["WmSvcServiceUrl"]#:#WebSite["WmSvcServicePort"]#"
		msdeploySite="#WebSite.Name#"
		userName="#WebSite.WebDeployPublishingAccount#"
		userPWD="#WebSite.WebDeployPublishingPassword#"
		destinationAppUrl="http://#WebSite.Name#/"
		<ad:if test="#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#">SQLServerDBConnectionString="server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#"</ad:if>
		<ad:if test="#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#">mySQLDBConnectionString="server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#"</ad:if>
		hostingProviderForumLink="http://support.acmehosting.com/"
		controlPanelLink="http://cp.acmehosting.com/"
	/>
</ad:if>
<ad:if test="#IsDefined("FtpAccount")#">
	<publishProfile
		profileName="#WebSite.Name# - FTP"
		publishMethod="FTP"
		publishUrl="ftp://#FtpServiceAddress#"
		ftpPassiveMode="True"
		userName="#FtpAccount.Name#"
		userPWD="#FtpAccount.Password#"
		destinationAppUrl="http://#WebSite.Name#/"
		<ad:if test="#Not(IsNull(MsSqlDatabase)) and Not(IsNull(MsSqlUser))#">SQLServerDBConnectionString="server=#MsSqlServerExternalAddress#;database=#MsSqlDatabase.Name#;uid=#MsSqlUser.Name#;pwd=#MsSqlUser.Password#"</ad:if>
		<ad:if test="#Not(IsNull(MySqlDatabase)) and Not(IsNull(MySqlUser))#">mySQLDBConnectionString="server=#MySqlAddress#;database=#MySqlDatabase.Name#;uid=#MySqlUser.Name#;pwd=#MySqlUser.Password#"</ad:if>
		hostingProviderForumLink="http://support.acmehosting.com/"
		controlPanelLink="http://cp.acmehosting.com/"
    />
</ad:if>
</publishData>

<!--
Control Panel:
Username: #User.Username#
Password: #User.Password#

Technical Contact:
support@acmehosting.com
-->')
GO