﻿using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public class SCN0LightEditControl : UserControl
    {
        private int _numFrames;

        private int _currentPage = 1;
        private LightAnimationFrame _currentFrame = LightAnimationFrame.Empty;
        private readonly NumericInputBox[] _boxes = new NumericInputBox[10];
        private Panel panel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Label label9;
        private Label label8;
        private Label label1;
        private NumericInputBox numStartZ;
        private NumericInputBox numEndY;
        private NumericInputBox numEndX;
        private NumericInputBox numStartX;
        private NumericInputBox numEndZ;
        private NumericInputBox numStartY;
        private Label label4;
        private Label label5;
        private Label label2;
        private Label label3;
        private Label label6;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Label label10;
        private NumericInputBox numRefBright;
        private NumericInputBox numRefDist;
        private NumericInputBox numSpotCut;
        private CLRControl lightCtrl;
        private CLRControl specCtrl;
        private Panel panel2;
        private NumericInputBox numSpotBright;
        private TabPage tabPage2;
        private VisEditor visEditor1;

        private SCN0LightNode _target;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SCN0LightNode TargetSequence
        {
            get => _target;
            set
            {
                if (_target == value)
                {
                    return;
                }

                _target = value;
                UpdateTarget();
            }
        }

        public SCN0LightEditControl()
        {
            InitializeComponent();
            _boxes[0] = numStartX;
            _boxes[1] = numStartY;
            _boxes[2] = numStartZ;
            _boxes[3] = numEndX;
            _boxes[4] = numEndY;
            _boxes[5] = numEndZ;
            _boxes[6] = numRefDist;
            _boxes[7] = numRefBright;
            _boxes[8] = numSpotCut;
            _boxes[9] = numSpotBright;

            for (int i = 0; i < 10; i++)
            {
                _boxes[i].Tag = i;
            }

            specCtrl._colorId = 1;
        }

        private void UpdateTarget()
        {
            listKeyframes.BeginUpdate();
            listKeyframes.Items.Clear();
            if (_target != null)
            {
                if (_target.FrameCount > 0)
                {
                    LightAnimationFrame a;
                    for (int x = 0; x < _target.FrameCount; x++)
                    {
                        if ((a = _target.GetAnimFrame(x)).HasKeys)
                        {
                            listKeyframes.Items.Add(a);
                        }
                    }

                    _numFrames = _target.FrameCount;

                    _currentPage = 0;
                    numFrame.Value = 1;
                    numFrame.Maximum = _numFrames;
                    lblFrameCount.Text = string.Format("/ {0}", _numFrames);
                }
                else
                {
                    numFrame.Value = 1;
                }
            }
            listKeyframes.EndUpdate();
            visEditor1.TargetNode = TargetSequence;
            lightCtrl.ColorSource = TargetSequence;
            specCtrl.ColorSource = TargetSequence;
            RefreshPage();
        }

        private void numFrame_ValueChanged(object sender, EventArgs e)
        {
            int page = (int)numFrame.Value - 1;
            if (_currentPage != page)
            {
                _currentPage = page;
                RefreshPage();
            }
        }

        private void RefreshPage()
        {
            if (_target != null)
            {
                //_currentPage = (int)numFrame.Value - 1;

                _currentFrame = _target.GetAnimFrame(_currentPage);

                numStartX.Value = _currentFrame.Start._x;
                numStartY.Value = _currentFrame.Start._y;
                numStartZ.Value = _currentFrame.Start._z;

                numEndX.Value = _currentFrame.End._x;
                numEndY.Value = _currentFrame.End._y;
                numEndZ.Value = _currentFrame.End._z;

                numSpotCut.Value = _currentFrame.SpotCutoff;
                numSpotBright.Value = _currentFrame.SpotBright;
                numRefDist.Value = _currentFrame.RefDist;
                numRefBright.Value = _currentFrame.RefBright;

                for (int i = 0; i < 10; i++)
                {
                    UpdateBox(i);
                }

                btnPrev.Enabled = _currentPage > 0;
                btnNext.Enabled = _currentPage < (_numFrames - 1);

                listKeyframes.SelectedIndex = FindKeyframe(_currentPage);
            }
        }

        private int FindKeyframe(int index)
        {
            int count = listKeyframes.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (((LightAnimationFrame)listKeyframes.Items[i]).Index == index)
                {
                    return i;
                }
            }

            return -1;
        }

        private void UpdateBox(int index)
        {
            if (_target.GetKeyframe((LightKeyframeMode)index, _currentPage) != null)
            {
                _boxes[index].BackColor = Color.Yellow;
            }
            else
            {
                _boxes[index].BackColor = Color.White;
            }
        }

        private unsafe void BoxChanged(object sender, EventArgs e)
        {
            NumericInputBox box = sender as NumericInputBox;
            LightAnimationFrame kf;
            float* pkf = (float*)&kf;
            float val = box.Value;
            int index = (int)box.Tag;
            int x;

            if (val != _currentFrame[index])
            {
                int kfIndex = FindKeyframe(_currentPage);

                if (float.IsNaN(val))
                {
                    //Value removed find keyframe and zero it out
                    if (kfIndex >= 0)
                    {
                        kf = (LightAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBools(index, false);
                        pkf[index] = val;
                        for (x = 0; (x < 10) && float.IsNaN(pkf[x]); x++)
                        {
                            ;
                        }

                        if (x == 10)
                        {
                            listKeyframes.Items.RemoveAt(kfIndex);
                            listKeyframes.SelectedIndex = -1;
                        }
                        else
                        {
                            listKeyframes.Items[kfIndex] = kf;
                        }
                    }

                    _target.RemoveKeyframe((LightKeyframeMode)index, _currentPage);
                    val = _target.GetAnimFrame(_currentPage)[index];
                    box.Value = val;
                }
                else
                {
                    if (kfIndex >= 0)
                    {
                        kf = (LightAnimationFrame)listKeyframes.Items[kfIndex];
                        kf.SetBools(index, true);
                        pkf[index] = val;
                        listKeyframes.Items[kfIndex] = kf;
                    }
                    else
                    {
                        kf = LightAnimationFrame.Empty;
                        kf.SetBools(index, true);
                        kf.Index = _currentPage;
                        pkf[index] = val;

                        int count = listKeyframes.Items.Count;
                        for (x = 0; (x < count) && (((LightAnimationFrame)listKeyframes.Items[x]).Index < _currentPage); x++)
                        {
                            ;
                        }

                        listKeyframes.Items.Insert(x, kf);
                        listKeyframes.SelectedIndex = x;
                    }

                    _target.SetKeyframe((LightKeyframeMode)index, _currentPage, val);
                }

                _currentFrame[index] = val;
                UpdateBox(index);
            }
        }

        private void listKeyframes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listKeyframes.SelectedIndex;
            if (index >= 0)
            {
                LightAnimationFrame f = (LightAnimationFrame)listKeyframes.SelectedItem;
                numFrame.Value = (decimal)(f.Index + 1);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e) { numFrame.Value--; }
        private void btnNext_Click(object sender, EventArgs e) { numFrame.Value++; }

        #region Designer

        private Label label7;
        private NumericUpDown numFrame;
        private Label lblFrameCount;
        private Button btnPrev;
        private Button btnNext;
        private ListBox listKeyframes;
        private GroupBox groupBox1;

        private void InitializeComponent()
        {
            label7 = new System.Windows.Forms.Label();
            numFrame = new System.Windows.Forms.NumericUpDown();
            lblFrameCount = new System.Windows.Forms.Label();
            btnPrev = new System.Windows.Forms.Button();
            btnNext = new System.Windows.Forms.Button();
            listKeyframes = new System.Windows.Forms.ListBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            panel1 = new System.Windows.Forms.Panel();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            panel2 = new System.Windows.Forms.Panel();
            numSpotBright = new System.Windows.Forms.NumericInputBox();
            label6 = new System.Windows.Forms.Label();
            numRefBright = new System.Windows.Forms.NumericInputBox();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            numStartY = new System.Windows.Forms.NumericInputBox();
            label10 = new System.Windows.Forms.Label();
            numEndZ = new System.Windows.Forms.NumericInputBox();
            numStartX = new System.Windows.Forms.NumericInputBox();
            numRefDist = new System.Windows.Forms.NumericInputBox();
            numEndX = new System.Windows.Forms.NumericInputBox();
            numEndY = new System.Windows.Forms.NumericInputBox();
            numSpotCut = new System.Windows.Forms.NumericInputBox();
            numStartZ = new System.Windows.Forms.NumericInputBox();
            label9 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            tabPage2 = new System.Windows.Forms.TabPage();
            visEditor1 = new System.Windows.Forms.VisEditor();
            tabPage3 = new System.Windows.Forms.TabPage();
            lightCtrl = new System.Windows.Forms.CLRControl();
            tabPage4 = new System.Windows.Forms.TabPage();
            specCtrl = new System.Windows.Forms.CLRControl();
            ((System.ComponentModel.ISupportInitialize)(numFrame)).BeginInit();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel2.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // label7
            // 
            label7.Location = new System.Drawing.Point(33, 5);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(41, 20);
            label7.TabIndex = 15;
            label7.Text = "Frame:";
            label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numFrame
            // 
            numFrame.Location = new System.Drawing.Point(80, 5);
            numFrame.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            numFrame.Name = "numFrame";
            numFrame.Size = new System.Drawing.Size(58, 20);
            numFrame.TabIndex = 0;
            numFrame.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            numFrame.ValueChanged += new System.EventHandler(numFrame_ValueChanged);
            // 
            // lblFrameCount
            // 
            lblFrameCount.Location = new System.Drawing.Point(144, 5);
            lblFrameCount.Name = "lblFrameCount";
            lblFrameCount.Size = new System.Drawing.Size(51, 20);
            lblFrameCount.TabIndex = 17;
            lblFrameCount.Text = "/ 10";
            lblFrameCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPrev
            // 
            btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnPrev.Location = new System.Drawing.Point(192, 3);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new System.Drawing.Size(23, 23);
            btnPrev.TabIndex = 1;
            btnPrev.Text = "<";
            btnPrev.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += new System.EventHandler(btnPrev_Click);
            // 
            // btnNext
            // 
            btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            btnNext.Location = new System.Drawing.Point(217, 3);
            btnNext.Name = "btnNext";
            btnNext.Size = new System.Drawing.Size(23, 23);
            btnNext.TabIndex = 2;
            btnNext.Text = ">";
            btnNext.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += new System.EventHandler(btnNext_Click);
            // 
            // listKeyframes
            // 
            listKeyframes.Dock = System.Windows.Forms.DockStyle.Fill;
            listKeyframes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listKeyframes.FormattingEnabled = true;
            listKeyframes.IntegralHeight = false;
            listKeyframes.ItemHeight = 14;
            listKeyframes.Location = new System.Drawing.Point(3, 16);
            listKeyframes.Name = "listKeyframes";
            listKeyframes.Size = new System.Drawing.Size(279, 104);
            listKeyframes.TabIndex = 18;
            listKeyframes.SelectedIndexChanged += new System.EventHandler(listKeyframes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listKeyframes);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(285, 123);
            groupBox1.TabIndex = 21;
            groupBox1.TabStop = false;
            groupBox1.Text = "Keyframes";
            // 
            // panel1
            // 
            panel1.Controls.Add(tabControl1);
            panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(293, 276);
            panel1.TabIndex = 23;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(293, 276);
            tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(panel2);
            tabPage1.Location = new System.Drawing.Point(4, 22);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new System.Drawing.Size(285, 250);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Lighting";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackColor = System.Drawing.SystemColors.Control;
            panel2.Controls.Add(numSpotBright);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(numRefBright);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(btnNext);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(btnPrev);
            panel2.Controls.Add(numStartY);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(numEndZ);
            panel2.Controls.Add(lblFrameCount);
            panel2.Controls.Add(numStartX);
            panel2.Controls.Add(numRefDist);
            panel2.Controls.Add(numEndX);
            panel2.Controls.Add(numFrame);
            panel2.Controls.Add(numEndY);
            panel2.Controls.Add(numSpotCut);
            panel2.Controls.Add(numStartZ);
            panel2.Controls.Add(label9);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label8);
            panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel2.Location = new System.Drawing.Point(0, 123);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(285, 127);
            panel2.TabIndex = 22;
            // 
            // numSpotBright
            // 
            numSpotBright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numSpotBright.Integral = false;
            numSpotBright.Location = new System.Drawing.Point(211, 84);
            numSpotBright.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numSpotBright.MaximumValue = 3.402823E+38F;
            numSpotBright.MinimumValue = -3.402823E+38F;
            numSpotBright.Name = "numSpotBright";
            numSpotBright.Size = new System.Drawing.Size(70, 20);
            numSpotBright.TabIndex = 19;
            numSpotBright.Text = "0";
            numSpotBright.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // label6
            // 
            label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label6.Location = new System.Drawing.Point(211, 27);
            label6.Margin = new System.Windows.Forms.Padding(0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(70, 20);
            label6.TabIndex = 8;
            label6.Text = "Z";
            label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numRefBright
            // 
            numRefBright.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRefBright.Integral = false;
            numRefBright.Location = new System.Drawing.Point(211, 103);
            numRefBright.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRefBright.MaximumValue = 3.402823E+38F;
            numRefBright.MinimumValue = -3.402823E+38F;
            numRefBright.Name = "numRefBright";
            numRefBright.Size = new System.Drawing.Size(70, 20);
            numRefBright.TabIndex = 13;
            numRefBright.Text = "0";
            numRefBright.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // label3
            // 
            label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label3.Location = new System.Drawing.Point(4, 65);
            label3.Margin = new System.Windows.Forms.Padding(0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(70, 20);
            label3.TabIndex = 2;
            label3.Text = "End Points";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label2.Location = new System.Drawing.Point(4, 84);
            label2.Margin = new System.Windows.Forms.Padding(0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(70, 20);
            label2.TabIndex = 1;
            label2.Text = "Spot Cutoff";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label5.Location = new System.Drawing.Point(142, 27);
            label5.Margin = new System.Windows.Forms.Padding(0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(70, 20);
            label5.TabIndex = 7;
            label5.Text = "Y";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label4.Location = new System.Drawing.Point(73, 27);
            label4.Margin = new System.Windows.Forms.Padding(0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(70, 20);
            label4.TabIndex = 4;
            label4.Text = "X";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numStartY
            // 
            numStartY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numStartY.Integral = false;
            numStartY.Location = new System.Drawing.Point(142, 46);
            numStartY.Margin = new System.Windows.Forms.Padding(0);
            numStartY.MaximumValue = 3.402823E+38F;
            numStartY.MinimumValue = -3.402823E+38F;
            numStartY.Name = "numStartY";
            numStartY.Size = new System.Drawing.Size(70, 20);
            numStartY.TabIndex = 4;
            numStartY.Text = "0";
            numStartY.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // label10
            // 
            label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label10.Location = new System.Drawing.Point(142, 84);
            label10.Margin = new System.Windows.Forms.Padding(0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(70, 20);
            label10.TabIndex = 14;
            label10.Text = "Spec Shine";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numEndZ
            // 
            numEndZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numEndZ.Integral = false;
            numEndZ.Location = new System.Drawing.Point(211, 65);
            numEndZ.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numEndZ.MaximumValue = 3.402823E+38F;
            numEndZ.MinimumValue = -3.402823E+38F;
            numEndZ.Name = "numEndZ";
            numEndZ.Size = new System.Drawing.Size(70, 20);
            numEndZ.TabIndex = 8;
            numEndZ.Text = "0";
            numEndZ.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numStartX
            // 
            numStartX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numStartX.Integral = false;
            numStartX.Location = new System.Drawing.Point(73, 46);
            numStartX.Margin = new System.Windows.Forms.Padding(0);
            numStartX.MaximumValue = 3.402823E+38F;
            numStartX.MinimumValue = -3.402823E+38F;
            numStartX.Name = "numStartX";
            numStartX.Size = new System.Drawing.Size(70, 20);
            numStartX.TabIndex = 3;
            numStartX.Text = "0";
            numStartX.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numRefDist
            // 
            numRefDist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numRefDist.Integral = false;
            numRefDist.Location = new System.Drawing.Point(73, 103);
            numRefDist.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numRefDist.MaximumValue = 3.402823E+38F;
            numRefDist.MinimumValue = -3.402823E+38F;
            numRefDist.Name = "numRefDist";
            numRefDist.Size = new System.Drawing.Size(70, 20);
            numRefDist.TabIndex = 12;
            numRefDist.Text = "0";
            numRefDist.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numEndX
            // 
            numEndX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numEndX.Integral = false;
            numEndX.Location = new System.Drawing.Point(73, 65);
            numEndX.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numEndX.MaximumValue = 3.402823E+38F;
            numEndX.MinimumValue = -3.402823E+38F;
            numEndX.Name = "numEndX";
            numEndX.Size = new System.Drawing.Size(70, 20);
            numEndX.TabIndex = 6;
            numEndX.Text = "0";
            numEndX.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numEndY
            // 
            numEndY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numEndY.Integral = false;
            numEndY.Location = new System.Drawing.Point(142, 65);
            numEndY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numEndY.MaximumValue = 3.402823E+38F;
            numEndY.MinimumValue = -3.402823E+38F;
            numEndY.Name = "numEndY";
            numEndY.Size = new System.Drawing.Size(70, 20);
            numEndY.TabIndex = 7;
            numEndY.Text = "0";
            numEndY.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numSpotCut
            // 
            numSpotCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numSpotCut.Integral = false;
            numSpotCut.Location = new System.Drawing.Point(73, 84);
            numSpotCut.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            numSpotCut.MaximumValue = 3.402823E+38F;
            numSpotCut.MinimumValue = -3.402823E+38F;
            numSpotCut.Name = "numSpotCut";
            numSpotCut.Size = new System.Drawing.Size(70, 20);
            numSpotCut.TabIndex = 11;
            numSpotCut.Text = "0";
            numSpotCut.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // numStartZ
            // 
            numStartZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            numStartZ.Integral = false;
            numStartZ.Location = new System.Drawing.Point(211, 46);
            numStartZ.Margin = new System.Windows.Forms.Padding(0);
            numStartZ.MaximumValue = 3.402823E+38F;
            numStartZ.MinimumValue = -3.402823E+38F;
            numStartZ.Name = "numStartZ";
            numStartZ.Size = new System.Drawing.Size(70, 20);
            numStartZ.TabIndex = 5;
            numStartZ.Text = "0";
            numStartZ.ValueChanged += new System.EventHandler(BoxChanged);
            // 
            // label9
            // 
            label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label9.Location = new System.Drawing.Point(142, 103);
            label9.Margin = new System.Windows.Forms.Padding(0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(70, 20);
            label9.TabIndex = 10;
            label9.Text = "Ref Bright";
            label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label1.Location = new System.Drawing.Point(4, 46);
            label1.Margin = new System.Windows.Forms.Padding(0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(70, 20);
            label1.TabIndex = 0;
            label1.Text = "Start Points";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label8.Location = new System.Drawing.Point(4, 103);
            label8.Margin = new System.Windows.Forms.Padding(0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(70, 20);
            label8.TabIndex = 9;
            label8.Text = "Ref Dist";
            label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(visEditor1);
            tabPage2.Location = new System.Drawing.Point(4, 22);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new System.Drawing.Size(285, 250);
            tabPage2.TabIndex = 4;
            tabPage2.Text = "Enabled";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // visEditor1
            // 
            visEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            visEditor1.Location = new System.Drawing.Point(0, 0);
            visEditor1.Name = "visEditor1";
            visEditor1.Size = new System.Drawing.Size(285, 250);
            visEditor1.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(lightCtrl);
            tabPage3.Location = new System.Drawing.Point(4, 22);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(285, 250);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Light Color";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // lightCtrl
            // 
            lightCtrl.ColorID = 0;
            lightCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            lightCtrl.Location = new System.Drawing.Point(0, 0);
            lightCtrl.Name = "lightCtrl";
            lightCtrl.Size = new System.Drawing.Size(285, 250);
            lightCtrl.TabIndex = 0;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(specCtrl);
            tabPage4.Location = new System.Drawing.Point(4, 22);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new System.Drawing.Size(285, 250);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Specular Color";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // specCtrl
            // 
            specCtrl.ColorID = 0;
            specCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            specCtrl.Location = new System.Drawing.Point(0, 0);
            specCtrl.Name = "specCtrl";
            specCtrl.Size = new System.Drawing.Size(285, 250);
            specCtrl.TabIndex = 0;
            // 
            // SCN0LightEditControl
            // 
            Controls.Add(panel1);
            Name = "SCN0LightEditControl";
            Size = new System.Drawing.Size(293, 276);
            ((System.ComponentModel.ISupportInitialize)(numFrame)).EndInit();
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
    }
}
