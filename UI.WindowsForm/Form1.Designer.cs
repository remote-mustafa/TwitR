namespace UI.WindowsForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTweetSend = new System.Windows.Forms.Button();
            this.btnMessageSend = new System.Windows.Forms.Button();
            this.btnUserLogin = new System.Windows.Forms.Button();
            this.lbxTweetList = new System.Windows.Forms.ListBox();
            this.flwMessageList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // btnTweetSend
            // 
            this.btnTweetSend.Location = new System.Drawing.Point(32, 12);
            this.btnTweetSend.Name = "btnTweetSend";
            this.btnTweetSend.Size = new System.Drawing.Size(125, 41);
            this.btnTweetSend.TabIndex = 0;
            this.btnTweetSend.Text = "Tweet Gönder";
            this.btnTweetSend.UseVisualStyleBackColor = true;
            // 
            // btnMessageSend
            // 
            this.btnMessageSend.Location = new System.Drawing.Point(449, 16);
            this.btnMessageSend.Name = "btnMessageSend";
            this.btnMessageSend.Size = new System.Drawing.Size(124, 37);
            this.btnMessageSend.TabIndex = 1;
            this.btnMessageSend.Text = "Mesaj Gönder";
            this.btnMessageSend.UseVisualStyleBackColor = true;
            // 
            // btnUserLogin
            // 
            this.btnUserLogin.Location = new System.Drawing.Point(601, 16);
            this.btnUserLogin.Name = "btnUserLogin";
            this.btnUserLogin.Size = new System.Drawing.Size(111, 37);
            this.btnUserLogin.TabIndex = 1;
            this.btnUserLogin.Text = "Giriş Yap";
            this.btnUserLogin.UseVisualStyleBackColor = true;
            this.btnUserLogin.Click += new System.EventHandler(this.btnUserLogin_Click);
            // 
            // lbxTweetList
            // 
            this.lbxTweetList.FormattingEnabled = true;
            this.lbxTweetList.ItemHeight = 15;
            this.lbxTweetList.Location = new System.Drawing.Point(33, 70);
            this.lbxTweetList.Name = "lbxTweetList";
            this.lbxTweetList.Size = new System.Drawing.Size(411, 379);
            this.lbxTweetList.TabIndex = 2;
            // 
            // flwMessageList
            // 
            this.flwMessageList.AutoSize = true;
            this.flwMessageList.Location = new System.Drawing.Point(455, 72);
            this.flwMessageList.Name = "flwMessageList";
            this.flwMessageList.Size = new System.Drawing.Size(257, 35);
            this.flwMessageList.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 470);
            this.Controls.Add(this.flwMessageList);
            this.Controls.Add(this.lbxTweetList);
            this.Controls.Add(this.btnUserLogin);
            this.Controls.Add(this.btnMessageSend);
            this.Controls.Add(this.btnTweetSend);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTweetSend;
        private System.Windows.Forms.Button btnMessageSend;
        private System.Windows.Forms.Button btnUserLogin;
        private System.Windows.Forms.ListBox lbxTweetList;
        private System.Windows.Forms.FlowLayoutPanel flwMessageList;
    }
}
