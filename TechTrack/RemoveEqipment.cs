using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TechTrack
{
    public partial class RemoveEqipment : Form
    {
        private List<Equipment> currentData;
        private string currentPath;

        public RemoveEqipment()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(new[] { "Компьютер", "Мышка", "Клавиатура", "Принтер" });
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
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

               }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void RemoveEqipment_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static implicit operator RemoveEqipment(updateEqipment v)
        {
            throw new NotImplementedException();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || currentData == null) return;

            string inventory = comboBox2.SelectedItem.ToString();
            var selected = currentData.FirstOrDefault(x => x.InventoryNumber == inventory);

            if (selected != null)
            {
                textBox1.Text = selected.Condition;
                textBox2.Text = selected.Room;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null || currentData == null)
            {
                MessageBox.Show("Пожалуйста, выберите устройство для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string inventory = comboBox2.SelectedItem.ToString();
            var selected = currentData.FirstOrDefault(x => x.InventoryNumber == inventory);

            if (selected != null)
            {
                DialogResult dialogResult = MessageBox.Show(
                    $"Вы уверены, что хотите удалить устройство с инвентарным номером {inventory}?","Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (dialogResult == DialogResult.Yes){
                    currentData.Remove(selected);
                    try 
                    {
                        File.WriteAllText(currentPath, JsonConvert.SerializeObject(currentData, Formatting.Indented));
                        MessageBox.Show("Устройство удалено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Устройство не найдено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
            
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
