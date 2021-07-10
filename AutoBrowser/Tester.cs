using AutoBrowser.Actions;
using AutoBrowser.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser
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
                autoWeb.Run(Actions);
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
                autoWeb = new AutoWebBrowser(formBrowser);
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
                        btnStart.Visible = false;
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
    }
}
