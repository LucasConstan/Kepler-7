using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Flyweight;

namespace Kepler7.Game
{
    // Ventana del Flyweight: muestra los tripulantes repartidos por subsistema, que comparten
    // solo 5 objetos de especie, y permite mandar refuerzos a un núcleo.
    public partial class FormTripulacion : Form
    {
        private readonly Partida _partida;
        private readonly Action<string> _registrar;
        private readonly HubProgreso _hub;

        public FormTripulacion(Partida partida, Action<string> registrar, HubProgreso hub)
        {
            InitializeComponent();
            new TransicionAparecer(this);
            _partida = partida;
            _registrar = registrar;
            _hub = hub;

            cmbCarril.Items.AddRange(Estacion.Subsistemas);
            cmbCarril.SelectedIndex = 0;
            btnEnviar.Text = "Enviar " + Partida.TripulantesPorEnvio + " tripulantes (" + Partida.CostoEnvioTripulantes + " cr)";

            Actualizar();
        }

        private void btnEnviar_Click(object? sender, EventArgs e)
        {
            int carril = cmbCarril.SelectedIndex;
            if (_partida.EnviarTripulantes(carril))
            {
                lblResultado.Text = "Llegaron " + Partida.TripulantesPorEnvio + " tripulantes a " + Estacion.Subsistemas[carril] + ".";
                _hub.DesbloquearLogro(CatalogoLogros.Flyweight);
            }
            else
            {
                lblResultado.Text = "No se pudo enviar a " + Estacion.Subsistemas[carril] + " (faltan créditos o el subsistema cayó).";
                _registrar("Tripulación: " + lblResultado.Text);
            }

            Actualizar();
        }

        private void Actualizar()
        {
            BibliotecaTripulantes tripulacion = _partida.Tripulacion;
            lblCreditos.Text = "Créditos: " + _partida.Estacion.Creditos;

            lstCarriles.BeginUpdate();
            lstCarriles.Items.Clear();
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                string estado = "";
                if (_partida.IntegridadDelCarril(carril) == 0)
                {
                    estado = "  (¡CAÍDO!)";
                }

                // "N0" escribe el número con separador de miles.
                lstCarriles.Items.Add(Estacion.Subsistemas[carril] + ": " + tripulacion.TripulantesEn(carril).ToString("N0") + " tripulantes" + estado);
            }
            lstCarriles.EndUpdate();

            lblComparacion.Text =
                "Tripulantes vivos: " + tripulacion.TotalVivos.ToString("N0") + " de " + tripulacion.TotalGenerados.ToString("N0") + " generados.\n" +
                "Especies en la fábrica: " + tripulacion.EspeciesEnFabrica + " (siempre 5).\n\n" +
                "Con los primeros 10.000 tripulantes:\n" +
                "CON Flyweight: " + tripulacion.ObjetosEspecieConFlyweight + " objetos Especie, " + (tripulacion.BytesConFlyweight / 1024) + " KB\n" +
                "SIN Flyweight: " + tripulacion.ObjetosEspecieSinFlyweight.ToString("N0") + " objetos Especie, " + (tripulacion.BytesSinFlyweight / 1024) + " KB";
        }
    }
}
