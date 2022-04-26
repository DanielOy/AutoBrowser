namespace AutoBrowser.Forms
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.ExecuteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.AddToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.EditToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ScheduletoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ProjectsDataGridView = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.browserDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.activeScriptsDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.projectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DeleteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            this.MainToolStrip.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.MainStatusStrip, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.MainToolStrip, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(524, 341);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 319);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Size = new System.Drawing.Size(524, 22);
            this.MainStatusStrip.TabIndex = 0;
            this.MainStatusStrip.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExecuteToolStripButton,
            this.AddToolStripButton,
            this.EditToolStripButton,
            this.DeleteToolStripButton,
            this.ScheduletoolStripButton});
            this.MainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Size = new System.Drawing.Size(524, 25);
            this.MainToolStrip.TabIndex = 1;
            this.MainToolStrip.Text = "toolStrip1";
            // 
            // ExecuteToolStripButton
            // 
            this.ExecuteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ExecuteToolStripButton.Image")));
            this.ExecuteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ExecuteToolStripButton.Name = "ExecuteToolStripButton";
            this.ExecuteToolStripButton.Size = new System.Drawing.Size(68, 22);
            this.ExecuteToolStripButton.Text = "Execute";
            this.ExecuteToolStripButton.Click += new System.EventHandler(this.ExecuteToolStripButton_Click);
            // 
            // AddToolStripButton
            // 
            this.AddToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("AddToolStripButton.Image")));
            this.AddToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddToolStripButton.Name = "AddToolStripButton";
            this.AddToolStripButton.Size = new System.Drawing.Size(49, 22);
            this.AddToolStripButton.Text = "Add";
            this.AddToolStripButton.Click += new System.EventHandler(this.AddToolStripButton_Click);
            // 
            // EditToolStripButton
            // 
            this.EditToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("EditToolStripButton.Image")));
            this.EditToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EditToolStripButton.Name = "EditToolStripButton";
            this.EditToolStripButton.Size = new System.Drawing.Size(47, 22);
            this.EditToolStripButton.Text = "Edit";
            this.EditToolStripButton.Click += new System.EventHandler(this.EditToolStripButton_Click);
            // 
            // ScheduletoolStripButton
            // 
            this.ScheduletoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ScheduletoolStripButton.Image")));
            this.ScheduletoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ScheduletoolStripButton.Name = "ScheduletoolStripButton";
            this.ScheduletoolStripButton.Size = new System.Drawing.Size(75, 22);
            this.ScheduletoolStripButton.Text = "Schedule";
            this.ScheduletoolStripButton.Click += new System.EventHandler(this.ScheduletoolStripButton_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ProjectsDataGridView, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(518, 285);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // ProjectsDataGridView
            // 
            this.ProjectsDataGridView.AllowUserToAddRows = false;
            this.ProjectsDataGridView.AllowUserToDeleteRows = false;
            this.ProjectsDataGridView.AllowUserToOrderColumns = true;
            this.ProjectsDataGridView.AutoGenerateColumns = false;
            this.ProjectsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ProjectsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProjectsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.browserDataGridViewTextBoxColumn,
            this.activeScriptsDataGridViewCheckBoxColumn});
            this.ProjectsDataGridView.DataSource = this.projectBindingSource;
            this.ProjectsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectsDataGridView.Location = new System.Drawing.Point(3, 3);
            this.ProjectsDataGridView.Name = "ProjectsDataGridView";
            this.ProjectsDataGridView.ReadOnly = true;
            this.ProjectsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProjectsDataGridView.Size = new System.Drawing.Size(512, 279);
            this.ProjectsDataGridView.TabIndex = 0;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // browserDataGridViewTextBoxColumn
            // 
            this.browserDataGridViewTextBoxColumn.DataPropertyName = "Browser";
            this.browserDataGridViewTextBoxColumn.HeaderText = "Browser";
            this.browserDataGridViewTextBoxColumn.Name = "browserDataGridViewTextBoxColumn";
            this.browserDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // activeScriptsDataGridViewCheckBoxColumn
            // 
            this.activeScriptsDataGridViewCheckBoxColumn.DataPropertyName = "ActiveScripts";
            this.activeScriptsDataGridViewCheckBoxColumn.HeaderText = "ActiveScripts";
            this.activeScriptsDataGridViewCheckBoxColumn.Name = "activeScriptsDataGridViewCheckBoxColumn";
            this.activeScriptsDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // projectBindingSource
            // 
            this.projectBindingSource.DataSource = typeof(AutoBrowser.Core.Project);
            this.projectBindingSource.Sort = "Name";
            // 
            // DeleteToolStripButton
            // 
            this.DeleteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteToolStripButton.Image")));
            this.DeleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DeleteToolStripButton.Name = "DeleteToolStripButton";
            this.DeleteToolStripButton.Size = new System.Drawing.Size(60, 22);
            this.DeleteToolStripButton.Text = "Delete";
            this.DeleteToolStripButton.Click += new System.EventHandler(this.DeleteToolStripButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 341);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoBrowser";
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ProjectsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.ToolStripButton ExecuteToolStripButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView ProjectsDataGridView;
        private System.Windows.Forms.ToolStripButton AddToolStripButton;
        private System.Windows.Forms.ToolStripButton EditToolStripButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn browserDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn activeScriptsDataGridViewCheckBoxColumn;
        private System.Windows.Forms.BindingSource projectBindingSource;
        private System.Windows.Forms.ToolStripButton ScheduletoolStripButton;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripButton DeleteToolStripButton;
    }
}