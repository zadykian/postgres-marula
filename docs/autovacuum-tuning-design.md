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



## bloat factor analysis

Tuning of parameters mentioned below is based on statistic values of average bloat fraction of all tables and indexes in database server.
Analysis consists of several steps:

1. Background periodic logging of average bloat factor into system storage table.
   Value is retrieved based on data from system view **pg_catalog.pg_stat_all_tables** and calculated for each table as **[n_live_tup] / [n_dead_tup]**.
   The interval of logging can be set via **Autovacuum.BloatTrackingIntervalInSeconds** configuration parameter. The default is **600 seconds** (10 minutes).
2. Selection of bloat values.
   To take into account impermanence of modifying operations intensity (updates  and deletions), only resent bloat factor values are included into selection.
   Left time bound of selection can be configured via **Autovacuum.MovingAverageWindowInSeconds** application's parameter. The default is 10800 seconds (3 hours).
3. Linear regression.
   Selection retrieved at previous step can be approximated to linear function to simplify further calculations. This process is based on Least Squares Method (LSM).
4. Bloat fraction trend analysis.
   As a measure of bloat trend used a **derivative of linear function** obtained at previous step, which is equal to tangent of angle between function's line and abscissa.
