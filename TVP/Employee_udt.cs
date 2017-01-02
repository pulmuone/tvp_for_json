using NpgsqlTypes;

namespace TVP
{
    /// <summary>
    /// Postgresql의 Composite Type과 이름까지 동일하게
    /// </summary>
    public class Employee_udt
    {
        [PgName("emp_id")] //pg 이름을 적어 준다.
        public int emp_id { get; set; } //pg와 달라도 상관없음.

        [PgName("emp_nm")]
        public string emp_nm { get; set; }

    }
}
