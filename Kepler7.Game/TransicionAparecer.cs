namespace Kepler7.Game
{
    // Hace que una ventana aparezca de a poco en vez de golpe: arranca transparente y un Timer
    // le va subiendo la opacidad. Se usa un Timer y no un Thread.Sleep para no congelar la interfaz.
    // Para usarla alcanza con poner "new TransicionAparecer(this);" en el constructor de la ventana.
    internal class TransicionAparecer
    {
        private const double PasoOpacidad = 0.12;

        private readonly Form _ventana;
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        public TransicionAparecer(Form ventana)
        {
            _ventana = ventana;
            _ventana.Opacity = 0;

            _timer.Interval = 15;
            _timer.Tick += Timer_Tick;

            // El Timer arranca recién cuando la ventana ya está en pantalla.
            _ventana.Shown += Ventana_Shown;
            _ventana.FormClosed += Ventana_FormClosed;
        }

        private void Ventana_Shown(object? sender, EventArgs e)
        {
            _timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            double nueva = _ventana.Opacity + PasoOpacidad;
            if (nueva >= 1)
            {
                _ventana.Opacity = 1;
                _timer.Stop();
            }
            else
            {
                _ventana.Opacity = nueva;
            }
        }

        private void Ventana_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
