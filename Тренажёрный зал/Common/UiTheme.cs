using System.Drawing;
using System.Windows.Forms;

namespace Тренажёрный_зал.Common
{
    /// <summary>
    /// Единый стиль оформления приложения: цвета, шрифты и вспомогательные методы
    /// для настройки элементов управления.
    /// </summary>
    internal static class UiTheme
    {
        public static readonly Color Accent = Color.FromArgb(37, 99, 235);
        public static readonly Color AccentHover = Color.FromArgb(29, 78, 216);
        public static readonly Color AccentDark = Color.FromArgb(30, 64, 175);
        public static readonly Color AccentSoft = Color.FromArgb(238, 244, 255);
        public static readonly Color Background = Color.FromArgb(243, 246, 251);
        public static readonly Color Surface = Color.White;
        public static readonly Color Border = Color.FromArgb(226, 232, 240);
        public static readonly Color HeaderBackground = Color.FromArgb(241, 245, 249);
        public static readonly Color TextPrimary = Color.FromArgb(30, 41, 59);
        public static readonly Color TextMuted = Color.FromArgb(100, 116, 139);
        public static readonly Color Danger = Color.FromArgb(220, 38, 38);

        public static readonly Font Base = new("Segoe UI", 9.75F);
        public static readonly Font Small = new("Segoe UI", 8.75F);
        public static readonly Font SemiBold = new("Segoe UI Semibold", 9.75F);
        public static readonly Font SectionTitle = new("Segoe UI Semibold", 11.5F);
        public static readonly Font Title = new("Segoe UI Semibold", 15F);
        public static readonly Font BigTitle = new("Segoe UI Semibold", 21F);

        /// <summary>Создаёт кнопку в едином стиле приложения.</summary>
        public static Button CreateButton(string text, bool primary, int width = 120)
        {
            var button = new Button
            {
                Text = text,
                Width = width,
                Height = 34,
                FlatStyle = FlatStyle.Flat,
                Font = SemiBold,
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false,
                BackColor = primary ? Accent : Surface,
                ForeColor = primary ? Color.White : AccentDark,
                Margin = new Padding(0, 0, 8, 0)
            };

            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.FlatAppearance.BorderColor = Border;
            button.FlatAppearance.MouseOverBackColor = primary ? AccentHover : AccentSoft;
            button.FlatAppearance.MouseDownBackColor = primary ? AccentDark : Color.FromArgb(219, 234, 254);
            return button;
        }

        /// <summary>Применяет к таблице единый стиль оформления.</summary>
        public static void StyleGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = false;
            grid.BackgroundColor = Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.GridColor = Border;
            grid.RowHeadersVisible = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.MultiSelect = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 40;
            grid.RowTemplate.Height = 34;
            grid.EnableHeadersVisualStyles = false;
            grid.Font = Base;
            grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = HeaderBackground,
                ForeColor = TextMuted,
                Font = SemiBold,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 6, 0),
                SelectionBackColor = HeaderBackground,
                SelectionForeColor = TextMuted
            };

            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Surface,
                ForeColor = TextPrimary,
                SelectionBackColor = AccentSoft,
                SelectionForeColor = AccentDark,
                Font = Base,
                Padding = new Padding(8, 0, 6, 0),
                WrapMode = DataGridViewTriState.False
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 250, 252)
            };
        }

        /// <summary>Создаёт поле ввода в едином стиле.</summary>
        public static TextBox CreateTextBox(string placeholder = "", int width = 230)
        {
            return new TextBox
            {
                Width = width,
                Height = 30,
                AutoSize = false,
                Font = Base,
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = placeholder,
                Margin = new Padding(0, 5, 12, 0)
            };
        }
    }
}
