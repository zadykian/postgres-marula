-- execution-order: 6

set search_path to SYSTEM_SCHEMA_NAME_TO_REPLACE;

create table parameters_values_history
(
	id                    bigserial          primary key,
	parameter_id          int                not null references calculated_parameters(id),
	calculated_value      non_empty_string   not null,
	status                calculation_status not null,
	calculation_date_time timestamp          not null default now()
);

comment on table  parameters_values_history                       is 'Parameter values calculated my marula service.';
comment on column parameters_values_history.id                    is 'Table identifier.';
comment on column parameters_values_history.parameter_id          is 'Calculated parameter id.';
comment on column parameters_values_history.calculated_value      is 'Calculated value.';
comment on column parameters_values_history.status                is 'Value calculation status.';
comment on column parameters_values_history.calculation_date_time is 'Calculation date and time.';