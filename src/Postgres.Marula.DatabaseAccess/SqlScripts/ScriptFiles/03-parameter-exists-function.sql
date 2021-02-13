-- execution-order: 3

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create function parameter_exists(in parameter_name non_empty_string)
	returns bool
	cost 1
	stable
	language sql as
$func$
	select exists(
		select null
		from pg_catalog.pg_settings
		where name = $1)
$func$;

comment on
	function parameter_exists(parameter_name non_empty_string)
	is 'Function to determine if the parameter with given name exists in database settings.';