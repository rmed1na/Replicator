DECLARE @machine AS SYSNAME
SET @machine = (SELECT @@SERVERNAME)
EXEC sp_adddistributor @distributor = @machine, @password = '$Password$'