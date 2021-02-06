-- execution-order: 1

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create type parameter_unit as enum
(
	'ms',
	'sec',
	'none'
);

comment on type parameter_unit is 'Unit of parameter calculated by marula.';