using Тренажёрный_зал.Forms;

namespace Тренажёрный_зал
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа в приложение. Сначала открывается форма
        /// авторизации, затем — главное окно информационной системы.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            using var login = new FormLogin();
            if (login.ShowDialog() != DialogResult.OK)
                return;

            Application.Run(new FormMain(login.UserName));
        }
    }
}
