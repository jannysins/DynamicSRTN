using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DynamicSRTN
{
    public partial class Form1 : Form
    {
        // Data classes using 'double' to support real numbers (decimals)
        public class Job
        {
            public int Id { get; set; }
            public double ArrivalTime { get; set; }
            public double BurstTime { get; set; }
            public double RemainingTime { get; set; }
            public double CompletionTime { get; set; }
            public double TurnAroundTime { get; set; }
            public double WaitingTime { get; set; }
        }

        public class GanttBlock
        {
            public int JobId { get; set; }
            public double StartTime { get; set; }
            public double EndTime { get; set; }
        }

        private List<Job> jobs;
        private List<GanttBlock> ganttChart;
        private bool isCalculated = false;

        public Form1()
        {
            InitializeComponent();
            SetupUI();

            btnSetJobs.Click += BtnSetJobs_Click;
            btnContinue.Click += BtnContinue_Click;
            btnExit.Click += BtnExit_Click;
            cbOptions.SelectedIndexChanged += CbOptions_SelectedIndexChanged;
        }

        private void SetupUI()
        {
            dgvInput.Visible = false;
            btnContinue.Visible = false;
            cbOptions.Visible = false;
            rtbOutput.Visible = false;
            pnlGantt.Visible = false;

            dgvInput.ColumnCount = 3;
            dgvInput.Columns[0].Name = "Job ID";
            dgvInput.Columns[0].ReadOnly = true;
            dgvInput.Columns[1].Name = "Arrival Time";
            dgvInput.Columns[2].Name = "Burst Time";
        }

        private void BtnSetJobs_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtNumJobs.Text, out int numJobs) && numJobs > 0)
            {
                dgvInput.Rows.Clear();
                for (int i = 1; i <= numJobs; i++)
                {
                    // Defaulting to 0.0 to indicate to the user that decimals are accepted
                    dgvInput.Rows.Add($"P{i}", "0.0", "0.0");
                }
                dgvInput.Visible = true;
                btnContinue.Visible = true;
                isCalculated = false;
            }
            else
            {
                MessageBox.Show("Please enter a valid positive integer for the number of jobs.");
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                RunSRTN();
                cbOptions.Visible = true;
                MessageBox.Show("Data processed successfully. Please select an option from the dropdown to view results.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Input Error: Please ensure all Arrival Times and Burst Times are valid numbers (e.g., 1.5).");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing data.\n\nDetails: " + ex.Message);
            }
        }

        private void RunSRTN()
        {
            jobs = new List<Job>();
            ganttChart = new List<GanttBlock>();

            // --- REAL NUMBER INPUT HANDLING ---
            // Here we parse the grid values as 'double' instead of 'int'
            for (int i = 0; i < dgvInput.Rows.Count; i++)
            {
                double arrival = double.Parse(dgvInput.Rows[i].Cells[1].Value.ToString());
                double burst = double.Parse(dgvInput.Rows[i].Cells[2].Value.ToString());

                jobs.Add(new Job
                {
                    Id = i + 1,
                    ArrivalTime = arrival,
                    BurstTime = burst,
                    RemainingTime = burst
                });
            }

            double currentTime = 0;
            int completedCount = 0;
            int prevJobId = -1;
            int n = jobs.Count;
            double epsilon = 0.00001; // Precision buffer for comparing decimals

            // Event-driven SRTN Algorithm for handling decimal interruptions
            while (completedCount < n)
            {
                var availableJobs = jobs.Where(j => j.ArrivalTime <= currentTime + epsilon && j.RemainingTime > epsilon).ToList();

                if (availableJobs.Count == 0)
                {
                    var nextJob = jobs.Where(j => j.RemainingTime > epsilon).OrderBy(j => j.ArrivalTime).FirstOrDefault();
                    if (nextJob != null)
                    {
                        currentTime = nextJob.ArrivalTime;
                    }
                    prevJobId = -1;
                    continue;
                }

                var currentJob = availableJobs.OrderBy(j => j.RemainingTime).ThenBy(j => j.ArrivalTime).First();

                var futureJobs = jobs.Where(j => j.ArrivalTime > currentTime + epsilon && j.RemainingTime > epsilon).ToList();
                double timeToNextArrival = futureJobs.Count > 0 ? futureJobs.Min(j => j.ArrivalTime) - currentTime : double.MaxValue;

                double runTime = Math.Min(currentJob.RemainingTime, timeToNextArrival);

                if (prevJobId != currentJob.Id)
                {
                    ganttChart.Add(new GanttBlock { JobId = currentJob.Id, StartTime = currentTime, EndTime = currentTime + runTime });
                    prevJobId = currentJob.Id;
                }
                else
                {
                    ganttChart.Last().EndTime += runTime;
                }

                currentTime += runTime;
                currentJob.RemainingTime -= runTime;

                if (currentJob.RemainingTime <= epsilon)
                {
                    currentJob.RemainingTime = 0;
                    currentJob.CompletionTime = currentTime;
                    currentJob.TurnAroundTime = currentJob.CompletionTime - currentJob.ArrivalTime;
                    currentJob.WaitingTime = currentJob.TurnAroundTime - currentJob.BurstTime;
                    completedCount++;
                }
            }

            isCalculated = true;
        }

        private void CbOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCalculated) return;

            rtbOutput.Clear();
            rtbOutput.Visible = false;
            pnlGantt.Visible = false;

            int selection = cbOptions.SelectedIndex + 1;

            switch (selection)
            {
                case 1: // GANTT Chart
                    pnlGantt.Visible = true;

                    float scale = 30f;
                    float totalWidth = 20f;
                    if (ganttChart.Count > 0)
                    {
                        totalWidth += (float)ganttChart.Last().EndTime * scale + 50f;
                    }

                    // Enable scrolling based on the calculated width of the chart
                    pnlGantt.AutoScroll = true;
                    pnlGantt.AutoScrollMinSize = new Size((int)totalWidth, pnlGantt.Height);

                    pnlGantt.Paint -= DrawGanttChart;
                    pnlGantt.Paint += DrawGanttChart;

                    pnlGantt.Invalidate();
                    break;

                case 2: // Process Waiting Time
                    rtbOutput.Visible = true;
                    foreach (var j in jobs) rtbOutput.AppendText($"Process P{j.Id} Waiting Time: {j.WaitingTime:F2}\n");
                    break;
                case 3: // Average Waiting Time
                    rtbOutput.Visible = true;
                    double avgWait = jobs.Average(j => j.WaitingTime);
                    rtbOutput.Text = $"Average Waiting Time: {avgWait:F2}";
                    break;
                case 4: // Process Completion Time
                    rtbOutput.Visible = true;
                    foreach (var j in jobs) rtbOutput.AppendText($"Process P{j.Id} Completion Time: {j.CompletionTime:F2}\n");
                    break;
                case 5: // Average Completion Time
                    rtbOutput.Visible = true;
                    double avgComp = jobs.Average(j => j.CompletionTime);
                    rtbOutput.Text = $"Average Completion Time: {avgComp:F2}";
                    break;
                case 6: // Process Turn Around Time
                    rtbOutput.Visible = true;
                    foreach (var j in jobs) rtbOutput.AppendText($"Process P{j.Id} Turn Around Time: {j.TurnAroundTime:F2}\n");
                    break;
                case 7: // Average Turn Around Time
                    rtbOutput.Visible = true;
                    double avgTat = jobs.Average(j => j.TurnAroundTime);
                    rtbOutput.Text = $"Average Turn Around Time: {avgTat:F2}";
                    break;
            }
        }

        private void DrawGanttChart(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            // Translates the drawing canvas based on scrollbar position
            g.TranslateTransform(pnlGantt.AutoScrollPosition.X, pnlGantt.AutoScrollPosition.Y);

            float x = 10f;
            float y = 20f;
            float blockHeight = 40f;
            float scale = 30f;

            foreach (var block in ganttChart)
            {
                float width = (float)(block.EndTime - block.StartTime) * scale;
                if (width <= 0) continue;

                g.DrawRectangle(Pens.Black, x, y, width, blockHeight);
                g.DrawString($"P{block.JobId}", this.Font, Brushes.Black, x + 5, y + 10);

                // Trimming the decimals to two places for cleaner visuals on the chart
                g.DrawString(block.StartTime.ToString("0.##"), this.Font, Brushes.Black, x, y + blockHeight + 5);

                x += width;
            }
            if (ganttChart.Count > 0)
            {
                g.DrawString(ganttChart.Last().EndTime.ToString("0.##"), this.Font, Brushes.Black, x, y + blockHeight + 5);
            }
        }

        private void btnSetJobs_Click_1(object sender, EventArgs e)
        {

        }

        private void pnlGantt_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbOptions_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}