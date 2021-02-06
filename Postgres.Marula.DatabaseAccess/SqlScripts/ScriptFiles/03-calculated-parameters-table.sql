-- execution-order: 3

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create table calculated_parameters
(
	id serial primary key,
	name varchar(64) not null check ( parameter_exists(name) ),
	unit parameter_unit not null
);

comment on table calculated_parameters       is 'Dictionary of parameters calculated by marula.';
comment on column calculated_parameters.id   is 'Table identifier.';
comment on column calculated_parameters.name is 'Parameter name. Matches [pg_catalog.pg_settings.name] value.';
comment on column calculated_parameters.unit is 'Parameter unit.';