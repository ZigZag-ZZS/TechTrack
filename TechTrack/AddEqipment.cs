using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TechTrack
{
    public partial class AddEqipment : Form
    {
        public AddEqipment()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(new[] { "Компьютер", "Мышка", "Клавиатура", "Принтер" });
            comboBox2.Items.AddRange(new[] { "Каб. 101", "Каб. 102", "Каб. 103" }); // пример
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 ||
                comboBox2.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            var newItem = new Equipment
            {
                InventoryNumber = textBox1.Text,
                Condition = textBox2.Text,
                Room = comboBox2.SelectedItem.ToString()
            };

            string path = GetPathByType(comboBox1.SelectedItem.ToString());
            string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@"..\..\{Path.GetFileName(path)}"));

            try
            {
                List<Equipment> data = File.Exists(path)
                    ? JsonConvert.DeserializeObject<List<Equipment>>(File.ReadAllText(path)) ?? new List<Equipment>()
                    : new List<Equipment>();

                data.Add(newItem);
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(path, json); // Сохраняем в bin\Debug

                // Копируем обновленный файл обратно в корневую папку проекта
                File.Copy(path, projectPath, true);

                MessageBox.Show("Устройство добавлено!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}\nПуть: {path}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.AppendAllText("error.log", $"{DateTime.Now}: {ex.Message}\n");
            }
        }


        private void SaveEquipment(Equipment equipment, string path)
        {
            List<Equipment> data = new List<Equipment>();

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                data = JsonConvert.DeserializeObject<List<Equipment>>(json) ?? new List<Equipment>();
            }

            data.Add(equipment);

            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
        }


        private string GetPathByType(string type)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory; // Путь к папке bin\Debug или bin\Release
            string path = "";

            if (type == "Компьютер")
                path = Path.Combine(basePath, "computers.json");
            else if (type == "Мышка")
                path = Path.Combine(basePath, "mice.json");
            else if (type == "Клавиатура")
                path = Path.Combine(basePath, "keyboards.json");
            else if (type == "Принтер")
                path = Path.Combine(basePath, "printers.json");
            else
                throw new Exception("Неизвестный тип");
            Console.WriteLine($"Путь к файлу: {path}");
            return path;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void AddEqipment_Load(object sender, EventArgs e)
        {

        }
    }
}
