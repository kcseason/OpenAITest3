using System.Text.RegularExpressions;

namespace PMPQuizApp
{
    public partial class Form1 : Form
    {
        // 存储正确答案和用户缓存的字典 (Key: RowIndex, ColIndex)
        private Dictionary<(int, int), string> correctAnswers = new Dictionary<(int, int), string>();
        private Dictionary<(int, int), string> userCache = new Dictionary<(int, int), string>();
        private Dictionary<(int, int, int), string> mergeCells = new Dictionary<(int, int, int), string>
        {
            { (0, 1, 2), "4.项目整合管理" },
            { (0, 3, 6), "5.项目范围管理" },
            { (0, 7, 12), "6.项目进度管理" },
            { (0, 13, 15), "7.项目成本管理" },
            { (0, 17, 19), "9.项目资源管理" },
            { (0, 21, 25), "11.项目风险管理" },
            { (1, 3, 6), "" },
            { (1, 7, 12), "" },
            { (1, 13, 15), "" },
            { (1, 17, 19), "" },
            { (1, 21, 25), "" },
            //{ (2, 18, 19), "" },
            { (3, 3, 6), "" },
            { (3, 7, 12), "" },
            { (3, 13, 15), "" },
            { (3, 22, 25), "" },
            { (4, 5, 6), "" },
            { (4, 8, 12), "" },
            { (4, 14, 15), "" },
            { (4, 18, 19), "" },
            { (4, 22, 25), "" },
            { (5, 3, 6), "" },
            { (5, 7, 12), "" },
            { (5, 13, 15), "" },
            { (5, 17, 19), "" },
            { (5, 21, 25), "" },
        };
        private bool isShowingAnswer = false;

        // 定义颜色常量（匹配图片）
        //private readonly Color ColorHeaderBlue = Color.FromArgb(0, 176, 240);
        //private readonly Color ColorHeaderYellow = Color.FromArgb(255, 192, 0);
        //private readonly Color ColorHeaderGreen = Color.FromArgb(146, 208, 80);
        //private readonly Color ColorProcessYellow = Color.FromArgb(255, 255, 0);
        //private readonly Color ColorProcessGreen = Color.FromArgb(0, 176, 80);
        //private readonly Color ColorProcessBlue = Color.FromArgb(0, 112, 192);
        //private readonly Color ColorProcessRed = Color.FromArgb(255, 0, 0);
        //private readonly Color ColorProcessGray = Color.FromArgb(191, 191, 191);

        private readonly Color ColorHeaderBlue = Color.White;
        private readonly Color ColorHeaderYellow = Color.White;
        private readonly Color ColorHeaderGreen = Color.White;
        private readonly Color ColorProcessYellow = Color.White;
        private readonly Color ColorProcessGreen = Color.White;
        private readonly Color ColorProcessBlue = Color.White;
        private readonly Color ColorProcessRed = Color.White;
        private readonly Color ColorProcessGray = Color.White;

        public Form1()
        {
            InitializeComponent();
            InitializeDataGrid();
            LoadAnswers();
        }

        private void InitializeDataGrid()
        {
            dgvMain.AllowUserToAddRows = false;
            dgvMain.RowHeadersVisible = false;
            dgvMain.ColumnHeadersVisible = false;
            dgvMain.BorderStyle = BorderStyle.None;
            dgvMain.BackgroundColor = Color.White;
            dgvMain.DefaultCellStyle.Font = new Font("微软雅黑", 12);
            dgvMain.GridColor = Color.Black;

            dgvMain.EditMode = DataGridViewEditMode.EditOnEnter;

            // 创建6列：1列知识领域 + 5个过程组
            dgvMain.Columns.Add(new DataGridViewTextBoxColumn() { Width = 150 }); // 知识领域
            dgvMain.Columns.Add(new DataGridViewTextBoxColumn() { Width = 150 }); // 启动
            dgvMain.Columns.Add(new DataGridViewTextBoxColumn() { Width = 200 }); // 规划
            dgvMain.Columns.Add(new DataGridViewTextBoxColumn() { Width = 200 }); // 执行
            dgvMain.Columns.Add(new DataGridViewTextBoxColumn() { Width = 180 }); // 监控
            dgvMain.Columns.Add(new DataGridViewTextBoxColumn() { Width = 160 }); // 收尾

            // 创建28行 (1行过程组标题 + 27行内容)
            dgvMain.Rows.Add(28);

            // 设置第一行（过程组标题）
            dgvMain[0, 0].Value = "知识领域";
            dgvMain[1, 0].Value = "启动过程组";
            dgvMain[2, 0].Value = "规划过程组";
            dgvMain[3, 0].Value = "执行过程组";
            dgvMain[4, 0].Value = "监控过程组";
            dgvMain[5, 0].Value = "收尾过程组";

            // 设置第一列（知识领域标题）- 坐标对应 LoadAnswers 的行分布
            dgvMain[0, 1].Value = "4.项目整合管理";
            dgvMain[0, 3].Value = "5.项目范围管理";
            dgvMain[0, 7].Value = "6.项目进度管理";
            dgvMain[0, 13].Value = "7.项目成本管理";
            dgvMain[0, 16].Value = "8.项目质量管理";
            dgvMain[0, 17].Value = "9.项目资源管理";
            dgvMain[0, 20].Value = "10.项目沟通管理";
            dgvMain[0, 21].Value = "11.项目风险管理";
            dgvMain[0, 26].Value = "12.项目采购管理";
            dgvMain[0, 27].Value = "13.项目干系人管理";

            // 设置标题列只读
            for (int r = 0; r < dgvMain.RowCount; r++) dgvMain[0, r].ReadOnly = true;
            for (int c = 0; c < dgvMain.ColumnCount; c++) dgvMain[c, 0].ReadOnly = true;
        }

        private void LoadAnswers()
        {
            // --- 4. 项目整合管理 (Row 1-2) ---
            correctAnswers[(1, 1)] = "4.1 制定项目章程";
            correctAnswers[(1, 2)] = "4.2 制定项目管理计划";
            correctAnswers[(1, 3)] = "4.3 指导与管理项目工作";
            correctAnswers[(2, 3)] = "4.4 管理项目知识";
            correctAnswers[(1, 4)] = "4.5 监控项目工作";
            correctAnswers[(2, 4)] = "4.6 实施整体变更控制";
            correctAnswers[(1, 5)] = "4.7 结束项目或阶段";

            // --- 5. 项目范围管理 (Row 3-6) ---
            correctAnswers[(3, 2)] = "5.1 规划范围管理";
            correctAnswers[(4, 2)] = "5.2 收集需求";
            correctAnswers[(5, 2)] = "5.3 定义范围";
            correctAnswers[(6, 2)] = "5.4 创建WBS";
            correctAnswers[(3, 4)] = "5.5 确认范围";
            correctAnswers[(4, 4)] = "5.6 控制范围";

            // --- 6. 项目时间管理 (Row 7-12) ---
            correctAnswers[(7, 2)] = "6.1 规划进度管理";
            correctAnswers[(8, 2)] = "6.2 定义活动";
            correctAnswers[(9, 2)] = "6.3 排列活动顺序";
            correctAnswers[(10, 2)] = "6.4 估算活动持续时间";
            correctAnswers[(11, 2)] = "6.5 制定进度计划";
            correctAnswers[(7, 4)] = "6.6 控制进度";

            // --- 7. 项目成本管理 (Row 13-15) ---
            correctAnswers[(13, 2)] = "7.1 规划成本管理";
            correctAnswers[(14, 2)] = "7.2 估算成本";
            correctAnswers[(15, 2)] = "7.3 制定预算";
            correctAnswers[(13, 4)] = "7.4 控制成本";

            // --- 8. 项目质量管理 (Row 16) ---
            correctAnswers[(16, 2)] = "8.1 规划质量管理";
            correctAnswers[(16, 3)] = "8.2 管理质量";
            correctAnswers[(16, 4)] = "8.3 控制质量";

            // --- 9. 人力资源管理 (Row 17-19) ---
            correctAnswers[(17, 2)] = "9.1 规划资源管理";
            correctAnswers[(18, 2)] = "9.2 估算活动资源";
            correctAnswers[(17, 3)] = "9.3 获取资源";
            correctAnswers[(18, 3)] = "9.4 建设团队";
            correctAnswers[(19, 3)] = "9.5 管理团队";
            correctAnswers[(17, 4)] = "9.6 控制资源";

            // --- 10. 项目沟通管理 (Row 20) ---
            correctAnswers[(20, 2)] = "10.1 规划沟通管理";
            correctAnswers[(20, 3)] = "10.2 管理沟通";
            correctAnswers[(20, 4)] = "10.3 监督沟通";

            // --- 11. 项目风险管理 (Row 21-25) ---
            correctAnswers[(21, 2)] = "11.1 规划风险管理";
            correctAnswers[(22, 2)] = "11.2 识别风险";
            correctAnswers[(23, 2)] = "11.3 实施定性风险分析";
            correctAnswers[(24, 2)] = "11.4 实施定量风险分析";
            correctAnswers[(25, 2)] = "11.5 规划风险应对";
            correctAnswers[(21, 3)] = "11.6 实施风险应对";
            correctAnswers[(21, 4)] = "11.7 监督风险";

            // --- 12. 项目采购管理 (Row 26) ---
            correctAnswers[(26, 2)] = "12.1 规划采购管理";
            correctAnswers[(26, 3)] = "12.2 实施采购";
            correctAnswers[(26, 4)] = "12.3 控制采购";

            // --- 13. 项目干系人管理 (Row 27) ---
            correctAnswers[(27, 1)] = "13.1 识别干系人";
            correctAnswers[(27, 2)] = "13.2 规划干系人管理";
            correctAnswers[(27, 3)] = "13.3 管理干系人参与";
            correctAnswers[(27, 4)] = "13.4 监督干系人参与";
        }

        private string Parse(string content)
        {
            var match = Regex.Match(content, @"(?<=\d{1,2}\.\d\ s*).+");
            // 如果成功，傳回文字；否則傳回空字串
            return match.Success ? match.Value.Trim() : string.Empty;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var answer = 0;
            foreach (var entry in correctAnswers)
            {
                var cell = dgvMain[entry.Key.Item2, entry.Key.Item1];
                string input = cell.Value?.ToString()?.Trim();
                if (Parse(entry.Value) != input)
                    cell.Style.BackColor = Color.Red;
                else
                {
                    cell.Style.BackColor = Color.White;
                    answer++;
                }
            }
            label1.Text = $"{answer}/{correctAnswers.Count}";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            foreach (var entry in correctAnswers)
            {
                var cell = dgvMain[entry.Key.Item2, entry.Key.Item1];
                cell.Value = "";
                cell.Style.BackColor = Color.White;
                cell.Style.ForeColor = Color.Black;
            }
            isShowingAnswer = false;
            btnAnswer.Text = "答案";
        }

        private void btnShowAnswer_Click(object sender, EventArgs e)
        {
            if (!isShowingAnswer)
            {
                userCache.Clear();
                foreach (var entry in correctAnswers)
                {
                    var cell = dgvMain[entry.Key.Item2, entry.Key.Item1];
                    userCache[entry.Key] = cell.Value?.ToString();
                    cell.Value = entry.Value;
                    cell.Style.ForeColor = Color.Blue;
                }
                btnAnswer.Text = "恢复";
            }
            else
            {
                foreach (var entry in correctAnswers)
                {
                    var cell = dgvMain[entry.Key.Item2, entry.Key.Item1];
                    cell.Value = userCache.ContainsKey(entry.Key) ? userCache[entry.Key] : "";
                    cell.Style.ForeColor = Color.Black;
                }
                btnAnswer.Text = "答案";
            }
            isShowingAnswer = !isShowingAnswer;
        }

        private void dgvMain_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            //// 1. 处理合并效果与标题颜色
            //Brush backBrush = Brushes.White;

            //// 列标题颜色 (第0行)
            //if (e.RowIndex == 0)
            //{
            //    if (e.ColumnIndex == 0) backBrush = new SolidBrush(ColorHeaderBlue);
            //    else if (e.ColumnIndex == 1) backBrush = new SolidBrush(ColorProcessYellow);
            //    else if (e.ColumnIndex == 2) backBrush = new SolidBrush(ColorHeaderGreen);
            //    else if (e.ColumnIndex == 3) backBrush = new SolidBrush(ColorProcessBlue);
            //    else if (e.ColumnIndex == 4) backBrush = new SolidBrush(ColorProcessRed);
            //    else if (e.ColumnIndex == 5) backBrush = new SolidBrush(ColorProcessGray);
            //}
            //// 行标题颜色 (第0列)
            //else if (e.ColumnIndex == 0)
            //{
            //    if (e.RowIndex >= 1 && e.RowIndex <= 2) backBrush = new SolidBrush(ColorHeaderBlue);
            //    else if (e.RowIndex >= 3 && e.RowIndex <= 6) backBrush = new SolidBrush(ColorHeaderYellow);
            //    else if (e.RowIndex >= 7 && e.RowIndex <= 12) backBrush = new SolidBrush(ColorHeaderGreen);
            //    else if (e.RowIndex >= 13 && e.RowIndex <= 15) backBrush = new SolidBrush(ColorHeaderGreen);
            //    else if (e.RowIndex == 16) backBrush = new SolidBrush(ColorHeaderGreen);
            //    else if (e.RowIndex >= 17 && e.RowIndex <= 19) backBrush = new SolidBrush(ColorHeaderYellow);
            //    else if (e.RowIndex == 20) backBrush = new SolidBrush(ColorHeaderYellow);
            //    else if (e.RowIndex >= 21 && e.RowIndex <= 25) backBrush = new SolidBrush(ColorHeaderYellow);
            //    else if (e.RowIndex == 26) backBrush = new SolidBrush(ColorHeaderYellow);
            //    else if (e.RowIndex == 27) backBrush = new SolidBrush(ColorHeaderYellow);
            //}

            //// 绘制背景
            //e.Graphics.FillRectangle(backBrush, e.CellBounds);

            //// 绘制内容 (处理跨行标题的文本显示逻辑)
            //// 只有合并区域的第一格才绘制文本
            //bool shouldPaintText = true;
            //if (e.ColumnIndex == 0 && e.RowIndex > 1)
            //{
            //    if (string.IsNullOrEmpty(dgvMain[e.ColumnIndex, e.RowIndex].Value?.ToString())) shouldPaintText = false;
            //}

            //if (shouldPaintText)
            //    e.PaintContent(e.CellBounds);

            //// 2. 模拟合并边框 (只在非合并边界画线)
            //Pen blackPen = new Pen(Color.Black, 1);
            //Rectangle rect = e.CellBounds;

            //// 默认画四边
            //e.Graphics.DrawLine(blackPen, rect.Left, rect.Top, rect.Right, rect.Top); // 上
            //e.Graphics.DrawLine(blackPen, rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1); // 下
            //e.Graphics.DrawLine(blackPen, rect.Left, rect.Top, rect.Left, rect.Bottom); // 左
            //e.Graphics.DrawLine(blackPen, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom); // 右
            MergeCell(e);
        }

        private void MergeCell(DataGridViewCellPaintingEventArgs e)
        {
            foreach (var item in mergeCells)
            {
                if (item.Key.Item1 == e.ColumnIndex)
                {
                    if (item.Key.Item2 == e.RowIndex)
                    {
                        using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                        using (Pen gridLinePen = new Pen(dgvMain.GridColor))
                        {
                            // 1. 擦除原本內容
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                            // 2. 繪製左右邊框
                            if (e.ColumnIndex == 0)
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

                            // 4. 繪製文字 (只在起始行繪製，計算兩行總高度)
                            Rectangle mergedArea = e.CellBounds;
                            mergedArea.Height = (item.Key.Item3 - item.Key.Item2) * mergedArea.Height;

                            TextRenderer.DrawText(e.Graphics, item.Value, e.CellStyle.Font,
                                mergedArea, e.CellStyle.ForeColor,
                                TextFormatFlags.Left | TextFormatFlags.Top);
                        }

                        // 5. 阻止系統重複繪製
                        e.Handled = true;
                    }
                    else if (item.Key.Item3 == e.RowIndex)
                    {
                        using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                        using (Pen gridLinePen = new Pen(dgvMain.GridColor))
                        {
                            // 1. 擦除原本內容
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                            // 2. 繪製左右邊框
                            if (e.ColumnIndex == 0)
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

                            // 3. 根據行位置繪製頂/底橫線
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                        }

                        // 5. 阻止系統重複繪製
                        e.Handled = true;
                    }
                    else if (e.RowIndex > item.Key.Item2 && e.RowIndex < item.Key.Item3)
                    {
                        using (Brush backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                        using (Pen gridLinePen = new Pen(dgvMain.GridColor))
                        {
                            // 1. 擦除原本內容
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);

                            // 2. 繪製左右邊框
                            if (e.ColumnIndex == 0)
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                        }

                        // 5. 阻止系統重複繪製
                        e.Handled = true;
                    }
                }
            }
        }
    }
}

namespace PMPQuizApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvMain;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAnswer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvMain = new DataGridView();
            btnSubmit = new Button();
            btnClear = new Button();
            btnAnswer = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvMain).BeginInit();
            SuspendLayout();
            // 
            // dgvMain
            // 
            dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMain.Location = new Point(14, 15);
            dgvMain.Margin = new Padding(4);
            dgvMain.Name = "dgvMain";
            dgvMain.RowTemplate.Height = 23;
            dgvMain.Size = new Size(1041, 665);
            dgvMain.TabIndex = 0;
            dgvMain.CellPainting += dgvMain_CellPainting;
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new Point(689, 696);
            btnSubmit.Margin = new Padding(4);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(105, 38);
            btnSubmit.TabIndex = 1;
            btnSubmit.Text = "提交";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(806, 696);
            btnClear.Margin = new Padding(4);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(105, 38);
            btnClear.TabIndex = 2;
            btnClear.Text = "清空";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnAnswer
            // 
            btnAnswer.Location = new Point(923, 696);
            btnAnswer.Margin = new Padding(4);
            btnAnswer.Name = "btnAnswer";
            btnAnswer.Size = new Size(105, 38);
            btnAnswer.TabIndex = 3;
            btnAnswer.Text = "答案";
            btnAnswer.UseVisualStyleBackColor = true;
            btnAnswer.Click += btnShowAnswer_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label1.ForeColor = Color.Red;
            label1.Location = new Point(70, 700);
            label1.Name = "label1";
            label1.Size = new Size(0, 25);
            label1.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1068, 753);
            Controls.Add(label1);
            Controls.Add(btnAnswer);
            Controls.Add(btnClear);
            Controls.Add(btnSubmit);
            Controls.Add(dgvMain);
            Margin = new Padding(4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PMP项目管理过程组记忆练习";
            ((System.ComponentModel.ISupportInitialize)dgvMain).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
    }
}
