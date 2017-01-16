using NpgsqlTypes;

namespace TVP
{
    /// <summary>
    /// Postgresql의 Composite Type과 이름까지 동일하게
    /// </summary>
    /// 
    public class EmployeeUdt
    {
        //pg 이름을 적어 준다. [PgName] 속성을 통해 필드별로 매핑을 제어 할 수 있습니다. 그러면 이름 변환기가 무시됩니다.
        [PgName("emp_id")] 
        public int EmpId { get; set; } //pg와 달라도 상관없음.

        [PgName("emp_nm")]
        public string EmpNm { get; set; }
    }
}
