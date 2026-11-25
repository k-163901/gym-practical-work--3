using Тренажёрный_зал.Common;
using Тренажёрный_зал.Data;

namespace Тренажёрный_зал.Forms
{
    /// <summary>
    /// Модальная форма добавления посещения.
    /// </summary>
    public class FormVisitEdit : FormEditBase
    {
        private readonly int _id;
        private readonly ComboBox _client;
        private readonly ComboBox _lesson;
        private readonly DateTimePicker _date;

        public FormVisitEdit(int id)
            : base("Новое посещение", "Отметьте посещение занятия клиентом")
        {
            _id = id;

            _client = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiTheme.Base,
                DataSource = DemoData.Clients.ToList(),
                DisplayMember = nameof(Client.FullName),
                ValueMember = nameof(Client.Id)
            };

            _lesson = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiTheme.Base,
                DataSource = DemoData.Lessons.ToList(),
                DisplayMember = nameof(Lesson.Title),
                ValueMember = nameof(Lesson.Id)
            };

            _date = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Value = DateTime.Today,
                Font = UiTheme.Base
            };

            AddField("Клиент *", _client);
            AddField("Занятие *", _lesson);
            AddField("Дата посещения", _date);

            FinishLayout();
        }

        /// <summary>Созданная запись посещения.</summary>
        public Visit Result { get; private set; } = new();

        protected override bool OnSubmit()
        {
            if (_client.SelectedItem is not Client client)
            {
                ShowError("Выберите клиента");
                _client.Focus();
                return false;
            }

            if (_lesson.SelectedItem is not Lesson lesson)
            {
                ShowError("Выберите занятие");
                _lesson.Focus();
                return false;
            }

            Result = new Visit
            {
                Id = _id,
                Client_id = client.Id,
                Lesson_id = lesson.Id,
                Visit_Date = _date.Value.ToString("dd.MM.yyyy")
            };

            return true;
        }
    }
}
