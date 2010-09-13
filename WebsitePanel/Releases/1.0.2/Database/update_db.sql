USE [${install.database}]
GO

-- update database version
declare @build_version nvarchar(10), @build_date datetime
set @build_version = '1.0.2.0'
set @build_date = '09/03/2010'
IF NOT EXISTS (SELECT * FROM dbo.Versions WHERE DatabaseVersion = @build_version)
INSERT INTO [Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
GO

-- add "ExchangeHostedEdition" resource group
IF NOT EXISTS (SELECT * FROM dbo.ResourceGroups WHERE GroupID = 33)
BEGIN
	INSERT INTO dbo.ResourceGroups (GroupID, GroupName, GroupOrder, GroupController) VALUES
	(33, 'ExchangeHostedEdition', 4, 'WebsitePanel.EnterpriseServer.ExchangeHostedEditionController')
END

-- add "ExchangeOrganization" item type
IF NOT EXISTS (SELECT * FROM dbo.[ServiceItemTypes] WHERE [ItemTypeID] = 40)
BEGIN
	INSERT INTO [dbo].[ServiceItemTypes]
           ([ItemTypeID], [GroupID]
           ,[DisplayName]
           ,[TypeName]
           ,[TypeOrder], [CalculateDiskspace], [CalculateBandwidth], [Suspendable], [Disposable], [Searchable], [Importable] ,[Backupable])
     VALUES
           (40, 33
           ,'ExchangeOrganization'
           ,'WebsitePanel.Providers.ExchangeHostedEdition.ExchangeOrganization, WebsitePanel.Providers.Base'
           ,1, 0, 0, 1, 1, 1, 0, 0)
END

-- add "ExchangeHostedEdition" DNS records
IF NOT EXISTS (SELECT * FROM dbo.ResourceGroupDnsRecords WHERE GroupID = 33)
BEGIN
	INSERT INTO dbo.ResourceGroupDnsRecords (RecordOrder, GroupID, RecordType, RecordName, RecordData, MXPriority) VALUES
	(1, 33, 'A', 'smtp', '[IP]', 0)
	INSERT INTO dbo.ResourceGroupDnsRecords (RecordOrder, GroupID, RecordType, RecordName, RecordData, MXPriority) VALUES
	(2, 33, 'MX', '', 'smtp.[DOMAIN_NAME]', 10)
	INSERT INTO dbo.ResourceGroupDnsRecords (RecordOrder, GroupID, RecordType, RecordName, RecordData, MXPriority) VALUES
	(3, 33, 'CNAME', 'autodiscover', '', 0)
	INSERT INTO dbo.ResourceGroupDnsRecords (RecordOrder, GroupID, RecordType, RecordName, RecordData, MXPriority) VALUES
	(4, 33, 'CNAME', 'owa', '', 0)
	INSERT INTO dbo.ResourceGroupDnsRecords (RecordOrder, GroupID, RecordType, RecordName, RecordData, MXPriority) VALUES
	(5, 33, 'CNAME', 'ecp', '', 0)
END

-- add "ExchangeHostedEdition" quotas
IF NOT EXISTS (SELECT * FROM dbo.Quotas WHERE GroupID = 33)
BEGIN
	INSERT INTO dbo.Quotas (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota, ItemTypeID) VALUES
	(340, 33, 1, 'ExchangeHostedEdition.Domains', 'Domains', 3, 0, NULL)
	INSERT INTO dbo.Quotas (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota, ItemTypeID) VALUES
	(341, 33, 2, 'ExchangeHostedEdition.Mailboxes', 'Mailboxes', 3, 0, NULL)
	INSERT INTO dbo.Quotas (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota, ItemTypeID) VALUES
	(342, 33, 3, 'ExchangeHostedEdition.Contacts', 'Contacts', 3, 0, NULL)
	INSERT INTO dbo.Quotas (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota, ItemTypeID) VALUES
	(343, 33, 4, 'ExchangeHostedEdition.DistributionLists', 'Distribution Lists', 3, 0, NULL)
END

-- add "Exchange2010SP1" provider
IF NOT EXISTS (SELECT * FROM dbo.Providers WHERE ProviderID = 207)
BEGIN
	INSERT INTO dbo.Providers (ProviderID, GroupID, ProviderName, ProviderType, DisplayName, EditorControl, DisableAutoDiscovery) VALUES
	(207, 33, 'Exchange2010SP1', 'WebsitePanel.Providers.ExchangeHostedEdition.Exchange2010SP1, WebsitePanel.Providers.ExchangeHostedEdition', 'Exchange Server 2010 SP1', 'Exchange2010SP1', 1)
END

-- add "Exchange2010SP1" provider default settings
IF NOT EXISTS (SELECT * FROM dbo.ServiceDefaultProperties WHERE ProviderID = 207)
BEGIN
	INSERT INTO dbo.ServiceDefaultProperties (ProviderID, PropertyName, PropertyValue) VALUES
	(207, 'location', 'en-us')
	INSERT INTO dbo.ServiceDefaultProperties (ProviderID, PropertyName, PropertyValue) VALUES
	(207, 'ecpURL', 'http://ecp.[DOMAIN_NAME]')
END

-- add new "mail" quota
IF NOT EXISTS (SELECT * FROM dbo.Quotas WHERE QuotaID = 102)
BEGIN
	INSERT INTO dbo.Quotas (QuotaID, GroupID, QuotaOrder, QuotaName, QuotaDescription, QuotaTypeID, ServiceQuota, ItemTypeID)
	VALUES (102, 4, 8, 'Mail.DisableSizeEdit', 'Disable Mailbox Size Edit', 1, 0, NULL)
END
GO

-- add "SmarterMail 7" provider
IF NOT EXISTS (SELECT * FROM dbo.Providers WHERE ProviderID = 64)
BEGIN
	INSERT INTO dbo.Providers( ProviderID, GroupID, ProviderName, DisplayName, ProviderType, EditorControl, DisableAutoDiscovery )
	VALUES ( 64, 4, 'SmarterMail', 'SmarterMail 7.x', 'WebsitePanel.Providers.Mail.SmarterMail7, WebsitePanel.Providers.Mail.SmarterMail7', 'SmarterMail60', NULL)
	
	INSERT INTO dbo.ServiceDefaultProperties (ProviderID, PropertyName, PropertyValue) VALUES (64, 'ServiceUrl', 'http://localhost:9998/services/')
	INSERT INTO dbo.ServiceDefaultProperties (ProviderID, PropertyName, PropertyValue) VALUES (64, 'ServerIPAddress', '127.0.0.1;127.0.0.1')
	INSERT INTO dbo.ServiceDefaultProperties (ProviderID, PropertyName, PropertyValue) VALUES (64, 'DomainsPath', '%SYSTEMDRIVE%\SmarterMail')
	INSERT INTO dbo.ServiceDefaultProperties (ProviderID, PropertyName, PropertyValue) VALUES (64, 'AdminUsername', 'admin')
	INSERT INTO dbo.ServiceDefaultProperties (ProviderID, PropertyName, PropertyValue) VALUES (64, 'AdminPassword', '')
END
GO

-- add "SharePoint 2010" provider
IF NOT EXISTS (SELECT * FROM dbo.Providers WHERE ProviderID = 208)
BEGIN
	INSERT INTO dbo.Providers( ProviderID, GroupID, ProviderName, DisplayName, ProviderType, EditorControl, DisableAutoDiscovery )
	VALUES ( 208, 20, 'HostedSharePoint2010', 'Hosted SharePoint Foundation 2010', 'WebsitePanel.Providers.HostedSolution.HostedSharePointServer2010, WebsitePanel.Providers.HostedSolution', 'HostedSharePoint30', NULL)
END
GO
