namespace Teleger
{
    partial class Form1
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonSendMsg = new System.Windows.Forms.Button();
            this.textBoxNumber = new System.Windows.Forms.TextBox();
            this.buttonGetMsg = new System.Windows.Forms.Button();
            this.textBoxMsgToSend = new System.Windows.Forms.TextBox();
            this.buttonLoadScript = new System.Windows.Forms.Button();
            this.textBoxGetMsgsCount = new System.Windows.Forms.TextBox();
            this.groupBoxMsgs = new System.Windows.Forms.GroupBox();
            this.groupBoxAuthorize = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.groupBoxContacts = new System.Windows.Forms.GroupBox();
            this.comboBoxContacts = new System.Windows.Forms.ComboBox();
            this.groupBoxMsgs.SuspendLayout();
            this.groupBoxAuthorize.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxContacts.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(21, 19);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(97, 20);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonSendMsg
            // 
            this.buttonSendMsg.Location = new System.Drawing.Point(21, 76);
            this.buttonSendMsg.Name = "buttonSendMsg";
            this.buttonSendMsg.Size = new System.Drawing.Size(97, 20);
            this.buttonSendMsg.TabIndex = 6;
            this.buttonSendMsg.Text = "send message";
            this.buttonSendMsg.UseVisualStyleBackColor = true;
            this.buttonSendMsg.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxNumber
            // 
            this.textBoxNumber.Location = new System.Drawing.Point(139, 19);
            this.textBoxNumber.Name = "textBoxNumber";
            this.textBoxNumber.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumber.TabIndex = 1;
            this.textBoxNumber.Text = "+13852492115";
            // 
            // buttonGetMsg
            // 
            this.buttonGetMsg.Location = new System.Drawing.Point(21, 31);
            this.buttonGetMsg.Name = "buttonGetMsg";
            this.buttonGetMsg.Size = new System.Drawing.Size(97, 22);
            this.buttonGetMsg.TabIndex = 4;
            this.buttonGetMsg.Text = "get message";
            this.buttonGetMsg.UseVisualStyleBackColor = true;
            this.buttonGetMsg.Click += new System.EventHandler(this.buttonGetMsg_Click);
            // 
            // textBoxMsgToSend
            // 
            this.textBoxMsgToSend.Location = new System.Drawing.Point(139, 76);
            this.textBoxMsgToSend.Name = "textBoxMsgToSend";
            this.textBoxMsgToSend.Size = new System.Drawing.Size(100, 20);
            this.textBoxMsgToSend.TabIndex = 5;
            // 
            // buttonLoadScript
            // 
            this.buttonLoadScript.Location = new System.Drawing.Point(77, 121);
            this.buttonLoadScript.Name = "buttonLoadScript";
            this.buttonLoadScript.Size = new System.Drawing.Size(97, 22);
            this.buttonLoadScript.TabIndex = 7;
            this.buttonLoadScript.Text = "load script";
            this.buttonLoadScript.UseVisualStyleBackColor = true;
            // 
            // textBoxGetMsgsCount
            // 
            this.textBoxGetMsgsCount.Location = new System.Drawing.Point(139, 31);
            this.textBoxGetMsgsCount.Name = "textBoxGetMsgsCount";
            this.textBoxGetMsgsCount.Size = new System.Drawing.Size(100, 20);
            this.textBoxGetMsgsCount.TabIndex = 3;
            // 
            // groupBoxMsgs
            // 
            this.groupBoxMsgs.Controls.Add(this.textBoxMsgToSend);
            this.groupBoxMsgs.Controls.Add(this.buttonSendMsg);
            this.groupBoxMsgs.Controls.Add(this.textBoxGetMsgsCount);
            this.groupBoxMsgs.Controls.Add(this.buttonGetMsg);
            this.groupBoxMsgs.Controls.Add(this.buttonLoadScript);
            this.groupBoxMsgs.Enabled = false;
            this.groupBoxMsgs.Location = new System.Drawing.Point(6, 130);
            this.groupBoxMsgs.Name = "groupBoxMsgs";
            this.groupBoxMsgs.Size = new System.Drawing.Size(260, 149);
            this.groupBoxMsgs.TabIndex = 10;
            this.groupBoxMsgs.TabStop = false;
            this.groupBoxMsgs.Text = "Messaging";
            // 
            // groupBoxAuthorize
            // 
            this.groupBoxAuthorize.Controls.Add(this.textBoxNumber);
            this.groupBoxAuthorize.Controls.Add(this.buttonConnect);
            this.groupBoxAuthorize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBoxAuthorize.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxAuthorize.Location = new System.Drawing.Point(6, 16);
            this.groupBoxAuthorize.Name = "groupBoxAuthorize";
            this.groupBoxAuthorize.Size = new System.Drawing.Size(260, 55);
            this.groupBoxAuthorize.TabIndex = 9;
            this.groupBoxAuthorize.TabStop = false;
            this.groupBoxAuthorize.Text = "Unauthorized";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(284, 311);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBoxContacts);
            this.tabPage1.Controls.Add(this.groupBoxAuthorize);
            this.tabPage1.Controls.Add(this.groupBoxMsgs);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(276, 285);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(276, 238);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(303, 34);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(269, 285);
            this.richTextBoxLog.TabIndex = 12;
            this.richTextBoxLog.Text = "";
            // 
            // groupBoxContacts
            // 
            this.groupBoxContacts.Controls.Add(this.comboBoxContacts);
            this.groupBoxContacts.Enabled = false;
            this.groupBoxContacts.Location = new System.Drawing.Point(6, 77);
            this.groupBoxContacts.Name = "groupBoxContacts";
            this.groupBoxContacts.Size = new System.Drawing.Size(260, 55);
            this.groupBoxContacts.TabIndex = 11;
            this.groupBoxContacts.TabStop = false;
            this.groupBoxContacts.Text = "Contacts";
            // 
            // comboBoxContacts
            // 
            this.comboBoxContacts.FormattingEnabled = true;
            this.comboBoxContacts.Location = new System.Drawing.Point(65, 19);
            this.comboBoxContacts.Name = "comboBoxContacts";
            this.comboBoxContacts.Size = new System.Drawing.Size(121, 21);
            this.comboBoxContacts.TabIndex = 0;
            this.comboBoxContacts.SelectedIndexChanged += new System.EventHandler(this.comboBoxContacts_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 326);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBoxMsgs.ResumeLayout(false);
            this.groupBoxMsgs.PerformLayout();
            this.groupBoxAuthorize.ResumeLayout(false);
            this.groupBoxAuthorize.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxContacts.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonSendMsg;
        private System.Windows.Forms.TextBox textBoxNumber;
        private System.Windows.Forms.Button buttonGetMsg;
        private System.Windows.Forms.TextBox textBoxMsgToSend;
        private System.Windows.Forms.Button buttonLoadScript;
        private System.Windows.Forms.TextBox textBoxGetMsgsCount;
        private System.Windows.Forms.GroupBox groupBoxMsgs;
        private System.Windows.Forms.GroupBox groupBoxAuthorize;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.GroupBox groupBoxContacts;
        private System.Windows.Forms.ComboBox comboBoxContacts;
    }
}

