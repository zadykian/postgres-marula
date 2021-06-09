# memory tuning design

This document provides detailed explanation of Marula's internal processes aimed to provide optimized memory consumption configuration.

## parameters being tuned

There is a list of database server parameters which are taken into account during tool development



### shared_buffers

Sets the amount of memory the database server uses for shared memory buffers.
This parameter has **postmaster** context, so it can't be adjusted without server restart.
Value is calculated as:

```
shared_buffers = 0.25 * {total_ram_size}
```

### work_mem

Sets the base maximum amount of memory to be used by a query operation 
(such as a sort or hash table) before writing to temporary disk files.
Calculation depends on total size of server RAM and value of **max_connections** parameter:

```
work_mem = 0.25 * {total_ram_size} / max_connections
```

### maintenance_work_mem

Specifies the maximum amount of memory to be used by maintenance operations,
such as VACUUM, CREATE INDEX, and ALTER TABLE ADD FOREIGN KEY.
Value is calculated as:

```
maintenance_work_mem = 0.05 * {total_ram_size}
```

### autovacuum_work_mem

Specifies the maximum amount of memory to be used by each autovacuum worker process.
Value depends on total RAM size and current value of **autovacuum_max_workers** parameter:

```
autovacuum_work_mem = 0.1 * {total_ram_size} / {autovacuum_max_workers}
```