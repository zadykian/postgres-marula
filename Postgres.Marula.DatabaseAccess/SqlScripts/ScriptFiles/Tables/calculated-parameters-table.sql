-- execution-order: 2

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create function parameter_exists(in parameter_name varchar(64))
    returns bool
    cost 1
    stable
    language sql as
$func$
    select exists(
        select 1
        from pg_catalog.pg_settings
        where name = $1)
$func$;

comment on function parameter_exists(parameter_name varchar) is '';

create table calculated_parameters
(
	id serial primary key,
	name varchar(64) not null check ( parameter_exists(name) ),
	unit parameter_unit not null
);

comment on table calculated_parameters is 'Dictionary of parameters calculated by marula.';
comment on column calculated_parameters.id is 'Table identifier.';
comment on column calculated_parameters.name is 'Parameter name. Matches [pg_catalog.pg_settings.name] value.';