namespace UI1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.javPopCheck = new System.Windows.Forms.CheckBox();
            this.ifOnlyFindSmallerCheckBox = new System.Windows.Forms.CheckBox();
            this.ifBtDig = new System.Windows.Forms.CheckBox();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.processButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.textBox1.Location = new System.Drawing.Point(414, 441);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(126, 21);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "D:\\test8";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(998, 373);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 16);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "ifCheckHisSize";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(998, 396);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(90, 16);
            this.checkBox2.TabIndex = 18;
            this.checkBox2.Text = "ifCheckSize";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(998, 415);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(96, 16);
            this.checkBox3.TabIndex = 19;
            this.checkBox3.Text = "isCheck168xC";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(414, 414);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(52, 21);
            this.textBox2.TabIndex = 26;
            this.textBox2.Text = "6";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(340, 415);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 27;
            this.label1.Text = "timeSpan";
            // 
            // javPopCheck
            // 
            this.javPopCheck.AutoSize = true;
            this.javPopCheck.Checked = true;
            this.javPopCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.javPopCheck.Location = new System.Drawing.Point(1111, 376);
            this.javPopCheck.Margin = new System.Windows.Forms.Padding(2);
            this.javPopCheck.Name = "javPopCheck";
            this.javPopCheck.Size = new System.Drawing.Size(60, 16);
            this.javPopCheck.TabIndex = 30;
            this.javPopCheck.Text = "javPop";
            this.javPopCheck.UseVisualStyleBackColor = true;
            this.javPopCheck.CheckedChanged += new System.EventHandler(this.javPopCheck_CheckedChanged);
            // 
            // ifOnlyFindSmallerCheckBox
            // 
            this.ifOnlyFindSmallerCheckBox.AutoSize = true;
            this.ifOnlyFindSmallerCheckBox.Location = new System.Drawing.Point(1111, 396);
            this.ifOnlyFindSmallerCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.ifOnlyFindSmallerCheckBox.Name = "ifOnlyFindSmallerCheckBox";
            this.ifOnlyFindSmallerCheckBox.Size = new System.Drawing.Size(126, 16);
            this.ifOnlyFindSmallerCheckBox.TabIndex = 31;
            this.ifOnlyFindSmallerCheckBox.Text = "ifOnlyFindSmaller";
            this.ifOnlyFindSmallerCheckBox.UseVisualStyleBackColor = true;
            this.ifOnlyFindSmallerCheckBox.CheckedChanged += new System.EventHandler(this.ifOnlyFindSmallerCheckBox_CheckedChanged);
            // 
            // ifBtDig
            // 
            this.ifBtDig.AutoSize = true;
            this.ifBtDig.Location = new System.Drawing.Point(1111, 418);
            this.ifBtDig.Margin = new System.Windows.Forms.Padding(2);
            this.ifBtDig.Name = "ifBtDig";
            this.ifBtDig.Size = new System.Drawing.Size(66, 16);
            this.ifBtDig.TabIndex = 32;
            this.ifBtDig.Text = "ifBtDig";
            this.ifBtDig.UseVisualStyleBackColor = true;
            this.ifBtDig.CheckedChanged += new System.EventHandler(this.ifBtDig_CheckedChanged);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(12, 12);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(1019, 314);
            this.textBoxLog.TabIndex = 33;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(50, 369);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(120, 20);
            this.comboBox1.TabIndex = 34;
            // 
            // processButton
            // 
            this.processButton.Location = new System.Drawing.Point(68, 420);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(101, 27);
            this.processButton.TabIndex = 35;
            this.processButton.Text = "Process";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.processButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 472);
            this.Controls.Add(this.processButton);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.ifBtDig);
            this.Controls.Add(this.ifOnlyFindSmallerCheckBox);
            this.Controls.Add(this.javPopCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "VideoFilter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button processButton;

        private System.Windows.Forms.ComboBox comboBox1;

        private System.Windows.Forms.TextBox textBoxLog;

        private System.Windows.Forms.CheckBox ifBtDig;

        private System.Windows.Forms.CheckBox ifOnlyFindSmallerCheckBox;

        private System.Windows.Forms.CheckBox javPopCheck;

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
    }
}

