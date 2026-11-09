namespace Kepler7.Game
{
    internal static class Program
    {
        // Acá arranca la aplicación.
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormPrincipal());
        }
    }
}
