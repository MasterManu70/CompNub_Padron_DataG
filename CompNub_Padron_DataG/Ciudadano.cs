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
    public partial class Ciudadano : Form
    {
        //string id = "0";
        List<string> ciudadano = new List<string>();
        //[0] = "id", [1] = "clave", [2] = "nacio", [3] = "estado", [4] = "genero", [5] = "local" 
        //[6] = "nombre", [7] = "paterno",  [8] = "materno",  [9] = "fecha",  [10] = "domicilio", [11] = "distrito",
        //[12] = "colonia",  [13] = "cp",  [14] = "ocupacion",  [15] = "numero", [16] = "ciudad", [17] = "código",
        public Ciudadano(List<string> ciudadano)
        {
            InitializeComponent();
            for (int i = 1; i <= 31; i++) comboBox6.Items.Add(i); //Días
            for (int i = 1; i <= 12; i++) comboBox7.Items.Add(i); //Meses
            for (int i = 1900; i <= 2020; i++) comboBox2.Items.Add(i); //Años
 
            if (ciudadano.Count > 1)
            {
                this.ciudadano = ciudadano;
                textBox1.Text = ciudadano[0];
                textBox2.Text = ciudadano[1];
                comboBox1.Text = ciudadano[2];
                comboBox15.Text = ciudadano[3];
                if (ciudadano[4] == "M") radioButton1.Checked = true; else radioButton2.Checked = true;
                comboBox16.Text = ciudadano[5];
                comboBox3.Text = ciudadano[6];
                comboBox4.Text = ciudadano[7];
                comboBox5.Text = ciudadano[8];

                comboBox6.Text = ciudadano[9].ToString().Substring(0, 2);

                comboBox7.Text = ciudadano[9].ToString().Substring(3, 2);

                comboBox2.Text = ciudadano[9].ToString().Substring(6, 4);
                comboBox8.Text = ciudadano[10];
                comboBox9.Text = ciudadano[11];
                comboBox10.Text = ciudadano[12];
                comboBox11.Text = ciudadano[13];
                comboBox12.Text = ciudadano[14];
                textBox3.Text = ciudadano[15];
                comboBox13.Text = ciudadano[16];
                comboBox14.Text = ciudadano[17];

                textBox3.Enabled = false;
            }
        }
        static DataSet ds;
        static void ConexionMySQL(string sql)
        {
            string cadconn = "Server=localhost; Database=optima_2; Uid=root; Pwd=12345678;";
            MySqlConnection cnn;
            MySqlCommand cmd;
            MySqlDataAdapter da;

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

        private void button2_Click(object sender, EventArgs e)
        {
            if (!camposVacios()) return;

            int genero = 1;

            if (radioButton1.Checked) genero = 1; else genero = 0;

            if (Convert.ToInt32(textBox1.Text) == 0)
            {
                ConexionMySQL("insert into ciudadanos values(null,'"
                    + textBox2.Text + "',"
                    + tableSelector("nacio", "nacio", comboBox1.Text) + ","
                    + tableSelector("estados", "estado", comboBox15.Text) + ","
                    + genero + ","
                    + tableSelector("local", "local", comboBox16.Text) + ","
                    + tableSelector("acronimos", "acronimo", comboBox3.Text) + ","
                    + tableSelector("acronimos", "acronimo", comboBox4.Text) + ","
                    + tableSelector("acronimos", "acronimo", comboBox5.Text) + ",'"
                    + comboBox2.Text + "-" + comboBox7.Text + "-" + comboBox6.Text + "',"
                    + tableSelector("domicilios", "domicilio", comboBox8.Text) + ","
                    + tableSelector("distritos", "distrito", comboBox9.Text) + ","
                    + tableSelector("colonias", "colonia", comboBox10.Text) + ","
                    + tableSelector("cp", "cp", comboBox11.Text) + ","
                    + tableSelector("ocupaciones", "ocupacion", comboBox12.Text) + ",'"
                    + textBox3.Text + "',"
                    + tableSelector("ciudades", "ciudad", comboBox13.Text) + ",'"
                    + comboBox14.Text + "');"
                    );

                MessageBox.Show("El registro ha sido agregado exitosamente.");
            }
            else
            {
                ConexionMySQL("update ciudadanos set clave='" + textBox2.Text + "',"
                    + "nacio=" + tableSelector("nacio","nacio",comboBox1.Text) + ","
                    + "estado=" + tableSelector("estados", "estado", comboBox15.Text) + ","
                    + "genero=" + genero + ","
                    + "local=" + tableSelector("local", "local", comboBox15.Text) + ","
                    + "nombre=" + tableSelector("acronimos", "acronimo", comboBox3.Text) + ","
                    + "paterno=" + tableSelector("acronimos", "acronimo", comboBox4.Text) + ","
                    + "materno=" + tableSelector("acronimos", "acronimo", comboBox5.Text) + ","
                    + "fecha='" + comboBox2.Text + "-" + comboBox7.Text + "-" + comboBox6.Text + "',"
                    + "domicilio=" + tableSelector("domicilios", "domicilio", comboBox8.Text) + ","
                    + "distrito=" + tableSelector("distritos", "distrito", comboBox9.Text) + ","
                    + "colonia=" + tableSelector("colonias", "colonia", comboBox10.Text) + ","
                    + "cp=" + tableSelector("cp", "cp", comboBox11.Text) + ","
                    + "ciudad=" + tableSelector("ciudades", "ciudad", comboBox13.Text) + ","
                    + "codigo='" + comboBox14.Text + "' where id=" + textBox1.Text + ";"
                    );
                MessageBox.Show("El registro ha sido modificado exitosamente.");
            }
        }

        static int tableSelector(string tabla, string campo, string dato)
        {
            int id = 0;
            if (tabla != "cp" && tabla != "nacio") dato = "'" + dato + "'";
            ConexionMySQL("select id from " + tabla + " where " + campo + "= " + dato + ";");

            try
            {
                id = Convert.ToInt32(ds.Tables["tabla"].Rows[0]["id"]);
            }
            catch(Exception)
            {
                ConexionMySQL("insert into " + tabla + " values(null," + dato + ");");
                ConexionMySQL("select last_insert_id() as id;");
                id = Convert.ToInt32(ds.Tables["tabla"].Rows[0]["id"]);
            }

            return id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        bool camposVacios()
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("El campo clave no debe estar vacío.");
                textBox2.Focus();
                return false;
            }
            if (comboBox1.Text == "")
            {
                MessageBox.Show("El campo nacio no debe estar vacío.");
                comboBox1.Focus();
                return false;
            }
            if (comboBox15.Text == "")
            {
                MessageBox.Show("El campo estado no debe estar vacío.");
                comboBox15.Focus();
                return false;
            }
            if (comboBox16.Text == "")
            {
                MessageBox.Show("El campo local no debe estar vacío.");
                comboBox16.Focus();
                return false;
            }
            if (comboBox3.Text == "")
            {
                MessageBox.Show("El campo nombre no debe estar vacío.");
                comboBox3.Focus();
                return false;
            }
            if (comboBox4.Text == "")
            {
                MessageBox.Show("El campo apellido paterno no debe estar vacío.");
                comboBox4.Focus();
                return false;
            }
            if (comboBox5.Text == "")
            {
                MessageBox.Show("El campo apellido materno no debe estar vacío.");
                comboBox5.Focus();
                return false;
            }
            if (comboBox6.Text == "")
            {
                MessageBox.Show("El campo día no debe estar vacío.");
                comboBox6.Focus();
                return false;
            }
            if (comboBox7.Text == "")
            {
                MessageBox.Show("El campo mes no debe estar vacío.");
                comboBox7.Focus();
                return false;
            }
            if (comboBox2.Text == "")
            {
                MessageBox.Show("El campo año no debe estar vacío.");
                comboBox2.Focus();
                return false;
            }
            if (comboBox8.Text == "")
            {
                MessageBox.Show("El campo domicilio no debe estar vacío.");
                comboBox8.Focus();
                return false;
            }
            if (comboBox9.Text == "")
            {
                MessageBox.Show("El campo distrito no debe estar vacío.");
                comboBox9.Focus();
                return false;
            }
            if (comboBox10.Text == "")
            {
                MessageBox.Show("El campo colonia no debe estar vacío.");
                comboBox10.Focus();
                return false;
            }
            if (comboBox11.Text == "")
            {
                MessageBox.Show("El campo cp no debe estar vacío.");
                comboBox11.Focus();
                return false;
            }
            if (comboBox12.Text == "")
            {
                MessageBox.Show("El campo ocupación no debe estar vacío.");
                comboBox12.Focus();
                return false;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("El campo número no debe estar vacío.");
                textBox3.Focus();
                return false;
            }
            if (comboBox13.Text == "")
            {
                MessageBox.Show("El campo ciudad no debe estar vacío.");
                comboBox15.Focus();
                return false;
            }
            if (comboBox14.Text == "")
            {
                MessageBox.Show("El campo código no debe estar vacío.");
                comboBox14.Focus();
                return false;
            }
            return true;
        }
    }
}
