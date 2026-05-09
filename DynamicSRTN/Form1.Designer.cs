namespace DynamicSRTN
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
            txtNumJobs = new TextBox();
            lblNumJobs = new Label();
            btnSetJobs = new Button();
            dgvInput = new DataGridView();
            btnContinue = new Button();
            btnExit = new Button();
            cbOptions = new ComboBox();
            rtbOutput = new RichTextBox();
            pnlGantt = new Panel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtTotalMemory = new TextBox();
            lblTotalMemory = new Label();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvInput).BeginInit();
            SuspendLayout();
            // 
            // txtNumJobs
            // 
            txtNumJobs.Location = new Point(141, 90);
            txtNumJobs.Name = "txtNumJobs";
            txtNumJobs.Size = new Size(68, 23);
            txtNumJobs.TabIndex = 0;
            // 
            // lblNumJobs
            // 
            lblNumJobs.AutoSize = true;
            lblNumJobs.Location = new Point(11, 92);
            lblNumJobs.Name = "lblNumJobs";
            lblNumJobs.Size = new Size(124, 15);
            lblNumJobs.TabIndex = 1;
            lblNumJobs.Text = "Enter Number of Jobs:";
            // 
            // btnSetJobs
            // 
            btnSetJobs.Location = new Point(215, 118);
            btnSetJobs.Name = "btnSetJobs";
            btnSetJobs.Size = new Size(70, 24);
            btnSetJobs.TabIndex = 2;
            btnSetJobs.Text = "Proceed";
            btnSetJobs.UseVisualStyleBackColor = true;
            // 
            // dgvInput
            // 
            dgvInput.AllowUserToAddRows = false;
            dgvInput.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInput.Location = new Point(12, 145);
            dgvInput.Name = "dgvInput";
            dgvInput.Size = new Size(424, 201);
            dgvInput.TabIndex = 3;
            // 
            // btnContinue
            // 
            btnContinue.Location = new Point(141, 352);
            btnContinue.Name = "btnContinue";
            btnContinue.Size = new Size(79, 24);
            btnContinue.TabIndex = 4;
            btnContinue.Text = "Continue";
            btnContinue.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(226, 352);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(80, 24);
            btnExit.TabIndex = 5;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            // 
            // cbOptions
            // 
            cbOptions.FormattingEnabled = true;
            cbOptions.Items.AddRange(new object[] { "GANTT Chart illustrating the execution of each process/job", "Process Waiting time", "average waiting time", "Process completion time", "average completion time", "process turn around time", "average turn around time", "Dynamic Partitioning Memory Map" });
            cbOptions.Location = new Point(12, 382);
            cbOptions.Name = "cbOptions";
            cbOptions.Size = new Size(424, 23);
            cbOptions.TabIndex = 6;
            // 
            // rtbOutput
            // 
            rtbOutput.Location = new Point(12, 411);
            rtbOutput.Name = "rtbOutput";
            rtbOutput.Size = new Size(424, 129);
            rtbOutput.TabIndex = 7;
            rtbOutput.Text = "";
            // 
            // pnlGantt
            // 
            pnlGantt.Location = new Point(460, 17);
            pnlGantt.Name = "pnlGantt";
            pnlGantt.Size = new Size(854, 346);
            pnlGantt.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(391, 251);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(412, 258);
            label2.Name = "label2";
            label2.Size = new Size(0, 15);
            label2.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 127);
            label3.Name = "label3";
            label3.Size = new Size(0, 15);
            label3.TabIndex = 11;
            // 
            // txtTotalMemory
            // 
            txtTotalMemory.Location = new Point(141, 119);
            txtTotalMemory.Name = "txtTotalMemory";
            txtTotalMemory.Size = new Size(68, 23);
            txtTotalMemory.TabIndex = 12;
            // 
            // lblTotalMemory
            // 
            lblTotalMemory.AutoSize = true;
            lblTotalMemory.Location = new Point(11, 119);
            lblTotalMemory.Name = "lblTotalMemory";
            lblTotalMemory.Size = new Size(110, 15);
            lblTotalMemory.TabIndex = 13;
            lblTotalMemory.Text = "Enter Job Size (MB):";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(11, 34);
            label4.Name = "label4";
            label4.Size = new Size(285, 32);
            label4.TabIndex = 14;
            label4.Text = "MAMAW DYNAMIC SRTN";
            label4.Click += label4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1322, 643);
            Controls.Add(label4);
            Controls.Add(lblTotalMemory);
            Controls.Add(txtTotalMemory);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pnlGantt);
            Controls.Add(rtbOutput);
            Controls.Add(cbOptions);
            Controls.Add(btnExit);
            Controls.Add(btnContinue);
            Controls.Add(dgvInput);
            Controls.Add(btnSetJobs);
            Controls.Add(lblNumJobs);
            Controls.Add(txtNumJobs);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvInput).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtNumJobs;
        private Label lblNumJobs;
        private Button btnSetJobs;
        private DataGridView dgvInput;
        private Button btnContinue;
        private Button btnExit;
        private ComboBox cbOptions;
        private RichTextBox rtbOutput;
        private Panel pnlGantt;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtTotalMemory;
        private Label lblTotalMemory;
        private Label label4;
    }
}
