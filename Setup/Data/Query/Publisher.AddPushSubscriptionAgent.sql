DECLARE @database VARCHAR(100);
DECLARE @publication VARCHAR(100);
SET @database = '$database$'
SET @publication = @database + 'DB'


EXEC sp_addpushsubscription_agent 
@publication = @publication, 
@subscriber = '$subscriberHostname$', 
@subscriber_db = @database, 
@subscriber_security_mode = 0, 
@subscriber_login = '$user$', 
@subscriber_password = '$password$', 
@frequency_type = 64, 
@frequency_interval = 0, 
@frequency_relative_interval = 0, 
@frequency_recurrence_factor = 0, 
@frequency_subday = 0, 
@frequency_subday_interval = 0, 
@active_start_time_of_day = 0, 
@active_end_time_of_day = 0, 
@active_start_date = 0, 
@active_end_date = 19950101