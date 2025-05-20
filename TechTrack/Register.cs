using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TechTrack
{
    public partial class Register : Form
    {
        private readonly string userFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");

        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;
            string confirmPassword = textBox3.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            var users = LoadUsers();

            if (users.Any(u => u.Username == username))
            {
                MessageBox.Show("Пользователь с таким логином уже существует.");
                return;
            }

            users.Add(new User { Username = username, Password = password });
            SaveUsers(users);

            MessageBox.Show("Регистрация успешна!");

            Login loginForm = new Login();
            loginForm.Show();
            this.Hide();
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

        private void SaveUsers(List<User> users)
        {
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(userFile, json);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var loginForm = new Login();
            loginForm.Show();
            //this.Hide();
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
