# lock management tuning design


This document contains explanation of decisions related to lock management parameter calculations

## parameters being tuned

### max_locks_per_transaction

The shared lock table tracks locks on
max_locks_per_transaction * (max_connections + max_prepared_transactions) objects.
This parameter controls the average number of object locks allocated for each transaction.
It has **postmaster** context, so it can't be adjusted without server restart.

Calculation is based on maximum count of child partitions among all partitioned and inherited tables.
Data is retrieved from **pg_catalog.pg_inherits** system catalog.

```
max_locks_per_transaction = 1.1 *
```