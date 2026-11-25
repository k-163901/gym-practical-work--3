using System.ComponentModel.DataAnnotations;

namespace Тренажёрный_зал.Data
{
    /// <summary>
    /// Посещение клиентом занятия.
    /// </summary>
    public class Visit
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Идентификатор клиента (внешний ключ на таблицу Clients).</summary>
        public int Client_id { get; set; }

        /// <summary>Идентификатор занятия (внешний ключ на таблицу Lessons).</summary>
        public int Lesson_id { get; set; }

        /// <summary>Дата посещения в формате дд.ММ.гггг.</summary>
        public string Visit_Date { get; set; } = string.Empty;
    }
}
