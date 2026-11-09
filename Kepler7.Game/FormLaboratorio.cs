using System.Collections.Generic;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Command;
using Kepler7.Core.Patrones.Prototype;

namespace Kepler7.Game
{
    // Ventana del Prototype: se clona un tripulante base del laboratorio en vez de crearlo de cero,
    // y se puede equipar al clon sin tocar al prototipo original.
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

            List<Tripulante> prototipos = new List<Tripulante>(_laboratorio.Prototipos);
            for (int i = 0; i < prototipos.Count; i++)
            {
                lstPrototipos.Items.Add(prototipos[i]);
            }
            lstPrototipos.SelectedIndex = 0;

            cmbEquipo.Items.AddRange(Equipo.Catalogo().ToArray());
            cmbEquipo.SelectedIndex = 0;

            ActualizarClones(null);
        }

        // Cómo se muestra cada prototipo en la lista.
        private void lstPrototipos_Format(object? sender, ListControlConvertEventArgs e)
        {
            Tripulante t = (Tripulante)e.ListItem!;
            e.Value = t.NombreCompleto + "  ·  Daño " + t.Danio + "  ·  Vida " + t.VidaMaxima + "  ·  " + t.Costo + " cr  ·  [" + NombresDelEquipo(t) + "]";
        }

        // Cómo se muestra cada clon de la reserva en la lista.
        private void lstClones_Format(object? sender, ListControlConvertEventArgs e)
        {
            Tripulante t = (Tripulante)e.ListItem!;

            // Los tripulantes originales no tienen de quién ser clones, así que a ellos no se les agrega nada.
            string origen = "";
            if (t.ClonadoDeId.HasValue)
            {
                origen = " (clon de #" + t.ClonadoDeId.Value + ")";
            }

            e.Value = t.NombreCompleto + origen + "  ·  Daño " + t.Danio + "  ·  Vida " + t.Vida + "/" + t.VidaMaxima + "  ·  [" + NombresDelEquipo(t) + "]";
        }

        private string NombresDelEquipo(Tripulante tripulante)
        {
            IReadOnlyList<Equipo> equipamiento = tripulante.Equipamiento;

            string nombres = "";
            for (int i = 0; i < equipamiento.Count; i++)
            {
                if (i > 0)
                {
                    nombres = nombres + ", ";
                }
                nombres = nombres + equipamiento[i].Nombre;
            }

            return nombres;
        }

        private void btnClonar_Click(object? sender, EventArgs e)
        {
            Tripulante prototipo = (Tripulante)lstPrototipos.SelectedItem!;
            if (_partida.Estacion.Creditos < prototipo.Costo)
            {
                lblDetalle.Text = "No alcanzan los créditos para clonar un " + prototipo.Rol + " (" + prototipo.Costo + " cr).";
                return;
            }

            Tripulante clon = _laboratorio.Clonar(prototipo.Rol);
            ConstruirComando comando = new ConstruirComando(_partida.Estacion, clon);
            if (!_gestor.Ejecutar(comando))
            {
                lblDetalle.Text = "No se pudo: " + comando.Descripcion;
                return;
            }

            string equipo = NombresDelEquipo(clon);
            lblDetalle.Text = clon.NombreCompleto + " clonado de " + prototipo.NombreCompleto + ". Equipo copiado: " + equipo + ".";
            _registrar("Laboratorio: " + clon.NombreCompleto + " clonado de " + prototipo.NombreCompleto + " (-" + prototipo.Costo + " cr)");
            _registrar("   · equipo copiado: " + equipo);

            ActualizarClones(clon);
        }

        private void btnEquipar_Click(object? sender, EventArgs e)
        {
            Tripulante? clon = lstClones.SelectedItem as Tripulante;
            if (clon == null)
            {
                lblDetalle.Text = "Seleccioná un tripulante de la reserva (las naves se configuran en el astillero).";
                return;
            }

            Equipo modelo = (Equipo)cmbEquipo.SelectedItem!;
            if (!_partida.Estacion.Gastar(modelo.Costo))
            {
                lblDetalle.Text = "No alcanzan los créditos para " + modelo.Nombre + " (" + modelo.Costo + " cr).";
                return;
            }

            clon.Equipar(modelo.Clonar());

            // Demostración: el prototipo original no cambió, porque el clon tiene su propia lista de equipo.
            Tripulante original = _laboratorio.ObtenerPrototipo(clon.Rol);
            lblDetalle.Text = clon.NombreCompleto + " + " + modelo.Nombre + " → Daño " + clon.Danio + ", Vida " + clon.VidaMaxima + ".\n" +
                              "El prototipo " + original.NombreCompleto + " no cambió: Daño " + original.Danio + ", Vida " + original.VidaMaxima + ", " +
                              original.Equipamiento.Count + " pieza(s) de equipo.";
            _registrar("Equipamiento: " + clon.NombreCompleto + " + " + modelo.Nombre + " → Daño " + clon.Danio + ", Vida " + clon.VidaMaxima + " (-" + modelo.Costo + " cr)");

            ActualizarClones(clon);
        }

        // Vuelve a armar la lista de clones (los tripulantes que están en la reserva).
        // Las naves y el héroe también están en la reserva, pero acá no se muestran.
        private void ActualizarClones(Tripulante? seleccionar)
        {
            lstClones.BeginUpdate();
            lstClones.Items.Clear();

            IReadOnlyList<Unidad> reserva = _partida.Estacion.Reserva;
            for (int i = 0; i < reserva.Count; i++)
            {
                if (reserva[i] is Tripulante)
                {
                    lstClones.Items.Add(reserva[i]);
                }
            }

            if (seleccionar != null)
            {
                lstClones.SelectedItem = seleccionar;
            }
            else if (lstClones.Items.Count > 0)
            {
                lstClones.SelectedIndex = 0;
            }

            lstClones.EndUpdate();

            lblCreditos.Text = "Créditos: " + _partida.Estacion.Creditos;
        }
    }
}
