using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TechTrack
{
    public partial class Login : Form
    {
        private readonly string userFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            var users = LoadUsers();

            if (users.Any(u => u.Username == username && u.Password == password))
            {
                Form1 mainForm = new Form1();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(userFile))
            {
                File.WriteAllText(userFile, "[]");
            }

            string json = File.ReadAllText(userFile);
            return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register regForm = new Register();
            regForm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
