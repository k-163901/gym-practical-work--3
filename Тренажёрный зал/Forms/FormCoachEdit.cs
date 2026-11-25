using Тренажёрный_зал.Common;
using Тренажёрный_зал.Data;

namespace Тренажёрный_зал.Forms
{
    /// <summary>
    /// Модальная форма добавления тренера.
    /// </summary>
    public class FormCoachEdit : FormEditBase
    {
        private readonly int _id;
        private readonly TextBox _lastName;
        private readonly TextBox _firstName;
        private readonly TextBox _midName;
        private readonly ComboBox _specialization;
        private readonly TextBox _phone;

        public FormCoachEdit(int id)
            : base("Новый тренер", "Заполните карточку тренера")
        {
            _id = id;

            _lastName = new TextBox { MaxLength = 50 };
            _firstName = new TextBox { MaxLength = 50 };
            _midName = new TextBox { MaxLength = 50 };
            _phone = new TextBox { MaxLength = 25, PlaceholderText = "+7 (900) 000-00-00" };

            _specialization = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = UiTheme.Base
            };
            _specialization.Items.AddRange(new object[]
            {
                "Силовой тренинг", "Кроссфит", "Йога и растяжка",
                "Фитнес-аэробика", "Бокс", "Плавание", "Стретчинг"
            });
            _specialization.SelectedIndex = 0;

            AddField("Фамилия *", _lastName);
            AddField("Имя *", _firstName);
            AddField("Отчество", _midName);
            AddField("Специализация *", _specialization);
            AddField("Телефон", _phone);

            FinishLayout();
            _lastName.Focus();
        }

        /// <summary>Созданная запись тренера.</summary>
        public Coache Result { get; private set; } = new();

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

            if (string.IsNullOrWhiteSpace(_specialization.Text))
            {
                ShowError("Укажите специализацию");
                _specialization.Focus();
                return false;
            }

            Result = new Coache
            {
                Id = _id,
                LastName = _lastName.Text.Trim(),
                FirstName = _firstName.Text.Trim(),
                MidName = _midName.Text.Trim(),
                Specialization = _specialization.Text.Trim(),
                Number = _phone.Text.Trim()
            };

            return true;
        }
    }
}
