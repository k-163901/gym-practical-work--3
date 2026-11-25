using Тренажёрный_зал.Common;
using Тренажёрный_зал.Data;

namespace Тренажёрный_зал.Forms
{
    /// <summary>
    /// Модальная форма добавления клиента.
    /// </summary>
    public class FormClientEdit : FormEditBase
    {
        private readonly int _id;
        private readonly TextBox _lastName;
        private readonly TextBox _firstName;
        private readonly TextBox _midName;
        private readonly DateTimePicker _birthDate;
        private readonly TextBox _phone;
        private readonly TextBox _email;

        public FormClientEdit(int id)
            : base("Новый клиент", "Заполните карточку клиента")
        {
            _id = id;

            _lastName = new TextBox { MaxLength = 50 };
            _firstName = new TextBox { MaxLength = 50 };
            _midName = new TextBox { MaxLength = 50 };
            _phone = new TextBox { MaxLength = 25, PlaceholderText = "+7 (900) 000-00-00" };
            _email = new TextBox { MaxLength = 60, PlaceholderText = "client@example.ru" };

            _birthDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                MaxDate = DateTime.Today,
                Value = new DateTime(2000, 1, 1),
                Font = UiTheme.Base
            };

            AddField("Фамилия *", _lastName);
            AddField("Имя *", _firstName);
            AddField("Отчество", _midName);
            AddField("Дата рождения", _birthDate);
            AddField("Телефон", _phone);
            AddField("E-mail", _email);

            FinishLayout();
            _lastName.Focus();
        }

        /// <summary>Созданная запись клиента.</summary>
        public Client Result { get; private set; } = new();

        protected override bool OnSubmit()
        {
            if (string.IsNullOrWhiteSpace(_lastName.Text))
            {
                ShowError("Укажите фамилию");
                _lastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(_firstName.Text))
            {
                ShowError("Укажите имя");
                _firstName.Focus();
                return false;
            }

            var email = _email.Text.Trim();
            if (email.Length > 0 && (!email.Contains('@') || !email.Contains('.')))
            {
                ShowError("Некорректный e-mail");
                _email.Focus();
                return false;
            }

            Result = new Client
            {
                Id = _id,
                LastName = _lastName.Text.Trim(),
                FirstName = _firstName.Text.Trim(),
                MidName = _midName.Text.Trim(),
                DateOfBirth = _birthDate.Value.ToString("dd.MM.yyyy"),
                Number = _phone.Text.Trim(),
                Email = email
            };

            return true;
        }
    }
}
