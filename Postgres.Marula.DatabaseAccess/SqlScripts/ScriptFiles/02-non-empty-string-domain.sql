-- execution-order: 2

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create domain non_empty_string as varchar(64)
not null
constraint string_is_not_empty check ( trim(value) != '' );

comment on type non_empty_string is 'Varchar value which must be not null and not empty.';