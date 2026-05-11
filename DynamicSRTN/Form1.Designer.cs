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
            btnclose = new Button();
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
            txtNumJobs.Location = new Point(193, 91);
            txtNumJobs.Name = "txtNumJobs";
            txtNumJobs.Size = new Size(68, 23);
            txtNumJobs.TabIndex = 0;
            // 
            // lblNumJobs
            // 
            lblNumJobs.AutoSize = true;
            lblNumJobs.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumJobs.Location = new Point(11, 92);
            lblNumJobs.Name = "lblNumJobs";
            lblNumJobs.Size = new Size(179, 21);
            lblNumJobs.TabIndex = 1;
            lblNumJobs.Text = "Enter Number of Jobs:";
            // 
            // btnSetJobs
            // 
            btnSetJobs.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSetJobs.Location = new Point(267, 102);
            btnSetJobs.Name = "btnSetJobs";
            btnSetJobs.Size = new Size(78, 24);
            btnSetJobs.TabIndex = 2;
            btnSetJobs.Text = "PROCEED";
            btnSetJobs.UseVisualStyleBackColor = true;
            // 
            // dgvInput
            // 
            dgvInput.AllowUserToAddRows = false;
            dgvInput.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInput.Location = new Point(11, 162);
            dgvInput.Name = "dgvInput";
            dgvInput.Size = new Size(424, 201);
            dgvInput.TabIndex = 3;
            // 
            // btnContinue
            // 
            btnContinue.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnContinue.Location = new Point(108, 369);
            btnContinue.Name = "btnContinue";
            btnContinue.Size = new Size(100, 24);
            btnContinue.TabIndex = 4;
            btnContinue.Text = "CONTINUE";
            btnContinue.UseVisualStyleBackColor = true;
            // 
            // btnclose
            // 
            btnclose.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnclose.Location = new Point(225, 369);
            btnclose.Name = "btnclose";
            btnclose.Size = new Size(80, 24);
            btnclose.TabIndex = 5;
            btnclose.Text = "CLOSE";
            btnclose.UseVisualStyleBackColor = true;
            btnclose.Click += btnExit_Click_1;
            // 
            // cbOptions
            // 
            cbOptions.FormattingEnabled = true;
            cbOptions.Items.AddRange(new object[] { "GANTT Chart illustrating the execution of each process/job", "Process Waiting time", "Average waiting time", "Process completion time", "Average completion time", "Process turn around time", "Average turn around time", "Dynamic Partitioning Memory Map" });
            cbOptions.Location = new Point(11, 399);
            cbOptions.Name = "cbOptions";
            cbOptions.Size = new Size(424, 23);
            cbOptions.TabIndex = 6;
            // 
            // rtbOutput
            // 
            rtbOutput.BackColor = Color.Gray;
            rtbOutput.Location = new Point(11, 428);
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
            txtTotalMemory.Location = new Point(193, 120);
            txtTotalMemory.Name = "txtTotalMemory";
            txtTotalMemory.Size = new Size(68, 23);
            txtTotalMemory.TabIndex = 12;
            // 
            // lblTotalMemory
            // 
            lblTotalMemory.AutoSize = true;
            lblTotalMemory.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalMemory.Location = new Point(12, 118);
            lblTotalMemory.Name = "lblTotalMemory";
            lblTotalMemory.Size = new Size(161, 21);
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
            BackColor = Color.Gray;
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
            Controls.Add(btnclose);
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
        private Button btnclose;
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
