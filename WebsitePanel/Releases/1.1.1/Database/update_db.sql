USE [${install.database}]
GO

-- update database version
DECLARE @build_version nvarchar(10), @build_date datetime
SET @build_version = '1.1.1.1'
SET @build_date = '2010-12-09T00:00:00' -- ISO 8601 Format (YYYY-MM-DDTHH:MM:SS)

IF NOT EXISTS (SELECT * FROM [dbo].[Versions] WHERE [DatabaseVersion] = @build_version)
BEGIN
	INSERT INTO [Versions] ([DatabaseVersion], [BuildDate]) VALUES (@build_version, @build_date)
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExchangeAccountEmailAddressExists]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[ExchangeAccountEmailAddressExists]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ExchangeAccountEmailAddressExists]
(
	@EmailAddress nvarchar(300),
	@Exists bit OUTPUT
)
AS

	SET @Exists = 0
	IF EXISTS(SELECT * FROM [dbo].[ExchangeAccountEmailAddresses] WHERE [EmailAddress] = @EmailAddress)
		BEGIN
			SET @Exists = 1
		END
	ELSE IF EXISTS(SELECT * FROM [dbo].[ExchangeAccounts] WHERE [PrimaryEmailAddress] = @EmailAddress AND [AccountType] <> 2)
		BEGIN
			SET @Exists = 1
		END

	RETURN
GO


IF NOT EXISTS (SELECT * FROM [dbo].[Quotas] WHERE [QuotaID] = 332)
BEGIN
INSERT INTO [dbo].[Quotas] ([QuotaID],[GroupID],[QuotaOrder],[QuotaName],[QuotaDescription],[QuotaTypeID],[ServiceQuota],[ItemTypeID])
     VALUES (332,2,21,'Web.SSL','SSL',1,0,NULL)
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SSLCertificates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SSLCertificates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[FriendlyName] [nvarchar](255) NULL,
	[Hostname] [nvarchar](255) NULL,
	[DistinguishedName] [nvarchar](500) NULL,
	[CSR] [ntext] NULL,
	[CSRLength] [int] NULL,
	[Certificate] [ntext] NULL,
	[Hash] [ntext] NULL,
	[Installed] [bit] NULL,
	[IsRenewal] [bit] NULL,
	[ValidFrom] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[SerialNumber] [nvarchar](250) NULL,
	[Pfx] [ntext] NULL,
	[PreviousId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  StoredProcedure [dbo].[GetSSLCertificateByID]    Script Date: 09/20/2010 23:38:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSSLCertificateByID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[GetSSLCertificateByID]
(
	@ActorID int,
	@ID int
)
AS

SELECT
	[ID], [UserID], [SiteID], [Hostname], [FriendlyName], [CSR], [Certificate], [Hash], [Installed], [IsRenewal], [PreviousId]
FROM
	[dbo].[SSLCertificates]
INNER JOIN
	[dbo].[ServiceItems] AS [SI] ON [SSLCertificates].[SiteID] = [SI].[ItemID]
WHERE
	[ID] = @ID AND [dbo].CheckActorPackageRights(@ActorID, [SI].[PackageID]) = 1

RETURN

' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetSiteCert]    Script Date: 09/20/2010 23:38:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetSiteCert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[GetSiteCert]
(
	@ActorID int,
	@ID int
)
AS

SELECT
	[UserID], [SiteID], [Hostname], [CSR], [Certificate], [Hash], [Installed], [IsRenewal]
FROM
	[dbo].[SSLCertificates]
INNER JOIN
	[dbo].[ServiceItems] AS [SI] ON [SSLCertificates].[SiteID] = [SI].[ItemID]
WHERE
	[SiteID] = @ID AND [Installed] = 1 AND [dbo].CheckActorPackageRights(@ActorID, [SI].[PackageID]) = 1
RETURN


' 
END
GO
/****** Object:  StoredProcedure [dbo].[GetCertificatesForSite]    Script Date: 09/20/2010 23:38:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCertificatesForSite]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetCertificatesForSite]
(
	@ActorID int,
	@PackageID int,
	@websiteid int
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR(''You are not allowed to access this package'', 16, 1)
	RETURN
END

SELECT
	[ID], [UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName], 
	[CSR], [CSRLength], [ValidFrom], [ExpiryDate], [Installed], [IsRenewal], 
	[PreviousId], [SerialNumber]
FROM
	[dbo].[SSLCertificates]
WHERE
	[SiteID] = @websiteid
RETURN

' 
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteCertificate]    Script Date: 09/20/2010 23:38:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteCertificate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[DeleteCertificate] 
(
	@ActorID int,
	@PackageID int,
	@id int
	
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR(''You are not allowed to access this package'', 16, 1)
	RETURN
END

-- insert record
DELETE FROM
	[dbo].[SSLCertificates]
WHERE
	[ID] = @id
           
RETURN

' 
END
GO
/****** Object:  StoredProcedure [dbo].[CompleteSSLRequest]    Script Date: 09/20/2010 23:38:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CompleteSSLRequest]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[CompleteSSLRequest] 
(
	@ActorID int,
	@PackageID int,
	@ID int,	
	@Certificate ntext,
	@SerialNumber nvarchar(250),
	@Hash ntext,
	@DistinguishedName nvarchar(500),
	@ValidFrom datetime,
	@ExpiryDate datetime
	
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR(''You are not allowed to access this package'', 16, 1)
	RETURN
END

-- insert record
UPDATE
	[dbo].[SSLCertificates]
SET	
	[Certificate] = @Certificate,
	[Installed] = 1,
	[SerialNumber] = @SerialNumber,
	[DistinguishedName] = @DistinguishedName,
	[Hash] = @Hash,
	[ValidFrom] = @ValidFrom,
	[ExpiryDate] = @ExpiryDate 
WHERE
	[ID] = @ID;           

' 
END
GO
/****** Object:  StoredProcedure [dbo].[AddSSLRequest]    Script Date: 09/20/2010 23:38:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddSSLRequest]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[AddSSLRequest] 
(
	@SSLID int OUTPUT,
	@ActorID int,
	@PackageID int,
	@UserID int,
	@WebSiteID int,
	@FriendlyName nvarchar(255),
	@HostName nvarchar(255),
	@CSR ntext,
	@CSRLength int,
	@DistinguishedName nvarchar(500),
	@IsRenewal bit = 0,
	@PreviousId int = NULL
	
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR(''You are not allowed to access this package'', 16, 1)
	RETURN
END

-- insert record
INSERT INTO [dbo].[SSLCertificates]
	([UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName], [CSR], [CSRLength], [IsRenewal], [PreviousId])
VALUES
	(@UserID, @WebSiteID, @FriendlyName, @HostName, @DistinguishedName, @CSR, @CSRLength, @IsRenewal, @PreviousId)

SET @SSLID = SCOPE_IDENTITY()
RETURN

' 
END
GO
/****** Object:  StoredProcedure [dbo].[AddPFX]    Script Date: 09/20/2010 23:38:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AddPFX]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[AddPFX] 
(
	@ActorID int,
	@PackageID int,
	@UserID int,
	@WebSiteID int,
	@FriendlyName nvarchar(255),
	@HostName nvarchar(255),	
	@CSRLength int,
	@DistinguishedName nvarchar(500),
	@SerialNumber nvarchar(250),
	@ValidFrom datetime,
	@ExpiryDate datetime
	
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR(''You are not allowed to access this package'', 16, 1)
	RETURN
END

-- insert record
INSERT INTO [dbo].[SSLCertificates]
	([UserID], [SiteID], [FriendlyName], [Hostname], [DistinguishedName], [CSRLength], [SerialNumber], [ValidFrom], [ExpiryDate], [Installed])
VALUES
	(@UserID, @WebSiteID, @FriendlyName, @HostName, @DistinguishedName, @CSRLength, @SerialNumber, @ValidFrom, @ExpiryDate, 1)

RETURN

' 
END
GO
/****** Object:  StoredProcedure [dbo].[CheckSSLExistsForWebsite]    Script Date: 09/20/2010 23:38:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckSSLExistsForWebsite]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[CheckSSLExistsForWebsite]
(
	@siteID int,	
	@SerialNumber nvarchar(250),
	@Result bit OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
*/

SET @Result = 0 -- OK

-- check if a SSL Certificate is installed for domain
IF EXISTS(SELECT [ID] FROM [dbo].[SSLCertificates] WHERE [SiteID] = @siteID 
--AND SerialNumber=@SerialNumber
)
BEGIN
	SET @Result = 1
	RETURN
END

RETURN

SET ANSI_NULLS ON
' 
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckSSL]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[CheckSSL]
(
	@siteID int,
	@Renewal bit = 0,	
	@Result int OUTPUT
)
AS

/*
@Result values:
	0 - OK
	-1 - already exists
*/

SET @Result = 0 -- OK

-- check if a SSL Certificate is installed for domain
IF EXISTS(SELECT [ID] FROM [dbo].[SSLCertificates] WHERE [SiteID] = @siteID)
BEGIN
	SET @Result = -1
	RETURN
END

--To Do add renewal stuff

RETURN

SET ANSI_NULLS ON
' 
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPackageUnassignedIPAddresses]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[GetPackageUnassignedIPAddresses]
(
	@ActorID int,
	@PackageID int,
	@PoolID int = 0
)
AS
BEGIN
	SELECT
		[PIP].[PackageAddressID],
		[IP].[AddressID],
		[IP].[ExternalIP],
		[IP].[InternalIP],
		[IP].[ServerID],
		[IP].[PoolID],
		[PIP].[IsPrimary],
		[IP].[SubnetMask],
		[IP].[DefaultGateway]
	FROM 
		[dbo].[PackageIPAddresses] AS [PIP]
	INNER JOIN
		[dbo].[IPAddresses] AS [IP] ON [PIP].[AddressID] = [IP].[AddressID]
	WHERE
		[PIP].[ItemID] IS NULL
		AND [PIP].[PackageID] = @PackageID
		AND (@PoolID = 0 OR @PoolID <> 0 AND [IP].[PoolID] = @PoolID)
		AND [dbo].CheckActorPackageRights(@ActorID, [PIP].[PackageID]) = 1
	ORDER BY
		[IP].[DefaultGateway], [IP].[ExternalIP]
END
' 
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetPendingSSLForWebsite]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

CREATE PROCEDURE [dbo].[GetPendingSSLForWebsite]
(
	@ActorID int,
	@PackageID int,
	@websiteid int,
	@Recursive bit = 1
)
AS

-- check rights
IF dbo.CheckActorPackageRights(@ActorID, @PackageID) = 0
BEGIN
	RAISERROR(''You are not allowed to access this package'', 16, 1)
	RETURN
END

SELECT
	[ID], [UserID], [SiteID], [Hostname], [CSR], [Certificate], [Hash], [Installed]
FROM
	[dbo].[SSLCertificates]
WHERE
	@websiteid = 2 AND [Installed] = 0 AND [IsRenewal] = 0

RETURN

' 
END
GO