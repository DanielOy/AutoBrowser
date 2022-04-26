using AutoBrowser.Core;
using AutoBrowser.Core.Actions;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBrowser.Forms
{
    public partial class Main : Form
    {
        #region Global Variables
        private string _projectsPath;
        #endregion

        #region Constructor
        public Main()
        {
            InitializeComponent();
            InitEnvironment();
        }
        #endregion

        #region Functions
        private void InitEnvironment()
        {
            _projectsPath = Path.Combine(Environment.CurrentDirectory, "Projects");
            Environment.CurrentDirectory = _projectsPath;
        }

        private SortableList<Project> LoadProjects()
        {
            SetStatusMessage("Loading projects");
            List<Project> projects = GetProjects();
            SetStatusMessage(string.Empty);

            return new SortableList<Project>(projects);
        }

        private List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>();
            var files = GetProjectFiles();

            if (files != null && files.Length != 0)
            {
                foreach (var file in files)
                {
                    projects.Add(new Project(file.Name));
                }
            }

            return projects;
        }

        private FileInfo[] GetProjectFiles()
        {
            var projectFolder = new DirectoryInfo(_projectsPath);

            return projectFolder.GetFiles($"*{Global.FileExtension}", SearchOption.TopDirectoryOnly);
        }

        private void SetStatusMessage(string message)
        {
            StatusLabel.Text = message;
        }
        #endregion

        #region Events
        private void ExecuteToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var projectSelected = ((Project)ProjectsDataGridView.SelectedRows[0].DataBoundItem);
                var copyActions = BaseAction.Copy(projectSelected.Actions);

                using (var frm = new Tester())
                {
                    frm.Actions = copyActions;
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Main_Shown(object sender, EventArgs e)
        {
            try
            {
                projectBindingSource.DataSource = await Task.Run(() => LoadProjects());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void AddToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var frm = new Editor())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        projectBindingSource.DataSource = await Task.Run(() => LoadProjects());
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
                var projectSelected = ((Project)ProjectsDataGridView.SelectedRows[0].DataBoundItem);

                using (var frm = new Editor())
                {
                    frm.Project = projectSelected;
                    frm.ShowDialog();
                }
                ProjectsDataGridView.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScheduletoolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var projectSelected = ((Project)ProjectsDataGridView.SelectedRows[0].DataBoundItem);

                //Scheduler scheduler = new Scheduler();
                //string fileFullPath = new FileInfo(projectSelected.GetFilePath()).FullName;
                //scheduler.AddTask(projectSelected.Name, fileFullPath, Scheduler.Frecuency.EachHour);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void DeleteToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                var projectSelected = ((Project)ProjectsDataGridView.SelectedRows[0].DataBoundItem);

                string msg = $"Are you sure want to delete the project <{projectSelected.Name}>?" +
                    $"\nThis action can not be undone.";

                if (MessageBox.Show(msg, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    System.IO.File.Delete(projectSelected.GetFilePath());

                    projectBindingSource.DataSource = await Task.Run(() => LoadProjects());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
