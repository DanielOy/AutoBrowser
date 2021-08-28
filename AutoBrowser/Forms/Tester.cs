using AutoBrowser.Core;
using AutoBrowser.Core.Actions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Forms
{
    public partial class Tester : Form
    {
        #region Global Variables
        private AutoWebBrowser autoWeb;
        #endregion

        #region Properties
        public string FileName { get; set; }
        public bool IsAuto { get; set; }
        public List<BaseAction> Actions { get; set; }
        public Project.Browsers Browser { get; set; } = Project.Browsers.WebBrowser;
        public bool ActiveScripts { get; set; }
        #endregion

        #region Constructor
        public Tester()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                autoWeb.Run(Actions);
                btnStart.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmTEST_Shown(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(FileName))
                {
                    var project = new Project(FileName);
                    this.Text = project.Name;
                    Actions = project.Actions;
                    Browser = project.Browser;
                    ActiveScripts = project.ActiveScripts;
                }

                if (Browser == Project.Browsers.WebView)
                {
                    var formView = new Microsoft.Web.WebView2.WinForms.WebView2();
                    MaintableLayout.Controls.Add(formView);
                    formView.Dock = DockStyle.Fill;
                    autoWeb = new AutoWebBrowser(formView, ActiveScripts);
                }
                else
                {
                    var formView = new WebBrowser();
                    MaintableLayout.Controls.Add(formView);
                    formView.Dock = DockStyle.Fill;
                    autoWeb = new AutoWebBrowser(formView);
                }

                autoWeb.ProgressChanged += (s, ev) => this.Text = ev.Description;
                autoWeb.ProcessFinished += (s, ev) =>
                {
                    if (IsAuto) { Environment.Exit(0); }
                    MessageBox.Show("Process Finished", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                if (!string.IsNullOrEmpty(FileName) && IsAuto)
                {
                    btnStart.Enabled = false;
                    autoWeb.Run(Actions);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Tester_FormClosing(object sender, FormClosingEventArgs e)
        {
            autoWeb.Stop();
        }
        #endregion
    }
}
