using System.Collections.Generic;
using System.Drawing;
using Kepler_7.Modelo;

namespace Kepler_7.Patrones.Builder
{
    /// <summary>
    /// BUILDER — Producto. Una nave de apoyo es un defensor especial que se arma por partes.
    /// Sus setters son internos: solo un builder puede configurarla.
    /// </summary>
    public class NaveApoyo : Unidad
    {
        private readonly List<string> _componentes = new List<string>();

        public string Tipo { get; internal set; } = "";
        public int Blindaje { get; internal set; }
        public int PotenciaDeFuego { get; internal set; }
        public int DisparosPorTick { get; internal set; }
        public int BonusRecursos { get; internal set; }

        public IReadOnlyList<string> Componentes => _componentes;

        public override int Danio => PotenciaDeFuego * DisparosPorTick;
        public override int VidaMaxima => Blindaje;

        internal void Identificar(string nombre, string simbolo, Color color, int costo)
        {
            Nombre = nombre;
            Simbolo = simbolo;
            Color = color;
            Costo = costo;
        }

        internal void AgregarComponente(string componente)
        {
            _componentes.Add(componente);
        }

        public override string Descripcion()
        {
            var texto = $"Nave {Tipo} | Daño {PotenciaDeFuego} x{DisparosPorTick} por tick | Blindaje {Vida}/{VidaMaxima}";
            if (BonusRecursos > 0) texto += $" | +{BonusRecursos} créditos por oleada";
            return texto;
        }
    }
}
