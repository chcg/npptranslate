namespace nppTranslateCS.Forms
{
    partial class frmBingCredentials
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
            this.groupBoxBINGSettings = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBoxMyMemory = new System.Windows.Forms.GroupBox();
            this.labelDisclaimer = new System.Windows.Forms.Label();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.email = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxEnginePrefence = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBoxBINGSettings.SuspendLayout();
            this.groupBoxMyMemory.SuspendLayout();
            this.groupBoxEnginePrefence.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxBINGSettings
            // 
            this.groupBoxBINGSettings.Controls.Add(this.label3);
            this.groupBoxBINGSettings.Controls.Add(this.label2);
            this.groupBoxBINGSettings.Controls.Add(this.linkLabel1);
            this.groupBoxBINGSettings.Controls.Add(this.textBox1);
            this.groupBoxBINGSettings.Controls.Add(this.label1);
            this.groupBoxBINGSettings.Controls.Add(this.textBox2);
            this.groupBoxBINGSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxBINGSettings.Location = new System.Drawing.Point(25, 101);
            this.groupBoxBINGSettings.Name = "groupBoxBINGSettings";
            this.groupBoxBINGSettings.Size = new System.Drawing.Size(390, 204);
            this.groupBoxBINGSettings.TabIndex = 10;
            this.groupBoxBINGSettings.TabStop = false;
            this.groupBoxBINGSettings.Text = "BING Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Client ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Client Secret";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(6, 87);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(385, 22);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://blogs.msdn.com/b/translation/p/gettingstarted1.aspx";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(124, 122);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(212, 22);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(364, 64);
            this.label1.TabIndex = 4;
            this.label1.Text = "You must obtain Client ID and Client Secret to use microsoft BING translation ser" +
    "vice. It is FREE and extremely easy to register. Please follow instructions at: " +
    " \r\n";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(124, 166);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(212, 22);
            this.textBox2.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(159, 336);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 32);
            this.button1.TabIndex = 11;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBoxMyMemory
            // 
            this.groupBoxMyMemory.Controls.Add(this.labelDisclaimer);
            this.groupBoxMyMemory.Controls.Add(this.textBoxEmail);
            this.groupBoxMyMemory.Controls.Add(this.email);
            this.groupBoxMyMemory.Controls.Add(this.linkLabel2);
            this.groupBoxMyMemory.Controls.Add(this.label4);
            this.groupBoxMyMemory.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMyMemory.Location = new System.Drawing.Point(24, 99);
            this.groupBoxMyMemory.Name = "groupBoxMyMemory";
            this.groupBoxMyMemory.Size = new System.Drawing.Size(390, 211);
            this.groupBoxMyMemory.TabIndex = 12;
            this.groupBoxMyMemory.TabStop = false;
            this.groupBoxMyMemory.Text = "MyMemory Settings";
            // 
            // labelDisclaimer
            // 
            this.labelDisclaimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDisclaimer.Location = new System.Drawing.Point(12, 147);
            this.labelDisclaimer.Name = "labelDisclaimer";
            this.labelDisclaimer.Size = new System.Drawing.Size(361, 60);
            this.labelDisclaimer.TabIndex = 4;
            this.labelDisclaimer.Text = "* Rest assured of the privacy from plugin developer. Email is purely optional. However, " +
    "MyMemory API allows you higher usage if you provide email id.";
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(92, 111);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(252, 22);
            this.textBoxEmail.TabIndex = 3;
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email.Location = new System.Drawing.Point(19, 113);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(53, 17);
            this.email.TabIndex = 2;
            this.email.Text = "E-mail";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.Location = new System.Drawing.Point(44, 76);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(292, 17);
            this.linkLabel2.TabIndex = 1;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "http://mymemory.translated.net/doc/spec.php";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(372, 41);
            this.label4.TabIndex = 0;
            this.label4.Text = "MyMemory is free translation service, has usage limit however. Visit following li" +
    "nk to know more.";
            // 
            // groupBoxEnginePrefence
            // 
            this.groupBoxEnginePrefence.Controls.Add(this.radioButton2);
            this.groupBoxEnginePrefence.Controls.Add(this.radioButton1);
            this.groupBoxEnginePrefence.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEnginePrefence.Location = new System.Drawing.Point(20, 12);
            this.groupBoxEnginePrefence.Name = "groupBoxEnginePrefence";
            this.groupBoxEnginePrefence.Size = new System.Drawing.Size(394, 73);
            this.groupBoxEnginePrefence.TabIndex = 13;
            this.groupBoxEnginePrefence.TabStop = false;
            this.groupBoxEnginePrefence.Text = "Engine Prefrence";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(222, 30);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(62, 21);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "BING";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.Location = new System.Drawing.Point(59, 30);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(97, 21);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "MyMemory";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // frmBingCredentials
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 387);
            this.Controls.Add(this.groupBoxBINGSettings);
            this.Controls.Add(this.groupBoxEnginePrefence);
            this.Controls.Add(this.groupBoxMyMemory);
            this.Controls.Add(this.button1);
            this.Name = "frmBingCredentials";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Engine Setting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBingCredentials_FormClosing);
            this.Load += new System.EventHandler(this.frmBingCredentials_Load);
            this.groupBoxBINGSettings.ResumeLayout(false);
            this.groupBoxBINGSettings.PerformLayout();
            this.groupBoxMyMemory.ResumeLayout(false);
            this.groupBoxMyMemory.PerformLayout();
            this.groupBoxEnginePrefence.ResumeLayout(false);
            this.groupBoxEnginePrefence.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxBINGSettings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBoxMyMemory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxEnginePrefence;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label email;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.Label labelDisclaimer;

    }
}