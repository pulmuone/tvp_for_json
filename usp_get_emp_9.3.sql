CREATE OR REPLACE FUNCTION public.usp_get_emp (
  p_emp_nm varchar
)
RETURNS json AS
$body$
/*
	ROW TO JSON
*/

    select
    	json_agg(TO_JSON(tmp))
    from
    (
        SELECT a.emp_id, a.emp_nm FROM public.employee a
        WHERE a.emp_nm = CASE WHEN p_emp_nm  = '' THEN a.emp_nm ELSE p_emp_nm END
    ) tmp;

$body$
LANGUAGE 'sql'
VOLATILE
CALLED ON NULL INPUT
SECURITY INVOKER
COST 100;


