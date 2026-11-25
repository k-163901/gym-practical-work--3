using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Тренажёрный_зал.Data
{
    /// <summary>
    /// Клиент тренажёрного зала.
    /// </summary>
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MidName { get; set; } = string.Empty;

        /// <summary>Дата рождения в формате дд.ММ.гггг.</summary>
        public string DateOfBirth { get; set; } = string.Empty;

        public string Number { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        /// <summary>Вычисляемое представление ФИО (в базе данных не хранится).</summary>
        [NotMapped]
        public string FullName => $"{LastName} {FirstName} {MidName}".Trim();

        public override string ToString() => FullName;
    }
}
