using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using IWshRuntimeLibrary;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Threading;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Drawing.Drawing2D;

namespace Setup_RadioPlayer
{
    public partial class MainForm : Form
    {
        private Panel panelWelcome;
        private Panel panelLicense;
        private Panel panelInstallPath;
        private Panel panelOptions;
        private Panel panelProgress;
        private Panel panelFinish;
        private CustomButton btnNext;
        private CustomButton btnBack;
        private CustomButton btnClose;
        private CustomButton btnMinimize;
        private Label lblTitle; // Заголовок установщика
        private System.Windows.Forms.Timer betaButtonTimer;
        private int betaButtonAnimationStep = 0;
        private bool betaButtonAnimatingIn = true;
        private BetaButton btnBeta;
        private string installPath;
        private bool agreeToLicense = false;
        private bool createDesktopShortcut = false;
        private bool createStartMenuShortcut = false;
        private bool runAtStartup = false;
        private bool visitWebsiteAfterInstall = false;
        private bool launchAppAfterInstall = true;

        private ProgressBar progressBar;
        private Label lblStatus;

        private string logFilePath;

        // Конструктор формы
        public MainForm()
        {
            if (!IsUserAdministrator())
            {
                // Перезапуск от имени администратора
                RestartAsAdministrator();
                return;
            }

            // Настройки формы
            this.Text = "Установник RadioPlayer";
            this.Size = new Size(800, 600);
            this.FormBorderStyle = FormBorderStyle.None; // Убираем стандартные границы
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = Properties.Resources.installer;

            // Устанавливаем фоновое изображение
            this.BackgroundImage = Properties.Resources.background1;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Инициализация лог файла
            logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install.log");
            Log("Запуск установщика.");

            // Инициализация панелей и элементов управления
            InitializeSharedControls();
            InitializePanels();

            // Показать панель приветствия
            ShowPanel(panelWelcome);
        }

        // Проверка прав администратора
        private bool IsUserAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        // Перезапуск приложения от имени администратора
        private void RestartAsAdministrator()
        {
            ProcessStartInfo proc = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Application.ExecutablePath,
                Verb = "runas" // Запуск от имени администратора
            };

            try
            {
                Process.Start(proc);
            }
            catch
            {
                // Пользователь отказался предоставить права администратора
                MessageBox.Show("Для установки програми необхідні права адміністратора.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.Exit();
        }

        private void InitializePanels()
        {
            // Инициализация каждой панели
            InitializeWelcomePanel();
            InitializeLicensePanel();
            InitializeInstallPathPanel();
            InitializeOptionsPanel();
            InitializeProgressPanel();
            InitializeFinishPanel();
        }
        private void BtnBeta_Click(object sender, EventArgs e)
        {
            // Показать сообщение о том, что это бета-версия
            MessageBox.Show("Це бета-версія програми. Можливі помилки та недоробки.\nЯкщо ви виявили проблему, повідомте нам на адресу support@deepradio.site", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BetaButtonTimer_Tick(object sender, EventArgs e)
        {
            if (betaButtonAnimatingIn)
            {
                if (btnBeta.Left < 0)
                {
                    btnBeta.Left += 5; // Кнопка выезжает слева
                }
                else
                {
                    betaButtonTimer.Stop();
                    betaButtonAnimatingIn = false;
                }
            }
        }

        private void InitializeSharedControls()
        {
            // Верхняя панель с заголовком и кнопками
            Panel topPanel = new Panel();
            topPanel.Size = new Size(this.Width, 40);
            topPanel.Location = new Point(0, 0);
            topPanel.BackColor = Color.FromArgb(45, 45, 48);
            topPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Иконка слева от заголовка
            PictureBox iconBox = new PictureBox();
            iconBox.Image = Properties.Resources.installer.ToBitmap();
            iconBox.SizeMode = PictureBoxSizeMode.StretchImage;
            iconBox.Size = new Size(24, 24);
            iconBox.Location = new Point(10, 8);

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "Установник RadioPlayer";
            lblTitle.Font = new Font("Arial", 12, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Location = new Point(40, 10);

            // Кастомная кнопка "Закрити"
            btnClose = new CustomButton();
            btnClose.Size = new Size(30, 30);
            btnClose.Location = new Point(this.Width - 45, 5); // Смещаем на 5 пикселей влево
            btnClose.Text = "X";
            btnClose.Font = new Font("Arial", 12, FontStyle.Bold);
            btnClose.BackColor = Color.FromArgb(45, 45, 48);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.HoverBackColor = Color.Red;
            btnClose.Click += BtnClose_Click;
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Кастомная кнопка "Згорнути"
            btnMinimize = new CustomButton();
            btnMinimize.Size = new Size(30, 30);
            btnMinimize.Location = new Point(this.Width - 80, 5); // Исправляем позицию
            btnMinimize.Text = "_";
            btnMinimize.Font = new Font("Arial", 12, FontStyle.Bold);
            btnMinimize.BackColor = Color.FromArgb(45, 45, 48);
            btnMinimize.ForeColor = Color.White;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.Click += (s, e) => { this.WindowState = FormWindowState.Minimized; };
            btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Добавляем элементы в верхнюю панель
            topPanel.Controls.Add(iconBox);
            topPanel.Controls.Add(lblTitle);
            topPanel.Controls.Add(btnClose);
            topPanel.Controls.Add(btnMinimize);

            // Добавляем верхнюю панель на форму
            this.Controls.Add(topPanel);
            btnBeta = new BetaButton();
            btnBeta.Location = new Point((this.Width - btnBeta.Width) / 2, 40);
            btnBeta.Click += BtnBeta_Click;
            this.Controls.Add(btnBeta);
            betaButtonTimer = new System.Windows.Forms.Timer();
            betaButtonTimer.Interval = 10;
            betaButtonTimer.Tick += BetaButtonTimer_Tick;
            betaButtonTimer.Start();

            topPanel.MouseDown += MainForm_MouseDown;
            lblTitle.MouseDown += MainForm_MouseDown;
            iconBox.MouseDown += MainForm_MouseDown;

            // Кастомная кнопка "Далі"
            btnNext = new CustomButton();
            btnNext.Text = "Далі";
            btnNext.Size = new Size(90, 30);  // Скорректирован размер кнопки
            btnNext.Location = new Point(this.Width - 120, this.Height - 80);  // Скорректирована позиция кнопки
            btnNext.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnNext.Click += BtnNext_Click;
            this.Controls.Add(btnNext);

            // Кастомная кнопка "Назад"
            btnBack = new CustomButton();
            btnBack.Text = "Назад";
            btnBack.Size = new Size(90, 30);  // Скорректирован размер кнопки
            btnBack.Location = new Point(this.Width - 230, this.Height - 80);  // Скорректирована позиция кнопки
            btnBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnBack.Click += BtnBack_Click;
            this.Controls.Add(btnBack);

            // Добавляем ссылки в левую часть нижней панели
            LinkLabel linkSupport = new LinkLabel();
            linkSupport.Text = "Підтримка: support@deepradio.site";
            linkSupport.Font = new Font("Arial", 10);
            linkSupport.LinkColor = Color.White;
            linkSupport.BackColor = Color.Transparent;
            linkSupport.AutoSize = true;
            linkSupport.Location = new Point(10, this.Height - 90);  // Позиция над кнопками
            linkSupport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            linkSupport.LinkClicked += (s, e) => { Process.Start("mailto:support@deepradio.site"); };

            LinkLabel linkWebsite = new LinkLabel();
            linkWebsite.Text = "Сайт: deepradio.site";
            linkWebsite.Font = new Font("Arial", 10);
            linkWebsite.LinkColor = Color.White;
            linkWebsite.BackColor = Color.Transparent;
            linkWebsite.AutoSize = true;
            linkWebsite.Location = new Point(10, this.Height - 70);  // Позиция над поддержкой
            linkWebsite.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            linkWebsite.LinkClicked += (s, e) => { Process.Start("https://deepradio.site"); };

            // Добавляем ссылки на форму
            this.Controls.Add(linkSupport);
            this.Controls.Add(linkWebsite);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Ви дійсно хочете скасувати установку?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // Обработчик для перетаскивания формы
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Реализация перетаскивания формы
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;
                Message m = Message.Create(this.Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref m);
            }
        }

        // Обработчики событий для общих кнопок
        private void BtnNext_Click(object sender, EventArgs e)
        {
            if (panelWelcome.Visible)
            {
                ShowPanel(panelLicense);
            }
            else if (panelLicense.Visible)
            {
                if (agreeToLicense)
                {
                    ShowPanel(panelInstallPath);
                }
                else
                {
                    MessageBox.Show("Ви повинні прийняти ліцензійну угоду для продовження.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (panelInstallPath.Visible)
            {
                if (string.IsNullOrEmpty(installPath))
                {
                    MessageBox.Show("Будь ласка, оберіть коректний шлях установки.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!IsValidInstallPath(installPath))
                {
                    MessageBox.Show("Вибрано некоректний або заборонений шлях установки.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!HasEnoughDiskSpace(installPath))
                {
                    MessageBox.Show("Недостатньо вільного місця на диску.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Directory.Exists(installPath))
                {
                    var result = MessageBox.Show("Папка вже існує. Ви хочете перезаписати її?", "Попередження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        ShowPanel(panelOptions);
                    }
                }
                else
                {
                    ShowPanel(panelOptions);
                }
            }
       
            else if (panelOptions.Visible)
            {
                ShowPanel(panelProgress);
                StartInstallation();
            }
            else if (panelFinish.Visible)
            {
                if (launchAppAfterInstall)
                {
                    LaunchApplication();
                }
                if (visitWebsiteAfterInstall)
                {
                    OpenWebsite();
                }
                this.Close();
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (panelLicense.Visible)
            {
                ShowPanel(panelWelcome);
            }
            else if (panelInstallPath.Visible)
            {
                ShowPanel(panelLicense);
            }
            else if (panelOptions.Visible)
            {
                ShowPanel(panelInstallPath);
            }
        }

        private void ShowPanel(Panel panel)
        {
            // Скрыть все панели
            panelWelcome.Visible = false;
            panelLicense.Visible = false;
            panelInstallPath.Visible = false;
            panelOptions.Visible = false;
            panelProgress.Visible = false;
            panelFinish.Visible = false;

            // Показать выбранную панель
            panel.Visible = true;

            // Изменение фонового изображения для каждой панели
            UpdateBackgroundImage(panel);

            // Настройка видимости кнопок "Назад" и "Далее"
            btnBack.Visible = !panelWelcome.Visible && !panelFinish.Visible && !panelProgress.Visible;
            btnNext.Visible = !panelProgress.Visible;

            // Изменение текста кнопки "Далі" на "Встановити" или "Готово" в зависимости от панели
            if (panelOptions.Visible)
            {
                btnNext.Text = "Встановити";
            }
            else if (panelFinish.Visible)
            {
                btnNext.Text = "Готово";
            }
            else
            {
                btnNext.Text = "Далі";
            }

            // Скрыть кнопку "Назад" на панели приветствия
            if (panelWelcome.Visible)
            {
                btnBack.Visible = false;
            }

            panel.Invalidate();
            panel.Update();
            this.Invalidate();
            this.Update();
        }

        private void UpdateBackgroundImage(Panel panel)
        {
            if (panel == panelWelcome)
            {
                this.BackgroundImage = Properties.Resources.background1;
            }
            else if (panel == panelLicense)
            {
                this.BackgroundImage = Properties.Resources.background2;
            }
            else if (panel == panelInstallPath)
            {
                this.BackgroundImage = Properties.Resources.background3;
            }
            else if (panel == panelOptions)
            {
                this.BackgroundImage = Properties.Resources.background4;
            }
            else if (panel == panelProgress)
            {
                this.BackgroundImage = Properties.Resources.background5;
            }
            else if (panel == panelFinish)
            {
                this.BackgroundImage = Properties.Resources.background6;
            }
            this.BackgroundImageLayout = ImageLayout.Stretch;
            GraphicsPath formPath = new GraphicsPath();
            int radius = 30;
            formPath.AddArc(0, 0, radius, radius, 180, 90);
            formPath.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            formPath.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            formPath.AddArc(0, this.Height - radius, radius, radius, 90, 90);
            formPath.CloseFigure();
            this.Region = new Region(formPath);

            // Добавление эффекта свечения
            this.Paint += MainForm_Paint;
        }
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Рисуем обводку формы
            Rectangle borderRect = new Rectangle(0, 0, this.Width, this.Height);

            using (GraphicsPath path = GetRoundedRectanglePath(borderRect, 30))
            {
                // Градиент от синего (с отступом 40 пикселей сверху) до желтого снизу
                using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                    new Rectangle(0, 40, this.Width, this.Height - 40), // Градиент начинается на высоте 40 пикселей
                    Color.Blue,  // Синий цвет сверху
                    Color.Yellow, // Желтый цвет снизу
                    LinearGradientMode.Vertical))  // Направление градиента сверху вниз
                {
                    using (Pen glowPen = new Pen(gradientBrush, 10))
                    {
                        glowPen.Alignment = PenAlignment.Outset;
                        g.DrawPath(glowPen, path);
                    }
                }
            }
        }

        // Пример метода для создания округленного прямоугольника
        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(rect.Location, size);

            // Верхний левый угол
            path.AddArc(arc, 180, 90);

            // Верхний правый угол
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Нижний правый угол
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Нижний левый угол
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        // Вспомогательный метод для создания пути с закругленными углами

        // Инициализация панелей
        private void InitializeWelcomePanel()
        {
            panelWelcome = new Panel();
            panelWelcome.Size = new Size(this.Width, this.Height - 100);
            panelWelcome.Location = new Point(0, 40); // Сдвиг для верхней панели
            panelWelcome.BackColor = Color.Transparent;

            // Заголовок
            Label lblWelcome = new Label();
            lblWelcome.Text = "Ласкаво просимо до установника RadioPlayer!";
            lblWelcome.Font = new Font("Arial", 24, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.AutoSize = false;
            lblWelcome.BackColor = Color.Transparent;
            lblWelcome.Size = new Size(700, 50);
            lblWelcome.Location = new Point(50, 50);
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Подробное описание
            Label lblDescription = new Label();
            lblDescription.Text = "Ця програма встановить кастомний RadioPlayer на ваш комп'ютер.\nНатисніть 'Далі' для продовження.";
            lblDescription.Font = new Font("Arial", 14);
            lblDescription.ForeColor = Color.White;
            lblDescription.AutoSize = false;
            lblDescription.BackColor = Color.Transparent;
            lblDescription.Size = new Size(700, 100);
            lblDescription.Location = new Point(50, 150); // Сдвинул ниже
            lblDescription.TextAlign = ContentAlignment.MiddleCenter;
            lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            panelWelcome.Controls.Add(lblWelcome);
            panelWelcome.Controls.Add(lblDescription);

            // Добавляем нижнюю панель с ссылками

            this.Controls.Add(panelWelcome);
        }

        private void InitializeLicensePanel()
        {
            panelLicense = new Panel();
            panelLicense.Size = new Size(this.Width, this.Height - 100);
            panelLicense.Location = new Point(0, 80);
            panelLicense.BackColor = Color.Transparent;

            Label lblLicense = new Label();
            lblLicense.Text = "Ліцензійна угода";
            lblLicense.Font = new Font("Arial", 16, FontStyle.Bold);
            lblLicense.ForeColor = Color.White;
            lblLicense.AutoSize = true;
            lblLicense.BackColor = Color.Transparent;
            lblLicense.Location = new Point(50, 60);

            RichTextBox rtbLicense = new RichTextBox();
            rtbLicense.Size = new Size(700, 250);
            rtbLicense.Location = new Point(50, 110);
            rtbLicense.ReadOnly = true;
            rtbLicense.BackColor = Color.White;
            rtbLicense.ForeColor = Color.Black;
            rtbLicense.ScrollBars = RichTextBoxScrollBars.Vertical;

            string licenseText = Properties.Resources.license;
            if (!string.IsNullOrEmpty(licenseText))
            {
                rtbLicense.Text = licenseText;
            }
            else
            {
                rtbLicense.Text = "Ліцензійна угода не знайдена.";
            }

            CheckBox cbAgree = new CheckBox();
            cbAgree.Text = "Я прочитав(ла) та погоджуюся з умовами ліцензійної угоди";
            cbAgree.ForeColor = Color.White;
            cbAgree.BackColor = Color.Transparent;
            cbAgree.AutoSize = true;
            cbAgree.Location = new Point(50, 420);
            cbAgree.CheckedChanged += (s, e) => { agreeToLicense = cbAgree.Checked; };

            panelLicense.Controls.Add(lblLicense);
            panelLicense.Controls.Add(rtbLicense);
            panelLicense.Controls.Add(cbAgree);

            // Добавляем нижнюю панель с ссылками

            this.Controls.Add(panelLicense);
        }

        private void InitializeInstallPathPanel()
        {
            panelInstallPath = new Panel();
            panelInstallPath.Size = new Size(this.Width, this.Height - 100);
            panelInstallPath.Location = new Point(0, 40);
            panelInstallPath.BackColor = Color.Transparent;

            Label lblInstruction = new Label();
            lblInstruction.Text = "Програма встановлення встановить Custom Radioplayer у наступну папку.\nЩоб продовжити, натисніть 'Далі'. Якщо ви хочете вибрати іншу папку, виберіть 'Огляд'.";
            lblInstruction.Font = new Font("Arial", 14);
            lblInstruction.ForeColor = Color.White;
            lblInstruction.AutoSize = false;
            lblInstruction.BackColor = Color.Transparent;
            lblInstruction.Size = new Size(700, 60);
            lblInstruction.Location = new Point(50, 20);
            lblInstruction.TextAlign = ContentAlignment.MiddleLeft;

            TextBox txtPath = new TextBox();
            txtPath.Size = new Size(600, 25);
            txtPath.Location = new Point(50, 100);
            txtPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "RadioPlayer");

            CustomButton btnBrowse = new CustomButton();
            btnBrowse.Text = "Огляд";
            btnBrowse.Size = new Size(80, 25);
            btnBrowse.Location = new Point(660, 100);
            btnBrowse.Click += (s, e) =>
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.SelectedPath = txtPath.Text;
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        txtPath.Text = fbd.SelectedPath;
                    }
                }
            };

            Label lblSpaceInfo = new Label();
            lblSpaceInfo.Text = "";
            lblSpaceInfo.Font = new Font("Arial", 10);
            lblSpaceInfo.ForeColor = Color.White;
            lblSpaceInfo.AutoSize = true;
            lblSpaceInfo.BackColor = Color.Transparent;
            lblSpaceInfo.Location = new Point(50, 140);

            txtPath.TextChanged += (s, e) =>
            {
                installPath = txtPath.Text;
                UpdateSpaceInfo(lblSpaceInfo, installPath);
            };

            // Инициализация переменных
            installPath = txtPath.Text;
            UpdateSpaceInfo(lblSpaceInfo, installPath);

            panelInstallPath.Controls.Add(lblInstruction);
            panelInstallPath.Controls.Add(txtPath);
            panelInstallPath.Controls.Add(btnBrowse);
            panelInstallPath.Controls.Add(lblSpaceInfo);

            // Добавляем нижнюю панель с ссылками

            this.Controls.Add(panelInstallPath);
        }

        private void InitializeOptionsPanel()
        {
            panelOptions = new Panel();
            panelOptions.Size = new Size(this.Width, this.Height - 100);
            panelOptions.Location = new Point(0, 40);
            panelOptions.BackColor = Color.Transparent;

            Label lblOptions = new Label();
            lblOptions.Text = "Оберіть додаткові опції установки:";
            lblOptions.Font = new Font("Arial", 16, FontStyle.Bold);
            lblOptions.ForeColor = Color.White;
            lblOptions.AutoSize = true;
            lblOptions.BackColor = Color.Transparent;
            lblOptions.Location = new Point(50, 40);

            // Чекбоксы в два столбца
            CheckBox cbLaunchAfterInstall = new CheckBox();
            cbLaunchAfterInstall.Text = "Запустити програму після встановлення";
            cbLaunchAfterInstall.ForeColor = Color.White;
            cbLaunchAfterInstall.BackColor = Color.Transparent;
            cbLaunchAfterInstall.AutoSize = true;
            cbLaunchAfterInstall.Location = new Point(50, 190);
            cbLaunchAfterInstall.Checked = true;
            cbLaunchAfterInstall.CheckedChanged += (s, e) => { launchAppAfterInstall = cbLaunchAfterInstall.Checked; };

            CheckBox cbVisitWebsite = new CheckBox();
            cbVisitWebsite.Text = "Відвідати сайт";
            cbVisitWebsite.ForeColor = Color.White;
            cbVisitWebsite.BackColor = Color.Transparent;
            cbVisitWebsite.AutoSize = true;
            cbVisitWebsite.Location = new Point(50, 210);
            cbVisitWebsite.CheckedChanged += (s, e) => { visitWebsiteAfterInstall = cbVisitWebsite.Checked; };

            CheckBox cbCreateDesktopShortcut = new CheckBox();
            cbCreateDesktopShortcut.Text = "Створити ярлик на робочому столі";
            cbCreateDesktopShortcut.ForeColor = Color.White;
            cbCreateDesktopShortcut.BackColor = Color.Transparent;
            cbCreateDesktopShortcut.AutoSize = true;
            cbCreateDesktopShortcut.Location = new Point(470, 210);
            cbCreateDesktopShortcut.CheckedChanged += (s, e) => { createDesktopShortcut = cbCreateDesktopShortcut.Checked; };

            CheckBox cbCreateStartMenuShortcut = new CheckBox();
            cbCreateStartMenuShortcut.Text = "Створити ярлик в меню Пуск";
            cbCreateStartMenuShortcut.ForeColor = Color.White;
            cbCreateStartMenuShortcut.BackColor = Color.Transparent;
            cbCreateStartMenuShortcut.AutoSize = true;
            cbCreateStartMenuShortcut.Location = new Point(470, 190);
            cbCreateStartMenuShortcut.CheckedChanged += (s, e) => { createStartMenuShortcut = cbCreateStartMenuShortcut.Checked; };

            CheckBox cbRunAtStartup = new CheckBox();
            cbRunAtStartup.Text = "Запускати програму разом з Windows";
            cbRunAtStartup.ForeColor = Color.White;
            cbRunAtStartup.BackColor = Color.Transparent;
            cbRunAtStartup.AutoSize = true;
            cbRunAtStartup.Location = new Point(50, 230);
            cbRunAtStartup.CheckedChanged += (s, e) => { runAtStartup = cbRunAtStartup.Checked; };
            cbLaunchAfterInstall.Font = new Font("Arial", 12);
            cbVisitWebsite.Font = new Font("Arial", 12);
            cbCreateDesktopShortcut.Font = new Font("Arial", 12);
            cbCreateStartMenuShortcut.Font = new Font("Arial", 12);
            cbRunAtStartup.Font = new Font("Arial", 12);
            panelOptions.Controls.Add(lblOptions);
            panelOptions.Controls.Add(cbLaunchAfterInstall);
            panelOptions.Controls.Add(cbVisitWebsite);
            panelOptions.Controls.Add(cbCreateDesktopShortcut);
            panelOptions.Controls.Add(cbCreateStartMenuShortcut);
            panelOptions.Controls.Add(cbRunAtStartup);

            // Добавляем нижнюю панель с ссылками
            this.Controls.Add(panelOptions);
        }
        private void AnimateProgressBar(int targetValue)
        {
            Task.Run(() =>
            {
                while (progressBar.Value < targetValue)
                {
                    Invoke(new Action(() =>
                    {
                        if (progressBar.Value < targetValue)
                        {
                            progressBar.Value++;
                        }
                    }));
                    Thread.Sleep(10); // Скорость анимации
                }
            });
        }
        private void InitializeProgressPanel()
        {
            panelProgress = new Panel();
            panelProgress.Size = new Size(this.Width, this.Height - 100);
            panelProgress.Location = new Point(0, 40);
            panelProgress.BackColor = Color.Transparent;

            Label lblProgress = new Label();
            lblProgress.Text = "Виконується установка, будь ласка зачекайте...";
            lblProgress.Font = new Font("Arial", 14, FontStyle.Bold);
            lblProgress.ForeColor = Color.White;
            lblProgress.AutoSize = true;
            lblProgress.BackColor = Color.Transparent;
            lblProgress.Location = new Point(50, 50);

            progressBar = new ProgressBar();
            progressBar.Size = new Size(700, 25);
            progressBar.Location = new Point(50, 90);
            progressBar.Style = ProgressBarStyle.Continuous;

            lblStatus = new Label();
            lblStatus.Text = "";
            lblStatus.Font = new Font("Arial", 12);
            lblStatus.ForeColor = Color.White;
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.Transparent;
            lblStatus.Location = new Point(50, 130);

            panelProgress.Controls.Add(lblProgress);
            panelProgress.Controls.Add(progressBar);
            panelProgress.Controls.Add(lblStatus);

            // Добавляем панель на форму
            this.Controls.Add(panelProgress);
        }

        private void InitializeFinishPanel()
        {
            panelFinish = new Panel();
            panelFinish.Size = new Size(this.Width, this.Height - 100);
            panelFinish.Location = new Point(0, 40);
            panelFinish.BackColor = Color.Transparent;

            Label lblFinish = new Label();
            lblFinish.Text = "Дякуємо за встановлення програми RadioPlayer!";
            lblFinish.Font = new Font("Arial", 24, FontStyle.Bold);
            lblFinish.ForeColor = Color.White;
            lblFinish.AutoSize = false;
            lblFinish.BackColor = Color.Transparent;
            lblFinish.Size = new Size(700, 100);
            lblFinish.Location = new Point(50, 50);
            lblFinish.TextAlign = ContentAlignment.MiddleCenter;

            // Предложение запустить программу или посетить сайт
            CheckBox cbLaunchApp = new CheckBox();
            cbLaunchApp.Text = "Запустити програму";
            cbLaunchApp.ForeColor = Color.White;
            cbLaunchApp.BackColor = Color.Transparent;
            cbLaunchApp.AutoSize = true;
            cbLaunchApp.Location = new Point(50, 180);
            cbLaunchApp.Checked = true;
            cbLaunchApp.CheckedChanged += (s, e) => { launchAppAfterInstall = cbLaunchApp.Checked; };

            CheckBox cbVisitWebsite = new CheckBox();
            cbVisitWebsite.Text = "Відвідати сайт";
            cbVisitWebsite.ForeColor = Color.White;
            cbVisitWebsite.BackColor = Color.Transparent;
            cbVisitWebsite.AutoSize = true;
            cbVisitWebsite.Location = new Point(50, 210);
            cbVisitWebsite.CheckedChanged += (s, e) => { visitWebsiteAfterInstall = cbVisitWebsite.Checked; };

            panelFinish.Controls.Add(lblFinish);
            panelFinish.Controls.Add(cbLaunchApp);
            panelFinish.Controls.Add(cbVisitWebsite);

            // Добавляем нижнюю панель с ссылками

            this.Controls.Add(panelFinish);
        }
        private async void StartInstallation()
        {
            // Отключаем кнопки во время установки
            btnNext.Enabled = false;
            btnBack.Enabled = false;

            try
            {
                // Шаг 1: Распаковка файлов (0-70%)
                UpdateStatus("Крок 1/4: Розпакування файлів...");
                progressBar.Value = 0;

                await FileManager.ExtractFilesAsync(installPath, (progress) =>
                {
                    // Обновляем прогрессбар (первые 70% - распаковка)
                    Invoke(new Action(() =>
                    {
                        int adjustedProgress = (int)(progress * 0.7);
                        progressBar.Value = adjustedProgress;
                        lblStatus.Text = $"Крок 1/4: Розпакування файлів... {progress}%";
                    }));
                });

                progressBar.Value = 70;

                // Проверка контрольной суммы главного исполняемого файла
                string filePath = Path.Combine(installPath, "data", "RadioPlayerV2.exe");

                if (!System.IO.File.Exists(filePath))
                {
                    throw new Exception($"Файл '{filePath}' не знайдено після розпакування.");
                }

                // Шаг 2: Проверка целостности (70-75%)
                UpdateStatus("Крок 2/4: Перевірка цілісності файлів...");
                progressBar.Value = 72;

                // Примечание: контрольную сумму нужно будет заменить на реальную
                string expectedChecksum = "skip"; // Используем "skip" чтобы пропустить проверку

                if (expectedChecksum != "skip" && !VerifyFileChecksum(filePath, expectedChecksum))
                {
                    var result = MessageBox.Show("Контрольна сума файлу не збігається. Можливо, файл пошкоджений. Ви бажаєте продовжити установку?",
                        "Попередження", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result != DialogResult.Yes)
                    {
                        throw new Exception("Установку скасовано користувачем через невідповідність контрольної суми.");
                    }
                    else
                    {
                        Log("Користувач вирішив продовжити установку незважаючи на попередження.");
                    }
                }
                else if (expectedChecksum == "skip")
                {
                    Log("Перевірка контрольної суми пропущена (не налаштована).");
                }
                else
                {
                    UpdateStatus("Контрольна сума вірна.");
                }

                progressBar.Value = 75;

                // Шаг 3: Создание ярлыков (75-85%)
                UpdateStatus("Крок 3/4: Створення ярликів...");
                progressBar.Value = 78;

                ShortcutManager.CreateShortcuts(installPath, createDesktopShortcut, createStartMenuShortcut);

                progressBar.Value = 85;

                // Шаг 4: Настройка автозапуска (85-95%)
                if (runAtStartup)
                {
                    UpdateStatus("Крок 4/4: Додавання в автозапуск...");
                    progressBar.Value = 88;
                    RegistryManager.AddToRegistry(installPath);
                    progressBar.Value = 95;
                }
                else
                {
                    progressBar.Value = 95;
                }

                // Завершение установки (95-100%)
                UpdateStatus("Завершення установки...");
                progressBar.Value = 98;
                await Task.Delay(300); // Небольшая задержка для плавности

                UpdateStatus("Установка завершена успішно!");
                progressBar.Value = 100;
                await Task.Delay(500); // Показываем 100% перед переходом

                ShowPanel(panelFinish);
                btnNext.Enabled = true;
                btnBack.Visible = false;
                Log("Установка завершена успішно.");
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                LogError(ex);
                Invoke(new Action(() =>
                {
                    MessageBox.Show("Під час установки сталася помилка: " + ex.Message,
                        "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnNext.Enabled = true;
                    btnBack.Enabled = true;
                    ShowPanel(panelInstallPath);
                }));
                // Откат установки
                RollbackInstallation();
            }
        }

        private void UpdateStatus(string status)
        {
            Invoke(new Action(() =>
            {
                lblStatus.Text = status;
                Log(status);
            }));
        }

        // Откат установки
        private void RollbackInstallation()
        {
            try
            {
                UpdateStatus("Откат установки...");

                if (!string.IsNullOrEmpty(installPath) && Directory.Exists(installPath))
                {
                    // Проверяем, что директория установки находится в Program Files
                    string programFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                    if (!installPath.StartsWith(programFilesDir, StringComparison.OrdinalIgnoreCase))
                    {
                        // Если путь не в Program Files, не удаляем директорию
                        Log($"Путь установки {installPath} не находится в Program Files. Откат установки пропущен.");
                    }
                    else
                    {
                        // Снимаем атрибут "Только чтение" со всех файлов и папок
                        RemoveReadOnlyAttribute(new DirectoryInfo(installPath));

                        // Удаляем директорию установки
                        Directory.Delete(installPath, true);
                        Log($"Директория {installPath} удалена.");
                    }
                }

                // Удаление ярлыков, созданных установщиком
                string desktopShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RadioPlayerV2.lnk");
                if (System.IO.File.Exists(desktopShortcut))
                {
                    System.IO.File.Delete(desktopShortcut);
                }

                string startMenuShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "RadioPlayer", "RadioPlayerV2.lnk");
                if (System.IO.File.Exists(startMenuShortcut))
                {
                    System.IO.File.Delete(startMenuShortcut);
                }

                Log("Установка отменена и изменения откатаны.");
                MessageBox.Show("Установка була скасована та зміни відмінені.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show("Помилка при відміні установки: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RemoveReadOnlyAttribute(DirectoryInfo directory)
        {
            foreach (var subDir in directory.GetDirectories())
            {
                RemoveReadOnlyAttribute(subDir);
            }

            foreach (var file in directory.GetFiles())
            {
                file.IsReadOnly = false;
            }

            directory.Attributes &= ~FileAttributes.ReadOnly;
        }

        // Запуск приложения
        private void LaunchApplication()
        {
            // ИСПРАВЛЕНО: Добавлена подпапка "data"
            string exePath = Path.Combine(installPath, "data", "RadioPlayerV2.exe");

            if (System.IO.File.Exists(exePath))
            {
                try
                {
                    Process.Start(exePath);
                    Log($"Програма запущена: {exePath}");
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    MessageBox.Show($"Не вдалося запустити програму: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string errorMsg = $"Файл програми не знайдено: {exePath}";
                Log(errorMsg);
                MessageBox.Show(errorMsg, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Открытие сайта
        private void OpenWebsite()
        {
            Process.Start("https://deepradio.site");
        }

        // Логирование
        private void Log(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}\n";
            System.IO.File.AppendAllText(logFilePath, logMessage);
        }

        private void LogError(Exception ex)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: ERROR: {ex}\n";
            System.IO.File.AppendAllText(logFilePath, logMessage);
        }

        // Дополнительные методы: UpdateSpaceInfo, GetRequiredSpace, etc.
        private void UpdateSpaceInfo(Label lbl, string path)
        {
            long requiredSpace = GetRequiredSpace();
            long availableSpace = GetAvailableSpace(path);

            lbl.Text = $"Необхідно місця: {requiredSpace / (1024 * 1024)} МБ, Доступно: {availableSpace / (1024 * 1024)} МБ";
        }

        private long GetRequiredSpace()
        {
            // Если data.zip встроен в ресурсы, получаем его размер
            long size = Properties.Resources.data.Length;
            return size * 2; // С запасом
        }

        private long GetAvailableSpace(string path)
        {
            try
            {
                string rootPath = Path.GetPathRoot(Path.GetFullPath(path));
                DriveInfo drive = new DriveInfo(rootPath);
                return drive.AvailableFreeSpace;
            }
            catch
            {
                return 0;
            }
        }

        private bool HasEnoughDiskSpace(string path)
        {
            long requiredSpace = GetRequiredSpace();
            long availableSpace = GetAvailableSpace(path);
            if (availableSpace >= requiredSpace)
            {
                return true;
            }
            else
            {
                MessageBox.Show($"Недостаточно места на диске. Требуется: {requiredSpace / (1024 * 1024)} МБ, доступно: {availableSpace / (1024 * 1024)} МБ.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool IsValidInstallPath(string path)
        {
            try
            {
                // Нормализуем пути
                string normalizedPath = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToLowerInvariant();
                Log($"Проверка пути установки: {normalizedPath}");

                // Запрещенные директории
                var forbiddenPaths = new List<string>
        {
            Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.Windows)).ToLowerInvariant(),
            Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.System)).ToLowerInvariant(),
            Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToLowerInvariant(),
            Path.GetFullPath(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)).ToLowerInvariant()
        };

                // Проверяем, начинается ли путь с запрещённых директорий
                foreach (var forbiddenPath in forbiddenPaths)
                {
                    Log($"Сравнение с запрещённым путём: {forbiddenPath}");
                    if (normalizedPath == forbiddenPath || normalizedPath.StartsWith(forbiddenPath + Path.DirectorySeparatorChar))
                    {
                        Log("Путь установки запрещён.");
                        return false;
                    }
                }

                // Дополнительная проверка: не разрешать установку непосредственно в корень диска
                string rootDir = Path.GetPathRoot(normalizedPath);
                if (string.Equals(normalizedPath, rootDir.TrimEnd(Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                {
                    Log("Установка в корневую директорию диска запрещена.");
                    return false;
                }

                Log("Путь установки разрешён.");
                return true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                // В случае ошибки считаем путь некорректным
                return false;
            }
        }

        private bool VerifyFileChecksum(string filePath, string expectedChecksum)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    var hash = sha256.ComputeHash(stream);
                    var fileChecksum = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    return fileChecksum == expectedChecksum.ToLowerInvariant();
                }
            }
        }
    }
}
