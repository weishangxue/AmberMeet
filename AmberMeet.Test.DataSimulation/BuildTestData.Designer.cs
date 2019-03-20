namespace AmberMeet.Test.DataSimulation
{
    partial class BuildTestData
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
            this.FictitiouUsersButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FictitiouUsersButton
            // 
            this.FictitiouUsersButton.Location = new System.Drawing.Point(65, 12);
            this.FictitiouUsersButton.Name = "FictitiouUsersButton";
            this.FictitiouUsersButton.Size = new System.Drawing.Size(75, 23);
            this.FictitiouUsersButton.TabIndex = 0;
            this.FictitiouUsersButton.Text = "虚构用户";
            this.FictitiouUsersButton.UseVisualStyleBackColor = true;
            this.FictitiouUsersButton.Click += new System.EventHandler(this.FictitiouUsersButton_Click);
            // 
            // BuildTestData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 48);
            this.Controls.Add(this.FictitiouUsersButton);
            this.Name = "BuildTestData";
            this.Text = "构建测试数据";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FictitiouUsersButton;
    }
}