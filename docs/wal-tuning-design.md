# wal tuning design

This document provides detailed explanation of Marula's internal processes aimed to provide optimized WAL-related
configuration of Postgres server.

## parameters being tuned

There is a list of database server parameters which are taken into account during tool development



### checkpoint_timeout

Maximum time between automatic WAL checkpoints.
Default value is 5 minutes which is too small for most modern server configurations.

Marula tunes it to 30 minutes.



### max_wal_size

Maximum size to let the WAL grow during automatic checkpoints.
This is a soft limit - WAL size can exceed max_wal_size under special circumstances.

This value is being calculated based on current workload.
More specifically, on how much WAL is produced by server during given period of time.

Actual formula looks like this:

​	**multiplier = (pg_version >= 11.0) ? 2 : 1**

​	**max_wal_size = {wal-traffic-per-second} * checkpoint_timeout * (multiplier + checkpoint_completion_target)**

The point is that the server needs to keep WAL files starting at the moment of the last completed checkpoint
plus the files accumulated during the current checkpoint. But for before Postgres 11 server also retained 
files from the last but one checkpoint.

WAL traffic is calculated as difference between values retrieved with given interval
by **pg_current_wal_insert_lsn()** server function.
The interval can be set via **LsnTrackingIntervalInSeconds** configuration parameter.



### checkpoint_warning

Write a message to the server log if checkpoints caused by the filling of WAL segment files
happen closer together than this amount of time.

The default is 30 seconds.

Is is calculated as **0.5 * checkpoint_timeout**



### checkpoint_completion_target

Specifies the target of checkpoint completion, as a fraction of total time between checkpoints.

The default is 0.5.

It is calculated as **min(0.9, (checkpoint_timeout - 2 min) / checkpoint_timeout)** to evenly
distribute the load produced by background WAL writer.