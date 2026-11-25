using System.ComponentModel.DataAnnotations;

namespace Тренажёрный_зал.Data
{
    /// <summary>
    /// Абонемент клиента.
    /// </summary>
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Вид абонемента, например «Месячный безлимит».</summary>
        public string View { get; set; } = string.Empty;

        /// <summary>Стоимость абонемента, руб.</summary>
        public string Price { get; set; } = string.Empty;

        /// <summary>Дата начала действия в формате дд.ММ.гггг.</summary>
        public string Start_date { get; set; } = string.Empty;

        /// <summary>Дата окончания действия в формате дд.ММ.гггг.</summary>
        public string End_date { get; set; } = string.Empty;

        /// <summary>Количество посещений, включённых в абонемент.</summary>
        public int Number_of_visits { get; set; }

        /// <summary>Идентификатор клиента (внешний ключ на таблицу Clients).</summary>
        public int Client_id { get; set; }

        public override string ToString() => View;
    }
}
