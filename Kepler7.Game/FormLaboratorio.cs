using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Command;
using Kepler7.Core.Patrones.Prototype;

namespace Kepler7.Game
{
    /// <summary>
    /// Ventana del PROTOTYPE: se clona un tripulante base del laboratorio en vez de crearlo de cero,
    /// y se puede equipar al clon sin tocar al prototipo original.
    /// </summary>
    public partial class FormLaboratorio : Form
    {
        private readonly Partida _partida;
        private readonly GestorComandos _gestor;
        private readonly LaboratorioClonacion _laboratorio;
        private readonly Action<string> _registrar;

        public FormLaboratorio(Partida partida, GestorComandos gestor, LaboratorioClonacion laboratorio, Action<string> registrar)
        {
            InitializeComponent();
            _partida = partida;
            _gestor = gestor;
            _laboratorio = laboratorio;
            _registrar = registrar;

            foreach (var prototipo in _laboratorio.Prototipos) lstPrototipos.Items.Add(prototipo);
            lstPrototipos.SelectedIndex = 0;

            cmbEquipo.Items.AddRange(Equipo.Catalogo().ToArray());
            cmbEquipo.SelectedIndex = 0;

            ActualizarClones(null);
        }

        // Cómo se muestra cada prototipo en la lista.
        private void lstPrototipos_Format(object? sender, ListControlConvertEventArgs e)
        {
            var t = (Tripulante)e.ListItem!;
            var equipo = string.Join(", ", t.Equipamiento.Select(eq => eq.Nombre));
            e.Value = $"{t.NombreCompleto}  ·  Daño {t.Danio}  ·  Vida {t.VidaMaxima}  ·  {t.Costo} cr  ·  [{equipo}]";
        }

        // Cómo se muestra cada clon de la reserva en la lista.
        private void lstClones_Format(object? sender, ListControlConvertEventArgs e)
        {
            var t = (Tripulante)e.ListItem!;
            var equipo = string.Join(", ", t.Equipamiento.Select(eq => eq.Nombre));
            string origen = t.ClonadoDeId.HasValue ? $" (clon de #{t.ClonadoDeId})" : "";
            e.Value = $"{t.NombreCompleto}{origen}  ·  Daño {t.Danio}  ·  Vida {t.Vida}/{t.VidaMaxima}  ·  [{equipo}]";
        }

        private void btnClonar_Click(object? sender, EventArgs e)
        {
            var prototipo = (Tripulante)lstPrototipos.SelectedItem!;
            if (_partida.Estacion.Creditos < prototipo.Costo)
            {
                lblDetalle.Text = $"No alcanzan los créditos para clonar un {prototipo.Rol} ({prototipo.Costo} cr).";
                return;
            }

            Tripulante clon = _laboratorio.Clonar(prototipo.Rol);
            var comando = new ConstruirComando(_partida.Estacion, clon);
            if (!_gestor.Ejecutar(comando))
            {
                lblDetalle.Text = "No se pudo: " + comando.Descripcion;
                return;
            }

            string equipo = string.Join(", ", clon.Equipamiento.Select(eq => eq.Nombre));
            lblDetalle.Text = $"{clon.NombreCompleto} clonado de {prototipo.NombreCompleto}. Equipo copiado: {equipo}.";
            _registrar($"Laboratorio: {clon.NombreCompleto} clonado de {prototipo.NombreCompleto} (-{prototipo.Costo} cr)");
            _registrar($"   · equipo copiado: {equipo}");

            ActualizarClones(clon);
        }

        private void btnEquipar_Click(object? sender, EventArgs e)
        {
            if (lstClones.SelectedItem is not Tripulante clon)
            {
                lblDetalle.Text = "Seleccioná un tripulante de la reserva (las naves se configuran en el astillero).";
                return;
            }

            var modelo = (Equipo)cmbEquipo.SelectedItem!;
            if (!_partida.Estacion.Gastar(modelo.Costo))
            {
                lblDetalle.Text = $"No alcanzan los créditos para {modelo.Nombre} ({modelo.Costo} cr).";
                return;
            }

            clon.Equipar(modelo.Clonar());

            // Demostración: el prototipo original no cambió, porque el clon tiene su propia lista de equipo.
            var original = _laboratorio.ObtenerPrototipo(clon.Rol);
            lblDetalle.Text = $"{clon.NombreCompleto} + {modelo.Nombre} → Daño {clon.Danio}, Vida {clon.VidaMaxima}.\n" +
                              $"El prototipo {original.NombreCompleto} no cambió: Daño {original.Danio}, Vida {original.VidaMaxima}, " +
                              $"{original.Equipamiento.Count} pieza(s) de equipo.";
            _registrar($"Equipamiento: {clon.NombreCompleto} + {modelo.Nombre} → Daño {clon.Danio}, Vida {clon.VidaMaxima} (-{modelo.Costo} cr)");

            ActualizarClones(clon);
        }

        // Vuelve a armar la lista de clones (los tripulantes que están en la reserva).
        private void ActualizarClones(Tripulante? seleccionar)
        {
            lstClones.BeginUpdate();
            lstClones.Items.Clear();
            foreach (var tripulante in _partida.Estacion.Reserva.OfType<Tripulante>()) lstClones.Items.Add(tripulante);
            if (seleccionar != null) lstClones.SelectedItem = seleccionar;
            else if (lstClones.Items.Count > 0) lstClones.SelectedIndex = 0;
            lstClones.EndUpdate();

            lblCreditos.Text = $"Créditos: {_partida.Estacion.Creditos}";
        }
    }
}
