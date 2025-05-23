using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TechTrack
{
    public partial class updateEqipment : Form
    {
        private List<Equipment> currentData;
        private string currentPath;

        public updateEqipment()
        {
            InitializeComponent();

            comboBox1.Items.AddRange(new[] { "Компьютер", "Мышка", "Клавиатура", "Принтер" });
            comboBox3.Items.AddRange(new[] { "Каб. 101", "Каб. 102", "Каб. 103" });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) return;

            string type = comboBox1.SelectedItem.ToString();
            currentPath = GetPathByType(type);

            try
            {
                if (File.Exists(currentPath))
                {
                    currentData = JsonConvert.DeserializeObject<List<Equipment>>(File.ReadAllText(currentPath)) ?? new List<Equipment>();
                    comboBox2.Items.Clear();
                    comboBox2.Items.AddRange(currentData.Select(x => x.InventoryNumber).ToArray());
                }
                else
                {
                    currentData = new List<Equipment>();
                    comboBox2.Items.Clear();
                }

                textBox1.Clear();
                comboBox3.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || currentData == null) return;

            string inventory = comboBox2.SelectedItem.ToString();
            var selected = currentData.FirstOrDefault(x => x.InventoryNumber == inventory);

            if (selected != null)
            {
                textBox1.Text = selected.Condition;
                comboBox3.SelectedItem = selected.Room;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || currentData == null)
            {
                MessageBox.Show("Пожалуйста, выберите устройство для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string inventory = comboBox2.SelectedItem.ToString();
            var selected = currentData.FirstOrDefault(x => x.InventoryNumber == inventory);

            if (selected != null)
            {
                selected.Condition = textBox1.Text;
                selected.Room = comboBox3.SelectedItem?.ToString() ?? "";

                File.WriteAllText(currentPath, JsonConvert.SerializeObject(currentData, Formatting.Indented));

                MessageBox.Show("Устройство обновлено!");
                Close();
            }
            else
            {
                MessageBox.Show("Устройство не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetPathByType(string type)
        {
            switch (type)
            {
                case "Компьютер":
                    return "computers.json";
                case "Мышка":
                    return "mice.json";
                case "Клавиатура":
                    return "keyboards.json";
                case "Принтер":
                    return "printers.json";
                default:
                    throw new Exception("Неизвестный тип устройства");
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide(); 
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
