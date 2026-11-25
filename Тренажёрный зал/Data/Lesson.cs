using System.ComponentModel.DataAnnotations;

namespace Тренажёрный_зал.Data
{
    /// <summary>
    /// Групповое занятие (тренировка) в тренажёрном зале.
    /// </summary>
    public class Lesson
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        /// <summary>Продолжительность занятия в минутах.</summary>
        public string Duration { get; set; } = string.Empty;

        /// <summary>Время начала занятия в формате чч:мм.</summary>
        public string Time { get; set; } = string.Empty;

        /// <summary>Расписание, например «Пн, Ср, Пт».</summary>
        public string Schedule { get; set; } = string.Empty;

        /// <summary>Идентификатор тренера (внешний ключ на таблицу Coaches).</summary>
        public int Coach_id { get; set; }

        public override string ToString() => Title;
    }
}
