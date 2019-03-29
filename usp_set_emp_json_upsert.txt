CREATE OR REPLACE FUNCTION public.usp_set_emp_json_upsert(p_employee_json json)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
		_sqlstate    TEXT;
		_message     TEXT;
		_context     TEXT;
BEGIN

/*
	INSERT INTO employee
	SELECT A.emp_id, A.emp_nm, now(), now() FROM json_to_recordset(p_employee_json) AS A(emp_id INTEGER, emp_nm VARCHAR)
--	SELECT A.emp_id, A.emp_nm, now(), now() FROM json_populate_recordset(null::employee_udt, p_employee_json) A --9.3
	ON CONFLICT (emp_id)
	DO UPDATE SET emp_nm = excluded.emp_nm
	WHERE  employee.emp_nm <> excluded.emp_nm;
*/

    WITH cte AS (SELECT A.emp_id, A.emp_nm FROM json_to_recordset(p_employee_json) AS A(emp_id INTEGER, emp_nm VARCHAR) ),
    del as
        (
			DELETE FROM employee
			WHERE NOT EXISTS (SELECT 1 FROM cte a WHERE a.emp_id = employee.emp_id)
        )
	INSERT INTO employee
	SELECT A.emp_id, A.emp_nm, now(), now() FROM cte  A
	ON CONFLICT (emp_id)
	DO UPDATE SET emp_nm = excluded.emp_nm
	WHERE  employee.emp_nm <> excluded.emp_nm
	;


 EXCEPTION
  WHEN OTHERS THEN
    GET STACKED DIAGNOSTICS
    	_sqlstate = RETURNED_SQLSTATE, _message =  MESSAGE_TEXT, _context = PG_EXCEPTION_CONTEXT;
      RAISE EXCEPTION 'sqlstate: %, message: %, context: [%]',_sqlstate, _message, replace(_context, E'n', ' <- ');
END;
$function$
;
