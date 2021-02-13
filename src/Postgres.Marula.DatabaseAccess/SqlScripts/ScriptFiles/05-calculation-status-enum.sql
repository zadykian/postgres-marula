-- execution-order: 5

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create type STATUS_ENUM_NAME_TO_REPLACE as enum
(
	'applied',
	'requires_confirmation',
	'requires_server_restart',
	'requires_confirmation_and_restart'
);

comment on type STATUS_ENUM_NAME_TO_REPLACE is 'Server parameter calculation status.';