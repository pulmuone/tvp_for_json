using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace TVP
{
    public partial class Form1 : Form
    {
        private string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "127.0.0.1", "5432", "postgres", "0152", "postgres");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            //NpgsqlParameter[] sqlParam = new NpgsqlParameter[1];
            //sqlParam[0] = new NpgsqlParameter("p_emp_nm", NpgsqlDbType.Varchar, 40);
            //sqlParam[0].Value = this.txtEmpNm.Text.Trim();

            var _param = new[] {
                new NpgsqlParameter
                {
                    ParameterName = "p_emp_nm",
                    NpgsqlDbType = NpgsqlDbType.Varchar,
                    Size = 40,
                    NpgsqlValue = this.txtEmpNm.Text.Trim()
                }
            };

            ds = SqlHelper.ExecuteDataset(this.connstring, CommandType.StoredProcedure, "usp_get_emp", _param);
            DataView dv = new DataView(ds.Tables[0])
            {
                Sort = "emp_id"
            };
            this.dataGridView1.DataSource = dv.ToTable();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<Employee_udt> lst_param = new List<Employee_udt>();

            //Employee_udt udt = null;
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1[0, i].Value != null && !string.IsNullOrEmpty(this.dataGridView1[0, i].Value.ToString()))
                {
                    //udt = new Employee_udt()
                    lst_param.Add(
                        new Employee_udt
                        {
                            emp_id = Convert.ToInt32(this.dataGridView1[0, i].Value),
                            emp_nm = this.dataGridView1[1, i].Value.ToString()
                        }
                    );
                }
            }
            
            var _param = new[] {
                new NpgsqlParameter
                {
                    ParameterName="p_employee",
                    NpgsqlDbType = NpgsqlDbType.Composite,
                    SpecificType = typeof(Employee_udt[]),
                    NpgsqlValue = lst_param                 
                }

            };

            SqlHelper.ExecuteNonQuery<Employee_udt>(this.connstring, "usp_set_emp", _param);
        }
    }
}
