using AutoBrowser.Actions;
using AutoBrowser.Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser
{
    public partial class Tester : Form
    {
        Classes.AutoWebBrowser autoWeb;

        public Tester()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                List<BaseAction> steps = new List<BaseAction>
                {

                };

                autoWeb.Run(steps);
                MessageBox.Show("Process Finished", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmTEST_Shown(object sender, EventArgs e)
        {
            autoWeb = new AutoWebBrowser(formBrowser);
        }
    }
}
