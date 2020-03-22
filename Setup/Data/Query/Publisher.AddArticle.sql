DECLARE @article VARCHAR(100)
SET @article = '$article$'

EXEC sp_addarticle 
@publication = N'$database$DB', 
@article = @article, 
@source_owner = N'dbo', 
@source_object = @article, 
@type = N'logbased', 
@description = NULL, 
@creation_script = NULL, 
@pre_creation_cmd = N'drop', 
@schema_option = 0x000000000803509D, 
@identityrangemanagementoption = N'manual', 
@destination_table = @article, 
@destination_owner = N'dbo', 
@vertical_partition = N'false';