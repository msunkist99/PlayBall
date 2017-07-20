namespace Retrosheet_Wrapper
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.startButton = new System.Windows.Forms.Button();
            this.outputTextbox = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.deleteFilesCheckbox = new System.Windows.Forms.CheckBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // outputTextbox
            // 
            this.outputTextbox.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTextbox.Location = new System.Drawing.Point(12, 41);
            this.outputTextbox.Multiline = true;
            this.outputTextbox.Name = "outputTextbox";
            this.outputTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextbox.Size = new System.Drawing.Size(492, 256);
            this.outputTextbox.TabIndex = 1;
            this.outputTextbox.Text = resources.GetString("outputTextbox.Text");
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(229, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // deleteFilesCheckbox
            // 
            this.deleteFilesCheckbox.AutoSize = true;
            this.deleteFilesCheckbox.Checked = true;
            this.deleteFilesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.deleteFilesCheckbox.Location = new System.Drawing.Point(130, 16);
            this.deleteFilesCheckbox.Name = "deleteFilesCheckbox";
            this.deleteFilesCheckbox.Size = new System.Drawing.Size(120, 17);
            this.deleteFilesCheckbox.TabIndex = 3;
            this.deleteFilesCheckbox.Text = "Delete Existing Files";
            this.deleteFilesCheckbox.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.SelectedPath = "C:\\Users\\mmr\\Documents\\retrosheet";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 300);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(516, 192);
            this.label1.TabIndex = 6;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 501);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deleteFilesCheckbox);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.outputTextbox);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox outputTextbox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox deleteFilesCheckbox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
    }
}

