using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Facade;

namespace Kepler7.Game
{
    /// <summary>
    /// Ventana del FACADE: el operador solo tiene tres botones y el ControlCentral coordina
    /// a los seis subsistemas. La ventana avisa con eventos cuando empieza la oleada o se apaga
    /// la estación, para que la ventana principal maneje el Timer.
    /// </summary>
    public partial class FormControlCentral : Form
    {
        private readonly ControlCentral _control;
        private readonly Partida _partida;
        private readonly Action<string> _registrar;

        public event Action? OleadaIniciada;
        public event Action? EstacionApagada;

        public FormControlCentral(ControlCentral control, Partida partida, Action<string> registrar)
        {
            InitializeComponent();
            _control = control;
            _partida = partida;
            _registrar = registrar;
            ActualizarBotones();
        }

        private void btnIniciarTurno_Click(object? sender, EventArgs e)
        {
            MostrarPasos(_control.IniciarTurno());
            if (_partida.Fase == FasePartida.Oleada) OleadaIniciada?.Invoke();
        }

        private void btnEmergencia_Click(object? sender, EventArgs e)
        {
            MostrarPasos(_control.EntrarEnEmergencia());
        }

        private void btnApagar_Click(object? sender, EventArgs e)
        {
            var respuesta = MessageBox.Show("¿Apagar la estación? La partida termina.", "Apagar Kepler-7",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (respuesta != DialogResult.Yes) return;

            MostrarPasos(_control.ApagarEstacion());
            EstacionApagada?.Invoke();
        }

        // Muestra la secuencia de acciones que coordinó la fachada y la copia al registro de la estación.
        private void MostrarPasos(List<string> pasos)
        {
            txtPasos.Text = string.Join(Environment.NewLine, pasos);
            foreach (var paso in pasos) _registrar(paso);
            ActualizarBotones();
        }

        private void ActualizarBotones()
        {
            bool terminada = _partida.Fase == FasePartida.Victoria || _partida.Fase == FasePartida.Derrota;
            btnIniciarTurno.Enabled = _partida.Fase == FasePartida.Preparacion;
            btnEmergencia.Enabled = _control.EmergenciaDisponible;
            btnApagar.Enabled = !terminada;
        }
    }
}
