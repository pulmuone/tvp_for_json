CREATE OR REPLACE FUNCTION public.usp_get_emp_json(p_emp_nm character varying)
 RETURNS json
 LANGUAGE sql
AS $function$
/*
지현명
*/

    select
    	json_agg(to_json(tmp))
    from
    (
        SELECT a.emp_id, a.emp_nm FROM public.employee a
        WHERE a.emp_nm = CASE WHEN p_emp_nm  = '' THEN a.emp_nm ELSE p_emp_nm END
    ) tmp;
$function$
;
