namespace Kepler7.Game
{
    internal static class Program
    {
        // Acá arranca la aplicación: primero la pantalla inicial y después el juego.
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Si en la pantalla inicial se elige Salir (o se cierra), el juego termina ahí.
            bool continuarGuardada;
            using (FormInicio inicio = new FormInicio())
            {
                if (inicio.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                continuarGuardada = inicio.ContinuarGuardada;
            }

            Application.Run(new FormPrincipal(continuarGuardada));
        }
    }
}
