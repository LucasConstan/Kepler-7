using System.Collections.Generic;
using System.Drawing;
using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Patrones.Builder
{
    // El producto que arman los builders. Sus setters son internos para que nadie
    // desde la interfaz pueda cambiarle los valores a una nave ya terminada.
    public class NaveApoyo : Unidad
    {
        private readonly List<string> _componentes = new List<string>();

        public string Tipo { get; internal set; } = "";
        public int Blindaje { get; internal set; }
        public int PotenciaDeFuego { get; internal set; }
        public int DisparosPorTick { get; internal set; }
        public int BonusRecursos { get; internal set; }

        public IReadOnlyList<string> Componentes
        {
            get { return _componentes; }
        }

        public override int Danio
        {
            get { return PotenciaDeFuego * DisparosPorTick; }
        }

        public override int VidaMaxima
        {
            get { return Blindaje; }
        }

        public override TurnoUnidad Turno { get; } = new TurnoNaveApoyo();

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
            string texto = "Nave " + Tipo + " | Daño " + PotenciaDeFuego + " x" + DisparosPorTick
                + " por tick | Blindaje " + Vida + "/" + VidaMaxima;

            if (BonusRecursos > 0)
            {
                texto = texto + " | +" + BonusRecursos + " créditos por oleada";
            }

            return texto;
        }
    }
}
