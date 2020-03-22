EXEC sp_addpublication @publication = N'$Database$DB', @description = N'', @retention = 0, @allow_push = N'true',
@repl_freq = N'continuous', @status = N'active', @independent_agent = N'true';