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
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    public partial class registracija : Form
    {
        public registracija()
        {
            InitializeComponent();
            regPassTextBox.UseSystemPasswordChar = true;
            regPonoviGesloTextBox.UseSystemPasswordChar = true;
        }

        private void registracija_Load(object sender, EventArgs e)
        {
            znamkaComboBox();
        }

        private void znamkaComboBox()
        {
            string komanda = "SELECT * FROM znamke";

            using (MySqlConnection conn = new MySqlConnection("datasource = mysql6001.site4now.net; username = a41906_projekt; password = salabajzer123; database = db_a41906_projekt; sslmode=none"))
            {
                conn.Open();
                using (MySqlCommand com = new MySqlCommand(komanda, conn))
                {
                    MySqlDataReader reader = com.ExecuteReader();
                    while (reader.Read())
                    {
                        regZnamkaComboBox.Items.Add(reader["ime"].ToString());
                    }
                    com.Dispose();
                }
                conn.Close();
            }
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            double cena = Convert.ToDouble(regCenaTextBox.Text);
            int letnik = Convert.ToInt32(regLetnikTextBox.Text);
            double moc = Convert.ToDouble(regMocTextBox.Text);
            int kubiki = Convert.ToInt32(regKubikiTextBox.Text);
            double prevozeni = Convert.ToDouble(regPrevozeniTextBox.Text);
            double poraba = Convert.ToDouble(regPorabaTextBox.Text);
            int rank = 1;

            string model = regModelComboBox.SelectedItem.ToString();

            string geslo = regPassTextBox.Text;
            string ponoviGeslo = regPonoviGesloTextBox.Text;

            if (geslo==ponoviGeslo)
            {
                try
                {
                    string ime = regImeTextBox.Text;
                    string priimek = regPriimekTextBox.Text;
                    string username = regUsernameTextBox.Text;
                    string password = GetMD5(regPassTextBox.Text);

                    string komanda = "INSERT INTO uporabniki (ime, priimek, username, password, rank) VALUES ('" + ime + "','" + priimek + "','" + username + "','" + password + "', " + rank + "); SELECT last_insert_id();";

                    int uporabnik_id;

                    using (MySqlConnection conn = new MySqlConnection("datasource = mysql6001.site4now.net; username = a41906_projekt; password = salabajzer123; database = db_a41906_projekt; sslmode=none"))
                    {
                        conn.Open();
                        using (MySqlCommand com = new MySqlCommand(komanda, conn))
                        {
                            uporabnik_id = Convert.ToInt32(com.ExecuteScalar());
                            com.Dispose();
                        }
                        conn.Close();
                    }


                    string comanda = "INSERT INTO avtomobili (cena, letnik, moc, kubiki, prevozeni, poraba, model_id, uporabnik_id) VALUES ('" + cena + "','" + letnik + "', '" + moc + "', '" + kubiki + "', '" + prevozeni + "', '" + poraba + "', (SELECT id FROM modeli WHERE ime='" + model + "'), "+ uporabnik_id +");";


                    using (MySqlConnection conn = new MySqlConnection("datasource = mysql6001.site4now.net; username = a41906_projekt; password = salabajzer123; database = db_a41906_projekt; sslmode=none"))
                    {
                        conn.Open();
                        using (MySqlCommand com = new MySqlCommand(comanda, conn))
                        {
                            com.ExecuteNonQuery();
                            com.Dispose();
                        }
                        conn.Close();
                    }

                    
                    MessageBox.Show("Uporabnik vnešen.");
                    regCenaTextBox.Clear();
                    regImeTextBox.Clear();
                    regKubikiTextBox.Clear();
                    regLetnikTextBox.Clear();
                    regMocTextBox.Clear();
                    regPassTextBox.Clear();
                    regPonoviGesloTextBox.Clear();
                    regPorabaTextBox.Clear();
                    regPrevozeniTextBox.Clear();
                    regPriimekTextBox.Clear();
                    regUsernameTextBox.Clear();
                    regZnamkaComboBox.ResetText();
                    regModelComboBox.ResetText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            else
            {
                MessageBox.Show("Gesli se ne ujemata");
            }
        }

        private void regZnamkaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            regModelComboBox.Items.Clear();
            string znamka = regZnamkaComboBox.SelectedItem.ToString();

            string komanda = "SELECT m.ime FROM modeli m INNER JOIN znamke z ON z.id=m.znamka_id WHERE z.ime = '" + znamka + "' ";

            using (MySqlConnection conn = new MySqlConnection("datasource = mysql6001.site4now.net; username = a41906_projekt; password = salabajzer123; database = db_a41906_projekt; sslmode=none"))
            {
                conn.Open();
                using (MySqlCommand com = new MySqlCommand(komanda, conn))
                {
                    MySqlDataReader reader = com.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        regModelComboBox.Items.Add(reader["ime"].ToString());
                    }
                    com.Dispose();
                }
                conn.Close();
            }
        }

        private void registracija_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void nazajBtn_Click(object sender, EventArgs e)
        {
            prijavaForm prijavaForm = new prijavaForm();
            this.Hide();
            prijavaForm.Show();
        }

        public string GetMD5(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                str.Append(result[i].ToString("x2"));
            }
            return str.ToString();
        }
    }
}
