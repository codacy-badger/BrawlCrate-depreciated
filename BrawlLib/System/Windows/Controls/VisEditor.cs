﻿using BrawlLib.SSBB;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class VisEditor : UserControl
    {
        #region Designer

        public ListBox listBox1;
        private Button btnAll;
        private Button btnInvert;
        private Button btnToggle;
        private Button btnSet;
        private Button btnClear;
        private Panel panel1;

        private void InitializeComponent()
        {
            listBox1 = new System.Windows.Forms.ListBox();
            panel1 = new System.Windows.Forms.Panel();
            btnToggle = new System.Windows.Forms.Button();
            btnSet = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            btnInvert = new System.Windows.Forms.Button();
            btnAll = new System.Windows.Forms.Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            listBox1.FormattingEnabled = true;
            listBox1.IntegralHeight = false;
            listBox1.ItemHeight = 10;
            listBox1.Location = new System.Drawing.Point(0, 20);
            listBox1.Name = "listBox1";
            listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            listBox1.Size = new System.Drawing.Size(310, 264);
            listBox1.TabIndex = 0;
            listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(listBox1_DrawItem);
            listBox1.SelectedIndexChanged += new System.EventHandler(listBox1_SelectedIndexChanged);
            listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(listBox1_KeyDown);
            listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(listBox1_MouseDoubleClick);
            // 
            // panel1
            // 
            panel1.Controls.Add(btnToggle);
            panel1.Controls.Add(btnSet);
            panel1.Controls.Add(btnClear);
            panel1.Controls.Add(btnInvert);
            panel1.Controls.Add(btnAll);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(310, 20);
            panel1.TabIndex = 1;
            // 
            // btnToggle
            // 
            btnToggle.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            btnToggle.Location = new System.Drawing.Point(257, 0);
            btnToggle.Name = "btnToggle";
            btnToggle.Size = new System.Drawing.Size(50, 20);
            btnToggle.TabIndex = 5;
            btnToggle.Text = "&Toggle";
            btnToggle.UseVisualStyleBackColor = true;
            btnToggle.Click += new System.EventHandler(btnToggle_Click);
            // 
            // btnSet
            // 
            btnSet.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            btnSet.Location = new System.Drawing.Point(206, 0);
            btnSet.Name = "btnSet";
            btnSet.Size = new System.Drawing.Size(50, 20);
            btnSet.TabIndex = 4;
            btnSet.Text = "&Set";
            btnSet.UseVisualStyleBackColor = true;
            btnSet.Click += new System.EventHandler(btnSet_Click);
            // 
            // btnClear
            // 
            btnClear.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
            btnClear.Location = new System.Drawing.Point(155, 0);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(50, 20);
            btnClear.TabIndex = 3;
            btnClear.Text = "&Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += new System.EventHandler(btnClear_Click);
            // 
            // btnInvert
            // 
            btnInvert.Location = new System.Drawing.Point(64, 0);
            btnInvert.Name = "btnInvert";
            btnInvert.Size = new System.Drawing.Size(50, 20);
            btnInvert.TabIndex = 2;
            btnInvert.Text = "&Invert";
            btnInvert.UseVisualStyleBackColor = true;
            btnInvert.Click += new System.EventHandler(btnInvert_Click);
            // 
            // btnAll
            // 
            btnAll.Location = new System.Drawing.Point(3, 0);
            btnAll.Name = "btnAll";
            btnAll.Size = new System.Drawing.Size(60, 20);
            btnAll.TabIndex = 1;
            btnAll.Text = "Select &All";
            btnAll.UseVisualStyleBackColor = true;
            btnAll.Click += new System.EventHandler(btnAll_Click);
            // 
            // VisEditor
            // 
            Controls.Add(listBox1);
            Controls.Add(panel1);
            Name = "VisEditor";
            Size = new System.Drawing.Size(310, 284);
            panel1.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        //public VIS0Editor _mainWindow;

        public EventHandler EntryChanged;
        public EventHandler IndexChanged;

        private IBoolArraySource _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBoolArraySource TargetNode
        {
            get => _targetNode;
            set { _targetNode = value; TargetChanged(); }
        }

        public VisEditor() { InitializeComponent(); }

        private void TargetChanged()
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();

            if (_targetNode != null)
            {
                for (int i = 0; i < _targetNode.EntryCount; i++)
                {
                    listBox1.Items.Add(_targetNode.GetEntry(i));
                }
            }

            listBox1.EndUpdate();
        }

        private void Toggle()
        {
            listBox1.BeginUpdate();

            int[] indices = new int[listBox1.SelectedIndices.Count];
            listBox1.SelectedIndices.CopyTo(indices, 0);
            foreach (int i in indices)
            {
                bool val = !(bool)listBox1.Items[i];
                listBox1.Items[i] = val;
                _targetNode.SetEntry(i, val);
            }
            foreach (int i in indices)
            {
                listBox1.SelectedIndices.Add(i);
            }

            listBox1.EndUpdate();

            if (EntryChanged != null)
            {
                EntryChanged(this, null);
            }
        }
        private void Clear()
        {
            listBox1.BeginUpdate();

            int[] indices = new int[listBox1.SelectedIndices.Count];
            listBox1.SelectedIndices.CopyTo(indices, 0);
            foreach (int i in indices)
            {
                listBox1.Items[i] = false;
                _targetNode.SetEntry(i, false);
            }
            foreach (int i in indices)
            {
                listBox1.SelectedIndices.Add(i);
            }

            listBox1.EndUpdate();

            if (EntryChanged != null)
            {
                EntryChanged(this, null);
            }
        }
        private void Set()
        {
            listBox1.BeginUpdate();

            int[] indices = new int[listBox1.SelectedIndices.Count];
            listBox1.SelectedIndices.CopyTo(indices, 0);
            foreach (int i in indices)
            {
                listBox1.Items[i] = true;
                _targetNode.SetEntry(i, true);
            }
            foreach (int i in indices)
            {
                listBox1.SelectedIndices.Add(i);
            }

            listBox1.EndUpdate();

            if (EntryChanged != null)
            {
                EntryChanged(this, null);
            }
        }
        private void SelectAll()
        {
            listBox1.BeginUpdate();
            _updating = true;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.SelectedIndices.Add(i);
            }

            _updating = false;
            listBox1.EndUpdate();
        }
        private void SelectInverse()
        {
            listBox1.BeginUpdate();
            _updating = true;
            int x;
            int count = listBox1.SelectedIndices.Count;
            int[] indices = new int[count];

            listBox1.SelectedIndices.CopyTo(indices, 0);
            listBox1.SelectedIndices.Clear();

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                for (x = 0; x < count; x++)
                {
                    if (indices[x] == i)
                    {
                        break;
                    }
                }

                if (x >= count)
                {
                    listBox1.SelectedIndices.Add(i);
                }
            }
            _updating = false;
            listBox1.EndUpdate();
        }

        private static readonly Font _renderFont = new Font(FontFamily.GenericMonospace, 9.0f);
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle r = e.Bounds;
            int index = e.Index;

            g.FillRectangle(Brushes.White, r);
            if (index >= 0)
            {
                if ((e.State & DrawItemState.Selected) != 0)
                {
                    g.FillRectangle(Brushes.LightBlue, r.X, r.Y, 210, r.Height);
                }

                g.DrawString(string.Format(" [{0:d2}]", index), _renderFont, Brushes.Black, 4.0f, e.Bounds.Y - 4);

                r.X += 100;
                r.Width = 30;

                if ((bool)listBox1.Items[index])
                {
                    g.FillRectangle(Brushes.Gray, r);
                    g.DrawString("✔", new Font("", 7), Brushes.Black, r.X + 9, r.Y - 1);
                }

                g.DrawRectangle(Pens.Black, r);
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Toggle();
            }
        }
        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Toggle();
            }
        }
        private void btnAll_Click(object sender, EventArgs e) { SelectAll(); }
        private void btnInvert_Click(object sender, EventArgs e) { SelectInverse(); }
        private void btnClear_Click(object sender, EventArgs e) { Clear(); }
        private void btnSet_Click(object sender, EventArgs e) { Set(); }
        private void btnToggle_Click(object sender, EventArgs e) { Toggle(); }

        public bool _updating = false;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_updating)
            {
                return;
            }

            if (IndexChanged != null)
            {
                IndexChanged(this, null);
            }
            //if (_mainWindow != null && !_updating)
            //    _mainWindow._mainWindow.SetFrame(listBox1.SelectedIndex);
        }
    }
}
