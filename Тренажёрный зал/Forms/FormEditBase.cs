using Тренажёрный_зал.Common;

namespace Тренажёрный_зал.Forms
{
    /// <summary>
    /// Базовая модальная форма для добавления записи. Содержит заголовок,
    /// область полей ввода и панель с кнопками «Добавить» и «Отмена».
    /// Наследники добавляют поля через <see cref="AddField"/> и проверяют
    /// данные в методе <see cref="OnSubmit"/>.
    /// </summary>
    public abstract class FormEditBase : Form
    {
        private const int FormWidth = 500;
        private const int RowHeight = 56;

        private readonly TableLayoutPanel _content;
        private readonly Label _errorLabel;
        private readonly Button _okButton;
        private int _rows;

        protected FormEditBase(string title, string subtitle)
        {
            Text = title;
            Font = UiTheme.Base;
            BackColor = UiTheme.Surface;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(FormWidth, 420);

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 82,
                BackColor = UiTheme.AccentSoft
            };
            header.Controls.Add(new Label
            {
                Text = title,
                Font = UiTheme.Title,
                ForeColor = UiTheme.AccentDark,
                AutoSize = true,
                Location = new Point(24, 15)
            });
            header.Controls.Add(new Label
            {
                Text = subtitle,
                Font = UiTheme.Base,
                ForeColor = UiTheme.TextMuted,
                AutoSize = true,
                Location = new Point(26, 48)
            });

            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 74,
                BackColor = Color.FromArgb(249, 250, 251)
            };
            footer.Paint += (_, e) =>
            {
                using var pen = new Pen(UiTheme.Border);
                e.Graphics.DrawLine(pen, 0, 0, footer.Width, 0);
            };

            _errorLabel = new Label
            {
                AutoSize = false,
                Location = new Point(24, 26),
                Size = new Size(190, 24),
                ForeColor = UiTheme.Danger,
                Font = UiTheme.Small,
                TextAlign = ContentAlignment.MiddleLeft
            };

            _okButton = UiTheme.CreateButton("Добавить", true, 128);
            _okButton.Location = new Point(FormWidth - 24 - 128, 20);
            _okButton.Click += (_, _) => Submit();

            var cancelButton = UiTheme.CreateButton("Отмена", false, 100);
            cancelButton.Location = new Point(FormWidth - 24 - 128 - 8 - 100, 20);
            cancelButton.DialogResult = DialogResult.Cancel;

            footer.Controls.Add(_errorLabel);
            footer.Controls.Add(cancelButton);
            footer.Controls.Add(_okButton);

            _content = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 0,
                Padding = new Padding(24, 12, 24, 8),
                BackColor = UiTheme.Surface
            };
            _content.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 152));
            _content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            Controls.Add(_content);
            Controls.Add(footer);
            Controls.Add(header);

            AcceptButton = _okButton;
            CancelButton = cancelButton;
        }

        /// <summary>Проверяет введённые данные и заполняет результат формы.</summary>
        protected abstract bool OnSubmit();

        /// <summary>Добавляет подпись и поле ввода в макет формы.</summary>
        protected void AddField(string caption, Control input)
        {
            var row = _rows++;
            _content.RowCount = _rows;
            _content.RowStyles.Add(new RowStyle(SizeType.Absolute, RowHeight));

            var label = new Label
            {
                Text = caption,
                AutoSize = true,
                ForeColor = UiTheme.TextPrimary,
                Font = UiTheme.Base,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 14, 12, 0)
            };

            input.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            input.Margin = new Padding(0, 10, 0, 10);
            input.Font = UiTheme.Base;
            if (input is TextBox textBox)
            {
                textBox.AutoSize = false;
                textBox.Height = 32;
            }

            _content.Controls.Add(label, 0, row);
            _content.Controls.Add(input, 1, row);
        }

        /// <summary>Задаёт итоговый размер окна в зависимости от числа полей.</summary>
        protected void FinishLayout()
        {
            ClientSize = new Size(FormWidth, 82 + 74 + 20 + _rows * RowHeight);
        }

        /// <summary>Показывает сообщение об ошибке в нижней панели формы.</summary>
        protected void ShowError(string message)
        {
            _errorLabel.Text = message;
            _errorLabel.ForeColor = UiTheme.Danger;
        }

        private void Submit()
        {
            _errorLabel.Text = string.Empty;
            if (!OnSubmit())
                return;

            DialogResult = DialogResult.OK;
        }
    }
}
