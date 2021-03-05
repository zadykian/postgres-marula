-- execution-order: 4

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create table PARAMETERS_TABLE_NAME_TO_REPLACE
(
	id   smallint         primary key generated always as identity,
	name non_empty_string check ( parameter_exists(name) ) unique
);

comment on table  PARAMETERS_TABLE_NAME_TO_REPLACE      is 'Dictionary of parameters calculated by marula.';
comment on column PARAMETERS_TABLE_NAME_TO_REPLACE.id   is 'Table identifier.';
comment on column PARAMETERS_TABLE_NAME_TO_REPLACE.name is 'Parameter name. Matches [pg_catalog.pg_settings.name] value.';