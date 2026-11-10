namespace Kepler7.Game
{
    // El cartelito que aparece abajo a la derecha cuando se gana un logro.
    // Aparece de a poco, se queda unos segundos y se desvanece solo. No le saca el foco a la ventana
    // en la que se está jugando, así no interrumpe aunque haya otra ventana abierta encima.
    public partial class FormAviso : Form
    {
        private const double PasoOpacidad = 0.1;
        private const int TicksVisible = 80; // con el Timer cada 30 ms, son unos 2,4 segundos

        // Cuántos avisos hay en pantalla, para apilarlos en vez de taparse unos con otros.
        private static int _abiertos = 0;

        // 0 = apareciendo, 1 = quieto, 2 = desapareciendo.
        private int _fase = 0;
        private int _ticksQuieto = 0;

        public FormAviso(string titulo, string texto, Form principal)
        {
            InitializeComponent();
            lblTitulo.Text = titulo;
            lblTexto.Text = texto;
            Opacity = 0;

            // Se ubica abajo a la derecha de la ventana principal, encima de los avisos que ya estén abiertos.
            Rectangle zona = principal.Bounds;
            Left = zona.Right - Width - 24;
            Top = zona.Bottom - Height - 24 - _abiertos * (Height + 8);
            _abiertos = _abiertos + 1;
        }

        // Se muestra sin activarse, así el teclado y el foco quedan donde estaban.
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        // Estilos de ventana de Windows: siempre arriba (0x8) y sin tomar el foco (0x08000000).
        // Se hace así y no con TopMost porque TopMost activa la ventana al mostrarla.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parametros = base.CreateParams;
                parametros.ExStyle = parametros.ExStyle | 0x00000008 | 0x08000000;
                return parametros;
            }
        }

        private void FormAviso_Shown(object? sender, EventArgs e)
        {
            timerAnimacion.Start();
        }

        private void timerAnimacion_Tick(object? sender, EventArgs e)
        {
            if (_fase == 0)
            {
                double nueva = Opacity + PasoOpacidad;
                if (nueva >= 1)
                {
                    Opacity = 1;
                    _fase = 1;
                }
                else
                {
                    Opacity = nueva;
                }
            }
            else if (_fase == 1)
            {
                _ticksQuieto = _ticksQuieto + 1;
                if (_ticksQuieto >= TicksVisible)
                {
                    _fase = 2;
                }
            }
            else
            {
                double nueva = Opacity - PasoOpacidad;
                if (nueva <= 0)
                {
                    timerAnimacion.Stop();
                    Close();
                }
                else
                {
                    Opacity = nueva;
                }
            }
        }

        private void FormAviso_FormClosed(object? sender, FormClosedEventArgs e)
        {
            _abiertos = _abiertos - 1;
        }
    }
}
