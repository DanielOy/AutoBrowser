namespace AutoBrowser.Forms
{
    partial class Tester
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tester));
            this.MaintableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnStart = new System.Windows.Forms.Button();
            this.MaintableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaintableLayout
            // 
            this.MaintableLayout.ColumnCount = 1;
            this.MaintableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MaintableLayout.Controls.Add(this.btnStart, 0, 0);
            this.MaintableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaintableLayout.Location = new System.Drawing.Point(0, 0);
            this.MaintableLayout.Name = "MaintableLayout";
            this.MaintableLayout.RowCount = 2;
            this.MaintableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.322654F));
            this.MaintableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.67735F));
            this.MaintableLayout.Size = new System.Drawing.Size(689, 437);
            this.MaintableLayout.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStart.Location = new System.Drawing.Point(3, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // Tester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 437);
            this.Controls.Add(this.MaintableLayout);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Tester";
            this.Text = "Tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tester_FormClosing);
            this.Shown += new System.EventHandler(this.FrmTEST_Shown);
            this.MaintableLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MaintableLayout;
        private System.Windows.Forms.Button btnStart;
    }
}

