﻿using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace AutoBrowser
{
    public partial class Main : Form
    {
        #region Constructor
        public Main()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void LoadProjects()
        {
            var files = new DirectoryInfo(Environment.CurrentDirectory).GetFiles($"*{Global.FileExtension}", SearchOption.TopDirectoryOnly);
            var dt = GetTableStructure();

            if (files != null && files.Length != 0)
            {
                foreach (var file in files)
                {
                    dt.Rows.Add(
                        file.Name.Replace(Global.FileExtension, ""),
                        file.CreationTime.ToString(),
                        file.LastWriteTime.ToString(),
                        Library.TextFormat.NoBytesToSize(file.Length));
                }
            }

            ProjectsDataGridView.DataSource = dt;
        }
        #endregion

        #region Functions
        private DataTable GetTableStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Project");
            dt.Columns.Add("Creation Date");
            dt.Columns.Add("Last Modify Date");
            dt.Columns.Add("Size");

            return dt;
        }
        #endregion

        #region Events
        private void ExecuteToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = ((DataRowView)ProjectsDataGridView.SelectedRows[0].DataBoundItem).Row;
                string fileName = selected["Project"].ToString();

                using (var frm = new Tester())
                {
                    frm.FileName = fileName + Global.FileExtension;
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            try
            {
                LoadProjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}