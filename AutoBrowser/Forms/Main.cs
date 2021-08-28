using AutoBrowser.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace AutoBrowser.Forms
{
    public partial class Main : Form
    {
        #region Global Variables
        private string projectPath;
        #endregion

        #region Constructor
        public Main()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void LoadProjects()
        {
            if (string.IsNullOrEmpty(projectPath))
            {
                projectPath = Path.Combine(Environment.CurrentDirectory, "Projects");
                Environment.CurrentDirectory = projectPath;
            }

            var files = new DirectoryInfo(projectPath).GetFiles($"*{Global.FileExtension}", SearchOption.TopDirectoryOnly);

            List<Project> projects = new List<Project>();

            if (files != null && files.Length != 0)
            {
                foreach (var file in files)
                {
                    projects.Add(new Project(file.Name));
                }
            }

            projectBindingSource.DataSource = projects;
        }
        #endregion

        #region Functions

        #endregion

        #region Events
        private void ExecuteToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = ((Project)ProjectsDataGridView.SelectedRows[0].DataBoundItem);
                
                using (var frm = new Tester())
                {
                    frm.Actions = selected.Actions;
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

        private void AddToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new Editor())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadProjects();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = ((Project)ProjectsDataGridView.SelectedRows[0].DataBoundItem);

                using (var frm = new Editor())
                {
                    frm.Project = selected;
                    frm.ShowDialog();
                }
                ProjectsDataGridView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
