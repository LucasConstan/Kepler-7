using System.Collections.Generic;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Bridge;
using Kepler7.Core.Patrones.Proxy;

namespace Kepler7.Game
{
    // Ventana del Hub y progreso: XP, niveles, monedas y logros.
    // Proxy: las monedas se retiran a través del ProxyBoveda, que controla el nivel y anota todo en el log.
    // Bridge: el progreso y el log se guardan en JSON, en archivo o en memoria, sin cambiar la Persistencia.
    public partial class FormHub : Form
    {
        private readonly HubProgreso _hub;
        private readonly Action<string> _registrar;

        public FormHub(HubProgreso hub, Action<string> registrar)
        {
            InitializeComponent();
            _hub = hub;
            _registrar = registrar;

            btnMision.Text = "Completar misión (+" + HubProgreso.XpPorMision + " XP, +" + HubProgreso.MonedasPorMision + " monedas)";
            cmbAlmacenamiento.Items.AddRange(Almacenamientos.Todos);
            cmbAlmacenamiento.SelectedItem = _hub.ObtenerAlmacenamiento();
            lblGuardado.Text = "Guardando en: " + _hub.ObtenerAlmacenamiento().Descripcion;

            Actualizar();
        }

        #region Progreso: XP, niveles y logros

        private void btnMision_Click(object? sender, EventArgs e)
        {
            List<string> mensajes = _hub.CompletarMision();

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

        #region Proxy: retirar monedas de la Bóveda

        private void btnRetirar_Click(object? sender, EventArgs e)
        {
            int cantidad = (int)nudCantidad.Value;

            // La ventana solo conoce IBoveda: no sabe que del otro lado está el ProxyBoveda.
            IBoveda boveda = _hub.ObtenerBoveda();
            if (boveda.Retirar(cantidad))
            {
                lblResultadoBoveda.Text = "Retiraste " + cantidad + " monedas.";
            }
            else
            {
                lblResultadoBoveda.Text = "Retiro rechazado (mirá el log).";
            }

            _registrar("Bóveda: " + lblResultadoBoveda.Text);
            Actualizar();
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
                lblGuardado.Text = "Progreso y log de la Bóveda guardados en JSON.\n" + almacenamiento.Descripcion;
            }
            catch (Exception ex)
            {
                // Por ejemplo, si no hay permiso para escribir en la carpeta.
                lblGuardado.Text = "No se pudo guardar: " + ex.Message;
            }

            _registrar("Hub: " + lblGuardado.Text.Replace("\n", " "));
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
            lblResultadoBoveda.Text = "";
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

            lstLogros.BeginUpdate();
            lstLogros.Items.Clear();
            if (progreso.Logros.Count == 0)
            {
                lstLogros.Items.Add("(todavía ninguno)");
            }
            for (int i = 0; i < progreso.Logros.Count; i++)
            {
                lstLogros.Items.Add("★ " + progreso.Logros[i]);
            }
            lstLogros.EndUpdate();

            lblBoveda.Text = "Saldo: " + saldo + " monedas. El Proxy solo deja retirar desde el nivel "
                + ProxyBoveda.NivelParaRetirar + " (tu nivel: " + progreso.Nivel + ").";

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
