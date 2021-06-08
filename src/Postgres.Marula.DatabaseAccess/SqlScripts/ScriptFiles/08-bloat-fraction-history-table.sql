-- execution-order: 8

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create table BLOAT_FRACTION_HISTORY_TABLE_NAME_TO_REPLACE
(
	log_timestamp          timestamp    not null default current_timestamp,
	average_bloat_fraction numeric(5,4) not null
);

comment on table  BLOAT_FRACTION_HISTORY_TABLE_NAME_TO_REPLACE
	is 'Average bloat fraction history.';
comment on column BLOAT_FRACTION_HISTORY_TABLE_NAME_TO_REPLACE.log_timestamp
	is 'Bloat fraction log timestamp.';
comment on column BLOAT_FRACTION_HISTORY_TABLE_NAME_TO_REPLACE.average_bloat_fraction
	is 'Value of average bloat fraction.';