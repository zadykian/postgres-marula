-- execution-order: 7

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create table WAL_LSN_HISTORY_TABLE_NAME_TO_REPLACE
(
	log_timestamp       timestamp         not null default current_timestamp,
	wal_insert_location pg_catalog.pg_lsn not null
);

comment on table  WAL_LSN_HISTORY_TABLE_NAME_TO_REPLACE                     is 'Write-Ahead Log insert locations.';
comment on column WAL_LSN_HISTORY_TABLE_NAME_TO_REPLACE.log_timestamp       is 'LSN log timestamp.';
comment on column WAL_LSN_HISTORY_TABLE_NAME_TO_REPLACE.wal_insert_location is 'WAL insert location.';