USE [${install.database}]
GO

-- update database version
DECLARE @build_version nvarchar(10), @build_date datetime
SET @build_version = '1.1.1.1'
SET @build_date = '2010-12-09T00:00:00' -- ISO 8601 Format (YYYY-MM-DDTHH:MM:SS)

