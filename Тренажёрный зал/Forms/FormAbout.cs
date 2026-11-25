using Тренажёрный_зал.Common;

namespace Тренажёрный_зал.Forms
{
    /// <summary>
    /// Форма «О программе»: сведения о назначении, версии и авторе приложения.
    /// </summary>
    public class FormAbout : Form
    {
        public FormAbout()
        {
            Text = "О программе";
            ClientSize = new Size(580, 600);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = UiTheme.Surface;
            Font = UiTheme.Base;

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = UiTheme.Accent
            };
            header.Controls.Add(new Label
            {
                Text = "ТРЕНАЖЁРНЫЙ ЗАЛ",
                Font = new Font("Segoe UI Semibold", 18F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(30, 30)
            });
            header.Controls.Add(new Label
            {
                Text = "Информационная система",
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(219, 234, 254),
                AutoSize = true,
                Location = new Point(32, 70)
            });
            header.Controls.Add(new Label
            {
                Text = $"Версия {AppInfo.Version}",
                Font = UiTheme.SemiBold,
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(32, 98)
            });

            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 66,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            var closeButton = UiTheme.CreateButton("Закрыть", true, 120);
            closeButton.Location = new Point(580 - 24 - 120, 16);
            closeButton.DialogResult = DialogResult.Cancel;
            footer.Controls.Add(closeButton);
            footer.Paint += (_, e) =>
            {
                using var pen = new Pen(UiTheme.Border);
                e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
            };

            var content = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.Surface
            };

            content.Controls.Add(new Label
            {
                Text = "О программе",
                Font = UiTheme.Title,
                ForeColor = UiTheme.TextPrimary,
                AutoSize = true,
                Location = new Point(32, 22)
            });

            content.Controls.Add(new Label
            {
                Text = AppInfo.Name,
                Font = UiTheme.SectionTitle,
                ForeColor = UiTheme.TextPrimary,
                AutoSize = false,
                Size = new Size(516, 26),
                Location = new Point(32, 62)
            });

            AddRow(content, "Версия", $"{AppInfo.Version} (демонстрационная)", 102);
            AddRow(content, "Автор", AppInfo.Author, 132);
            AddRow(content, "Группа", AppInfo.Group, 162);
            AddRow(content, "Технологии", AppInfo.Technology, 192);

            content.Controls.Add(new Label
            {
                Text = "Назначение",
                Font = UiTheme.SemiBold,
                ForeColor = UiTheme.TextMuted,
                AutoSize = true,
                Location = new Point(32, 232)
            });

            content.Controls.Add(new Label
            {
                Text = "Учебное приложение, разработанное в рамках практической работы № 3. " +
                       "Интерфейс Windows Forms предназначен для сопровождения базы данных " +
                       "тренажёрного зала: клиенты, тренеры, занятия, абонементы и посещения. " +
                       "Приложение не подключается к базе данных и работает с тестовыми " +
                       "данными, создаваемыми в коде.",
                Font = UiTheme.Base,
                ForeColor = UiTheme.TextPrimary,
                AutoSize = false,
                Size = new Size(516, 96),
                Location = new Point(32, 258)
            });

            content.Controls.Add(new Label
            {
                Text = $"© {AppInfo.Year} · {AppInfo.ShortName} · Учебный проект",
                Font = UiTheme.Small,
                ForeColor = UiTheme.TextMuted,
                AutoSize = true,
                Location = new Point(32, 372)
            });

            Controls.Add(content);
            Controls.Add(footer);
            Controls.Add(header);

            CancelButton = closeButton;
        }

        private static void AddRow(Control parent, string caption, string value, int top)
        {
            parent.Controls.Add(new Label
            {
                Text = caption,
                AutoSize = true,
                ForeColor = UiTheme.TextMuted,
                Location = new Point(32, top)
            });
            parent.Controls.Add(new Label
            {
                Text = value,
                AutoSize = true,
                ForeColor = UiTheme.TextPrimary,
                Font = UiTheme.SemiBold,
                Location = new Point(148, top)
            });
        }
    }
}
