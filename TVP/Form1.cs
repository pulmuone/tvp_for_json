using Newtonsoft.Json;
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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Login();
        }

        private async void Login()
        {
            var p = new LoginUser { Compky = "A001", Wareky = "10", Userid = "90773532", Passwd = "3532" };
            string jsonString = JsonConvert.SerializeObject(p);

             string result = await RequestService.Instance.AuthorizationAsync(jsonString);

            LoginUser loginUser = JsonConvert.DeserializeObject<LoginUser>(result);
            System.Diagnostics.Debug.WriteLine(loginUser.Auth_Token);

            Global.token = loginUser.Auth_Token;

            Debug.WriteLine(Global.token);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string responseResult = string.Empty;
            string requestParamJson = string.Empty;

            dataGridView1.DataSource = null;

            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            Dictionary<string, string> requestDic = new Dictionary<string, string>();
            requestDic.Add("UFN", "{? = call usp_get_emp_json(?)}"); //프로시저
            requestDic.Add("p_emp_nm", this.txtEmpNm.Text.Trim()); //프로시저 파라미터와 동일하게

            requestParamJson = JsonConvert.SerializeObject(requestDic);

            responseResult = await RequestService.Instance.GetRequestAsync(requestParamJson);
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(responseResult);

            if (dt != null || dt.Rows.Count >= 1)
            {
                DataView dv = new DataView(dt)
                {
                    Sort = "emp_id"
                };
                this.dataGridView1.DataSource = dv.ToTable();
            }

            sw.Stop();
            Debug.WriteLine("btnSearch_Click : " + sw.Elapsed.ToString());
            MessageBox.Show("Search");
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string responseResult = string.Empty;
            string requestParamJson = string.Empty;
            //컬럼 잘라서 사용해도 가능
            //string jsonStringtmp = JsonConvert.SerializeObject((DataTable)this.dataGridView1.DataSource);

            /*
            List<EmployeeUdt> lst_param = new List<EmployeeUdt>();
            for (int i = 0; i < this.dataGridView1.Rows.Count; i++)
            {
                if (this.dataGridView1[0, i].Value != null && !string.IsNullOrEmpty(this.dataGridView1[0, i].Value.ToString()))
                {
                    lst_param.Add
                    (
                        new EmployeeUdt
                        {
                            emp_id = Convert.ToInt32(this.dataGridView1[0, i].Value),
                            emp_nm = this.dataGridView1[1, i].Value.ToString()
                        }
                    );
                }
            }
            */
            List<BarcodeLabel> lst_param = new List<BarcodeLabel>();
            lst_param.Add(new BarcodeLabel { slipno = "A001101904010197", lbbrcd = "10351687120020000100001", status = "Y" });
            lst_param.Add(new BarcodeLabel { slipno = "A001101904010197", lbbrcd = "10350522790060000100001", status = "Y" });
            lst_param.Add(new BarcodeLabel { slipno = "A001101904010197", lbbrcd = "10351687120160003000001", status = "Y" });
            string jsonString = JsonConvert.SerializeObject(lst_param);

            Dictionary<string, string> requestDic = new Dictionary<string, string>();
            //requestDic.Add("UFN", "{? = call usp_set_emp_json_upsert(?)}");  //함수 호출
            //requestDic.Add("p_employee_json", jsonString);

            //requestDic.Add("UFN", "{? = call ufn_set_lobsm020(?, ?)}");  //함수 호출
            requestDic.Add("UFN", "{? = call ufn_set_lobsm040(?, ?)}");  //함수 호출
            requestDic.Add("p_barcode_json", jsonString);
            requestDic.Add("p_userid", "90773532");

            //requestDic.Add("USP", "call public.usp_set_emp_json_proc(?)"); //프로시저 호출

            requestParamJson = JsonConvert.SerializeObject(requestDic);
             
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            responseResult = await RequestService.Instance.SetRequestAsync(requestParamJson);

            sw.Stop();
            Debug.WriteLine("btnSave_Click : " + sw.Elapsed.ToString());

            MessageBox.Show(responseResult);
        }
    }
}
