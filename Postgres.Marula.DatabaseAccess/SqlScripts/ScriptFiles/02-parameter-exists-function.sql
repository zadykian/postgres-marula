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

comment on
	function parameter_exists(parameter_name varchar)
	is 'Function to determine if the parameter with given name exists in database settings.';