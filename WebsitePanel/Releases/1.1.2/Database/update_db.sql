USE [${install.database}]
GO

-- update database version
DECLARE @build_version nvarchar(10), @build_date datetime
SET @build_version = N'${release.version}'
SET @build_date = '${release.date}T00:00:00' -- ISO 8601 Format (YYYY-MM-DDTHH:MM:SS)

IF NOT EXISTS (SELECT * FROM [dbo].[Versions] WHERE [DatabaseVersion] = @build_version)
BEGIN
	INSERT INTO [Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
END
GO

IF EXISTS (SELECT * FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName] = N'WebPolicy' AND [PropertyName] = N'PublishingProfile')
BEGIN
	DELETE FROM [dbo].[UserSettings] WHERE [UserID] = 1 AND [SettingsName] = N'WebPolicy' AND [PropertyName] = N'PublishingProfile'
END
GO

INSERT INTO [dbo].[UserSettings] ([UserID], [SettingsName], [PropertyName], [PropertyValue]) VALUES(1, N'WebPolicy', N'PublishingProfile', N'
<?xml version="1.0" encoding="utf-8"?>
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
