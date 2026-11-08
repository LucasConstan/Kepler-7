using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.ChainOfResponsibility;
using Kepler7.Core.Patrones.State;
using Kepler7.Core.Patrones.Visitor;

namespace Kepler7.Game
{
    /// <summary>
    /// Ventana del VISITOR (la pantalla de Inventario del Hub): la tabla, el total y los botones
    /// usan un visitante distinto cada uno, sin preguntar de qué tipo es cada ítem.
    /// </summary>
    public partial class FormInventario : Form
    {
        private readonly Partida _partida;
        private readonly Action<string> _registrar;

        public FormInventario(Partida partida, Action<string> registrar)
        {
            InitializeComponent();
            _partida = partida;
            _registrar = registrar;
            Actualizar();
        }

        private void btnEquipar_Click(object? sender, EventArgs e)
        {
            ItemInventario? item = ObtenerItemElegido();
            if (item == null)
            {
                return;
            }

            VisitanteEquipar equipar = new VisitanteEquipar(_partida.ObtenerHeroe(), _partida.ObtenerInventario());
            item.Aceptar(equipar);

            _registrar("Inventario: el Comandante equipó " + item.ObtenerNombre());
            Actualizar();
        }

        private void btnVender_Click(object? sender, EventArgs e)
        {
            ItemInventario? item = ObtenerItemElegido();
            if (item == null)
            {
                return;
            }

            VisitanteValor valor = new VisitanteValor();
            item.Aceptar(valor);

            _partida.ObtenerInventario().Quitar(item);
            _partida.Estacion.Ganar(valor.ObtenerTotal());

            _registrar("Inventario: vendido " + item.ObtenerNombre() + " (+" + valor.ObtenerTotal() + " cr)");
            Actualizar();
        }

        private ItemInventario? ObtenerItemElegido()
        {
            if (dgvItems.CurrentRow == null)
            {
                return null;
            }
            return _partida.ObtenerInventario().ObtenerItems()[dgvItems.CurrentRow.Index];
        }

        private void Actualizar()
        {
            Inventario inventario = _partida.ObtenerInventario();

            dgvItems.Rows.Clear();
            foreach (ItemInventario item in inventario.ObtenerItems())
            {
                VisitanteDescripcion descripcion = new VisitanteDescripcion();
                item.Aceptar(descripcion);

                VisitanteValor valorItem = new VisitanteValor();
                item.Aceptar(valorItem);

                dgvItems.Rows.Add(item.ObtenerNombre(), descripcion.ObtenerTipo(), descripcion.ObtenerEfecto(), valorItem.ObtenerTotal());
            }

            VisitanteValor valorTotal = new VisitanteValor();
            inventario.AceptarEnTodos(valorTotal);
            lblValorTotal.Text = "Valor total del inventario: " + valorTotal.ObtenerTotal() + " cr";
            lblCreditos.Text = "Créditos: " + _partida.Estacion.Creditos;

            bool hayItems = inventario.ObtenerItems().Count > 0;
            btnEquipar.Enabled = hayItems;
            btnVender.Enabled = hayItems;

            ActualizarHeroe();
        }

        private void ActualizarHeroe()
        {
            Heroe heroe = _partida.ObtenerHeroe();

            string defensas = "";
            foreach (Defensa defensa in heroe.ObtenerCadena().ObtenerDefensas())
            {
                if (defensas != "")
                {
                    defensas = defensas + ", ";
                }
                defensas = defensas + defensa.ObtenerNombre();
            }

            lblHeroe.Text = "Daño: " + heroe.Danio + "     Vida: " + heroe.Vida + " / " + heroe.VidaMaxima + "\n" +
                            "Arma: " + heroe.ObtenerNombreArma() + "\n" +
                            "Armadura: " + heroe.ObtenerNombreArmadura() + "\n" +
                            "Defensas: " + defensas;
        }
    }
}
