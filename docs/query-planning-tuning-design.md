# query planning tuning design

This document describes calculations of parameters related to query planning processes in PostgreSQL

## parameters being tuned

There is a list of database server parameters which are taken into account during tool development



### effective_cache_size

Sets the planner's assumption about the effective size of the disk cache that is available to a single query.
Calculation depends on total RAM size:

```
effective_cache_size = 0.75 * {total_ram_size}
```