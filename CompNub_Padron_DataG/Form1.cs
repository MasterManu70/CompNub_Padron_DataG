using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CompNub_Padron_DataG
{
    public partial class Form1 : Form
    {
        public bool avanzado = false;
        public int limitR = 30;
        public string query = @"select * from
                            (select 
                            ciudadanos.id,
                            ciudadanos.clave,
                            nacio.nacio,
                            estados.estado,
                            ciudadanos.genero,
                            local.local,
                            concat(a1.acronimo, ' ', a2.acronimo, ' ', a3.acronimo, ' ') as completo,
                            a1.acronimo as nombre,
                            a2.acronimo as paterno,
                            a3.acronimo as materno,
                            ciudadanos.fecha,
                            domicilios.domicilio,
                            distritos.distrito,
                            colonias.colonia,
                            cp.cp,
                            ocupaciones.ocupacion,
                            ciudadanos.numero,
                            ciudades.ciudad,
                            ciudadanos.codigo

                            from ciudadanos

                            inner join
                            nacio
                            on nacio.id = ciudadanos.nacio

                            inner join
                            estados
                            on estados.id = ciudadanos.estado

                            inner join
                            local
                            on local.id = ciudadanos.local

                            inner join
                            acronimos as a1
                            on 
                            ciudadanos.nombre = a1.id

                            inner join
                            acronimos as a2
                            on 
                            ciudadanos.paterno = a2.id

                            inner join
                            acronimos as a3
                            on 
                            ciudadanos.materno = a3.id

                            inner join
                            domicilios
                            on domicilios.id = ciudadanos.domicilio

                            inner join
                            distritos
                            on distritos.id = ciudadanos.distrito

                            inner join
                            colonias
                            on colonias.id = ciudadanos.colonia

                            inner join
                            cp
                            on cp.id = ciudadanos.cp

                            inner join
                            ocupaciones
                            on ocupaciones.id = ciudadanos.ocupacion

                            inner join
                            ciudades
                            on ciudades.id = ciudadanos.ciudad) as a";
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
            bool rowColor = true;
            string g = "M";
            dataGridView1.Rows.Clear();
            ConexionMySQL(query + " where a.completo like '%" + textBox1.Text + "%' " + alterQuery(textBox1.Text) + " limit "+ numericUpDown2.Value + "," + numericUpDown1.Value + ";");
            for (int i = 0; i < ds.Tables["tabla"].Rows.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["id"]);
                dataGridView1.Rows[i].Cells[1].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["clave"]);
                dataGridView1.Rows[i].Cells[2].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["nacio"]);
                dataGridView1.Rows[i].Cells[3].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["estado"]);
                if (ds.Tables["tabla"].Rows[i]["genero"].ToString() == "1") g = "M"; else g = "F";
                dataGridView1.Rows[i].Cells[4].Value = g;
                dataGridView1.Rows[i].Cells[5].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["local"]);
                dataGridView1.Rows[i].Cells[6].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["completo"]);
                dataGridView1.Rows[i].Cells[7].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["nombre"]);
                dataGridView1.Rows[i].Cells[8].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["paterno"]);
                dataGridView1.Rows[i].Cells[9].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["materno"]);
                dataGridView1.Rows[i].Cells[10].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["fecha"].ToString().Substring(0, 10));
                dataGridView1.Rows[i].Cells[11].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["domicilio"]);
                dataGridView1.Rows[i].Cells[12].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["distrito"]);
                dataGridView1.Rows[i].Cells[13].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["colonia"]);
                dataGridView1.Rows[i].Cells[14].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["cp"]);
                dataGridView1.Rows[i].Cells[15].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["ocupacion"]);
                dataGridView1.Rows[i].Cells[16].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["numero"]);
                dataGridView1.Rows[i].Cells[17].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["ciudad"]);
                dataGridView1.Rows[i].Cells[18].Value = Convert.ToString(ds.Tables["tabla"].Rows[i]["codigo"]);

                if(rowColor) dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White; else dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                rowColor = !rowColor;
            }
        }

        private string alterQuery(string text)
        {
            string alterQuery = "";
            if (checkBox1.Checked && checkBox13.Checked) alterQuery += " or a.id like '%" + text + "%'";
            if (checkBox2.Checked && checkBox13.Checked) alterQuery += " or a.clave like '%" + text + "%'";
            if (checkBox3.Checked && checkBox13.Checked) alterQuery += " or a.estado like '%" + text + "%'";
            if (checkBox4.Checked && checkBox13.Checked) alterQuery += " or a.local like '%" + text + "%'";
            if (checkBox5.Checked && checkBox13.Checked) alterQuery += " or a.fecha like '%" + text + "%'";
            if (checkBox6.Checked && checkBox13.Checked) alterQuery += " or a.distrito like '%" + text + "%'";
            if (checkBox7.Checked && checkBox13.Checked) alterQuery += " or a.domicilio like '%" + text + "%'";
            if (checkBox8.Checked && checkBox13.Checked) alterQuery += " or a.colonia like '%" + text + "%'";
            if (checkBox9.Checked && checkBox13.Checked) alterQuery += " or a.cp like '%" + text + "%'";
            if (checkBox10.Checked && checkBox13.Checked) alterQuery += " or a.ocupacion like '%" + text + "%'";
            if (checkBox11.Checked && checkBox13.Checked) alterQuery += " or a.numero like '%" + text + "%'";
            if (checkBox12.Checked && checkBox13.Checked) alterQuery += " or a.codigo like '%" + text + "%'";
            return alterQuery;
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            avanzado=!avanzado;
            checkBox1.Enabled = avanzado;
            checkBox2.Enabled = avanzado;
            checkBox3.Enabled = avanzado;
            checkBox4.Enabled = avanzado;
            checkBox5.Enabled = avanzado;
            checkBox6.Enabled = avanzado;
            checkBox7.Enabled = avanzado;
            checkBox8.Enabled = avanzado;
            checkBox9.Enabled = avanzado;
            checkBox10.Enabled = avanzado;
            checkBox11.Enabled = avanzado;
            checkBox12.Enabled = avanzado;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string index = "1";
            try
            {
                index = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            catch (Exception)
            {
            }

            axWindowsMediaPlayer1.URL = "resources\\vid\\video" + index.ToString() + ".mp4";
            pictureBox1.Image = Image.FromFile(Path.Combine(Application.StartupPath, "resources\\img\\foto" + index.ToString() + ".png"));
            axWindowsMediaPlayer2.URL = "resources\\aud\\audio" + index.ToString() + ".mp3";
        }
    }
}
