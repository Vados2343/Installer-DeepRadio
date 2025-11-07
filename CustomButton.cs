using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Setup_RadioPlayer
{
    public class CustomButton : Button
    {
        public Color HoverBackColor { get; set; }
        public Color DefaultBackColor { get; set; }

        public CustomButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.FromArgb(45, 45, 48); // Современный темный фон
            this.ForeColor = Color.White; // Белый текст
            this.Font = new Font("Arial", 10, FontStyle.Regular);
            this.Size = new Size(100, 40); // Размер кнопки
            this.DefaultBackColor = this.BackColor;

            this.MouseEnter += CustomButton_MouseEnter;
            this.MouseLeave += CustomButton_MouseLeave;
        }

        private void CustomButton_MouseEnter(object sender, EventArgs e)
        {
            if (this.HoverBackColor != Color.Empty)
            {
                this.BackColor = this.HoverBackColor;
            }
        }

        private void CustomButton_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = this.DefaultBackColor;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Создаем путь с закругленными углами
            GraphicsPath path = new GraphicsPath();
            int radius = 10;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            // Применение формы к кнопке
            this.Region = new Region(path);

            // Рисование кнопки
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                g.FillPath(brush, path);
            }

            // Рисование обводки при наведении
            if (this.ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                using (Pen pen = new Pen(Color.FromArgb(150, Color.White), 2))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Рисование текста
            TextRenderer.DrawText(g, this.Text, this.Font, this.ClientRectangle, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}
