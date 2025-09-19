using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PMQuanLyHangTonKho.Lib;

namespace PMQuanLyHangTonKho.Views.muaban
{
    public partial class frmBaoCao : Form
    {
        private DataGridView dtgvTop10;  
        private Label lblTitle;

        private readonly string[] _top10Headers = { "STT", "Mã hàng", "Tên hàng", "SL bán", "Doanh thu" };
        private readonly int[] _top10Widths = { 60, 120, 260, 110, 140 };

        public frmBaoCao()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            Load += (s, e) => { EnsureControls(); RefreshReport(); EnsureChartLayout(); };
            Resize += (s, e) => LayoutAuto();
        }

        private void EnsureControls()
        {
            chartRevenue = chartRevenue ?? Controls.Find("chartRevenue", true).OfType<Chart>().FirstOrDefault();
            dtgvTop10 = dtgvTop10 ?? Controls.Find("dtgvTop10", true).OfType<DataGridView>().FirstOrDefault();
            lblTitle = lblTitle ?? Controls.Find("lblTitle", true).OfType<Label>().FirstOrDefault();

            if (lblTitle == null)
            {
                lblTitle = new Label
                {
                    Name = "lblTitle",
                    AutoSize = false,
                    Text = "Tổng quan bán hàng",
                    Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                    ForeColor = Color.FromArgb(32, 32, 32),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                Controls.Add(lblTitle);
            }

            if (chartRevenue == null)
            {
                chartRevenue = new Chart { Name = "chartRevenue", BackColor = Color.White };
                chartRevenue.ChartAreas.Add(new ChartArea("Main"));
                Controls.Add(chartRevenue);
            }
            else if (chartRevenue.ChartAreas.Count == 0)
            {
                chartRevenue.ChartAreas.Add(new ChartArea("Main"));
            }

            if (dtgvTop10 == null)
            {
                dtgvTop10 = new DataGridView
                {
                    Name = "dtgvTop10",
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    RowHeadersVisible = false,
                    BackgroundColor = Color.White
                };
                Controls.Add(dtgvTop10);
            }

            LayoutAuto();
        }

        private void LayoutAuto()
        {
            int margin = 12;
            lblTitle?.SetBounds(margin, margin, ClientSize.Width - margin * 2, 32);
            int topY = (lblTitle?.Bottom ?? 0) + 6;

            int w = ClientSize.Width - margin * 2;
            int hAvail = ClientSize.Height - topY - margin;

            int hChart = (int)Math.Round(hAvail * 0.55);
            if (hChart < 180) hChart = 180;

            chartRevenue?.SetBounds(margin, topY, w, hChart);
            dtgvTop10?.SetBounds(margin, (chartRevenue?.Bottom ?? topY) + 8, w,
                                 ClientSize.Height - ((chartRevenue?.Bottom ?? topY) + 8) - margin);
        }

        private void RefreshReport()
        {
            var dtTop10 = GetTop10Products();  
            LoadTop10ChartByProduct(dtTop10);
            LoadTop10Grid(dtTop10);
            NormalizeChartLayout(chartRevenue);
        }

        private DataTable GetTop10Products()
        {
            var sql = @"
                SELECT TOP 10
                       ROW_NUMBER() OVER (ORDER BY SUM(d.LineAmount) DESC) AS STT,
                       p.Id   AS [Mã hàng],
                       p.Name AS [Tên hàng],
                       SUM(d.Qty)        AS [SL bán],
                       SUM(d.LineAmount) AS [Doanh thu]
                FROM SalesInvoiceDetail d
                INNER JOIN Products p ON p.Id = d.ProductId
                GROUP BY p.Id, p.Name
                ORDER BY SUM(d.LineAmount) DESC";
            return Models.SQL.GetData(sql);
        }

        private void LoadTop10ChartByProduct(DataTable dt)
        {
            if (chartRevenue == null) return;

            chartRevenue.Series.Clear();
            var ca = chartRevenue.ChartAreas[0];

            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisX.LabelStyle.Angle = -20;  
            ca.AxisX.IsMarginVisible = false;

            ca.AxisY.MajorGrid.Enabled = true;
            ca.AxisY.LabelStyle.Format = "#,0";

            var s = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelFormat = "#,0",
                IsVisibleInLegend = false
            };

            foreach (DataRow r in dt.Rows)
            {
                string name = Convert.ToString(r["Tên hàng"]);
                double val = Convert.ToDouble(r["Doanh thu"]);
                var p = new DataPoint { YValues = new[] { val } };
                p.AxisLabel = name;         
                p.ToolTip = $"{name}: {val:#,0}";
                s.Points.Add(p);
            }

            chartRevenue.Series.Add(s);
            chartRevenue.Legends.Clear();    
        }

        private void LoadTop10Grid(DataTable dt)
        {
            if (dtgvTop10 == null) return;

            CssDatagridview.LoadData(null, dtgvTop10, "SELECT * FROM (" + ToInlineSql(dt) + ") t", _top10Headers, _top10Widths);

            // Định dạng số
            if (dtgvTop10.Columns.Contains("SL bán"))
                dtgvTop10.Columns["SL bán"].DefaultCellStyle.Format = "#,0";
            if (dtgvTop10.Columns.Contains("Doanh thu"))
                dtgvTop10.Columns["Doanh thu"].DefaultCellStyle.Format = "#,0";
        }

        private static string ToInlineSql(DataTable dt)
        {
            var rows = dt.AsEnumerable().Select(r =>
                $"SELECT {r["STT"]} AS [STT], " +
                $"N'{r["Mã hàng"].ToString().Replace("'", "''")}' AS [Mã hàng], " +
                $"N'{r["Tên hàng"].ToString().Replace("'", "''")}' AS [Tên hàng], " +
                $"{Convert.ToDecimal(r["SL bán"])} AS [SL bán], " +
                $"{Convert.ToDecimal(r["Doanh thu"])} AS [Doanh thu]"
            );
            return string.Join(" UNION ALL ", rows);
        }

        private void EnsureChartLayout()
        {
            foreach (var ch in Controls.OfType<Chart>())
                NormalizeChartLayout(ch);
        }

        private void NormalizeChartLayout(Chart ch)
        {
            if (ch == null || ch.ChartAreas.Count == 0) return;

            ch.Legends.Clear();
            foreach (Series ss in ch.Series) ss.IsVisibleInLegend = false;

            var ca = ch.ChartAreas[0];
            ca.Position.Auto = false;
            ca.Position = new ElementPosition(0f, 0f, 100f, 100f);

            ca.InnerPlotPosition.Auto = false;
            ca.InnerPlotPosition = new ElementPosition(5f, 5f, 92f, 90f);

            ca.AxisX.IsMarginVisible = false;
            ca.RecalculateAxesScale();
        }
    }
}
