using System.Collections.Generic;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Builder;
using Kepler7.Core.Patrones.Command;

namespace Kepler7.Game
{
    // Ventana del Builder: se elige una receta y el Astillero arma la nave paso a paso.
    // Pagarla y mandarla a la reserva es un comando, así se puede deshacer.
    public partial class FormAstillero : Form
    {
        private readonly Partida _partida;
        private readonly GestorComandos _gestor;
        private readonly Action<string> _registrar;

        // El director: siempre sigue el mismo orden de ensamblaje.
        private readonly Astillero _astillero = new Astillero();

        public FormAstillero(Partida partida, GestorComandos gestor, Action<string> registrar)
        {
            InitializeComponent();
            _partida = partida;
            _gestor = gestor;
            _registrar = registrar;

            cmbRecetas.Items.Add(new NaveExploradoraBuilder());
            cmbRecetas.Items.Add(new NaveCombateBuilder());
            cmbRecetas.Items.Add(new NaveCargaBuilder());
            cmbRecetas.SelectedIndex = 0;

            ActualizarCreditos();
        }

        private void cmbRecetas_SelectedIndexChanged(object? sender, EventArgs e)
        {
            INaveApoyoBuilder receta = (INaveApoyoBuilder)cmbRecetas.SelectedItem!;
            lblDescripcion.Text = receta.DescripcionReceta + "\nCosto: " + receta.Costo + " créditos";
        }

        private void btnEnsamblar_Click(object? sender, EventArgs e)
        {
            INaveApoyoBuilder receta = (INaveApoyoBuilder)cmbRecetas.SelectedItem!;
            if (_partida.Estacion.Creditos < receta.Costo)
            {
                lblResultado.Text = "No alcanzan los créditos para una nave " + receta.NombreReceta + " (" + receta.Costo + " cr).";
                return;
            }

            // El director arma la nave paso a paso; la receta elegida decide cada pieza.
            NaveApoyo nave = _astillero.Ensamblar(receta);
            ConstruirComando comando = new ConstruirComando(_partida.Estacion, nave);
            if (!_gestor.Ejecutar(comando))
            {
                lblResultado.Text = "No se pudo: " + comando.Descripcion;
                return;
            }

            lstComponentes.Items.Clear();
            IReadOnlyList<string> componentes = nave.Componentes;
            for (int i = 0; i < componentes.Count; i++)
            {
                lstComponentes.Items.Add(componentes[i]);
            }
            lblResultado.Text = nave.NombreCompleto + " ensamblada y enviada a la reserva.";

            _registrar("Astillero: ensamblada " + nave.NombreCompleto + " (-" + nave.Costo + " cr)");
            for (int i = 0; i < componentes.Count; i++)
            {
                _registrar("   · " + componentes[i]);
            }

            ActualizarCreditos();
        }

        private void ActualizarCreditos()
        {
            lblCreditos.Text = "Créditos: " + _partida.Estacion.Creditos;
        }
    }
}
