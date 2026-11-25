using Тренажёрный_зал.Common;
using Тренажёрный_зал.Data;

namespace Тренажёрный_зал.Forms
{
    /// <summary>
    /// Модальная форма добавления занятия.
    /// </summary>
    public class FormLessonEdit : FormEditBase
    {
        private readonly int _id;
        private readonly TextBox _title;
        private readonly ComboBox _coach;
        private readonly NumericUpDown _duration;
        private readonly DateTimePicker _time;
        private readonly ComboBox _schedule;

        public FormLessonEdit(int id)
            : base("Новое занятие", "Заполните карточку занятия")
        {
            _id = id;

            _title = new TextBox { MaxLength = 60, PlaceholderText = "Например, силовая тренировка" };

            _coach = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiTheme.Base,
                DataSource = DemoData.Coaches.ToList(),
                DisplayMember = nameof(Coache.FullName),
                ValueMember = nameof(Coache.Id)
            };

            _duration = new NumericUpDown
            {
                Minimum = 15,
                Maximum = 240,
                Value = 60,
                Increment = 5,
                Font = UiTheme.Base,
                ThousandsSeparator = false
            };

            _time = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true,
                Value = DateTime.Today.AddHours(18),
                Font = UiTheme.Base
            };

            _schedule = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = UiTheme.Base
            };
            _schedule.Items.AddRange(new object[]
            {
                "Пн, Ср, Пт", "Вт, Чт", "Пн, Ср", "Вт, Чт, Сб",
                "Пн, Пт", "Сб, Вс", "Вт, Сб", "Ежедневно"
            });
            _schedule.SelectedIndex = 0;

            AddField("Название *", _title);
            AddField("Тренер *", _coach);
            AddField("Длительность, мин", _duration);
            AddField("Время начала", _time);
            AddField("Расписание *", _schedule);

            FinishLayout();
            _title.Focus();
        }

        /// <summary>Созданная запись занятия.</summary>
        public Lesson Result { get; private set; } = new();

        protected override bool OnSubmit()
        {
            if (string.IsNullOrWhiteSpace(_title.Text))
            {
                ShowError("Укажите название");
                _title.Focus();
                return false;
            }

            if (_coach.SelectedItem is not Coache coach)
            {
                ShowError("Выберите тренера");
                _coach.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(_schedule.Text))
            {
                ShowError("Укажите расписание");
                _schedule.Focus();
                return false;
            }

            Result = new Lesson
            {
                Id = _id,
                Title = _title.Text.Trim(),
                Coach_id = coach.Id,
                Duration = ((int)_duration.Value).ToString(),
                Time = _time.Value.ToString("HH:mm"),
                Schedule = _schedule.Text.Trim()
            };

            return true;
        }
    }
}
