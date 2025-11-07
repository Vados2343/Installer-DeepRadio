using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Setup_RadioPlayer
{
    public class BetaButton : Button
    {
        private Timer animationTimer;
        private Timer colorPhaseTimer;
        private int colorOffset = 0; // Смещение цвета для анимации
        private bool isYellowPhase = false; // Флаг, указывающий на фазу полного желтого цвета
        private bool isBluePhase = false; // Флаг для полной синей фазы

        public BetaButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackColor = Color.Transparent;
            this.Font = new Font("Arial", 8, FontStyle.Bold);
            this.Text = "BETA";
            this.Size = new Size(370, 20);
            this.TextAlign = ContentAlignment.MiddleCenter;

            // Отключаем рисование фона и границы
            this.SetStyle(ControlStyles.Opaque, true);

            // Инициализируем таймер для анимации
            animationTimer = new Timer();
            animationTimer.Interval = 1000; // Скорость анимации
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();

            // Инициализируем таймер для смены цветовой фазы
            colorPhaseTimer = new Timer();
            colorPhaseTimer.Interval = 5000; // Фаза длится 5 секунд
            colorPhaseTimer.Tick += ColorPhaseTimer_Tick;
            colorPhaseTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (!isYellowPhase && !isBluePhase) // Если не в фазе полного цвета, двигаем анимацию
            {
                // Декрементируем смещение цвета для противоположного движения
                colorOffset = (colorOffset - 1 + this.Text.Length) % this.Text.Length;
                this.Invalidate(); // Перерисовываем кнопку
            }
        }

        private void ColorPhaseTimer_Tick(object sender, EventArgs e)
        {
            if (!isYellowPhase)
            {
                // Переход в фазу полного желтого цвета
                isYellowPhase = true;
                isBluePhase = false;
            }
            else if (!isBluePhase)
            {
                // Переход в фазу полного синего цвета
                isBluePhase = true;
                isYellowPhase = false;
            }
            else
            {
                // Возврат к обычной анимации "две буквы синего и две желтого"
                isYellowPhase = false;
                isBluePhase = false;
            }

            this.Invalidate(); // Перерисовываем кнопку
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Создаем путь для трапециевидной формы
            GraphicsPath path = new GraphicsPath();
            int width = this.Width;
            int height = this.Height;

            // Точки для создания трапеции
            Point[] points = new Point[]
            {
                new Point(0, 0),                // Верхний левый угол
                new Point(width, 0),            // Верхний правый угол
                new Point(width - 30, height),  // Нижний правый угол
                new Point(30, height)           // Нижний левый угол
            };
            path.AddPolygon(points);

            // Применяем форму к кнопке
            this.Region = new Region(path);

            // Рисуем кнопку с темным фоном
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(50, 50, 50))) // Темный фон
            {
                g.FillPath(brush, path);
            }

            // Рисуем текст
            string text = this.Text;
            int letterWidth = this.Width / text.Length;

            for (int i = 0; i < text.Length; i++)
            {
                Color animatedColor;

                // Определяем цвет на основе текущей фазы
                if (isYellowPhase)
                {
                    animatedColor = Color.Yellow;
                }
                else if (isBluePhase)
                {
                    animatedColor = Color.SkyBlue;
                }
                else
                {
                    // Анимация "две буквы синего и две желтого"
                    int position = (i + colorOffset + text.Length) % text.Length;
                    if ((position / 2) % 2 == 0)
                    {
                        animatedColor = Color.Blue;
                    }
                    else
                    {
                        animatedColor = Color.Yellow;
                    }
                }

                // Рисуем текст с текущим цветом
                using (Font font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Bold))
                {
                    Rectangle letterRect = new Rectangle(i * letterWidth, 0, letterWidth, this.Height);
                    TextRenderer.DrawText(g, text[i].ToString(), font, letterRect, animatedColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
        }
    }
}
