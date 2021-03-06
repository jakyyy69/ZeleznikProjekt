﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public static string pot;
        public static string potv;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = prijavaForm.ime;
            textBox2.Text = prijavaForm.priimek;
            izberiPotComboBox();
            if(prijavaForm.rank == 10)
            {
                izbrisiPotButton.Visible = true;
            }
            else
            {
                izbrisiPotButton.Visible = false;
            }
        }

        private void izberiPotComboBox()
        {
            string komanda = "SELECT p.cas AS cas, p.opis AS opis, k.ime AS ime FROM poti p INNER JOIN zacetniKraji zk ON zk.id=p.zacetniKraj INNER JOIN kraji k ON k.id=zk.kraj_id";

            using (MySqlConnection conn = new MySqlConnection("datasource = mysql6001.site4now.net; username = a41906_projekt; password = salabajzer123; database = db_a41906_projekt; sslmode=none"))
            {
                conn.Open();
                using (MySqlCommand com = new MySqlCommand(komanda, conn))
                {
                    MySqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        izbiraComboBox.Items.Add(reader.GetString("ime"));
                    }
                    com.Dispose();
                }
                conn.Close();
            }
        }

        private void ustvariPotBtn_Click(object sender, EventArgs e)
        {
            ustvariPot ustvaripot = new ustvariPot();
            ustvaripot.Show();
            this.Hide();
        }

        private void odjavaButton_Click(object sender, EventArgs e)
        {
            string message = "Ali ste prepricani da se zelite odjaviti?";
            string naslov = "Odjava!";

            var result = MessageBox.Show(message, naslov, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Odjava uspešna.", "Odjava!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                prijavaForm prijavaForm = new prijavaForm();
                this.Hide();
                prijavaForm.Show();
            }
            else
            {

            }
        }

        private void izpisiPotBtn_Click(object sender, EventArgs e)
        {
            potv = izbira2ComboBox.SelectedItem.ToString();
            izpisPoti izpisPoti = new izpisPoti();
            this.Hide();
            izpisPoti.Show();
        }

        private void izbiraComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            izbira2ComboBox.Items.Clear();
            pot = izbiraComboBox.SelectedItem.ToString();

            string komanda = "SELECT k.ime AS ime FROM kraji k1 INNER JOIN zacetnikraji zk ON k1.id=zk.kraj_id INNER JOIN poti p ON p.zacetniKraj=zk.id INNER JOIN koncnikraji kk ON kk.id=p.koncniKraj INNER JOIN kraji k ON kk.kraj_id=k.id WHERE (zk.kraj_id=(SELECT k2.id from kraji k2 WHERE (k2.ime='"+pot+"')))";

            using (MySqlConnection conn = new MySqlConnection("datasource = mysql6001.site4now.net; username = a41906_projekt; password = salabajzer123; database = db_a41906_projekt; sslmode=none"))
            {
                conn.Open();
                using (MySqlCommand com = new MySqlCommand(komanda, conn))
                {
                    MySqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        izbira2ComboBox.Items.Add(reader.GetString("ime"));
                    }
                    com.Dispose();
                }
                conn.Close();
            }
        }
    }
}
