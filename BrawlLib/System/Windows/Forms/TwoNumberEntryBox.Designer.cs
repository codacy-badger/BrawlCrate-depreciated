namespace System.Windows.Forms
{
    partial class TwoNumberEntryBox
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
            this.label2 = new System.Windows.Forms.Label();
            this.numericInputBox1 = new System.Windows.Forms.NumericInputBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.numericInputBox2 = new System.Windows.Forms.NumericInputBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 20);
            this.label2.TabIndex = 1;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericInputBox1
            // 
            this.numericInputBox1.Integral = true;
            this.numericInputBox1.Location = new System.Drawing.Point(146, 12);
            this.numericInputBox1.MaximumValue = 65535F;
            this.numericInputBox1.MinimumValue = 1F;
            this.numericInputBox1.Name = "numericInputBox1";
            this.numericInputBox1.Size = new System.Drawing.Size(100, 20);
            this.numericInputBox1.TabIndex = 3;
            this.numericInputBox1.Text = "1";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(175, 64);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(94, 64);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 5;
            this.btnOkay.Text = "&Okay";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // numericInputBox2
            // 
            this.numericInputBox2.Integral = true;
            this.numericInputBox2.Location = new System.Drawing.Point(146, 38);
            this.numericInputBox2.MaximumValue = 65535F;
            this.numericInputBox2.MinimumValue = 1F;
            this.numericInputBox2.Name = "numericInputBox2";
            this.numericInputBox2.Size = new System.Drawing.Size(100, 20);
            this.numericInputBox2.TabIndex = 7;
            this.numericInputBox2.Text = "1";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 20);
            this.label1.TabIndex = 8;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TwoNumberEntryBox
            // 
            this.AcceptButton = this.btnOkay;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(258, 95);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericInputBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.numericInputBox1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TwoNumberEntryBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private global::System.Windows.Forms.Label label2;
        private global::System.Windows.Forms.NumericInputBox numericInputBox1;
        private Button btnCancel;
        private Button btnOkay;
        private NumericInputBox numericInputBox2;
        private Label label1;
    }
}