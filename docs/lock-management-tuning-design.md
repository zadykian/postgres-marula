# lock management tuning design


This document contains explanation of decisions related to lock management parameter calculations

## parameters being tuned

There is a list of lock management parameters with calculation details

### max_locks_per_transaction

The shared lock table tracks locks on **max_locks_per_transaction * (max_connections + max_prepared_transactions)** objects.
This parameter controls the average number of object locks allocated for each transaction.
It has **postmaster** context, so it can't be adjusted without server restart.

Calculation is based on maximum count of child partitions among all partitioned and inherited tables.
Data is being retrieved from **pg_catalog.pg_inherits** system catalog; sub-partitions are also taken into account.

If current value is more then calculated one, it stays untouched, because it could be adjusted manually
on the assumption of some other aspects which didn't relate to tables partitioning.

```
max_locks_per_transaction = max(1.2 * {max_partitions_count}, max_locks_per_transaction)
```