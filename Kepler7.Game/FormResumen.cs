using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Game
{
    // Pantalla final: cómo terminó la partida, los números del resumen y lo que se ganó en el Hub.
    // Los números arrancan en 0 y van subiendo hasta su valor, para que el final se sienta como un cierre.
    // "Jugar de nuevo" devuelve DialogResult.Retry, y la ventana principal se encarga de reiniciar.
    public partial class FormResumen : Form
    {
        private const int PasosConteo = 25; // con el Timer cada 40 ms, el conteo dura un segundo

        private readonly ResumenPartida _resumen;
        private int _paso = 0;

        public FormResumen(ResumenPartida resumen, List<string> recompensa)
        {
            InitializeComponent();
            _resumen = resumen;
            new TransicionAparecer(this);

            if (resumen.Victoria)
            {
                lblTitulo.Text = "¡Victoria!";
                lblTitulo.ForeColor = Color.SeaGreen;
                lblMotivo.Text = "Resististe las " + resumen.TotalOleadas + " oleadas: Kepler-7 está a salvo.";
            }
            else if (resumen.FueApagada)
            {
                lblTitulo.Text = "Partida terminada";
                lblTitulo.ForeColor = Color.DarkOrange;
                lblMotivo.Text = "La estación fue apagada desde el Control central.";
            }
            else
            {
                lblTitulo.Text = "Derrota";
                lblTitulo.ForeColor = Color.Firebrick;
                lblMotivo.Text = "Cayeron " + resumen.SubsistemasCaidos.Count + " subsistemas y Kepler-7 se perdió.";
            }

            string textoRecompensa = "";
            for (int i = 0; i < recompensa.Count; i++)
            {
                if (i > 0)
                {
                    textoRecompensa = textoRecompensa + "\n";
                }
                textoRecompensa = textoRecompensa + recompensa[i];
            }
            lblRecompensa.Text = textoRecompensa;

            MostrarDatos();
        }

        private void FormResumen_Shown(object? sender, EventArgs e)
        {
            timerConteo.Start();
        }

        private void timerConteo_Tick(object? sender, EventArgs e)
        {
            _paso = _paso + 1;
            if (_paso >= PasosConteo)
            {
                _paso = PasosConteo;
                timerConteo.Stop();
            }

            MostrarDatos();
        }

        // Muestra cada número según cuánto avanzó el conteo: en el último paso ya es el valor real.
        private void MostrarDatos()
        {
            string caidos = Parte(_resumen.SubsistemasCaidos.Count).ToString();
            if (_paso == PasosConteo && _resumen.SubsistemasCaidos.Count > 0)
            {
                caidos = caidos + " (" + UnirNombres(_resumen.SubsistemasCaidos) + ")";
            }

            lblDatos.Text =
                "Oleadas superadas: " + Parte(_resumen.OleadasSuperadas) + " de " + _resumen.TotalOleadas + "\n" +
                "Enemigos derrotados: " + Parte(_resumen.EnemigosDerrotados) + "\n" +
                "Defensores perdidos: " + Parte(_resumen.DefensoresPerdidos) + "\n" +
                "Tripulantes perdidos: " + Parte(_resumen.TripulantesPerdidos).ToString("N0") + "\n" +
                "Subsistemas caídos: " + caidos + "\n" +
                "Créditos al final: " + Parte(_resumen.CreditosFinales) + "\n" +
                "Ticks jugados: " + Parte(_resumen.TicksJugados);
        }

        private int Parte(int valor)
        {
            return valor * _paso / PasosConteo;
        }

        private string UnirNombres(List<string> nombres)
        {
            string texto = "";
            for (int i = 0; i < nombres.Count; i++)
            {
                if (i > 0)
                {
                    texto = texto + ", ";
                }
                texto = texto + nombres[i];
            }
            return texto;
        }
    }
}
