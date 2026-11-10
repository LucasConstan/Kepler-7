using System.Collections.Generic;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.ChainOfResponsibility;
using Kepler7.Core.Patrones.State;
using Kepler7.Core.Patrones.Visitor;

namespace Kepler7.Game
{
    // Ventana del Visitor: la tabla, el total y los botones usan cada uno un visitante distinto,
    // sin preguntar de qué tipo es cada ítem.
    public partial class FormInventario : Form
    {
        private readonly Partida _partida;
        private readonly Action<string> _registrar;
        private readonly HubProgreso _hub;

        public FormInventario(Partida partida, Action<string> registrar, HubProgreso hub)
        {
            InitializeComponent();
            new TransicionAparecer(this);
            _partida = partida;
            _registrar = registrar;
            _hub = hub;
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
            _hub.DesbloquearLogro(CatalogoLogros.Visitor);
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

        // Las filas de la tabla están en el mismo orden que la lista de ítems, por eso alcanza con el índice.
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
            List<ItemInventario> items = inventario.ObtenerItems();

            dgvItems.Rows.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                ItemInventario item = items[i];

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

            bool hayItems = items.Count > 0;
            btnEquipar.Enabled = hayItems;
            btnVender.Enabled = hayItems;

            ActualizarHeroe();
        }

        private void ActualizarHeroe()
        {
            Heroe heroe = _partida.ObtenerHeroe();
            List<Defensa> cadena = heroe.ObtenerCadena().ObtenerDefensas();

            string defensas = "";
            for (int i = 0; i < cadena.Count; i++)
            {
                if (defensas != "")
                {
                    defensas = defensas + ", ";
                }
                defensas = defensas + cadena[i].ObtenerNombre();
            }

            lblHeroe.Text = "Daño: " + heroe.Danio + "     Vida: " + heroe.Vida + " / " + heroe.VidaMaxima + "\n" +
                            "Arma: " + heroe.ObtenerNombreArma() + "\n" +
                            "Armadura: " + heroe.ObtenerNombreArmadura() + "\n" +
                            "Defensas: " + defensas;
        }
    }
}
