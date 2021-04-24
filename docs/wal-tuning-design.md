# wal tuning design

This document provides detailed explanation of Marula's internal processes aimed to provide optimized WAL-related
configuration of Postgres server.

## parameters being tuned

There is a list of database server parameters which are taken into account during tool development:

### checkpoint_timeout

<p>
Maximum time between automatic WAL checkpoints.
Default value is 5 minutes which is too small for most modern server configurations.
</p>
<p>
Marula tunes it to 30 minutes.
</p>

### max_wal_size

<p>
Maximum size to let the WAL grow during automatic checkpoints.
This is a soft limit - WAL size can exceed max_wal_size under special circumstances.
</p>
<p>
This value is being calculated based on current workload.
More specifically, on how much WAL is produced by server during given period of time.
</p>
