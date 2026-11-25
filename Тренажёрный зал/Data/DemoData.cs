namespace Тренажёрный_зал.Data
{
    /// <summary>
    /// Тестовые данные приложения. Заполняются непосредственно в коде
    /// при запуске программы (требование практической работы № 3).
    /// Все изменения хранятся только в оперативной памяти.
    /// </summary>
    internal static class DemoData
    {
        public static List<Client> Clients { get; } = new();
        public static List<Coache> Coaches { get; } = new();
        public static List<Lesson> Lessons { get; } = new();
        public static List<Subscription> Subscriptions { get; } = new();
        public static List<Visit> Visits { get; } = new();

        static DemoData()
        {
            Reset();
        }

        /// <summary>Возвращает набор тестовых данных к исходному состоянию.</summary>
        public static void Reset()
        {
            ResetClients();
            ResetCoaches();
            ResetLessons();
            ResetSubscriptions();
            ResetVisits();
        }

        private static void ResetClients()
        {
            Clients.Clear();
            Clients.AddRange(new[]
            {
                new Client { Id = 1, LastName = "Агатов", FirstName = "Дмитрий", MidName = "Антонович", DateOfBirth = "24.09.2001", Number = "+7 (985) 248-57-89", Email = "d.agatov@mail.ru" },
                new Client { Id = 2, LastName = "Белова", FirstName = "Мария", MidName = "Сергеевна", DateOfBirth = "12.03.1998", Number = "+7 (916) 402-11-73", Email = "m.belova@mail.ru" },
                new Client { Id = 3, LastName = "Волков", FirstName = "Артём", MidName = "Игоревич", DateOfBirth = "05.11.1993", Number = "+7 (903) 771-45-20", Email = "a.volkov@yandex.ru" },
                new Client { Id = 4, LastName = "Громова", FirstName = "Екатерина", MidName = "Павловна", DateOfBirth = "17.07.2000", Number = "+7 (925) 118-63-04", Email = "e.gromova@gmail.com" },
                new Client { Id = 5, LastName = "Дорохов", FirstName = "Никита", MidName = "Андреевич", DateOfBirth = "29.01.1995", Number = "+7 (999) 512-08-46", Email = "n.dorohov@mail.ru" },
                new Client { Id = 6, LastName = "Ершова", FirstName = "Анна", MidName = "Викторовна", DateOfBirth = "03.06.2002", Number = "+7 (910) 355-92-17", Email = "a.ershova@mail.ru" },
                new Client { Id = 7, LastName = "Жуков", FirstName = "Павел", MidName = "Олегович", DateOfBirth = "21.12.1991", Number = "+7 (926) 640-27-51", Email = "p.zhukov@yandex.ru" },
                new Client { Id = 8, LastName = "Зайцева", FirstName = "Ольга", MidName = "Дмитриевна", DateOfBirth = "08.04.1997", Number = "+7 (977) 230-74-18", Email = "o.zaytseva@gmail.com" }
            });
        }

        private static void ResetCoaches()
        {
            Coaches.Clear();
            Coaches.AddRange(new[]
            {
                new Coache { Id = 1, LastName = "Соколов", FirstName = "Андрей", MidName = "Викторович", Specialization = "Силовой тренинг", Number = "+7 (901) 111-22-33" },
                new Coache { Id = 2, LastName = "Морозова", FirstName = "Ирина", MidName = "Павловна", Specialization = "Йога и растяжка", Number = "+7 (901) 222-33-44" },
                new Coache { Id = 3, LastName = "Кузнецов", FirstName = "Максим", MidName = "Сергеевич", Specialization = "Кроссфит", Number = "+7 (901) 333-44-55" },
                new Coache { Id = 4, LastName = "Лебедева", FirstName = "Наталья", MidName = "Игоревна", Specialization = "Фитнес-аэробика", Number = "+7 (901) 444-55-66" },
                new Coache { Id = 5, LastName = "Орлов", FirstName = "Виктор", MidName = "Анатольевич", Specialization = "Бокс", Number = "+7 (901) 555-66-77" }
            });
        }

        private static void ResetLessons()
        {
            Lessons.Clear();
            Lessons.AddRange(new[]
            {
                new Lesson { Id = 1, Title = "Силовая тренировка", Duration = "60", Time = "18:00", Schedule = "Пн, Ср, Пт", Coach_id = 1 },
                new Lesson { Id = 2, Title = "Йога для начинающих", Duration = "75", Time = "09:00", Schedule = "Вт, Чт", Coach_id = 2 },
                new Lesson { Id = 3, Title = "Кроссфит интенсив", Duration = "50", Time = "19:30", Schedule = "Пн, Ср", Coach_id = 3 },
                new Lesson { Id = 4, Title = "Фитнес-аэробика", Duration = "45", Time = "17:00", Schedule = "Вт, Чт, Сб", Coach_id = 4 },
                new Lesson { Id = 5, Title = "Бокс: техника", Duration = "60", Time = "20:00", Schedule = "Пн, Пт", Coach_id = 5 },
                new Lesson { Id = 6, Title = "Стретчинг", Duration = "45", Time = "10:30", Schedule = "Сб, Вс", Coach_id = 2 },
                new Lesson { Id = 7, Title = "Функциональный тренинг", Duration = "55", Time = "12:00", Schedule = "Вт, Сб", Coach_id = 3 }
            });
        }

        private static void ResetSubscriptions()
        {
            Subscriptions.Clear();
            Subscriptions.AddRange(new[]
            {
                new Subscription { Id = 1, View = "Месячный безлимит", Price = "3200", Start_date = "01.11.2026", End_date = "30.11.2026", Number_of_visits = 999, Client_id = 1 },
                new Subscription { Id = 2, View = "Утро", Price = "1900", Start_date = "01.11.2026", End_date = "30.11.2026", Number_of_visits = 12, Client_id = 2 },
                new Subscription { Id = 3, View = "Годовой безлимит", Price = "28000", Start_date = "01.07.2026", End_date = "30.06.2027", Number_of_visits = 999, Client_id = 3 },
                new Subscription { Id = 4, View = "Студенческий", Price = "1500", Start_date = "05.11.2026", End_date = "04.12.2026", Number_of_visits = 8, Client_id = 4 },
                new Subscription { Id = 5, View = "Месячный безлимит", Price = "3200", Start_date = "10.11.2026", End_date = "09.12.2026", Number_of_visits = 999, Client_id = 5 },
                new Subscription { Id = 6, View = "Пробный", Price = "900", Start_date = "15.11.2026", End_date = "21.11.2026", Number_of_visits = 3, Client_id = 6 },
                new Subscription { Id = 7, View = "Утро", Price = "1900", Start_date = "01.10.2026", End_date = "31.10.2026", Number_of_visits = 12, Client_id = 7 },
                new Subscription { Id = 8, View = "Месячный безлимит", Price = "3200", Start_date = "12.11.2026", End_date = "11.12.2026", Number_of_visits = 999, Client_id = 8 }
            });
        }

        private static void ResetVisits()
        {
            Visits.Clear();
            Visits.AddRange(new[]
            {
                new Visit { Id = 1, Client_id = 1, Lesson_id = 1, Visit_Date = "18.11.2026" },
                new Visit { Id = 2, Client_id = 1, Lesson_id = 3, Visit_Date = "19.11.2026" },
                new Visit { Id = 3, Client_id = 2, Lesson_id = 2, Visit_Date = "19.11.2026" },
                new Visit { Id = 4, Client_id = 3, Lesson_id = 5, Visit_Date = "17.11.2026" },
                new Visit { Id = 5, Client_id = 4, Lesson_id = 4, Visit_Date = "18.11.2026" },
                new Visit { Id = 6, Client_id = 5, Lesson_id = 7, Visit_Date = "20.11.2026" },
                new Visit { Id = 7, Client_id = 6, Lesson_id = 6, Visit_Date = "15.11.2026" },
                new Visit { Id = 8, Client_id = 7, Lesson_id = 2, Visit_Date = "16.11.2026" },
                new Visit { Id = 9, Client_id = 8, Lesson_id = 1, Visit_Date = "20.11.2026" },
                new Visit { Id = 10, Client_id = 3, Lesson_id = 7, Visit_Date = "20.11.2026" }
            });
        }
    }
}
