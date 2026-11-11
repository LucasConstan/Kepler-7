using System.Collections.Generic;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Bridge;

namespace Kepler7.Game
{
    // Ventana del Bridge: se elige un tipo de mensaje y un cifrado por separado, y se combinan en el momento.
    // Cambiar el tipo crea un mensaje nuevo con el cifrado elegido; cambiar el cifrado NO crea otro mensaje:
    // se lo cambia al mensaje que ya existe (CambiarCifrado). Así se ve que las dos jerarquías son independientes.
    public partial class FormComunicaciones : Form
    {
        // Una sola instancia de cada cifrado mientras la ventana está abierta (el cuántico guarda sus claves).
        private readonly ICifrado[] _cifrados = { new SinCifrado(), new CifradoAES(), new CifradoCuantico() };
        private static readonly string[] TiposMensaje = { "Texto", "Alerta", "Emergencia" };

        private readonly List<Transmision> _historial;
        private readonly Action<string> _registrar;
        private readonly HubProgreso _hub;
        private Mensaje _mensaje;

        public FormComunicaciones(List<Transmision> historial, Action<string> registrar, HubProgreso hub)
        {
            InitializeComponent();
            new TransicionAparecer(this);
            _historial = historial;
            _registrar = registrar;
            _hub = hub;

            cmbTipo.Items.AddRange(TiposMensaje);
            for (int i = 0; i < _cifrados.Length; i++)
            {
                cmbCifrado.Items.Add(_cifrados[i].Nombre);
            }

            _mensaje = new MensajeTexto(_cifrados[0]);
            cmbTipo.SelectedIndex = 0;
            cmbCifrado.SelectedIndex = 0;
            txtContenido.Text = "Resistimos la oleada. Pedimos más créditos para reparar los núcleos.";

            for (int i = 0; i < _historial.Count; i++)
            {
                lstHistorial.Items.Add(_historial[i]);
            }
            MostrarCombinacion();
        }

        private void cmbTipo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ICifrado cifrado = _mensaje.ObtenerCifrado();
            switch (cmbTipo.SelectedIndex)
            {
                case 1:
                    _mensaje = new MensajeAlerta(cifrado);
                    break;
                case 2:
                    _mensaje = new MensajeEmergencia(cifrado);
                    break;
                default:
                    _mensaje = new MensajeTexto(cifrado);
                    break;
            }
            MostrarCombinacion();
        }

        private void cmbCifrado_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbCifrado.SelectedIndex < 0)
            {
                return;
            }

            // BRIDGE: el mismo mensaje cambia de cifrado en plena ejecución.
            _mensaje.CambiarCifrado(_cifrados[cmbCifrado.SelectedIndex]);
            MostrarCombinacion();
        }

        private void btnEnviar_Click(object? sender, EventArgs e)
        {
            string contenido = txtContenido.Text.Trim();
            if (contenido == "")
            {
                lblCombinacion.Text = "Escribí algo para mandar a la Tierra.";
                return;
            }

            Transmision transmision = _mensaje.Enviar(contenido);
            _historial.Add(transmision);
            lstHistorial.Items.Add(transmision);
            lstHistorial.SelectedItem = transmision;

            _registrar("Comunicaciones: " + transmision.TipoMensaje + " enviado a la Tierra con " + transmision.NombreCifrado);
            if (!(_mensaje.ObtenerCifrado() is SinCifrado))
            {
                _hub.DesbloquearLogro(CatalogoLogros.Bridge);
            }
        }

        private void lstHistorial_SelectedIndexChanged(object? sender, EventArgs e)
        {
            Transmision? transmision = lstHistorial.SelectedItem as Transmision;
            if (transmision != null)
            {
                MostrarTransmision(transmision);
            }
        }

        private void MostrarCombinacion()
        {
            ICifrado cifrado = _mensaje.ObtenerCifrado();
            lblCombinacion.Text = "Combinación actual: " + _mensaje.Tipo + " (prioridad " + _mensaje.Prioridad + ") + "
                + cifrado.Nombre + ". " + cifrado.Descripcion;
        }

        private void MostrarTransmision(Transmision t)
        {
            string nl = Environment.NewLine;
            string texto = "MENSAJE ENVIADO (" + t.TipoMensaje + " + " + t.NombreCifrado + ", " + t.Hora.ToString("HH:mm:ss") + ")" + nl
                + t.TextoOriginal + nl + nl
                + "RESULTADO DEL CIFRADO (lo que viaja por el espacio)" + nl
                + t.TextoCifrado + nl + nl
                + "RECIBIDO EN LA TIERRA" + nl
                + t.TextoRecibido + nl
                + (t.LlegoIntacto ? "✔ Llegó intacto." : "✖ No coincide con el original.") + nl + nl;

            for (int i = 0; i < t.Pasos.Count; i++)
            {
                texto = texto + t.Pasos[i] + nl;
            }

            txtResultado.Text = texto;
        }
    }
}
