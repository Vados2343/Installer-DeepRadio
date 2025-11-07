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
        private bool isHovered = false;
        private int hoverAnimationProgress = 0;
        private System.Windows.Forms.Timer animationTimer;

        public CustomButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.FromArgb(45, 45, 48); // Современный темный фон
            this.ForeColor = Color.White; // Белый текст
            this.Font = new Font("Segoe UI", 10, FontStyle.Regular); // Более современный шрифт
            this.Size = new Size(100, 40); // Размер кнопки
            this.DefaultBackColor = this.BackColor;
            this.Cursor = Cursors.Hand; // Курсор руки при наведении

            // Настройка анимации
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 15;
            animationTimer.Tick += AnimationTimer_Tick;

            this.MouseEnter += CustomButton_MouseEnter;
            this.MouseLeave += CustomButton_MouseLeave;
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (isHovered && hoverAnimationProgress < 100)
            {
                hoverAnimationProgress += 10;
                this.Invalidate();
            }
            else if (!isHovered && hoverAnimationProgress > 0)
            {
                hoverAnimationProgress -= 10;
                this.Invalidate();
            }
            else
            {
                animationTimer.Stop();
            }
        }

        private void CustomButton_MouseEnter(object sender, EventArgs e)
        {
            isHovered = true;
            animationTimer.Start();

            if (this.HoverBackColor != Color.Empty)
            {
                this.BackColor = this.HoverBackColor;
            }
            else
            {
                // Светлее на 20 единиц
                this.BackColor = Color.FromArgb(
                    Math.Min(255, this.DefaultBackColor.R + 20),
                    Math.Min(255, this.DefaultBackColor.G + 20),
                    Math.Min(255, this.DefaultBackColor.B + 20)
                );
            }
        }

        private void CustomButton_MouseLeave(object sender, EventArgs e)
        {
            isHovered = false;
            animationTimer.Start();
            this.BackColor = this.DefaultBackColor;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Создаем путь с закругленными углами
            GraphicsPath path = new GraphicsPath();
            int radius = 12; // Увеличен радиус для более современного вида
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius - 1, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius - 1, this.Height - radius - 1, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius - 1, radius, radius, 90, 90);
            path.CloseFigure();

            // Применение формы к кнопке
            this.Region = new Region(path);

            // Рисование тени (эффект глубины)
            if (hoverAnimationProgress > 0)
            {
                int shadowOffset = (int)(3 * (hoverAnimationProgress / 100.0));
                using (GraphicsPath shadowPath = new GraphicsPath())
                {
                    shadowPath.AddArc(shadowOffset, shadowOffset, radius, radius, 180, 90);
                    shadowPath.AddArc(this.Width - radius - 1 + shadowOffset, shadowOffset, radius, radius, 270, 90);
                    shadowPath.AddArc(this.Width - radius - 1 + shadowOffset, this.Height - radius - 1 + shadowOffset, radius, radius, 0, 90);
                    shadowPath.AddArc(shadowOffset, this.Height - radius - 1 + shadowOffset, radius, radius, 90, 90);
                    shadowPath.CloseFigure();

                    using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                    {
                        g.FillPath(shadowBrush, shadowPath);
                    }
                }
            }

            // Рисование градиента для кнопки
            using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                this.ClientRectangle,
                this.BackColor,
                Color.FromArgb(
                    Math.Max(0, this.BackColor.R - 15),
                    Math.Max(0, this.BackColor.G - 15),
                    Math.Max(0, this.BackColor.B - 15)
                ),
                LinearGradientMode.Vertical))
            {
                g.FillPath(gradientBrush, path);
            }

            // Рисование обводки при наведении с анимацией
            if (hoverAnimationProgress > 0)
            {
                int alpha = (int)(150 * (hoverAnimationProgress / 100.0));
                using (Pen pen = new Pen(Color.FromArgb(alpha, 100, 180, 255), 2))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Добавляем блик сверху
            if (hoverAnimationProgress > 0)
            {
                Rectangle highlightRect = new Rectangle(radius / 2, 1, this.Width - radius, this.Height / 3);
                using (LinearGradientBrush highlightBrush = new LinearGradientBrush(
                    highlightRect,
                    Color.FromArgb((int)(50 * (hoverAnimationProgress / 100.0)), 255, 255, 255),
                    Color.FromArgb(0, 255, 255, 255),
                    LinearGradientMode.Vertical))
                {
                    g.FillRectangle(highlightBrush, highlightRect);
                }
            }

            // Рисование текста с тенью
            if (hoverAnimationProgress > 20)
            {
                // Тень текста
                TextRenderer.DrawText(g, this.Text, this.Font,
                    new Rectangle(this.ClientRectangle.X + 1, this.ClientRectangle.Y + 1, this.ClientRectangle.Width, this.ClientRectangle.Height),
                    Color.FromArgb(100, 0, 0, 0),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            // Основной текст
            TextRenderer.DrawText(g, this.Text, this.Font, this.ClientRectangle, this.ForeColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
