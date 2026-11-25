using Тренажёрный_зал.Common;
using Тренажёрный_зал.Data;

namespace Тренажёрный_зал.Forms
{
    /// <summary>
    /// Модальная форма добавления абонемента.
    /// </summary>
    public class FormSubscriptionEdit : FormEditBase
    {
        private readonly int _id;
        private readonly ComboBox _client;
        private readonly ComboBox _view;
        private readonly NumericUpDown _price;
        private readonly DateTimePicker _startDate;
        private readonly DateTimePicker _endDate;
        private readonly NumericUpDown _visits;

        public FormSubscriptionEdit(int id)
            : base("Новый абонемент", "Заполните данные абонемента")
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

            _view = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                Font = UiTheme.Base
            };
            _view.Items.AddRange(new object[]
            {
                "Месячный безлимит", "Годовой безлимит", "Утро",
                "Студенческий", "Пробный", "Персональный"
            });
            _view.SelectedIndex = 0;

            _price = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 200000,
                Value = 3200,
                Increment = 100,
                Font = UiTheme.Base,
                ThousandsSeparator = true,
                DecimalPlaces = 0
            };

            _startDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Value = DateTime.Today,
                Font = UiTheme.Base
            };

            _endDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd.MM.yyyy",
                Value = DateTime.Today.AddMonths(1),
                Font = UiTheme.Base
            };

            _visits = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 999,
                Value = 12,
                Font = UiTheme.Base
            };

            AddField("Клиент *", _client);
            AddField("Вид абонемента *", _view);
            AddField("Цена, руб.", _price);
            AddField("Дата начала", _startDate);
            AddField("Дата окончания", _endDate);
            AddField("Посещений", _visits);

            FinishLayout();
        }

        /// <summary>Созданная запись абонемента.</summary>
        public Subscription Result { get; private set; } = new();

        protected override bool OnSubmit()
        {
            if (_client.SelectedItem is not Client client)
            {
                ShowError("Выберите клиента");
                _client.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(_view.Text))
            {
                ShowError("Укажите вид абонемента");
                _view.Focus();
                return false;
            }

            if (_endDate.Value.Date <= _startDate.Value.Date)
            {
                ShowError("Дата окончания должна быть позже");
                _endDate.Focus();
                return false;
            }

            Result = new Subscription
            {
                Id = _id,
                Client_id = client.Id,
                View = _view.Text.Trim(),
                Price = ((int)_price.Value).ToString(),
                Start_date = _startDate.Value.ToString("dd.MM.yyyy"),
                End_date = _endDate.Value.ToString("dd.MM.yyyy"),
                Number_of_visits = (int)_visits.Value
            };

            return true;
        }
    }
}
