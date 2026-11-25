using Microsoft.EntityFrameworkCore;

namespace Тренажёрный_зал.Data
{
    /// <summary>
    /// Контекст базы данных, спроектированный в практических работах № 1–2.
    /// В практической работе № 3 контекст не используется: приложение работает
    /// с тестовыми данными в памяти и не подключается к базе данных.
    /// </summary>
    public class GymContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Coache> Coaches { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=gym.db");
    }
}
