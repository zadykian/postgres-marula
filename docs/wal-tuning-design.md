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

```
multiplier = (pg_version >= 11.0) ? 1 : 2
max_wal_size = {wal-traffic-per-second} * checkpoint_timeout * (multiplier + checkpoint_completion_target)
```

The point is that the server needs to keep WAL files starting at the moment of the last completed checkpoint
plus the files accumulated during the current checkpoint. But for before Postgres 11 server also retained 
files from the last but one checkpoint.

Calculation of WAL traffic consists of two processes:

1. Background periodic logging of server's LSN into system storage table. To get current LSN **pg_catalog.pg_current_wal_insert_lsn()** function is used.
   The interval of logging can be set via **LsnTrackingIntervalInSeconds** configuration parameter. The default is **60 seconds**.

2. To take into account volatility of WAL-related operations, WAL traffic is calculated as **moving average** of values retrieved from server during background logging.
   Time window is determined by **MovingAverageWindowInSeconds** app's configuration parameter. Default value is **10800 seconds** (3 hours).

   Single selection item calculated as difference of two adjacent values in LSN log table.
   For example, let's say we have following records in LSN log, which are in moving average window:

   | log_timestamp       | wal_insert_location |
   | :-----------------: | :-----------------: |
   | 2021-04-27 09:00:00 | 32/A0000000         |
   | 2021-04-27 09:01:00 | 32/A0000010         |
   | 2021-04-27 09:02:00 | 32/A0000020         |





### checkpoint_warning

Write a message to the server log if checkpoints caused by the filling of WAL segment files
happen closer together than this amount of time.

The default is 30 seconds.

Is is calculated as:

```
checkpoint_warning = 0.8 * checkpoint_timeout
```



### checkpoint_completion_target

Specifies the target of checkpoint completion, as a fraction of total time between checkpoints.

The default is 0.5.

To evenly distribute the load produced by background WAL writer, is calculated as:

```
checkpoint_completion_target = min(0.9, (checkpoint_timeout - 2 min) / checkpoint_timeout)
```
