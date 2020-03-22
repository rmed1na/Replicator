DECLARE @database VARCHAR(100);
DECLARE @publication VARCHAR(100);
SET @database = '$database$'
SET @publication = @database + 'DB'

EXEC sp_addsubscription 
@publication = @publication,
@subscriber = '$hostname$', 
@destination_db = @database, 
@subscription_type = N'Push', 
@sync_type = N'none', 
@article = N'all', 
@update_mode = N'read only', 
@subscriber_type = 0