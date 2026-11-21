using System.Drawing.Drawing2D;

namespace AndrewHull;

public class MainForm : Form
{
    private readonly List<Point> points = new List<Point>();
    private List<Point> hull = new List<Point>();

    public MainForm()
    {
        Text = "Выпуклая оболочка — метод Эндрю";
        ClientSize = new Size(900, 600);
        MinimumSize = new Size(700, 400);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.White;
        DoubleBuffered = true;

        FlowLayoutPanel toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(8),
            BackColor = SystemColors.Control
        };

        Button buildButton = new Button
        {
            Text = "Построить оболочку",
            AutoSize = true
        };
        buildButton.Click += BuildButton_Click;

        Button clearButton = new Button
        {
            Text = "Очистить",
            AutoSize = true
        };
        clearButton.Click += ClearButton_Click;

        Label hint = new Label
        {
            Text = "Щёлкните по белому полю, чтобы добавить точку.",
            AutoSize = true,
            Margin = new Padding(12, 8, 3, 3)
        };

        toolbar.Controls.Add(buildButton);
        toolbar.Controls.Add(clearButton);
        toolbar.Controls.Add(hint);
        Controls.Add(toolbar);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.Button != MouseButtons.Left || points.Contains(e.Location))
        {
            return;
        }

        points.Add(e.Location);
        hull.Clear();
        Invalidate();
    }

    private void BuildButton_Click(object? sender, EventArgs e)
    {
        hull = ConvexHull.Build(points);
        Invalidate();
    }

    private void ClearButton_Click(object? sender, EventArgs e)
    {
        points.Clear();
        hull.Clear();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        using Pen hullPen = new Pen(Color.RoyalBlue, 2);

        if (hull.Count >= 3)
        {
            e.Graphics.DrawPolygon(hullPen, hull.ToArray());
        }
        else if (hull.Count == 2)
        {
            e.Graphics.DrawLine(hullPen, hull[0], hull[1]);
        }

        foreach (Point point in points)
        {
            e.Graphics.FillEllipse(Brushes.Black, point.X - 4, point.Y - 4, 8, 8);
        }
    }
}
