using Тренажёрный_зал.Common;

namespace Тренажёрный_зал.Forms
{
    /// <summary>
    /// Форма авторизации пользователя. Доступ к основной части приложения
    /// открывается только после успешного входа.
    /// </summary>
    public class FormLogin : Form
    {
        private readonly TextBox _loginTextBox;
        private readonly TextBox _passwordTextBox;
        private readonly CheckBox _showPassword;
        private readonly Label _errorLabel;

        public FormLogin()
        {
            Text = "Вход в систему";
            ClientSize = new Size(460, 620);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            AutoScaleMode = AutoScaleMode.None;
            BackColor = UiTheme.Surface;
            Font = UiTheme.Base;

            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = UiTheme.Accent
            };
            header.Controls.Add(new Label
            {
                Text = "ТРЕНАЖЁРНЫЙ ЗАЛ",
                Font = new Font("Segoe UI Semibold", 20F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(30, 34)
            });
            header.Controls.Add(new Label
            {
                Text = "Информационная система",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.FromArgb(219, 234, 254),
                AutoSize = true,
                Location = new Point(32, 78)
            });
            header.Controls.Add(new Label
            {
                Text = "Практическая работа № 3 · Windows Forms",
                Font = UiTheme.Small,
                ForeColor = Color.FromArgb(191, 219, 254),
                AutoSize = true,
                Location = new Point(33, 108)
            });

            var loginCaption = new Label
            {
                Text = "Логин",
                AutoSize = true,
                ForeColor = UiTheme.TextPrimary,
                Location = new Point(32, 178)
            };

            _loginTextBox = new TextBox
            {
                Location = new Point(32, 202),
                Size = new Size(396, 32),
                Font = new Font("Segoe UI", 11F),
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "admin"
            };

            var passwordCaption = new Label
            {
                Text = "Пароль",
                AutoSize = true,
                ForeColor = UiTheme.TextPrimary,
                Location = new Point(32, 248)
            };

            _passwordTextBox = new TextBox
            {
                Location = new Point(32, 272),
                Size = new Size(396, 32),
                Font = new Font("Segoe UI", 11F),
                AutoSize = false,
                BorderStyle = BorderStyle.FixedSingle,
                UseSystemPasswordChar = false,
                PasswordChar = '●'
            };

            _showPassword = new CheckBox
            {
                Text = "Показать пароль",
                AutoSize = true,
                ForeColor = UiTheme.TextMuted,
                Location = new Point(32, 312)
            };

            _errorLabel = new Label
            {
                AutoSize = false,
                Location = new Point(32, 342),
                Size = new Size(396, 22),
                ForeColor = UiTheme.Danger,
                Font = UiTheme.Small,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var loginButton = UiTheme.CreateButton("Войти", true, 396);
            loginButton.Location = new Point(32, 374);
            loginButton.Height = 44;
            loginButton.Font = new Font("Segoe UI Semibold", 11F);
            loginButton.Click += LoginButton_Click;

            var exitButton = UiTheme.CreateButton("Выход", false, 396);
            exitButton.Location = new Point(32, 426);
            exitButton.Height = 38;
            exitButton.DialogResult = DialogResult.Cancel;

            var hint = new Label
            {
                Text = "Демо-доступ:  admin / 1234",
                AutoSize = false,
                Location = new Point(32, 486),
                Size = new Size(396, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = UiTheme.TextMuted,
                Font = UiTheme.Base,
                BackColor = UiTheme.AccentSoft
            };

            var footer = new Label
            {
                Text = $"Версия {AppInfo.Version} · Windows Forms",
                AutoSize = false,
                Location = new Point(32, 560),
                Size = new Size(396, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = UiTheme.TextMuted,
                Font = UiTheme.Small
            };

            Controls.Add(header);
            Controls.Add(loginCaption);
            Controls.Add(_loginTextBox);
            Controls.Add(passwordCaption);
            Controls.Add(_passwordTextBox);
            Controls.Add(_showPassword);
            Controls.Add(_errorLabel);
            Controls.Add(loginButton);
            Controls.Add(exitButton);
            Controls.Add(hint);
            Controls.Add(footer);

            AcceptButton = loginButton;
            CancelButton = exitButton;
            ActiveControl = _loginTextBox;
            _loginTextBox.SelectAll();

            _showPassword.CheckedChanged += (_, _) =>
                _passwordTextBox.PasswordChar = _showPassword.Checked ? '\0' : '●';
            _loginTextBox.TextChanged += (_, _) => _errorLabel.Text = string.Empty;
            _passwordTextBox.TextChanged += (_, _) => _errorLabel.Text = string.Empty;
        }

        /// <summary>Имя вошедшего пользователя.</summary>
        public string UserName { get; private set; } = string.Empty;

        private void LoginButton_Click(object? sender, EventArgs e)
        {
            var login = _loginTextBox.Text.Trim();
            var password = _passwordTextBox.Text;

            if (login.Length == 0 || password.Length == 0)
            {
                ShowError("Введите логин и пароль");
                return;
            }

            if (!string.Equals(login, AppInfo.DemoLogin, StringComparison.OrdinalIgnoreCase)
                || password != AppInfo.DemoPassword)
            {
                ShowError("Неверный логин или пароль");
                _passwordTextBox.SelectAll();
                _passwordTextBox.Focus();
                return;
            }

            UserName = login;
            DialogResult = DialogResult.OK;
        }

        private void ShowError(string message)
        {
            _errorLabel.Text = message;
        }
    }
}
