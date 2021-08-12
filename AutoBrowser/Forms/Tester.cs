using AutoBrowser.Core;
using AutoBrowser.Core.Actions;
using Microsoft.Web.WebView2.WinForms;
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
                var formView = new WebView2();
                MaintableLayout.Controls.Add(formView);
                formView.Dock = DockStyle.Fill;

                autoWeb = new AutoWebBrowser(formView);

                autoWeb.ProgressChanged += (s, ev) => this.Text = ev.Description;
                autoWeb.ProcessFinished += (s, ev) =>
                {
                    if (IsAuto) { Environment.Exit(0); }
                    MessageBox.Show("Process Finished", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                if (!string.IsNullOrEmpty(FileName))
                {
                    this.Text = FileName.Replace(Global.FileExtension, "");
                    Actions = new Project().LoadProject(FileName);

                    if (IsAuto)
                    {
                        btnStart.Enabled = false;
                        autoWeb.Run(Actions);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void Tester_FormClosing(object sender, FormClosingEventArgs e)
        {
            autoWeb.Stop();
        }
    }
}
