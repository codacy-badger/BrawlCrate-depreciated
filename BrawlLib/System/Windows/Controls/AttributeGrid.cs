using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using System.Data;
using BrawlLib.Imaging;
using System.Drawing;

namespace System.Windows.Forms
{
    public unsafe class AttributeGrid : UserControl
    {
        #region Designer

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtgrdAttributes = new System.Windows.Forms.DataGridView();
            this.description = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnInf = new System.Windows.Forms.Button();
            this.btnMinusInf = new System.Windows.Forms.Button();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblCNoA = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rdoFloat = new System.Windows.Forms.RadioButton();
            this.rdoInt = new System.Windows.Forms.RadioButton();
            this.rdoColor = new System.Windows.Forms.RadioButton();
            this.rdoDegrees = new System.Windows.Forms.RadioButton();
            this.rdoUnknown = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdAttributes)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtgrdAttributes
            // 
            this.dtgrdAttributes.AllowUserToAddRows = false;
            this.dtgrdAttributes.AllowUserToDeleteRows = false;
            this.dtgrdAttributes.AllowUserToResizeRows = false;
            this.dtgrdAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgrdAttributes.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dtgrdAttributes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dtgrdAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgrdAttributes.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Format = "N4";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dtgrdAttributes.DefaultCellStyle = dataGridViewCellStyle1;
            this.dtgrdAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgrdAttributes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dtgrdAttributes.EnableHeadersVisualStyles = false;
            this.dtgrdAttributes.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dtgrdAttributes.Location = new System.Drawing.Point(0, 0);
            this.dtgrdAttributes.MultiSelect = false;
            this.dtgrdAttributes.Name = "dtgrdAttributes";
            this.dtgrdAttributes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dtgrdAttributes.RowHeadersWidth = 8;
            this.dtgrdAttributes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dtgrdAttributes.RowTemplate.Height = 16;
            this.dtgrdAttributes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dtgrdAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtgrdAttributes.Size = new System.Drawing.Size(479, 200);
            this.dtgrdAttributes.TabIndex = 5;
            this.dtgrdAttributes.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtgrdAttributes_CellEndEdit);
            this.dtgrdAttributes.CurrentCellChanged += new System.EventHandler(this.dtgrdAttributes_CurrentCellChanged);
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.BackColor = System.Drawing.SystemColors.Control;
            this.description.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.description.Cursor = System.Windows.Forms.Cursors.Default;
            this.description.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.ForeColor = System.Drawing.Color.Black;
            this.description.Location = new System.Drawing.Point(0, 0);
            this.description.Name = "description";
            this.description.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.description.Size = new System.Drawing.Size(479, 74);
            this.description.TabIndex = 6;
            this.description.Text = "No Description Available.";
            this.description.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.description_LinkClicked);
            this.description.TextChanged += new System.EventHandler(this.description_TextChanged);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 200);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(479, 3);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnInf);
            this.panel1.Controls.Add(this.btnMinusInf);
            this.panel1.Controls.Add(this.lblColor);
            this.panel1.Controls.Add(this.lblCNoA);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.description);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 203);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(479, 102);
            this.panel1.TabIndex = 8;
            // 
            // btnInf
            // 
            this.btnInf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInf.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInf.Location = new System.Drawing.Point(446, 44);
            this.btnInf.Name = "btnInf";
            this.btnInf.Size = new System.Drawing.Size(30, 30);
            this.btnInf.TabIndex = 13;
            this.btnInf.Text = "∞";
            this.btnInf.UseVisualStyleBackColor = true;
            this.btnInf.Visible = false;
            this.btnInf.Click += new System.EventHandler(this.btnInf_Click);
            // 
            // btnMinusInf
            // 
            this.btnMinusInf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinusInf.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinusInf.Location = new System.Drawing.Point(412, 44);
            this.btnMinusInf.Name = "btnMinusInf";
            this.btnMinusInf.Size = new System.Drawing.Size(30, 30);
            this.btnMinusInf.TabIndex = 12;
            this.btnMinusInf.Text = "-∞";
            this.btnMinusInf.UseVisualStyleBackColor = true;
            this.btnMinusInf.Visible = false;
            this.btnMinusInf.Click += new System.EventHandler(this.btnMinusInf_Click);
            // 
            // lblColor
            // 
            this.lblColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblColor.Location = new System.Drawing.Point(394, 60);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(41, 14);
            this.lblColor.TabIndex = 10;
            this.lblColor.Visible = false;
            this.lblColor.Click += new System.EventHandler(this.lblColor_Click);
            // 
            // lblCNoA
            // 
            this.lblCNoA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCNoA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCNoA.Location = new System.Drawing.Point(434, 60);
            this.lblCNoA.Name = "lblCNoA";
            this.lblCNoA.Size = new System.Drawing.Size(41, 14);
            this.lblCNoA.TabIndex = 11;
            this.lblCNoA.Visible = false;
            this.lblCNoA.Click += new System.EventHandler(this.lblColor_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.rdoFloat, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.rdoInt, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rdoColor, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.rdoDegrees, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.rdoUnknown, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 77);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(479, 25);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // rdoFloat
            // 
            this.rdoFloat.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoFloat.AutoSize = true;
            this.rdoFloat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoFloat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoFloat.Location = new System.Drawing.Point(0, 0);
            this.rdoFloat.Margin = new System.Windows.Forms.Padding(0);
            this.rdoFloat.Name = "rdoFloat";
            this.rdoFloat.Size = new System.Drawing.Size(95, 25);
            this.rdoFloat.TabIndex = 0;
            this.rdoFloat.TabStop = true;
            this.rdoFloat.Text = "Float";
            this.rdoFloat.UseVisualStyleBackColor = true;
            this.rdoFloat.CheckedChanged += new System.EventHandler(this.radioButtonsChanged);
            // 
            // rdoInt
            // 
            this.rdoInt.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoInt.AutoSize = true;
            this.rdoInt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoInt.Location = new System.Drawing.Point(95, 0);
            this.rdoInt.Margin = new System.Windows.Forms.Padding(0);
            this.rdoInt.Name = "rdoInt";
            this.rdoInt.Size = new System.Drawing.Size(95, 25);
            this.rdoInt.TabIndex = 1;
            this.rdoInt.TabStop = true;
            this.rdoInt.Text = "Integer";
            this.rdoInt.UseVisualStyleBackColor = true;
            this.rdoInt.CheckedChanged += new System.EventHandler(this.radioButtonsChanged);
            // 
            // rdoColor
            // 
            this.rdoColor.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoColor.AutoSize = true;
            this.rdoColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoColor.Location = new System.Drawing.Point(190, 0);
            this.rdoColor.Margin = new System.Windows.Forms.Padding(0);
            this.rdoColor.Name = "rdoColor";
            this.rdoColor.Size = new System.Drawing.Size(95, 25);
            this.rdoColor.TabIndex = 1;
            this.rdoColor.TabStop = true;
            this.rdoColor.Text = "Color";
            this.rdoColor.UseVisualStyleBackColor = true;
            this.rdoColor.CheckedChanged += new System.EventHandler(this.radioButtonsChanged);
            // 
            // rdoDegrees
            // 
            this.rdoDegrees.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoDegrees.AutoSize = true;
            this.rdoDegrees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoDegrees.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDegrees.Location = new System.Drawing.Point(285, 0);
            this.rdoDegrees.Margin = new System.Windows.Forms.Padding(0);
            this.rdoDegrees.Name = "rdoDegrees";
            this.rdoDegrees.Size = new System.Drawing.Size(95, 25);
            this.rdoDegrees.TabIndex = 2;
            this.rdoDegrees.TabStop = true;
            this.rdoDegrees.Text = "Degrees";
            this.rdoDegrees.UseVisualStyleBackColor = true;
            this.rdoDegrees.CheckedChanged += new System.EventHandler(this.radioButtonsChanged);
            // 
            // rdoUnknown
            // 
            this.rdoUnknown.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoUnknown.AutoSize = true;
            this.rdoUnknown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoUnknown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoUnknown.Location = new System.Drawing.Point(380, 0);
            this.rdoUnknown.Margin = new System.Windows.Forms.Padding(0);
            this.rdoUnknown.Name = "rdoUnknown";
            this.rdoUnknown.Size = new System.Drawing.Size(99, 25);
            this.rdoUnknown.TabIndex = 1;
            this.rdoUnknown.TabStop = true;
            this.rdoUnknown.Text = "Hexidecimal";
            this.rdoUnknown.UseVisualStyleBackColor = true;
            this.rdoUnknown.CheckedChanged += new System.EventHandler(this.radioButtonsChanged);
            // 
            // AttributeGrid
            // 
            this.Controls.Add(this.dtgrdAttributes);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "AttributeGrid";
            this.Size = new System.Drawing.Size(479, 305);
            ((System.ComponentModel.ISupportInitialize)(this.dtgrdAttributes)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public AttributeInfo[] AttributeArray { get; set; }

        public AttributeGrid()
        {
            InitializeComponent();
            _dlgColor = new GoodColorDialog();
        }

        private DataGridView dtgrdAttributes;
        public RichTextBox description;
        private Splitter splitter1;
        public bool called = false;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private RadioButton rdoFloat;
        private RadioButton rdoInt;
        private RadioButton rdoColor;
        private RadioButton rdoDegrees;
        private RadioButton rdoUnknown;
        private Label lblColor;
        private Label lblCNoA;
        private Button btnInf;
        private Button btnMinusInf;

        public event EventHandler CellEdited;
        public event EventHandler DictionaryChanged;

        private IAttributeList _targetNode;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IAttributeList TargetNode
        {
            get { return _targetNode; }
            set { _targetNode = value; if (!called && value != null) { LoadData(); called = true; } TargetChanged(); }
        }

        public void LoadData()
        {
            attributes.Columns.Clear();
            attributes.Rows.Clear();

            //Setup Attribute Table.
            attributes.Columns.Add("Name");
            attributes.Columns.Add("Value");
            //attributes.Columns[0].ReadOnly = true;
            dtgrdAttributes.DataSource = attributes;
        }

        DataTable attributes = new DataTable();
        public unsafe void TargetChanged()
        {
            lblColor.Visible = false;
            lblCNoA.Visible = false;
            btnInf.Visible = btnMinusInf.Visible = false;
            rdoColor.Enabled = rdoDegrees.Enabled = rdoFloat.Enabled = rdoInt.Enabled = rdoUnknown.Enabled = true;

            if (TargetNode == null)
                return;

            attributes.Rows.Clear();
            for (int i = 0; i < TargetNode.NumEntries; i++) {
                if (i < AttributeArray.Length)
                    attributes.Rows.Add(AttributeArray[i]._name);
                else
                    attributes.Rows.Add("0x" + (i * 4).ToString("X"));
            }

            //Add attributes to the attribute table.
            for (int i = 0; i < TargetNode.NumEntries; i++)
                RefreshRow(i);

            if(AttributeArray.Length <= 0)
                rdoColor.Enabled = rdoDegrees.Enabled = rdoFloat.Enabled = rdoInt.Enabled = rdoUnknown.Enabled = false;
        }

        private void RefreshRow(int i)
        {
            if (AttributeArray.Length <= i || AttributeArray[i]._type == 2)
                attributes.Rows[i][1] = (float)((bfloat*)TargetNode.AttributeAddress)[i] * Maths._rad2degf;
            else if (AttributeArray[i]._type == 1)
                attributes.Rows[i][1] = (int)((bint*)TargetNode.AttributeAddress)[i];
            else if (AttributeArray[i]._type == 3)
            {
                attributes.Rows[i][1] = ((RGBAPixel*)TargetNode.AttributeAddress)[i];
                lblColor.BackColor = (Color)(((RGBAPixel*)TargetNode.AttributeAddress)[i]);
                lblCNoA.BackColor = Color.FromArgb((((RGBAPixel*)TargetNode.AttributeAddress)[i]).R, (((RGBAPixel*)TargetNode.AttributeAddress)[i]).G, (((RGBAPixel*)TargetNode.AttributeAddress)[i]).B);
            }
            else if (AttributeArray[i]._type == 4)
                attributes.Rows[i][1] = "0x" + ((int)((bint*)TargetNode.AttributeAddress)[i]).ToString("X8");
            else
                attributes.Rows[i][1] = (float)((bfloat*)TargetNode.AttributeAddress)[i];
        }

        public void SetFloat(int index, float value) { TargetNode.SetFloat(index, value); }
        public float GetFloat(int index) { return TargetNode.GetFloat(index); }
        public void SetInt(int index, int value) { TargetNode.SetInt(index, value); }
        public int GetInt(int index) { return TargetNode.GetInt(index); }
        public void SetRGBAPixel(int index, string value) { TargetNode.SetRGBAPixel(index, value); }
        public RGBAPixel GetRGBAPixel(int index) { return TargetNode.GetRGBAPixel(index); }
        public void SetHex(int index, string value) { TargetNode.SetHex(index, value); }
        public String GetHex(int index) { return TargetNode.GetHex(index); }

        private unsafe void dtgrdAttributes_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgrdAttributes.CurrentCell == null) return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;
            string value = attributes.Rows[index][1].ToString();

            string name = attributes.Rows[index][0].ToString();
            if (AttributeArray[index]._name != name)
            {
                AttributeArray[index]._name = name;
                if (DictionaryChanged != null) DictionaryChanged.Invoke(this, EventArgs.Empty);
                return;
            }

            byte* buffer = (byte*)TargetNode.AttributeAddress;
            lblColor.Visible = false;
            lblCNoA.Visible = false;
            btnInf.Visible = btnMinusInf.Visible = false;

            if (AttributeArray[index]._type == 4) // Hex
            {
                string field0 = (value.ToString() ?? "").Split(' ')[0];
                int fromBase = field0.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) ? 16 : 10;
                int temp = Convert.ToInt32(field0, fromBase);
                if (((bint*)buffer)[index] != temp)
                {
                    ((bint*)buffer)[index] = temp;
                    TargetNode.SignalPropertyChange();
                }
                if(fromBase == 10)
                {
                    AttributeArray[index]._type = 1;
                }
            }
            else if (AttributeArray[index]._type == 3) // color
            {
                lblColor.Visible = true;
                lblCNoA.Visible = true;
                RGBAPixel p = new RGBAPixel();

                string s = value.ToString();
                char[] delims = new char[] { ',', 'R', 'G', 'B', 'A', ':', ' ' };
                string[] arr = s.Split(delims, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length == 4)
                {
                    byte.TryParse(arr[0], out p.R);
                    byte.TryParse(arr[1], out p.G);
                    byte.TryParse(arr[2], out p.B);
                    byte.TryParse(arr[3], out p.A);

                    if (((RGBAPixel*)buffer)[index] != p)
                    {
                        ((RGBAPixel*)buffer)[index] = p;
                        TargetNode.SignalPropertyChange();
                    }
                    lblColor.BackColor = (Color)p;
                    lblCNoA.BackColor = Color.FromArgb(p.R, p.G, p.B);
                }
            }
            else if (AttributeArray[index]._type == 2) //degrees
            {
                float val;
                if (!float.TryParse(value, out val))
                    value = ((float)(((bfloat*)buffer)[index])).ToString();
                else
                {
                    if (((bfloat*)buffer)[index] != val * Maths._deg2radf)
                    {
                        ((bfloat*)buffer)[index] = val * Maths._deg2radf;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }
            else if (AttributeArray[index]._type == 1) //int
            {
                int val;
                if (!int.TryParse(value, out val))
                    value = ((int)(((bint*)buffer)[index])).ToString();
                else
                {
                    if (((bint*)buffer)[index] != val)
                    {
                        ((bint*)buffer)[index] = val;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }
            else //float/radians
            {
                btnInf.Visible = btnMinusInf.Visible = true;
                float val;
                if (!float.TryParse(value, out val))
                    value = ((float)(((bfloat*)buffer)[index])).ToString();
                else
                {
                    if (((bfloat*)buffer)[index] != val)
                    {
                        ((bfloat*)buffer)[index] = val;
                        TargetNode.SignalPropertyChange();
                    }
                }
            }

            attributes.Rows[index][1] = value;
            if (AttributeArray[index]._type == 3)
                attributes.Rows[index][1] = ((RGBAPixel*)buffer)[index].ToString();
            if (CellEdited != null) CellEdited.Invoke(this, EventArgs.Empty);
        }

        private void dtgrdAttributes_CurrentCellChanged(object sender, EventArgs e)
        {
            lblColor.Visible = false;
            lblCNoA.Visible = false;
            btnInf.Visible = btnMinusInf.Visible = false;
            if (dtgrdAttributes.CurrentCell == null) return;
            if(AttributeArray.Length <= 0)
            {
                description.Text = "";
                return;
            }
            int index = dtgrdAttributes.CurrentCell.RowIndex;

            //Display description of the selected attribute.
            _updating = true;
            description.Text = AttributeArray[index]._description;
            _updating = false;

            (AttributeArray[index]._type == 1 ? rdoInt
                : AttributeArray[index]._type == 3 ? rdoColor
                : AttributeArray[index]._type == 2 ? rdoDegrees
                : AttributeArray[index]._type == 4 ? rdoUnknown
                : rdoFloat).Checked = true;

            if(AttributeArray[index]._type == 3)
            {
                lblColor.Visible = true;
                lblCNoA.Visible = true;
                lblColor.BackColor = (Color)(((RGBAPixel*)TargetNode.AttributeAddress)[index]);
                lblCNoA.BackColor = Color.FromArgb((((RGBAPixel*)TargetNode.AttributeAddress)[index]).R, (((RGBAPixel*)TargetNode.AttributeAddress)[index]).G, (((RGBAPixel*)TargetNode.AttributeAddress)[index]).B);
            } else if(AttributeArray[index]._type == 0)
                btnInf.Visible = btnMinusInf.Visible = true;
        }

        private bool _updating = false;
        private void description_TextChanged(object sender, EventArgs e)
        {
            if (_updating || AttributeArray.Length == 0)
                return;

            int index = dtgrdAttributes.CurrentCell.RowIndex;
            if (index >= 0 && AttributeArray.Length > index)
            {
                AttributeArray[index]._description = description.Text;
                if (DictionaryChanged != null) DictionaryChanged.Invoke(this, EventArgs.Empty);
            }
        }
        private void description_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", e.LinkText);
        }

        private void radioButtonsChanged(object sender, EventArgs e)
        {
            lblColor.Visible = false;
            lblCNoA.Visible = false;
            btnInf.Visible = btnMinusInf.Visible = false;
            if (dtgrdAttributes.CurrentCell == null)
                return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;
            int ntype =
                rdoFloat.Checked ? 0
                : rdoInt.Checked ? 1
                : rdoColor.Checked ? 3
                : rdoDegrees.Checked ? 2
                : rdoUnknown.Checked ? 4
                : -1;
			if (ntype != AttributeArray[index]._type) {
				AttributeArray[index]._type = ntype;
				if (DictionaryChanged != null) DictionaryChanged.Invoke(this, EventArgs.Empty);
				RefreshRow(index);
			}
            if(ntype == 3)
            {
                lblColor.Visible = true;
                lblCNoA.Visible = true;
            } else if (ntype == 0)
                btnInf.Visible = btnMinusInf.Visible = true;
        }

        private GoodColorDialog _dlgColor;
        private void lblColor_Click(object sender, EventArgs e)
        {
            if (!lblColor.Visible)
                return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;
            _dlgColor.Color = (Color)(((RGBAPixel*)TargetNode.AttributeAddress)[index]);
            if (_dlgColor.ShowDialog(this) == DialogResult.OK)
            {
                ((RGBAPixel*)TargetNode.AttributeAddress)[index] = (ARGBPixel)_dlgColor.Color;
                TargetNode.SignalPropertyChange();
                RefreshRow(index);
            }
        }

        private void btnMinusInf_Click(object sender, EventArgs e)
        {
            if (!btnMinusInf.Visible)
                return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;
            if(AttributeArray[index]._type == 0)
            {
                ((bfloat*)TargetNode.AttributeAddress)[index] = float.NegativeInfinity;
                TargetNode.SignalPropertyChange();
                RefreshRow(index);
            }
        }

        private void btnInf_Click(object sender, EventArgs e)
        {
            if (!btnInf.Visible)
                return;
            int index = dtgrdAttributes.CurrentCell.RowIndex;
            if (AttributeArray[index]._type == 0)
            {
                ((bfloat*)TargetNode.AttributeAddress)[index] = float.PositiveInfinity;
                TargetNode.SignalPropertyChange();
                RefreshRow(index);
            }
        }
    }

    public enum ValType
    {
        Float = 0,
        Int = 1,
        Degrees = 2,
        Color = 3,
        Unknown = 4
    }
}
