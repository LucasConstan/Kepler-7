using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Bridge;

namespace Kepler7.Game
{
    // La pantalla con la que arranca el juego: el nombre, los integrantes del grupo y el menú para empezar.
    // No arranca la partida ella misma: devuelve OK y le deja dicho a Program si hay que retomar la guardada.
    public partial class FormInicio : Form
    {
        private static readonly string[] Integrantes = { "Lucas Constantinidis", "Juan Ignacio Lambezat", "Matías Elebi", "Joaquin Bustamante", "Eliana Szinanski" };

        private readonly PartidaGuardada? _guardada;

        // True si se eligió "Continuar partida"; false si se eligió "Nueva partida".
        public bool ContinuarGuardada { get; private set; }

        public FormInicio()
        {
            InitializeComponent();
            new TransicionAparecer(this);

            string nombres = "";
            for (int i = 0; i < Integrantes.Length; i++)
            {
                if (i > 0)
                {
                    nombres = nombres + "  ·  ";
                }
                nombres = nombres + Integrantes[i];
            }
            lblIntegrantes.Text = "Integrantes\n" + nombres;

            // Solo se mira si hay una partida guardada; retomarla o descartarla lo hace la ventana principal.
            _guardada = LeerPartidaGuardada();
            if (_guardada == null)
            {
                btnContinuar.Enabled = false;
                lblGuardada.Text = "No hay ninguna partida guardada.";
            }
            else
            {
                lblGuardada.Text = "Guardada el " + _guardada.Fecha + ": oleada " + _guardada.NumeroOleada + ", " + _guardada.Creditos + " créditos.";
            }
        }

        private static PartidaGuardada? LeerPartidaGuardada()
        {
            try
            {
                PersistenciaPartida persistencia = new PersistenciaPartida(Almacenamientos.Archivo);
                return persistencia.Cargar();
            }
            catch (IOException)
            {
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
        }

        private void btnNueva_Click(object? sender, EventArgs e)
        {
            if (_guardada != null)
            {
                DialogResult respuesta = MessageBox.Show(
                    "Hay una partida guardada en la oleada " + _guardada.NumeroOleada + ".\nSi empezás una nueva, la guardada se descarta. ¿Seguís?",
                    "Nueva partida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (respuesta != DialogResult.Yes)
                {
                    return;
                }
            }

            ContinuarGuardada = false;
            DialogResult = DialogResult.OK;
        }

        private void btnContinuar_Click(object? sender, EventArgs e)
        {
            ContinuarGuardada = true;
            DialogResult = DialogResult.OK;
        }

        private void btnComoJugar_Click(object? sender, EventArgs e)
        {
            using (FormComoJugar ventana = new FormComoJugar())
            {
                ventana.ShowDialog(this);
            }
        }
    }
}
