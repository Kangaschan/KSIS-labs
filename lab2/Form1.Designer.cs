namespace WindowsFormsApp1
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtBoxTCPtime = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.txtBoxUPDtime = new System.Windows.Forms.TextBox();
            this.txtBoxTCPLost = new System.Windows.Forms.TextBox();
            this.txtBoxUDPLost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBoxIP = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBoxUDPSPEED = new System.Windows.Forms.TextBox();
            this.txtBoxTCPSPEED = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtBoxTCPtime
            // 
            this.txtBoxTCPtime.Enabled = false;
            this.txtBoxTCPtime.Location = new System.Drawing.Point(132, 80);
            this.txtBoxTCPtime.Name = "txtBoxTCPtime";
            this.txtBoxTCPtime.Size = new System.Drawing.Size(452, 22);
            this.txtBoxTCPtime.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(432, 299);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(152, 69);
            this.button2.TabIndex = 2;
            this.button2.Text = "Начать";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtBoxUPDtime
            // 
            this.txtBoxUPDtime.Enabled = false;
            this.txtBoxUPDtime.Location = new System.Drawing.Point(132, 109);
            this.txtBoxUPDtime.Name = "txtBoxUPDtime";
            this.txtBoxUPDtime.Size = new System.Drawing.Size(452, 22);
            this.txtBoxUPDtime.TabIndex = 3;
            // 
            // txtBoxTCPLost
            // 
            this.txtBoxTCPLost.Enabled = false;
            this.txtBoxTCPLost.Location = new System.Drawing.Point(132, 152);
            this.txtBoxTCPLost.Name = "txtBoxTCPLost";
            this.txtBoxTCPLost.Size = new System.Drawing.Size(452, 22);
            this.txtBoxTCPLost.TabIndex = 4;
            // 
            // txtBoxUDPLost
            // 
            this.txtBoxUDPLost.Enabled = false;
            this.txtBoxUDPLost.Location = new System.Drawing.Point(132, 182);
            this.txtBoxUDPLost.Name = "txtBoxUDPLost";
            this.txtBoxUDPLost.Size = new System.Drawing.Size(452, 22);
            this.txtBoxUDPLost.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "TCP time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "UDP time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "TCP Loss";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "UDP Loss";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(93, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "IP";
            // 
            // txtBoxIP
            // 
            this.txtBoxIP.Location = new System.Drawing.Point(132, 32);
            this.txtBoxIP.Name = "txtBoxIP";
            this.txtBoxIP.Size = new System.Drawing.Size(452, 22);
            this.txtBoxIP.TabIndex = 10;
            this.txtBoxIP.Text = "127.0.0.1";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(323, 324);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(76, 20);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "сервер";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "UDP speed";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(46, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "TCP speed";
            // 
            // txtBoxUDPSPEED
            // 
            this.txtBoxUDPSPEED.Enabled = false;
            this.txtBoxUDPSPEED.Location = new System.Drawing.Point(132, 252);
            this.txtBoxUDPSPEED.Name = "txtBoxUDPSPEED";
            this.txtBoxUDPSPEED.Size = new System.Drawing.Size(452, 22);
            this.txtBoxUDPSPEED.TabIndex = 14;
            // 
            // txtBoxTCPSPEED
            // 
            this.txtBoxTCPSPEED.Enabled = false;
            this.txtBoxTCPSPEED.Location = new System.Drawing.Point(132, 222);
            this.txtBoxTCPSPEED.Name = "txtBoxTCPSPEED";
            this.txtBoxTCPSPEED.Size = new System.Drawing.Size(452, 22);
            this.txtBoxTCPSPEED.TabIndex = 13;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 380);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtBoxUDPSPEED);
            this.Controls.Add(this.txtBoxTCPSPEED);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBoxIP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBoxUDPLost);
            this.Controls.Add(this.txtBoxTCPLost);
            this.Controls.Add(this.txtBoxUPDtime);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtBoxTCPtime);
            this.Name = "MainForm";
            this.Text = "Ksis lab2";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxTCPtime;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtBoxUPDtime;
        private System.Windows.Forms.TextBox txtBoxTCPLost;
        private System.Windows.Forms.TextBox txtBoxUDPLost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBoxIP;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBoxUDPSPEED;
        private System.Windows.Forms.TextBox txtBoxTCPSPEED;
    }
}

