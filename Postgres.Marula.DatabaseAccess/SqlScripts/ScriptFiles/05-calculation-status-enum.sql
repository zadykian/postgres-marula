-- execution-order: 5

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create type calculation_status as enum
(
	'applied',
	'requires_confirmation',
	'requires_server_restart'
);

comment on type calculation_status is 'Server parameter calculation status.';