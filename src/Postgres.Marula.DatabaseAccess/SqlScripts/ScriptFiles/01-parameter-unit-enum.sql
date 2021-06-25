-- execution-order: 1

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create type UNIT_ENUM_NAME_TO_REPLACE as enum
(
	'bytes',
	'kilobytes',
	'megabytes',
	'gigabytes',
	'terabytes',
	'milliseconds',
	'seconds',
	'enum',
	'none'
);

comment on type UNIT_ENUM_NAME_TO_REPLACE is 'Unit of parameter calculated by marula.';