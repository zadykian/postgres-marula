# autovacuum tuning design

This document provides detailed explanation of Marula's internal processes aimed to provide optimized configuration of Postgres autovacuum daemon.

## parameters being tuned

There is a list of database server parameters which affect autovacuum process and which are taken into account during tool development:

### autovacuum

Controls whether the server should run the autovacuum daemon.
Obviously, in context of automatic configuration, this parameter should be set to **true**.

### track_counts

Enables collection of statistics on database activity.
Autovacuum launcher process uses statistics to create list of databases which had some activity. So collection of statistics is required for autovacuum, and this parameter is always set to **true**.



## table size expected value (EV)

To configure parameters related to deciding should table be vacuumed or not,
Marula performs calculations of table size expected value. Calculation is based on data in **pg_catalog.pg_stat_user_tables** system view.
Empty tables are not taken into account.



### autovacuum_vacuum_scale_factor

Specifies a fraction of the table size to add to autovacuum_vacuum_threshold when deciding whether to trigger a vacuum. The default is **0.2** (20% of table size).

Factor calculated based on **table size EV**:

```
autovacuum_vacuum_scale_factor = min(0.2, 10^4 / {table_size_expected_value})
```

### autovacuum_vacuum_threshold

Specifies the minimum number of updated or deleted tuples needed to trigger a vacuum in any one table. The default is **50** tuples.

Value calculated based on **table size EV** and can be represented by formula:
```
autovacuum_vacuum_threshold = 0.01 * {table_size_expected_value}
```



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
   **[A] Derivative of linear function** obtained at previous step is used as a **measure of bloat trend**.
   It's equal to tangent of angle between function's line and abscissa.
   So, if the derivative is positive, it means that percent of dead tuples in database is increasing, otherwise - it's decreasing.
   Besides derivative, **[B] constant part** of approximated linear function is considered as a measure of **dead tuples proportion** in all selection range.
   Accordingly, even if derivative is equal or close to zero (it means that fraction of dead tuples is almost permanent),
   but constant part is high, autovacuum daemon should be tuned more aggressively.

As a result, we get two values to calculate autovacuum-related parameters:

* [A] trend coefficient, which can be equal to any rational number;
* [B] bloat constant, which theoretically takes values in range [0..1].



### autovacuum_vacuum_cost_delay

Specifies the cost delay value that will be used in automatic vacuum operations.
For PG12+ the default value is **2 milliseconds**, but for PG11 and earlier versions it's set to 20 milliseconds by default.
It's always being tuned to **2 milliseconds**.

### autovacuum_vacuum_cost_limit

Specifies the cost limit value that will be used in automatic vacuum operations.
If there are no table bloat stats in system storage - for example, when application analyzes database server for the first time - the value is calculated as:

```
autovacuum_vacuum_cost_limit = 500 * autovacuum_max_workers
```

Otherwise, if statistics are collected already, it is calculated based on its' current value and values mentioned above - **trend coefficient** and **bloat constant**.

todo

### autovacuum_naptime

Specifies the minimum delay between autovacuum runs on any given database.
The default is one minute (1min).
Marula tunes it to **30 seconds**.

### autovacuum_max_workers

Specifies the maximum number of autovacuum processes (other than the autovacuum launcher) that may be running at any one time. The default is **three**.

To tune this value, information about server hardware is required. HW into is retrieved from machine by agent http service.
After HW info is received, value is calculated as:

```
autovacuum_max_workers = 0.5 * {number_of_cpu_cores}
```