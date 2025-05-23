using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TechTrack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadAllData();
        }

        private void LoadAllData()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            dataGridView1.DataSource = LoadData(Path.Combine(basePath, "computers.json"));
            dataGridView2.DataSource = LoadData(Path.Combine(basePath, "mice.json"));
            dataGridView3.DataSource = LoadData(Path.Combine(basePath, "keyboards.json"));
            dataGridView4.DataSource = LoadData(Path.Combine(basePath, "printers.json"));
        }


        private List<Equipment> LoadData(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.WriteAllText(path, "[]");
                    return new List<Equipment>();
                }

                string json = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<List<Equipment>>(json);
                return data ?? new List<Equipment>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных из {path}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Equipment>();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            AddEqipment addForm = new AddEqipment();
            addForm.FormClosed += (s, args) => LoadAllData();
            addForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateEqipment updateForm = new updateEqipment();
            updateForm.FormClosed += (s, args) => LoadAllData();
            updateForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
          this.Close();
        }

        private void RenameColumns(DataGridView dgv, Dictionary<string, string> columnNames)
        {
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (columnNames.ContainsKey(column.Name))
                {
                    column.HeaderText = columnNames[column.Name];
                }
            }
        }

        private void RenameAllColumns(Dictionary<string, string> columnNames)
        {
            foreach (var dgv in new[] { dataGridView1, dataGridView2, dataGridView3, dataGridView4 })
            {
                RenameColumns(dgv, columnNames);
            }
        }





        void StylizeDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;

            dgv.RowsDefaultCellStyle.BackColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            dgv.DefaultCellStyle.ForeColor = Color.Black;

            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;

            dgv.RowTemplate.Height = 35;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgv.BorderStyle = BorderStyle.None;

            dgv.ReadOnly = true;

            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            foreach (var dgv in new[] { dataGridView1, dataGridView2, dataGridView3, dataGridView4 })
            {
                StylizeDataGridView(dgv);
            }

            var columnNames = new Dictionary<string, string>
    {
        { "Name", "Наименование" },
        { "Model", "Модель" },
        { "SerialNumber", "Серийный номер" },
        { "Location", "Расположение" },
        { "Status", "Состояние" }
    };

            RenameAllColumns(columnNames);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RemoveEqipment RemoveForm = new RemoveEqipment();
            RemoveForm.FormClosed += (s, args) => LoadAllData();
            RemoveForm.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
