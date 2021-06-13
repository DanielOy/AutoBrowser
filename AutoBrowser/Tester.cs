using AutoBrowser.Actions;
using AutoBrowser.Classes;
using AutoBrowser.Enums;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser
{
    public partial class Tester : Form
    {
        #region Global Variables
        private AutoWebBrowser autoWeb;
        private bool IsAuto = false;
        #endregion

        #region Properties
        public string FileName { get; set; }
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
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmTEST_Shown(object sender, EventArgs e)
        {
            IsAuto = !string.IsNullOrEmpty(FileName);

            autoWeb = new AutoWebBrowser(formBrowser);
            autoWeb.ProgressChanged += (s, ev) => this.Text = ev.Description;
            autoWeb.ProcessFinished += (s, ev) =>
            {
                if (IsAuto) { Environment.Exit(0); } 
                #endregion
                MessageBox.Show("Process Finished", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            if (IsAuto)
            {
                btnStart.Visible = false;
                List<BaseAction> actions = new Project().LoadProject(FileName);
                autoWeb.Run(actions);
            }
        }
    }
}
