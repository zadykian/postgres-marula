# autovacuum tuning design

This document provides detailed explanation of Marula's internal processes aimed to provide optimized configuration of Postgres autovacuum daemon.

## parameters being tuned

There is a list of database server parameters which affect autovacuum process and which are taken into account during tool development:

### autovacuum (boolean)

Controls whether the server should run the autovacuum daemon.
Obviously, in context of automatic configuration, this parameter should be set to 'true'.

### track_counts (boolean)

Enables collection of statistics on database activity.
Autovacuum launcher process uses statistics to create list of databases which had some activity. So collection of statistics is required for autovacuum.