
CREATE TABLE public.employee (
  emp_id INTEGER NOT NULL,
  emp_nm VARCHAR(40),
  first_in_time TIMESTAMP WITH TIME ZONE DEFAULT clock_timestamp(),
  last_chg_time TIMESTAMP WITH TIME ZONE DEFAULT clock_timestamp(),
  CONSTRAINT employee_pkey PRIMARY KEY(emp_id)
)
WITH (oids = false);

CREATE TYPE public.employee_udt AS (
  emp_id INTEGER,
  emp_nm VARCHAR(40)
);

truncate table employee;

insert INTO employee(emp_id, emp_nm)
VALUES (1, 'gwise'), (2, 'jinsun'), (3, 'sunju')


begin;

insert INTO employee(emp_id, emp_nm) VALUES (1, 'gwise');
insert INTO employee(emp_id, emp_nm) VALUES (2, 'gwise');
insert INTO employee(emp_id, emp_nm) VALUES (3, 'gwise');
insert INTO employee(emp_id, emp_nm) VALUES (4, 'gwise');
insert INTO employee(emp_id, emp_nm) VALUES (5, 'gwise');

commit;

select * from employee;



--임시 데이터 생성
truncate table employee;

insert into employee
 SELECT
    gs as idx,
    --'테스트 문자열' || gs AS test_string,
    left(md5(random()::text), 10) AS random_string
FROM
    generate_series(1, 1000) AS gs;
