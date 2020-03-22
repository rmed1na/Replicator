DECLARE @Database VARCHAR(50)
SET @Database = '$Database$'
IF EXISTS(SELECT * FROM sys.databases WHERE name = @Database)
BEGIN
	EXEC sp_replicationdboption @dbname = @Database, @optname = N'publish', @value = N'true';
END