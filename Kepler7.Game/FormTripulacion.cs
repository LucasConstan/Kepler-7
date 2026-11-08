using Kepler7.Core.Modelo;

namespace Kepler7.Game
{
    /// <summary>
    /// Ventana del FLYWEIGHT: muestra los 10.000 tripulantes repartidos por subsistema
    /// (que comparten solo 5 objetos de especie) y permite mandar refuerzos a un núcleo.
    /// </summary>
    public partial class FormTripulacion : Form
    {
        private readonly Partida _partida;
        private readonly Action<string> _registrar;

        public FormTripulacion(Partida partida, Action<string> registrar)
        {
            InitializeComponent();
            _partida = partida;
            _registrar = registrar;

            cmbCarril.Items.AddRange(Estacion.Subsistemas);
            cmbCarril.SelectedIndex = 0;
            btnEnviar.Text = $"Enviar {Partida.TripulantesPorEnvio} tripulantes ({Partida.CostoEnvioTripulantes} cr)";

            Actualizar();
        }

        private void btnEnviar_Click(object? sender, EventArgs e)
        {
            int carril = cmbCarril.SelectedIndex;
            if (_partida.EnviarTripulantes(carril))
            {
                lblResultado.Text = $"Llegaron {Partida.TripulantesPorEnvio} tripulantes a {Estacion.Subsistemas[carril]}.";
            }
            else
            {
                lblResultado.Text = $"No se pudo enviar a {Estacion.Subsistemas[carril]} (faltan créditos o el subsistema cayó).";
                _registrar("Tripulación: " + lblResultado.Text);
            }
            Actualizar();
        }

        private void Actualizar()
        {
            var tripulacion = _partida.Tripulacion;
            lblCreditos.Text = $"Créditos: {_partida.Estacion.Creditos}";

            lstCarriles.BeginUpdate();
            lstCarriles.Items.Clear();
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                string estado = _partida.IntegridadDelCarril(carril) == 0 ? "  (¡CAÍDO!)" : "";
                lstCarriles.Items.Add($"{Estacion.Subsistemas[carril]}: {tripulacion.TripulantesEn(carril):N0} tripulantes{estado}");
            }
            lstCarriles.EndUpdate();

            lblComparacion.Text =
                $"Tripulantes vivos: {tripulacion.TotalVivos:N0} de {tripulacion.TotalGenerados:N0} generados.\n" +
                $"Especies en la fábrica: {tripulacion.EspeciesEnFabrica} (siempre 5).\n\n" +
                $"Con los primeros 10.000 tripulantes:\n" +
                $"CON Flyweight: {tripulacion.ObjetosEspecieConFlyweight} objetos Especie, {tripulacion.BytesConFlyweight / 1024} KB\n" +
                $"SIN Flyweight: {tripulacion.ObjetosEspecieSinFlyweight:N0} objetos Especie, {tripulacion.BytesSinFlyweight / 1024} KB";
        }
    }
}
