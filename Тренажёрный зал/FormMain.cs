using System.ComponentModel;
using System.Globalization;
using Тренажёрный_зал.Common;
using Тренажёрный_зал.Data;
using Тренажёрный_зал.Forms;

namespace Тренажёрный_зал
{
    /// <summary>
    /// Главное окно приложения: меню, вкладки с таблицами, поиск и фильтрация.
    /// </summary>
    public class FormMain : Form
    {
        private readonly string _userName;

        private readonly BindingList<Client> _clientsView = new();
        private readonly BindingList<Coache> _coachesView = new();
        private readonly BindingList<Lesson> _lessonsView = new();
        private readonly BindingList<Subscription> _subscriptionsView = new();
        private readonly BindingList<Visit> _visitsView = new();

        private TabControl _tabs = null!;
        private ToolStripStatusLabel _statusCount = null!;
        private ToolStripStatusLabel _statusUser = null!;

        private DataGridView _gridClients = null!;
        private DataGridView _gridCoaches = null!;
        private DataGridView _gridLessons = null!;
        private DataGridView _gridSubscriptions = null!;
        private DataGridView _gridVisits = null!;

        private TextBox _txtClientSearch = null!;
        private TextBox _txtCoachSearch = null!;
        private TextBox _txtLessonSearch = null!;
        private TextBox _txtSubscriptionSearch = null!;
        private TextBox _txtVisitSearch = null!;

        private ComboBox _cmbClientFilter = null!;
        private ComboBox _cmbCoachFilter = null!;
        private ComboBox _cmbLessonFilter = null!;
        private ComboBox _cmbSubscriptionFilter = null!;
        private ComboBox _cmbVisitFilter = null!;

        private DataGridViewTextBoxColumn _colLessonCoach = null!;
        private DataGridViewTextBoxColumn _colSubscriptionClient = null!;
        private DataGridViewTextBoxColumn _colVisitClient = null!;
        private DataGridViewTextBoxColumn _colVisitLesson = null!;

        private bool _suspendFilter;

        public FormMain(string userName)
        {
            _userName = string.IsNullOrWhiteSpace(userName) ? AppInfo.DemoLogin : userName;
            BuildInterface();
            BindData();
        }

        #region Построение интерфейса

        private void BuildInterface()
        {
            Text = $"{AppInfo.ShortName} — информационная система (демо-режим)";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1280, 800);
            MinimumSize = new Size(1000, 640);
            BackColor = UiTheme.Background;
            Font = UiTheme.Base;
            AutoScaleMode = AutoScaleMode.None;

            var menu = BuildMenu();
            var status = BuildStatusBar();

            _tabs = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = UiTheme.SemiBold,
                Padding = new Point(18, 6),
                SizeMode = TabSizeMode.Fixed,
                ItemSize = new Size(148, 38)
            };
            _tabs.TabPages.Add(BuildClientsTab());
            _tabs.TabPages.Add(BuildCoachesTab());
            _tabs.TabPages.Add(BuildLessonsTab());
            _tabs.TabPages.Add(BuildSubscriptionsTab());
            _tabs.TabPages.Add(BuildVisitsTab());
            _tabs.SelectedIndexChanged += (_, _) =>
            {
                RefreshLookups();
                UpdateStatus();
            };
            Shown += (_, _) => UpdateStatus();

            Controls.Add(_tabs);
            Controls.Add(status);
            Controls.Add(menu);
            MainMenuStrip = menu;
        }

        private MenuStrip BuildMenu()
        {
            var menu = new MenuStrip
            {
                Dock = DockStyle.Top,
                Font = UiTheme.Base,
                BackColor = UiTheme.Surface,
                Padding = new Padding(8, 2, 0, 2)
            };

            var fileMenu = new ToolStripMenuItem("Файл");
            var resetItem = new ToolStripMenuItem("Сбросить демо-данные")
            {
                ShortcutKeys = Keys.Control | Keys.R
            };
            resetItem.Click += (_, _) => ResetDemoData();
            var exitItem = new ToolStripMenuItem("Выход");
            exitItem.Click += (_, _) => Close();
            fileMenu.DropDownItems.Add(resetItem);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add(exitItem);

            var recordsMenu = new ToolStripMenuItem("Записи");
            var addItem = new ToolStripMenuItem("Добавить запись")
            {
                ShortcutKeys = Keys.Control | Keys.N
            };
            addItem.Click += (_, _) => AddCurrent();
            var deleteItem = new ToolStripMenuItem("Удалить запись")
            {
                ShortcutKeys = Keys.Control | Keys.D
            };
            deleteItem.Click += (_, _) => DeleteCurrent();
            var findItem = new ToolStripMenuItem("Перейти к поиску")
            {
                ShortcutKeys = Keys.Control | Keys.F
            };
            findItem.Click += (_, _) => FocusSearch();
            recordsMenu.DropDownItems.Add(addItem);
            recordsMenu.DropDownItems.Add(deleteItem);
            recordsMenu.DropDownItems.Add(new ToolStripSeparator());
            recordsMenu.DropDownItems.Add(findItem);

            var helpMenu = new ToolStripMenuItem("Справка");
            var aboutItem = new ToolStripMenuItem("О программе")
            {
                ShortcutKeys = Keys.F1
            };
            aboutItem.Click += (_, _) => ShowAbout();
            helpMenu.DropDownItems.Add(aboutItem);

            menu.Items.Add(fileMenu);
            menu.Items.Add(recordsMenu);
            menu.Items.Add(helpMenu);
            return menu;
        }

        private StatusStrip BuildStatusBar()
        {
            var status = new StatusStrip
            {
                BackColor = UiTheme.Surface,
                SizingGrip = false,
                Font = UiTheme.Small,
                Padding = new Padding(8, 2, 8, 2)
            };

            _statusCount = new ToolStripStatusLabel
            {
                Spring = true,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = UiTheme.TextPrimary
            };
            _statusUser = new ToolStripStatusLabel
            {
                Text = $"Пользователь: {_userName}",
                ForeColor = UiTheme.TextMuted
            };
            var modeLabel = new ToolStripStatusLabel
            {
                Text = "Демо-режим: изменения не сохраняются в БД",
                ForeColor = UiTheme.TextMuted
            };
            var dateLabel = new ToolStripStatusLabel
            {
                Text = DateTime.Now.ToString("dd.MM.yyyy"),
                ForeColor = UiTheme.TextMuted
            };

            status.Items.AddRange(new ToolStripItem[]
            {
                _statusCount,
                modeLabel,
                new ToolStripStatusLabel("   "),
                _statusUser,
                new ToolStripStatusLabel("   "),
                dateLabel
            });

            return status;
        }

        private static TabPage CreateTabPage(string title)
        {
            return new TabPage(title)
            {
                BackColor = UiTheme.Background,
                Padding = new Padding(16, 10, 16, 16),
                UseVisualStyleBackColor = false
            };
        }

        private static DataGridView CreateGrid()
        {
            var grid = new DataGridView { Dock = DockStyle.Fill };
            UiTheme.StyleGrid(grid);
            return grid;
        }

        private static Panel WrapGrid(DataGridView grid)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UiTheme.Border,
                Padding = new Padding(1)
            };
            card.Controls.Add(grid);
            return card;
        }

        private static DataGridViewTextBoxColumn AddColumn(
            DataGridView grid,
            string header,
            string propertyName,
            int weight,
            bool readOnly = false,
            bool rightAlign = false)
        {
            var column = new DataGridViewTextBoxColumn
            {
                HeaderText = header,
                DataPropertyName = propertyName,
                FillWeight = weight,
                MinimumWidth = 45,
                ReadOnly = readOnly,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            if (rightAlign)
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grid.Columns.Add(column);
            return column;
        }

        private SearchBar BuildSearchBar(
            string placeholder,
            string filterCaption,
            string[] filterItems,
            EventHandler addHandler,
            EventHandler deleteHandler,
            EventHandler changedHandler)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 54,
                BackColor = UiTheme.Background
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0),
                BackColor = UiTheme.Background
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var left = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = UiTheme.Background,
                Padding = new Padding(0, 10, 0, 0)
            };

            var searchLabel = new Label
            {
                Text = "Поиск:",
                AutoSize = true,
                ForeColor = UiTheme.TextMuted,
                Margin = new Padding(0, 8, 8, 0)
            };

            var search = UiTheme.CreateTextBox(placeholder, 230);
            search.Margin = new Padding(0, 0, 12, 0);

            var filterLabel = new Label
            {
                Text = filterCaption,
                AutoSize = true,
                ForeColor = UiTheme.TextMuted,
                Margin = new Padding(0, 8, 8, 0)
            };

            var filter = new ComboBox
            {
                Width = 190,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = UiTheme.Base,
                Margin = new Padding(0, 1, 10, 0)
            };
            filter.Items.AddRange(filterItems);
            if (filter.Items.Count > 0)
                filter.SelectedIndex = 0;

            var reset = UiTheme.CreateButton("Сброс", false, 76);
            reset.Height = 30;
            reset.Margin = new Padding(0);

            left.Controls.Add(searchLabel);
            left.Controls.Add(search);
            left.Controls.Add(filterLabel);
            left.Controls.Add(filter);
            left.Controls.Add(reset);

            var right = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = UiTheme.Background,
                Anchor = AnchorStyles.Right
            };

            var add = UiTheme.CreateButton("Добавить", true, 118);
            add.Click += addHandler;
            var delete = UiTheme.CreateButton("Удалить", false, 110);
            delete.Click += deleteHandler;

            right.Controls.Add(add);
            right.Controls.Add(delete);

            layout.Controls.Add(left, 0, 0);
            layout.Controls.Add(right, 1, 0);
            panel.Controls.Add(layout);

            search.TextChanged += changedHandler;
            filter.SelectedIndexChanged += changedHandler;
            reset.Click += (_, _) =>
            {
                _suspendFilter = true;
                try
                {
                    search.Clear();
                    if (filter.Items.Count > 0)
                        filter.SelectedIndex = 0;
                }
                finally
                {
                    _suspendFilter = false;
                }

                changedHandler(search, EventArgs.Empty);
            };

            return new SearchBar(panel, search, filter);
        }

        private TabPage BuildClientsTab()
        {
            var page = CreateTabPage("Клиенты");
            _gridClients = CreateGrid();
            _gridClients.CellFormatting += LookupCellFormatting;
            _gridClients.CellEndEdit += (_, _) => RefreshLookups();

            AddColumn(_gridClients, "№", nameof(Client.Id), 30, true);
            AddColumn(_gridClients, "Фамилия", nameof(Client.LastName), 100);
            AddColumn(_gridClients, "Имя", nameof(Client.FirstName), 90);
            AddColumn(_gridClients, "Отчество", nameof(Client.MidName), 100);
            AddColumn(_gridClients, "Дата рождения", nameof(Client.DateOfBirth), 95);
            AddColumn(_gridClients, "Телефон", nameof(Client.Number), 125);
            AddColumn(_gridClients, "E-mail", nameof(Client.Email), 150);

            var bar = BuildSearchBar(
                "ФИО, телефон или e-mail",
                "Абонемент:",
                new[] { "Все клиенты", "С действующим абонементом", "Без действующего абонемента" },
                (_, _) => AddClient(),
                (_, _) => DeleteClient(),
                (_, _) => ApplyClientsFilter());

            _txtClientSearch = bar.Search;
            _cmbClientFilter = bar.Filter;

            page.Controls.Add(WrapGrid(_gridClients));
            page.Controls.Add(bar.Panel);
            return page;
        }

        private TabPage BuildCoachesTab()
        {
            var page = CreateTabPage("Тренеры");
            _gridCoaches = CreateGrid();
            _gridCoaches.CellFormatting += LookupCellFormatting;
            _gridCoaches.CellEndEdit += (_, _) => _gridLessons.Invalidate();

            AddColumn(_gridCoaches, "№", nameof(Coache.Id), 30, true);
            AddColumn(_gridCoaches, "Фамилия", nameof(Coache.LastName), 100);
            AddColumn(_gridCoaches, "Имя", nameof(Coache.FirstName), 90);
            AddColumn(_gridCoaches, "Отчество", nameof(Coache.MidName), 100);
            AddColumn(_gridCoaches, "Специализация", nameof(Coache.Specialization), 145);
            AddColumn(_gridCoaches, "Телефон", nameof(Coache.Number), 120);

            var specializations = new[] { "Все специализации" }
                .Concat(DemoData.Coaches
                    .Select(c => c.Specialization)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct()
                    .OrderBy(s => s))
                .ToArray();

            var bar = BuildSearchBar(
                "ФИО, специализация или телефон",
                "Специализация:",
                specializations,
                (_, _) => AddCoach(),
                (_, _) => DeleteCoach(),
                (_, _) => ApplyCoachesFilter());

            _txtCoachSearch = bar.Search;
            _cmbCoachFilter = bar.Filter;

            page.Controls.Add(WrapGrid(_gridCoaches));
            page.Controls.Add(bar.Panel);
            return page;
        }

        private TabPage BuildLessonsTab()
        {
            var page = CreateTabPage("Занятия");
            _gridLessons = CreateGrid();
            _gridLessons.CellFormatting += LookupCellFormatting;
            _gridLessons.CellEndEdit += (_, _) => _gridVisits.Invalidate();

            AddColumn(_gridLessons, "№", nameof(Lesson.Id), 30, true);
            AddColumn(_gridLessons, "Название", nameof(Lesson.Title), 165);
            _colLessonCoach = AddColumn(_gridLessons, "Тренер", string.Empty, 145, true);
            AddColumn(_gridLessons, "Длительность, мин", nameof(Lesson.Duration), 90, false, true);
            AddColumn(_gridLessons, "Время", nameof(Lesson.Time), 65);
            AddColumn(_gridLessons, "Расписание", nameof(Lesson.Schedule), 130);

            var coachNames = new[] { "Все тренеры" }
                .Concat(DemoData.Coaches.Select(c => c.FullName).Distinct().OrderBy(n => n))
                .ToArray();

            var bar = BuildSearchBar(
                "Название, тренер или расписание",
                "Тренер:",
                coachNames,
                (_, _) => AddLesson(),
                (_, _) => DeleteLesson(),
                (_, _) => ApplyLessonsFilter());

            _txtLessonSearch = bar.Search;
            _cmbLessonFilter = bar.Filter;

            page.Controls.Add(WrapGrid(_gridLessons));
            page.Controls.Add(bar.Panel);
            return page;
        }

        private TabPage BuildSubscriptionsTab()
        {
            var page = CreateTabPage("Абонементы");
            _gridSubscriptions = CreateGrid();
            _gridSubscriptions.CellFormatting += LookupCellFormatting;
            _gridSubscriptions.CellEndEdit += (_, _) => _gridClients.Invalidate();

            AddColumn(_gridSubscriptions, "№", nameof(Subscription.Id), 30, true);
            AddColumn(_gridSubscriptions, "Вид", nameof(Subscription.View), 130);
            _colSubscriptionClient = AddColumn(_gridSubscriptions, "Клиент", string.Empty, 150, true);
            AddColumn(_gridSubscriptions, "Цена, ₽", nameof(Subscription.Price), 75, false, true);
            AddColumn(_gridSubscriptions, "Начало", nameof(Subscription.Start_date), 85);
            AddColumn(_gridSubscriptions, "Окончание", nameof(Subscription.End_date), 85);
            AddColumn(_gridSubscriptions, "Посещений", nameof(Subscription.Number_of_visits), 75, false, true);

            var views = new[] { "Все виды" }
                .Concat(DemoData.Subscriptions
                    .Select(s => s.View)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct()
                    .OrderBy(v => v))
                .ToArray();

            var bar = BuildSearchBar(
                "Вид, клиент или дата",
                "Вид:",
                views,
                (_, _) => AddSubscription(),
                (_, _) => DeleteSubscription(),
                (_, _) => ApplySubscriptionsFilter());

            _txtSubscriptionSearch = bar.Search;
            _cmbSubscriptionFilter = bar.Filter;

            page.Controls.Add(WrapGrid(_gridSubscriptions));
            page.Controls.Add(bar.Panel);
            return page;
        }

        private TabPage BuildVisitsTab()
        {
            var page = CreateTabPage("Посещения");
            _gridVisits = CreateGrid();
            _gridVisits.CellFormatting += LookupCellFormatting;

            AddColumn(_gridVisits, "№", nameof(Visit.Id), 30, true);
            _colVisitClient = AddColumn(_gridVisits, "Клиент", string.Empty, 180, true);
            _colVisitLesson = AddColumn(_gridVisits, "Занятие", string.Empty, 180, true);
            AddColumn(_gridVisits, "Дата посещения", nameof(Visit.Visit_Date), 100);

            var lessonTitles = new[] { "Все занятия" }
                .Concat(DemoData.Lessons.Select(l => l.Title).Distinct().OrderBy(t => t))
                .ToArray();

            var bar = BuildSearchBar(
                "Клиент, занятие или дата",
                "Занятие:",
                lessonTitles,
                (_, _) => AddVisit(),
                (_, _) => DeleteVisit(),
                (_, _) => ApplyVisitsFilter());

            _txtVisitSearch = bar.Search;
            _cmbVisitFilter = bar.Filter;

            page.Controls.Add(WrapGrid(_gridVisits));
            page.Controls.Add(bar.Panel);
            return page;
        }

        #endregion

        #region Данные и фильтрация

        private void BindData()
        {
            _gridClients.DataSource = _clientsView;
            _gridCoaches.DataSource = _coachesView;
            _gridLessons.DataSource = _lessonsView;
            _gridSubscriptions.DataSource = _subscriptionsView;
            _gridVisits.DataSource = _visitsView;

            ApplyAllFilters();
            UpdateStatus();
        }

        private void ApplyAllFilters()
        {
            ApplyClientsFilter();
            ApplyCoachesFilter();
            ApplyLessonsFilter();
            ApplySubscriptionsFilter();
            ApplyVisitsFilter();
        }

        private void ApplyClientsFilter()
        {
            if (_suspendFilter)
                return;

            var query = _txtClientSearch.Text.Trim();
            var mode = _cmbClientFilter.SelectedIndex;
            var matched = DemoData.Clients.Where(c => ClientMatches(c, query, mode)).ToList();
            ReplaceItems(_clientsView, matched);
            _gridClients.Invalidate();
            UpdateStatus();
        }

        private void ApplyCoachesFilter()
        {
            if (_suspendFilter)
                return;

            var query = _txtCoachSearch.Text.Trim();
            var specialization = _cmbCoachFilter.SelectedItem as string ?? "Все специализации";
            var matched = DemoData.Coaches.Where(c => CoachMatches(c, query, specialization)).ToList();
            ReplaceItems(_coachesView, matched);
            _gridCoaches.Invalidate();
            UpdateStatus();
        }

        private void ApplyLessonsFilter()
        {
            if (_suspendFilter)
                return;

            var query = _txtLessonSearch.Text.Trim();
            var coachName = _cmbLessonFilter.SelectedItem as string ?? "Все тренеры";
            var matched = DemoData.Lessons.Where(l => LessonMatches(l, query, coachName)).ToList();
            ReplaceItems(_lessonsView, matched);
            _gridLessons.Invalidate();
            UpdateStatus();
        }

        private void ApplySubscriptionsFilter()
        {
            if (_suspendFilter)
                return;

            var query = _txtSubscriptionSearch.Text.Trim();
            var view = _cmbSubscriptionFilter.SelectedItem as string ?? "Все виды";
            var matched = DemoData.Subscriptions.Where(s => SubscriptionMatches(s, query, view)).ToList();
            ReplaceItems(_subscriptionsView, matched);
            _gridSubscriptions.Invalidate();
            UpdateStatus();
        }

        private void ApplyVisitsFilter()
        {
            if (_suspendFilter)
                return;

            var query = _txtVisitSearch.Text.Trim();
            var lessonTitle = _cmbVisitFilter.SelectedItem as string ?? "Все занятия";
            var matched = DemoData.Visits.Where(v => VisitMatches(v, query, lessonTitle)).ToList();
            ReplaceItems(_visitsView, matched);
            _gridVisits.Invalidate();
            UpdateStatus();
        }

        private static void ReplaceItems<T>(BindingList<T> view, IReadOnlyList<T> items)
        {
            view.RaiseListChangedEvents = false;
            try
            {
                view.Clear();
                foreach (var item in items)
                    view.Add(item);
            }
            finally
            {
                view.RaiseListChangedEvents = true;
            }

            view.ResetBindings();
        }

        private static bool Contains(string source, string query) =>
            query.Length == 0 || source.Contains(query, StringComparison.OrdinalIgnoreCase);

        private static bool ClientMatches(Client client, string query, int mode)
        {
            var textMatch = Contains(client.FullName, query)
                            || Contains(client.Number, query)
                            || Contains(client.Email, query);
            if (!textMatch)
                return false;

            return mode switch
            {
                1 => HasActiveSubscription(client),
                2 => !HasActiveSubscription(client),
                _ => true
            };
        }

        private static bool HasActiveSubscription(Client client) =>
            DemoData.Subscriptions.Any(s =>
                s.Client_id == client.Id && ParseDate(s.End_date) >= DateTime.Today);

        private static bool CoachMatches(Coache coach, string query, string specialization)
        {
            if (specialization != "Все специализации"
                && !string.Equals(coach.Specialization, specialization, StringComparison.OrdinalIgnoreCase))
                return false;

            return Contains(coach.FullName, query)
                   || Contains(coach.Specialization, query)
                   || Contains(coach.Number, query);
        }

        private static bool LessonMatches(Lesson lesson, string query, string coachName)
        {
            var coach = DemoData.Coaches.FirstOrDefault(c => c.Id == lesson.Coach_id);
            if (coachName != "Все тренеры"
                && !string.Equals(coach?.FullName, coachName, StringComparison.OrdinalIgnoreCase))
                return false;

            return Contains(lesson.Title, query)
                   || Contains(lesson.Schedule, query)
                   || Contains(lesson.Time, query)
                   || Contains(coach?.FullName ?? string.Empty, query);
        }

        private static bool SubscriptionMatches(Subscription subscription, string query, string view)
        {
            var clientName = DemoData.Clients
                .FirstOrDefault(c => c.Id == subscription.Client_id)?.FullName ?? string.Empty;

            if (view != "Все виды"
                && !string.Equals(subscription.View, view, StringComparison.OrdinalIgnoreCase))
                return false;

            return Contains(subscription.View, query)
                   || Contains(clientName, query)
                   || Contains(subscription.Start_date, query)
                   || Contains(subscription.End_date, query);
        }

        private static bool VisitMatches(Visit visit, string query, string lessonTitle)
        {
            var clientName = DemoData.Clients
                .FirstOrDefault(c => c.Id == visit.Client_id)?.FullName ?? string.Empty;
            var lesson = DemoData.Lessons.FirstOrDefault(l => l.Id == visit.Lesson_id)?.Title ?? string.Empty;

            if (lessonTitle != "Все занятия"
                && !string.Equals(lesson, lessonTitle, StringComparison.OrdinalIgnoreCase))
                return false;

            return Contains(clientName, query)
                   || Contains(lesson, query)
                   || Contains(visit.Visit_Date, query);
        }

        private static DateTime ParseDate(string value) =>
            DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date)
                ? date
                : DateTime.MinValue;

        private void LookupCellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || sender is not DataGridView grid)
                return;

            var item = grid.Rows[e.RowIndex].DataBoundItem;

            switch (item)
            {
                case Lesson lesson when e.ColumnIndex == _colLessonCoach.Index:
                    e.Value = DemoData.Coaches.FirstOrDefault(c => c.Id == lesson.Coach_id)?.FullName ?? "—";
                    e.FormattingApplied = true;
                    break;

                case Subscription subscription when e.ColumnIndex == _colSubscriptionClient.Index:
                    e.Value = DemoData.Clients.FirstOrDefault(c => c.Id == subscription.Client_id)?.FullName ?? "—";
                    e.FormattingApplied = true;
                    break;

                case Visit visit when e.ColumnIndex == _colVisitClient.Index:
                    e.Value = DemoData.Clients.FirstOrDefault(c => c.Id == visit.Client_id)?.FullName ?? "—";
                    e.FormattingApplied = true;
                    break;

                case Visit visit when e.ColumnIndex == _colVisitLesson.Index:
                    e.Value = DemoData.Lessons.FirstOrDefault(l => l.Id == visit.Lesson_id)?.Title ?? "—";
                    e.FormattingApplied = true;
                    break;
            }
        }

        private void RefreshLookups()
        {
            _gridLessons.Invalidate();
            _gridSubscriptions.Invalidate();
            _gridVisits.Invalidate();
        }

        private void UpdateStatus()
        {
            if (_statusCount is null || _tabs is null)
                return;

            var (title, visible, total) = (_tabs.SelectedIndex < 0 ? 0 : _tabs.SelectedIndex) switch
            {
                0 => ("Клиенты", _clientsView.Count, DemoData.Clients.Count),
                1 => ("Тренеры", _coachesView.Count, DemoData.Coaches.Count),
                2 => ("Занятия", _lessonsView.Count, DemoData.Lessons.Count),
                3 => ("Абонементы", _subscriptionsView.Count, DemoData.Subscriptions.Count),
                _ => ("Посещения", _visitsView.Count, DemoData.Visits.Count)
            };

            _statusCount.Text = $"{title}: {visible} из {total}";
        }

        #endregion

        #region Добавление записей

        private void AddClient()
        {
            using var form = new FormClientEdit(NextId(DemoData.Clients, c => c.Id));
            if (form.ShowDialog(this) != DialogResult.OK)
                return;

            DemoData.Clients.Add(form.Result);
            ResetSearch(_txtClientSearch, _cmbClientFilter);
            ApplyClientsFilter();
            RefreshLookups();
        }

        private void AddCoach()
        {
            using var form = new FormCoachEdit(NextId(DemoData.Coaches, c => c.Id));
            if (form.ShowDialog(this) != DialogResult.OK)
                return;

            DemoData.Coaches.Add(form.Result);
            ResetSearch(_txtCoachSearch, _cmbCoachFilter);
            ApplyCoachesFilter();
            _gridLessons.Invalidate();
        }

        private void AddLesson()
        {
            using var form = new FormLessonEdit(NextId(DemoData.Lessons, l => l.Id));
            if (form.ShowDialog(this) != DialogResult.OK)
                return;

            DemoData.Lessons.Add(form.Result);
            ResetSearch(_txtLessonSearch, _cmbLessonFilter);
            ApplyLessonsFilter();
            _gridVisits.Invalidate();
        }

        private void AddSubscription()
        {
            using var form = new FormSubscriptionEdit(NextId(DemoData.Subscriptions, s => s.Id));
            if (form.ShowDialog(this) != DialogResult.OK)
                return;

            DemoData.Subscriptions.Add(form.Result);
            ResetSearch(_txtSubscriptionSearch, _cmbSubscriptionFilter);
            ApplySubscriptionsFilter();
            ApplyClientsFilter();
            RefreshLookups();
        }

        private void AddVisit()
        {
            using var form = new FormVisitEdit(NextId(DemoData.Visits, v => v.Id));
            if (form.ShowDialog(this) != DialogResult.OK)
                return;

            DemoData.Visits.Add(form.Result);
            ResetSearch(_txtVisitSearch, _cmbVisitFilter);
            ApplyVisitsFilter();
        }

        #endregion

        #region Удаление записей

        private void DeleteClient()
        {
            _gridClients.EndEdit();
            if (_gridClients.CurrentRow?.DataBoundItem is not Client client)
            {
                Inform("Выберите клиента в таблице.");
                return;
            }

            var subscriptionCount = DemoData.Subscriptions.Count(s => s.Client_id == client.Id);
            var visitCount = DemoData.Visits.Count(v => v.Client_id == client.Id);

            var message = $"Удалить клиента «{client.FullName}»?";
            if (subscriptionCount + visitCount > 0)
            {
                message += $"\n\nБудут удалены связанные записи: абонементы — {subscriptionCount}, " +
                           $"посещения — {visitCount}.";
            }

            message += "\n\nИзменения не сохраняются в базе данных.";

            if (!Confirm(message))
                return;

            DemoData.Subscriptions.RemoveAll(s => s.Client_id == client.Id);
            DemoData.Visits.RemoveAll(v => v.Client_id == client.Id);
            DemoData.Clients.Remove(client);
            ApplyAllFilters();
            RefreshLookups();
        }

        private void DeleteCoach()
        {
            _gridCoaches.EndEdit();
            if (_gridCoaches.CurrentRow?.DataBoundItem is not Coache coach)
            {
                Inform("Выберите тренера в таблице.");
                return;
            }

            var lessons = DemoData.Lessons.Where(l => l.Coach_id == coach.Id).ToList();
            var lessonIds = lessons.Select(l => l.Id).ToHashSet();
            var visitCount = DemoData.Visits.Count(v => lessonIds.Contains(v.Lesson_id));

            var message = $"Удалить тренера «{coach.FullName}»?";
            if (lessons.Count + visitCount > 0)
            {
                message += $"\n\nБудут удалены связанные занятия — {lessons.Count}, " +
                           $"посещения — {visitCount}.";
            }

            message += "\n\nИзменения не сохраняются в базе данных.";

            if (!Confirm(message))
                return;

            DemoData.Lessons.RemoveAll(l => l.Coach_id == coach.Id);
            DemoData.Visits.RemoveAll(v => lessonIds.Contains(v.Lesson_id));
            DemoData.Coaches.Remove(coach);
            ApplyAllFilters();
        }

        private void DeleteLesson()
        {
            _gridLessons.EndEdit();
            if (_gridLessons.CurrentRow?.DataBoundItem is not Lesson lesson)
            {
                Inform("Выберите занятие в таблице.");
                return;
            }

            var visitCount = DemoData.Visits.Count(v => v.Lesson_id == lesson.Id);
            var message = $"Удалить занятие «{lesson.Title}»?";
            if (visitCount > 0)
                message += $"\n\nБудут удалены связанные посещения: {visitCount}.";

            message += "\n\nИзменения не сохраняются в базе данных.";

            if (!Confirm(message))
                return;

            DemoData.Visits.RemoveAll(v => v.Lesson_id == lesson.Id);
            DemoData.Lessons.Remove(lesson);
            ApplyAllFilters();
        }

        private void DeleteSubscription()
        {
            _gridSubscriptions.EndEdit();
            if (_gridSubscriptions.CurrentRow?.DataBoundItem is not Subscription subscription)
            {
                Inform("Выберите абонемент в таблице.");
                return;
            }

            if (!Confirm($"Удалить абонемент «{subscription.View}»?\n\nИзменения не сохраняются в базе данных."))
                return;

            DemoData.Subscriptions.Remove(subscription);
            ApplySubscriptionsFilter();
            ApplyClientsFilter();
        }

        private void DeleteVisit()
        {
            _gridVisits.EndEdit();
            if (_gridVisits.CurrentRow?.DataBoundItem is not Visit visit)
            {
                Inform("Выберите посещение в таблице.");
                return;
            }

            if (!Confirm("Удалить запись о посещении?\n\nИзменения не сохраняются в базе данных."))
                return;

            DemoData.Visits.Remove(visit);
            ApplyVisitsFilter();
        }

        #endregion

        #region Вспомогательные методы

        private void AddCurrent()
        {
            switch (_tabs.SelectedIndex)
            {
                case 0:
                    AddClient();
                    break;
                case 1:
                    AddCoach();
                    break;
                case 2:
                    AddLesson();
                    break;
                case 3:
                    AddSubscription();
                    break;
                default:
                    AddVisit();
                    break;
            }
        }

        private void DeleteCurrent()
        {
            switch (_tabs.SelectedIndex)
            {
                case 0:
                    DeleteClient();
                    break;
                case 1:
                    DeleteCoach();
                    break;
                case 2:
                    DeleteLesson();
                    break;
                case 3:
                    DeleteSubscription();
                    break;
                default:
                    DeleteVisit();
                    break;
            }
        }

        private void FocusSearch()
        {
            var box = _tabs.SelectedIndex switch
            {
                0 => _txtClientSearch,
                1 => _txtCoachSearch,
                2 => _txtLessonSearch,
                3 => _txtSubscriptionSearch,
                _ => _txtVisitSearch
            };

            box.Focus();
            box.SelectAll();
        }

        private void ShowAbout()
        {
            using var form = new FormAbout();
            form.ShowDialog(this);
        }

        private void ResetDemoData()
        {
            if (MessageBox.Show(
                    this,
                    "Вернуть исходные тестовые данные?\nВсе добавленные и изменённые записи будут потеряны.",
                    "Сброс демо-данных",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            DemoData.Reset();

            _suspendFilter = true;
            try
            {
                _txtClientSearch.Clear();
                _txtCoachSearch.Clear();
                _txtLessonSearch.Clear();
                _txtSubscriptionSearch.Clear();
                _txtVisitSearch.Clear();
                _cmbClientFilter.SelectedIndex = 0;
                _cmbCoachFilter.SelectedIndex = 0;
                _cmbLessonFilter.SelectedIndex = 0;
                _cmbSubscriptionFilter.SelectedIndex = 0;
                _cmbVisitFilter.SelectedIndex = 0;
            }
            finally
            {
                _suspendFilter = false;
            }

            ApplyAllFilters();
            RefreshLookups();
            Inform("Тестовые данные восстановлены.");
        }

        private static void ResetSearch(TextBox search, ComboBox filter)
        {
            search.Clear();
            if (filter.Items.Count > 0)
                filter.SelectedIndex = 0;
        }

        private static int NextId<T>(IEnumerable<T> items, Func<T, int> idSelector)
        {
            var list = items.ToList();
            return list.Count == 0 ? 1 : list.Max(idSelector) + 1;
        }

        private bool Confirm(string message)
        {
            return MessageBox.Show(
                this,
                message,
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        private void Inform(string message)
        {
            MessageBox.Show(this, message, AppInfo.ShortName,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private sealed class SearchBar
        {
            public SearchBar(Panel panel, TextBox search, ComboBox filter)
            {
                Panel = panel;
                Search = search;
                Filter = filter;
            }

            public Panel Panel { get; }
            public TextBox Search { get; }
            public ComboBox Filter { get; }
        }
    }
}
