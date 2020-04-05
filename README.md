# MSSQL Transactional Replication Setup

This console configures a transactional replication schema from scratch on a Linux SQL Server Installation. Bidirectional option is also available (data flows from subscriber to publisher). 


**Pre requisites:**

  1. SQL Server 2017 installation over Linux (Docker is recommended)
  2. Database engines must be on the same network, so they can see each other by hostname (if using Docker, just run `docker network create network_name` and then `docker network connect network_name container_name`)
  3. SQL Agent must be enabled (it's disabled by default). (if using Docker, just run `docker exec -it container_name /opt/mssql/bin/mssql-conf set sqlagent.enabled true`)
  4. Replication Data folder (ReplData for this solution) must be made manually for the distributor (if using Docker, just run `docker exec -it container_name mkdir /var/opt/mssql/ReplData`. Container must be restarted after this.)
  

**Steps to configure new schema:**
  
  1. Run the console
  2. Choose action 1 on the menu displayed (*Setup initial replication schema*)
  3. Provide distributor and publisher data
  4. Provide articles, if any
  5. Provide subscribers, if any
  6. Confirm bidirectional replication, if needed
  7. Provide articles, if any

**Setps to add new subscriber to existing publication (replication):**
  
  1. Run the console
  2. Choose action 2 on the menu displayed (*Add subscription to publication*)
  3. Provide distributor and publisher data
  4. Confirm bidirectional replication, if needed
  5. Provide articles, if any

**Important Notes:**
  + Distribution database name is set to `distribution` by default
  + New publications will have a default name of "Database Name" + "DB" by default
  
