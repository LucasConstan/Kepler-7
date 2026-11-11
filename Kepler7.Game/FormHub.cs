using System.Collections.Generic;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Bridge;
using Kepler7.Core.Patrones.Proxy;

namespace Kepler7.Game
{
    // Ventana del Hub y progreso: XP, niveles, monedas y logros.
    // Proxy: las monedas solo se gastan en la tienda, y cada compra es un retiro que pasa por el ProxyBoveda,
    // que controla el nivel y anota todo en el log.
    // El progreso, el log y la partida (entre oleadas) se guardan en JSON, en archivo o en memoria,
    // sin cambiar las persistencias (un segundo uso del Bridge, cuyo ejercicio principal está en FormComunicaciones).
    public partial class FormHub : Form
    {
        private readonly HubProgreso _hub;
        private readonly Partida _partida;
        private readonly Action<string> _registrar;

        public FormHub(HubProgreso hub, Partida partida, Action<string> registrar)
        {
            InitializeComponent();
            new TransicionAparecer(this);
            _hub = hub;
            _partida = partida;
            _registrar = registrar;

            lblMensaje.Text = "Cada logro que ganes en una partida vale +" + HubProgreso.XpPorLogro + " XP y +"
                + HubProgreso.MonedasPorLogro + " monedas. Se pueden cobrar desde que superás la primera oleada.";
            cmbAlmacenamiento.Items.AddRange(Almacenamientos.Todos);
            cmbAlmacenamiento.SelectedItem = _hub.ObtenerAlmacenamiento();
            lblGuardado.Text = "Guardando en: " + _hub.ObtenerAlmacenamiento().Descripcion;

            cmbMejoras.Items.AddRange(CatalogoMejoras.Todas);
            cmbMejoras.SelectedIndex = 0;

            Actualizar();
        }

        #region Progreso: XP, niveles y logros

        private void btnMision_Click(object? sender, EventArgs e)
        {
            List<string> mensajes = _hub.CobrarRecompensaDeLogro();

            string texto = "";
            for (int i = 0; i < mensajes.Count; i++)
            {
                if (i > 0)
                {
                    texto = texto + " ";
                }
                texto = texto + mensajes[i];
                _registrar("Hub: " + mensajes[i]);
            }
            lblMensaje.Text = texto;

            Actualizar();
        }

        #endregion

        #region Tienda: mejoras para la próxima partida (se pagan a través del Proxy)

        private void cmbMejoras_SelectedIndexChanged(object? sender, EventArgs e)
        {
            Mejora? mejora = cmbMejoras.SelectedItem as Mejora;
            if (mejora != null)
            {
                lblDetalleMejora.Text = mejora.Descripcion;
            }
        }

        private void btnComprar_Click(object? sender, EventArgs e)
        {
            Mejora? mejora = cmbMejoras.SelectedItem as Mejora;
            if (mejora == null)
            {
                return;
            }

            string resultado = _hub.ComprarMejora(mejora);
            lblResultadoTienda.Text = resultado;
            _registrar("Tienda: " + resultado);
            Actualizar();
        }

        // Arma el texto de lo que ya está comprado, por ejemplo "Créditos extra x2, Escudo de reserva".
        private string TextoMejorasPendientes(List<string> pendientes)
        {
            string texto = "";
            Mejora[] todas = CatalogoMejoras.Todas;
            for (int i = 0; i < todas.Length; i++)
            {
                int cantidad = 0;
                for (int j = 0; j < pendientes.Count; j++)
                {
                    if (pendientes[j] == todas[i].Nombre)
                    {
                        cantidad = cantidad + 1;
                    }
                }

                if (cantidad > 0)
                {
                    if (texto != "")
                    {
                        texto = texto + ", ";
                    }
                    texto = texto + todas[i].Nombre;
                    if (cantidad > 1)
                    {
                        texto = texto + " x" + cantidad;
                    }
                }
            }

            if (texto == "")
            {
                return "Todavía no compraste nada para la próxima partida.";
            }
            return "Para la próxima partida: " + texto;
        }

        #endregion

        #region Bridge: guardar y cargar en JSON

        private void cmbAlmacenamiento_SelectedIndexChanged(object? sender, EventArgs e)
        {
            IAlmacenamiento? elegido = cmbAlmacenamiento.SelectedItem as IAlmacenamiento;
            if (elegido == null || elegido == _hub.ObtenerAlmacenamiento())
            {
                return;
            }

            _hub.CambiarAlmacenamiento(elegido);
            lblGuardado.Text = "Ahora se guarda en: " + elegido.Descripcion + "\nUsá Guardar o Cargar para probarlo.";
            _registrar("Hub: el guardado ahora usa " + elegido.Nombre + ".");
        }

        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            IAlmacenamiento almacenamiento = _hub.ObtenerAlmacenamiento();
            try
            {
                _hub.Guardar();

                // La partida solo se guarda entre oleadas: en pleno combate habría que guardar enemigos y turnos a medias.
                string textoPartida;
                if (_partida.Fase == FasePartida.Preparacion)
                {
                    _hub.GuardarPartida(_partida.CrearGuardado());
                    textoPartida = "Partida guardada en la oleada " + _partida.NumeroOleada + ": al abrir el juego te pregunta si querés seguirla.";
                }
                else if (_partida.Fase == FasePartida.Oleada)
                {
                    textoPartida = "La partida no se guardó: se guarda entre oleadas, y ahora hay una en curso.";
                }
                else
                {
                    textoPartida = "La partida ya terminó, así que no hay partida para guardar.";
                }

                lblGuardado.Text = "Progreso y log de la Bóveda guardados. " + textoPartida + "\n" + almacenamiento.Descripcion;
            }
            catch (Exception ex)
            {
                // Por ejemplo, si no hay permiso para escribir en la carpeta.
                lblGuardado.Text = "No se pudo guardar: " + ex.Message;
            }

            _registrar("Hub: " + lblGuardado.Text.Replace("\n", " "));
            Actualizar();
        }

        private void btnCargar_Click(object? sender, EventArgs e)
        {
            IAlmacenamiento almacenamiento = _hub.ObtenerAlmacenamiento();
            try
            {
                _hub.Cargar();
                lblGuardado.Text = "Se cargó el progreso y el log desde " + almacenamiento.Nombre + ".\n" + almacenamiento.Descripcion;
            }
            catch (Exception ex)
            {
                lblGuardado.Text = "No se pudo cargar: " + ex.Message;
            }

            lblMensaje.Text = "";
            lblResultadoTienda.Text = "";
            _registrar("Hub: " + lblGuardado.Text.Replace("\n", " "));
            Actualizar();
        }

        #endregion

        private void Actualizar()
        {
            Progreso progreso = _hub.ObtenerProgreso();
            int saldo = _hub.ObtenerBoveda().ConsultarSaldo();
            int xpEnEsteNivel = progreso.Xp % Progreso.XpPorNivel;

            lblProgreso.Text = "Nivel " + progreso.Nivel + "\n" +
                               "XP total: " + progreso.Xp + "  (" + xpEnEsteNivel + "/" + Progreso.XpPorNivel + " para el próximo nivel)\n" +
                               "Monedas: " + saldo;
            pbNivel.Maximum = Progreso.XpPorNivel;
            pbNivel.Value = xpEnEsteNivel;

            // El botón solo se habilita si hay recompensas de logros para cobrar.
            btnMision.Text = "Cobrar recompensa de logro (" + progreso.CargasDeLogro + " disponibles)";
            btnMision.Enabled = progreso.CargasDeLogro > 0;

            // Los logros de patrones se marcan con estrella llena si se ganaron en esta partida, y llevan
            // la cuenta de todas las veces que se cobraron. Los que faltan muestran la pista para conseguirlos.
            Logro[] todos = CatalogoLogros.Todos;
            int enEstaPartida = 0;
            int dePatrones = 0;
            lstLogros.BeginUpdate();
            lstLogros.Items.Clear();
            for (int i = 0; i < todos.Length; i++)
            {
                string encabezado = todos[i].Nombre + "  [" + todos[i].Patron + "]";

                if (todos[i].Patron == CatalogoLogros.PatronProgreso)
                {
                    if (progreso.Logros.Contains(todos[i].Nombre))
                    {
                        lstLogros.Items.Add("★ " + encabezado);
                    }
                    else
                    {
                        lstLogros.Items.Add("☆ " + encabezado + ": " + todos[i].ComoSeConsigue);
                    }
                }
                else
                {
                    dePatrones = dePatrones + 1;
                    string total = "";
                    int veces = progreso.VecesQueSeConsiguio(todos[i].Nombre);
                    if (veces == 1)
                    {
                        total = "  · conseguido 1 vez";
                    }
                    else if (veces > 1)
                    {
                        total = "  · conseguido " + veces + " veces";
                    }

                    if (_hub.GanadoEnEstaPartida(todos[i].Nombre))
                    {
                        enEstaPartida = enEstaPartida + 1;
                        if (_hub.PendienteDeAcreditar(todos[i].Nombre))
                        {
                            lstLogros.Items.Add("★ " + encabezado + " (esta partida, se acredita al superar la 1ª oleada)" + total);
                        }
                        else
                        {
                            lstLogros.Items.Add("★ " + encabezado + " (esta partida, acreditado)" + total);
                        }
                    }
                    else
                    {
                        lstLogros.Items.Add("☆ " + encabezado + ": " + todos[i].ComoSeConsigue + total);
                    }
                }
            }
            lstLogros.EndUpdate();
            lblLogros.Text = "Esta partida: " + enEstaPartida + " de " + dePatrones + " logros";

            lblBoveda.Text = "Saldo: " + saldo + " monedas. El Proxy solo deja gastarlas desde el nivel "
                + ProxyBoveda.NivelParaRetirar + " (tu nivel: " + progreso.Nivel + ").";
            lblPendientes.Text = TextoMejorasPendientes(progreso.MejorasPendientes);

            IReadOnlyList<string> log = _hub.ObtenerLogBoveda();
            lstLogBoveda.BeginUpdate();
            lstLogBoveda.Items.Clear();
            for (int i = 0; i < log.Count; i++)
            {
                lstLogBoveda.Items.Add(log[i]);
            }
            lstLogBoveda.TopIndex = Math.Max(0, lstLogBoveda.Items.Count - 1);
            lstLogBoveda.EndUpdate();
        }
    }
}
