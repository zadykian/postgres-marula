-- execution-order: 6

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create table VALUES_HISTORY_TABLE_NAME_TO_REPLACE
(
	id                    int                         primary key generated always as identity,
	parameter_id          smallint                    not null references calculated_parameters(id),
	calculated_value      non_empty_string,
	unit			      UNIT_ENUM_NAME_TO_REPLACE   not null,
	status                STATUS_ENUM_NAME_TO_REPLACE not null,
	calculation_date_time timestamp                   not null default current_timestamp
);

comment on table  VALUES_HISTORY_TABLE_NAME_TO_REPLACE                       is 'Parameter values calculated my marula service.';
comment on column VALUES_HISTORY_TABLE_NAME_TO_REPLACE.id                    is 'Table identifier.';
comment on column VALUES_HISTORY_TABLE_NAME_TO_REPLACE.parameter_id          is 'Calculated parameter id.';
comment on column VALUES_HISTORY_TABLE_NAME_TO_REPLACE.calculated_value      is 'Calculated value.';
comment on column VALUES_HISTORY_TABLE_NAME_TO_REPLACE.unit                  is 'Parameter unit.';
comment on column VALUES_HISTORY_TABLE_NAME_TO_REPLACE.status                is 'Value calculation status.';
comment on column VALUES_HISTORY_TABLE_NAME_TO_REPLACE.calculation_date_time is 'Calculation date and time.';