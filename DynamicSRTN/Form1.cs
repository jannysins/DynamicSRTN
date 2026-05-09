using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DynamicSRTN
{
    public partial class Form1 : Form
    {
        public class Job
        {
            public int Id { get; set; }
            public double ArrivalTime { get; set; }
            public double BurstTime { get; set; }
            public double MemorySize { get; set; } // NEW: Memory requirement
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

        // NEW: Class to represent memory partitions
        public class MemoryBlock
        {
            public string Name { get; set; }
            public double Size { get; set; }
            public bool IsAllocated { get; set; }
            public Color BlockColor { get; set; }
        }

        private List<Job> jobs;
        private List<GanttBlock> ganttChart;
        private List<MemoryBlock> memoryMap; // NEW: Holds the memory snapshot
        private double totalSystemMemory = 0;
        private bool isCalculated = false;

        public Form1()
        {

            this.WindowState = FormWindowState.Maximized;

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

            // NEW: Hide Exit button initially
            btnExit.Visible = false;

            // Updated column count to include Memory
            dgvInput.ColumnCount = 4;
            dgvInput.Columns[0].Name = "Job ID";
            dgvInput.Columns[0].ReadOnly = true;
            dgvInput.Columns[1].Name = "Arrival Time";
            dgvInput.Columns[2].Name = "Burst Time";
            dgvInput.Columns[3].Name = "Memory Size (MB)";
        }

        private void BtnSetJobs_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtNumJobs.Text, out int numJobs) && numJobs > 0)
            {
                dgvInput.Rows.Clear();
                for (int i = 1; i <= numJobs; i++)
                {
                    // FIXED: Changed the last parameter from "100.0" to "0.0"
                    dgvInput.Rows.Add($"P{i}", "0.0", "0.0", "0.0");
                }
                dgvInput.Visible = true;
                btnContinue.Visible = true;
                isCalculated = false;
                btnExit.Visible = true;
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
                // NEW: Force the DataGridView to commit any cell the user is currently editing
                dgvInput.EndEdit();

                // Ensure total memory is provided
                if (!double.TryParse(txtTotalMemory.Text, out totalSystemMemory) || totalSystemMemory <= 0)
                {
                    MessageBox.Show("Please enter a valid Total Memory size.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate DataGridView inputs before running the algorithm
                for (int i = 0; i < dgvInput.Rows.Count; i++)
                {
                    // Skip the empty new row at the bottom if AllowUserToAddRows is true
                    if (dgvInput.Rows[i].IsNewRow) continue;

                    var arrivalCell = dgvInput.Rows[i].Cells[1].Value;
                    var burstCell = dgvInput.Rows[i].Cells[2].Value;
                    var memoryCell = dgvInput.Rows[i].Cells[3].Value;

                    // Parse the values and ensure Burst Time and Memory Size are strictly > 0
                    bool isArrivalValid = arrivalCell != null && double.TryParse(arrivalCell.ToString(), out double at) && at >= 0;
                    bool isBurstValid = burstCell != null && double.TryParse(burstCell.ToString(), out double bt) && bt > 0; // MUST be > 0
                    bool isMemoryValid = memoryCell != null && double.TryParse(memoryCell.ToString(), out double mem) && mem > 0;

                    if (!isArrivalValid || !isBurstValid || !isMemoryValid)
                    {
                        MessageBox.Show($"Please enter valid numbers for Job P{i + 1}.\n\n" +
                                        $"- Arrival Time must be 0 or higher.\n" +
                                        $"- Burst Time MUST be greater than 0.\n" +
                                        $"- Memory Size MUST be greater than 0.",
                                        "Invalid Input Detected",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return; // Stops processing entirely, preventing the freeze!
                    }
                }

                RunSRTN();
                CalculateMemoryMap();

                cbOptions.Visible = true;
                MessageBox.Show("Data processed successfully. Please select an option from the dropdown to view results.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing data.\n\nDetails: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ... [RunSRTN method remains exactly the same as previous code] ...
        private void RunSRTN()
        {
            jobs = new List<Job>();
            ganttChart = new List<GanttBlock>();

            for (int i = 0; i < dgvInput.Rows.Count; i++)
            {
                double arrival = double.Parse(dgvInput.Rows[i].Cells[1].Value.ToString());
                double burst = double.Parse(dgvInput.Rows[i].Cells[2].Value.ToString());
                double memory = double.Parse(dgvInput.Rows[i].Cells[3].Value.ToString()); // Parse Memory

                jobs.Add(new Job
                {
                    Id = i + 1,
                    ArrivalTime = arrival,
                    BurstTime = burst,
                    MemorySize = memory,
                    RemainingTime = burst
                });
            }

            double currentTime = 0;
            int completedCount = 0;
            int prevJobId = -1;
            int n = jobs.Count;
            double epsilon = 0.00001;

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
                    else
                    {
                        // FAILSAFE: If no available jobs and no future jobs exist, break the loop to prevent freezing.
                        break;
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

        // NEW: Calculates First-Fit Dynamic Partitioning for a snapshot view
        private void CalculateMemoryMap()
        {
            memoryMap = new List<MemoryBlock>();

            // Assume 10% of total memory is reserved for the OS at the bottom
            double osSize = totalSystemMemory * 0.10;
            double availableMemory = totalSystemMemory - osSize;

            memoryMap.Add(new MemoryBlock { Name = "OS", Size = osSize, IsAllocated = true, BlockColor = Color.LightPink });

            // Initialize the rest as one giant free partition
            memoryMap.Add(new MemoryBlock { Name = "Free Partition", Size = availableMemory, IsAllocated = false, BlockColor = Color.LightYellow });

            // Allocate jobs using First-Fit (Placing them above the OS)
            foreach (var job in jobs)
            {
                for (int i = 0; i < memoryMap.Count; i++)
                {
                    if (!memoryMap[i].IsAllocated && memoryMap[i].Size >= job.MemorySize)
                    {
                        // Split the block
                        double leftover = memoryMap[i].Size - job.MemorySize;

                        memoryMap[i].Name = $"P{job.Id}";
                        memoryMap[i].Size = job.MemorySize;
                        memoryMap[i].IsAllocated = true;
                        memoryMap[i].BlockColor = Color.SlateBlue;

                        if (leftover > 0)
                        {
                            memoryMap.Insert(i + 1, new MemoryBlock { Name = "Free Partition", Size = leftover, IsAllocated = false, BlockColor = Color.LightYellow });
                        }
                        break; // Move to the next job once allocated
                    }
                }
            }
        }

        private void CbOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isCalculated) return;

            rtbOutput.Clear();
            rtbOutput.Visible = false;
            pnlGantt.Visible = false;
            pnlGantt.Paint -= DrawGanttChart;
            pnlGantt.Paint -= DrawMemoryMap; // Ensure previous paints are cleared

            int selection = cbOptions.SelectedIndex + 1;

            if (selection == 1) // GANTT Chart
            {
                pnlGantt.Visible = true;
                float scale = 30f;
                float totalWidth = 20f;
                if (ganttChart.Count > 0) totalWidth += (float)ganttChart.Last().EndTime * scale + 50f;

                pnlGantt.AutoScroll = true;
                pnlGantt.AutoScrollMinSize = new Size((int)totalWidth, pnlGantt.Height);
                pnlGantt.Paint += DrawGanttChart;
                pnlGantt.Invalidate();
            }
            else if (selection == 8) // Memory Map
            {
                pnlGantt.Visible = true;
                pnlGantt.AutoScroll = true;

                // NEW: Calculate the exact total height we will be drawing so the scrollbar knows how far to go
                int totalDrawHeight = 0;
                foreach (var block in memoryMap)
                {
                    totalDrawHeight += (int)Math.Max((float)block.Size, 30f); // Match the minimum 30px height rule
                }
                totalDrawHeight += 100; // Add some top/bottom padding margin

                // Apply the calculated height to the scroll limits
                pnlGantt.AutoScrollMinSize = new Size(pnlGantt.Width, totalDrawHeight);

                pnlGantt.Paint += DrawMemoryMap;
                pnlGantt.Invalidate();
            }
            // ... [Cases 2 through 7 remain exactly the same, mapping to rtbOutput] ...
            else
            {
                rtbOutput.Visible = true;
                switch (selection)
                {
                    case 2: foreach (var j in jobs) rtbOutput.AppendText($"P{j.Id} Waiting Time: {j.WaitingTime:F2}\n"); break;
                    case 3: rtbOutput.Text = $"Average Waiting Time: {jobs.Average(j => j.WaitingTime):F2}"; break;
                    case 4: foreach (var j in jobs) rtbOutput.AppendText($"P{j.Id} Completion Time: {j.CompletionTime:F2}\n"); break;
                    case 5: rtbOutput.Text = $"Average Completion Time: {jobs.Average(j => j.CompletionTime):F2}"; break;
                    case 6: foreach (var j in jobs) rtbOutput.AppendText($"P{j.Id} Turn Around Time: {j.TurnAroundTime:F2}\n"); break;
                    case 7: rtbOutput.Text = $"Average Turn Around Time: {jobs.Average(j => j.TurnAroundTime):F2}"; break;
                }
            }
        }

        // ... [DrawGanttChart remains exactly the same] ...
        private void DrawGanttChart(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            g.TranslateTransform(pnlGantt.AutoScrollPosition.X, 0);

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
                g.DrawString(block.StartTime.ToString("0.##"), this.Font, Brushes.Black, x, y + blockHeight + 5);
                x += width;
            }
            if (ganttChart.Count > 0) g.DrawString(ganttChart.Last().EndTime.ToString("0.##"), this.Font, Brushes.Black, x, y + blockHeight + 5);
        }

        // NEW: Draws the stacked memory map
        private void DrawMemoryMap(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            // This applies the vertical scroll calculation
            g.TranslateTransform(0, pnlGantt.AutoScrollPosition.Y);

            float blockWidth = 150f;
            float x = 50f;

            // FIXED: Start from the bottom of our newly calculated virtual scrolling canvas
            float currentY = pnlGantt.AutoScrollMinSize.Height - 50f;

            var reversedMap = Enumerable.Reverse(memoryMap).ToList();

            foreach (var block in reversedMap)
            {
                float blockHeight = Math.Max((float)block.Size, 30f);
                currentY -= blockHeight; // Move up by the block's height to draw the next one

                using (Brush brush = new SolidBrush(block.BlockColor))
                {
                    g.FillRectangle(brush, x, currentY, blockWidth, blockHeight);
                }
                g.DrawRectangle(Pens.Black, x, currentY, blockWidth, blockHeight);

                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(block.Name, this.Font, Brushes.Black, new RectangleF(x, currentY, blockWidth, blockHeight), sf);

                g.DrawString($"{block.Size}K", this.Font, Brushes.Black, x + blockWidth + 10, currentY + (blockHeight / 2) - 8);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}