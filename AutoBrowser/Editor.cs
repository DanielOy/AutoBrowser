﻿using AutoBrowser.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AutoBrowser
{
    //TODO: Allow move steps

    public partial class Editor : Form
    {
        #region Global Variables
        private string fileName;
        private bool isEdit = false;
        private TreeNode currentNode;
        private bool? isEditNode = null;
        #endregion

        #region Properties
        public string FileName { get => fileName; set { fileName = value; isEdit = true; } }
        #endregion

        #region Constructor
        public Editor()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void LoadProject()
        {
            var actions = new Classes.Project().LoadProject(fileName);
            StepsTreeView.Nodes.AddRange(ActionsToNodes(actions));
        }

        private TreeNode[] ActionsToNodes(List<BaseAction> actions)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (var action in actions)
            {
                TreeNode node = null;
                switch (action)
                {
                    case ExtractElement e:
                        node = new TreeNode(action.GetType().Name) { Tag = action };
                        e.NodePath.ForEach(n => node.Nodes.Add(new TreeNode(n.GetType().Name) { Tag = n }));
                        break;
                    case Repeat r:
                        node = new TreeNode(action.GetType().Name) { Tag = action };
                        node.Nodes.AddRange(ActionsToNodes(r.Actions));
                        break;
                    case Conditional c:
                        node = new TreeNode(action.GetType().Name) { Tag = action };
                        node.Nodes.AddRange(ActionsToNodes(c.Actions));
                        break;
                    default:
                        node = new TreeNode(action.GetType().Name) { Tag = action };
                        break;
                }
                nodes.Add(node);
            }
            return nodes.ToArray();
        }

        private Type GetTypeByName(string action)
        {
            if (Node.GetSubtypeNames().Contains(action))
            {
                return Node.GetSubtypes().ToList().Find(x => x.Name == action);
            }
            else
            {
                return BaseAction.GetActions().ToList().Find(x => x.Name == action);
            }
        }

        private PropertyInfo GetProperty(Type type, string name)
        {
            return (type.GetProperties())
               .Where(x => x.Name == name)?
               .ToList()?[0];
        }

        private void CreateControls(Type type, object action)
        {
            PropertyInfo[] properties = type.GetProperties().Where(x => x.PropertyType != typeof(List<BaseAction>) && x.PropertyType != typeof(List<Node>)).ToArray();
            ClearFormInputs();

            int rowCount = ControlsTableLayoutPanel.RowStyles.Count;
            for (int i = rowCount - 1; i > 0; i--)
            {
                ControlsTableLayoutPanel.RowStyles.RemoveAt(i);
            }

            int tableHeight = 20;
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];

                ControlsTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));

                ControlsTableLayoutPanel.Controls.Add(new Label() { Text = prop.Name, Dock = DockStyle.Fill, Padding = new Padding(3, 7, 3, 3) }, 0, (i + 1));

                Control control;
                if (prop.PropertyType.BaseType == typeof(Enum))
                {
                    control = new ComboBox() { Name = prop.Name, Dock = DockStyle.Fill };
                    (control as ComboBox).Items.AddRange(Enum.GetNames(prop.PropertyType));
                }
                else
                {
                    control = new TextBox() { Name = prop.Name, Dock = DockStyle.Fill };

                    if (prop.PropertyType == typeof(int))
                    {
                        control.KeyPress += Txtbox_KeyPress; ;
                    }
                }

                if (action != null)
                {
                    string value = action.GetType().GetProperty(prop.Name).GetValue(action).ToString();
                    control.Text = value;
                }

                ControlsTableLayoutPanel.Controls.Add(control, 1, (i + 1));

                tableHeight += 25;
            }
            ControlsTableLayoutPanel.Height = tableHeight;
        }

        private void Txtbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void ClearFormInputs()
        {
            ControlsTableLayoutPanel.Controls.Clear();
        }

        private void TreeButtonsEnabled(bool enabled)
        {
            AddButton.Enabled = enabled;
            DeleteButton.Enabled = enabled;
            Editbutton.Enabled = enabled;

            if (enabled) { Text = "Editor"; }
        }

        private void FormButtonsEnabled(bool enabled)
        {
            SaveButton.Enabled = enabled;
            ActionsComboBox.Enabled = enabled;
            CancelButton.Enabled = enabled;
        }

        private TreeNode FormToNode()
        {
            string actionName = ActionsComboBox.SelectedItem.ToString();
            Type type = GetTypeByName(actionName);
            object instance = Activator.CreateInstance(type);

            foreach (Control control in ControlsTableLayoutPanel.Controls)
            {
                if (control is TextBox textBox)
                {
                    var prop = GetProperty(type, textBox.Name);
                    if (prop == null)
                    {
                        continue;
                    }
                    else if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(object))
                    {
                        prop.SetValue(instance, textBox.Text);
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        prop.SetValue(instance, Convert.ToInt32(textBox.Text.Trim()));
                    }
                }
                else if (control is ComboBox combo)
                {
                    var prop = GetProperty(type, combo.Name);
                    if (prop == null)
                    {
                        continue;
                    }
                    else if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(object))
                    {
                        prop.SetValue(instance, combo.SelectedItem.ToString());
                    }
                }
            }

            TreeNode node = new TreeNode(actionName) { Tag = instance };
            return node;
        }

        private bool FindNode(TreeNode rootNode, TreeNode searchNode, TreeNode replacedWith)
        {
            int index = rootNode.Nodes.IndexOf(searchNode);

            if (index == -1)
            {
                foreach (TreeNode childNode in rootNode.Nodes)
                {
                    bool found = FindNode(childNode, searchNode, replacedWith);
                    if (found) { return found; }
                }
            }
            else
            {
                rootNode.Nodes[index].Tag = replacedWith.Tag;
                return true;
            }
            return false;
        }

        private bool FindNode(TreeNodeCollection rootNode, TreeNode searchNode, TreeNode replacedWith)
        {
            foreach (TreeNode childNode in rootNode)
            {
                bool found = FindNode(childNode, searchNode, replacedWith);
                if (found)
                {
                    return found;
                }
            }
            return false;
        }

        private List<BaseAction> NodesToActions(TreeNodeCollection nodes)
        {
            List<BaseAction> actions = new List<BaseAction>();
            foreach (TreeNode node in nodes)
            {
                var action = node.Tag;
                if (action is Repeat r)
                {
                    r.Actions = NodesToActions(node.Nodes);
                    actions.Add(r);
                }
                else if (action is Conditional c)
                {
                    c.Actions = NodesToActions(node.Nodes);
                    actions.Add(c);
                }
                else
                {
                    actions.Add((BaseAction)node.Tag);
                }
            }
            return actions;
        }
        #endregion

        #region Events
        private void Editor_Shown(object sender, EventArgs e)
        {
            try
            {
                FormButtonsEnabled(false);
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = "Temp" + DateTime.Now.ToString("yyMMdd");
                }
                else
                {
                    LoadProject();
                    StepsTreeView.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                TreeButtonsEnabled(false);
                FormButtonsEnabled(true);
                ClearFormInputs();

                currentNode = StepsTreeView.SelectedNode;
                Text = $"Editor [{currentNode?.FullPath?.Replace("\\", " -> ")}] -> NewAction";
                isEditNode = false;

                InitActionsComboItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitActionsComboItems()
        {
            ActionsComboBox.Items.Clear();
            if ((currentNode?.Tag is ExtractElement && !(isEditNode ?? false))|| currentNode?.Tag is Node)
            {
                ActionsComboBox.Items.AddRange(Node.GetSubtypeNames());
            }
            else
            {
                ActionsComboBox.Items.AddRange(BaseAction.GetActionNames());
            }
        }

        private void Editbutton_Click(object sender, EventArgs e)
        {
            try
            {
                FormButtonsEnabled(true);
                TreeButtonsEnabled(false);
                isEditNode = true;

                currentNode = StepsTreeView.SelectedNode;
                string action = StepsTreeView.SelectedNode?.Text;

                if (!string.IsNullOrEmpty(action))
                {
                    InitActionsComboItems();
                    ActionsComboBox.Enabled = false;
                    ActionsComboBox.SelectedItem = action;

                    Text = $"Editor [{currentNode.FullPath.Replace("\\", " -> ")}]";

                    Type type = GetTypeByName(action);
                    if (type == null) { type = Node.GetSubtypes().ToList().Find(x => x.Name == action); }
                    CreateControls(type, StepsTreeView.SelectedNode.Tag);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            StepsTreeView.SelectedNode?.Remove();
        }

        private void ActionsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string action = ActionsComboBox.SelectedItem.ToString();

                if (!string.IsNullOrEmpty(action))
                {
                    Type type = GetTypeByName(action);
                    CreateControls(type, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = FormToNode();

                if (isEditNode == true)
                {
                    bool found = FindNode(StepsTreeView.Nodes, currentNode, node);
                }
                else if (isEditNode == false)
                {
                    if (currentNode == null)
                    {
                        StepsTreeView.Nodes.Add(node);
                    }
                    else if (currentNode.Tag.GetType() == typeof(Repeat) || StepsTreeView.SelectedNode.Tag.GetType() == typeof(Conditional))
                    {
                        currentNode.Nodes.Add(node);
                    }
                    else if (currentNode.Tag.GetType() == typeof(ExtractElement))
                    {
                        currentNode.Nodes.Add(node);
                    }
                    else
                    {
                        if (currentNode.Parent == null)
                        {
                            StepsTreeView.Nodes.Add(node);
                        }
                        else
                        {
                            currentNode.Parent.Nodes.Add(node);
                        }
                    }
                }

                TreeButtonsEnabled(true);
                FormButtonsEnabled(false);
                ActionsComboBox.Text = "";
                ClearFormInputs();
                isEditNode = null;
                currentNode = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            currentNode = null;
            ClearFormInputs();
            TreeButtonsEnabled(true);
            FormButtonsEnabled(false);
            ActionsComboBox.Text = "";
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (StepsTreeView.Nodes.Count == 0)
                {
                    MessageBox.Show("Please add at least one action before save.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                if (!isEdit)
                {
                    var result = Library.Forms.InputBox.Show("Please insert the name of the project to save", "Project Name", out string projectName);
                    if (result != DialogResult.OK)
                    {
                        return;
                    }
                    fileName = projectName.Replace(" ", "_") + Global.FileExtension;
                }

                List<BaseAction> actions = NodesToActions(StepsTreeView.Nodes);

                (new Classes.Project()).SaveProject(actions, fileName);
                MessageBox.Show("Project saved successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TestToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                List<BaseAction> originalActions = NodesToActions(StepsTreeView.Nodes);

                var Project = new Classes.Project();
                string tempFile = "TempFile.aweb";

                if (File.Exists(tempFile)) { File.Delete(tempFile); }

                Project.SaveProject(originalActions, tempFile);
                List<BaseAction> actions = Project.LoadProject(tempFile);
                actions.ForEach(x => x.InitVariables());

                if (File.Exists(tempFile)) { File.Delete(tempFile); }

                using (var frm = new Tester())
                {
                    frm.Actions = actions;
                    frm.IsAuto = false;
                    frm.ShowDialog();
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
