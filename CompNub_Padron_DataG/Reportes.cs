using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CompNub_Padron_DataG
{
    public partial class Reportes : Form
    {
        string listaCargada = "";
        int queryIndex = 1;
        public Reportes()
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

        private void Reportes_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                if (listaCargada != "colonias") comboBox1.DataSource = Form1.coloniasL;
                queryIndex = 1;
                listaCargada = "colonias";
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                if (listaCargada != "colonias") comboBox1.DataSource = Form1.coloniasL;
                queryIndex = 2;
                listaCargada = "colonias";
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                if (listaCargada != "cp") comboBox1.DataSource = Form1.cpL;
                queryIndex = 3;
                listaCargada = "cp";
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                if (listaCargada != "colonias") comboBox1.DataSource = Form1.coloniasL;
                queryIndex = 4;
                listaCargada = "colonias";
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
            {
                if (listaCargada != "calles") comboBox1.DataSource = Form1.domiciliosL;
                queryIndex = 5;
                listaCargada = "calles";
            }
        }
        public static string reporte = "";
        private void button1_Click(object sender, EventArgs e)
        {
            int id;
            string limite = "";
            string habilitado = "";
            if (!checkBox1.Checked) habilitado = " where status = 1";

            if(radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false && radioButton4.Checked == false && radioButton5.Checked == false)
            {
                MessageBox.Show("Debe seleccionar algún tipo de consulta.");
                return;
            }

            if (numericUpDown1.Value > 0) limite = " limit " + numericUpDown2.Value + "," + numericUpDown1.Value + "";

            switch (queryIndex)
            {
                case 1:
                    ConexionMySQL("select id from colonias where colonia='" + comboBox1.Text + "';");
                    id = Convert.ToInt32(ds.Tables["tabla"].Rows[0]["id"]);
                    ConexionMySQL(@"select completo from (select ciudadanos.id,
                                concat(a1.acronimo, ' ', a2.acronimo, ' ', a3.acronimo, ' ') as completo,
				                ciudadanos.colonia
                                from ciudadanos
                                inner join
                                acronimos as a1
                                on
                                ciudadanos.nombre = a1.id
                                inner
                                join
                                acronimos as a2
                                on
                                ciudadanos.paterno = a2.id
                                inner
                                join
                                acronimos as a3
                                on
                                ciudadanos.materno = a3.id " + habilitado + ") as a where colonia = " + id + limite + ";");
                    reporte = "Ciudadanos en la colonia " + comboBox1.Text;
                    break;
                case 2:
                    ConexionMySQL("select id from colonias where colonia='" + comboBox1.Text + "';");
                    id = Convert.ToInt32(ds.Tables["tabla"].Rows[0]["id"]);
                    ConexionMySQL(@"select distinct domicilio from (select domicilios.domicilio,
				                ciudadanos.colonia
                                from ciudadanos
                                inner join
                                domicilios
                                on
                                ciudadanos.domicilio = domicilios.id) as a 
				                where colonia = " + id + limite + ";");
                    reporte = "Calles que cruzan en la colonia " + comboBox1.Text;
                    break;
                case 3:
                    reporte = "Colonias en el código postal " + comboBox1.Text;
                    ConexionMySQL("select id from cp where cp=" + comboBox1.Text + ";");
                    id = Convert.ToInt32(ds.Tables["tabla"].Rows[0]["id"]);
                    ConexionMySQL(@"select distinct colonia from (select colonias.colonia,
				                ciudadanos.cp
                                from ciudadanos
                                inner join
                                colonias
                                on
                                ciudadanos.colonia = colonias.id) as a 
				                where cp = " + id + limite + ";");
                    break;
                case 4:
                    reporte = "Cantidad de ciudadanos en la colonia " + comboBox1.Text;
                    ConexionMySQL("select id from colonias where colonia='" + comboBox1.Text + "';");
                    id = Convert.ToInt32(ds.Tables["tabla"].Rows[0]["id"]);
                    ConexionMySQL("select count(*) as ciudadanos from ciudadanos where colonia = " + id + ";");
                    break;
                case 5:
                    reporte = "Colonias por las que cruza la calle " + comboBox1.Text;
                    ConexionMySQL("select id from domicilios where domicilio='" + comboBox1.Text + "';");
                    id = Convert.ToInt32(ds.Tables["tabla"].Rows[0]["id"]);
                    ConexionMySQL(@"select distinct colonia from (select colonias.colonia,
				                ciudadanos.domicilio
                                from ciudadanos
                                inner join
                                colonias
                                on
                                ciudadanos.colonia = colonias.id) as a 
				                where domicilio = " + id + limite + ";");
                    break;
                default:
                    break;
            }
            dataGridView1.DataSource = ds.Tables["tabla"];
            dataGridView1.AutoResizeColumns();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.DroppedDown = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("No hay registros.");
                return;
            }
            string archivo = reporte + ".html";
            using (StreamWriter item = File.CreateText(archivo))
            {
                item.WriteLine("<html>");
                item.WriteLine("<head> <title> Reporte </title>");
                item.WriteLine("</head>");
                item.WriteLine("<body>");
                item.WriteLine("<table border=5>");
                item.WriteLine("<tr>");
                item.WriteLine("<td colspan=19><center><h1>REPORTE INE</h1></center></td>");
                item.WriteLine("</tr>");
                item.WriteLine("<tr>");
                item.WriteLine("<td><strong>" + dataGridView1.Columns[0].Name.ToUpper() + "</td></strong>");

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    item.WriteLine("<tr>");
                    item.WriteLine("<td>" + dataGridView1.Rows[i].Cells[0].Value + "</td>");
                    item.WriteLine("</tr>");
                }

                item.WriteLine("</table>");
                item.WriteLine("</body>");
                item.WriteLine("</html>");
            }
            MessageBox.Show("El reporte se ha generado con éxito");
        }
    }
}
