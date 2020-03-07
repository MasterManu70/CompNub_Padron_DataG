using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CompNub_Padron_DataG
{
    public partial class Form1 : Form
    {
        public int limitR = 30;
        public string query = "select * from ciudadanos;";
        public Form1()
        {
            InitializeComponent();
        }

        string cadconn = "Server=localhost; Database=optima_2; Uid=root; Pwd=12345678;";
        MySqlConnection cnn;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataSet ds;

        private void ConexionMySQL(string sql)
        {
            cnn = new MySqlConnection(cadconn);
            cmd = new MySqlCommand(sql, cnn);
            da = new MySqlDataAdapter(cmd);
            ds = new DataSet();

            try
            {
                da.Fill(ds, "tabla");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error No. " + ex.ErrorCode, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConexionMySQL(query);
            dataGridView1.DataSource = ds.Tables["tabla"];
        }
    }
}
