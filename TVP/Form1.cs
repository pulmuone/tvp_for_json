using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace TVP
{
    public partial class Form1 : Form
    {
        //private string connstring = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};Minimum Pool Size={5};Maximum Pool Size={6};Application Name={7};", "127.0.0.1", "5432", "postgres", "0152", "postgres","10", "100", "WMS_APP");
        private string connstring = "Server=127.0.0.1;User Id=postgres;Password=0152;Database=postgres;Minimum Pool Size=10;Maximum Pool Size=100;Application Name=WMS_APP";

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
            List<EmployeeUdt> lst_param = new List<EmployeeUdt>();

            //DataView dv = new DataView(((DataTable)this.dataGridView1.DataSource));

            //foreach (DataRowView item in dv)
            //{
            //    //Debug.WriteLine(item.DataView
            //}


            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1[0, i].Value != null && !string.IsNullOrEmpty(this.dataGridView1[0, i].Value.ToString()))
                {
                    //udt = new Employee_udt()
                    lst_param.Add(
                        new EmployeeUdt
                        {
                            EmpId = Convert.ToInt32(this.dataGridView1[0, i].Value),
                            EmpNm = this.dataGridView1[1, i].Value.ToString()
                        }
                    );
                }
            }
            
            var _param = new[] {
                new NpgsqlParameter
                {
                    ParameterName="p_employee",
                    NpgsqlDbType = NpgsqlDbType.Composite,
                    SpecificType = typeof(EmployeeUdt[]),
                    NpgsqlValue = lst_param                 
                }

            };

            SqlHelper.ExecuteNonQuery<EmployeeUdt>(this.connstring, "usp_set_emp", _param);
        }
    }
}
