using System.Drawing;
using Kepler7.Core.Patrones.Strategy;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Modelo
{
    public enum TipoEnemigo
    {
        Pirata,
        Dron,
        Criatura
    }

    /// <summary>
    /// Unidad enemiga. Cómo decide qué hacer depende de su estrategia de IA (patrón Strategy),
    /// que se puede cambiar en cualquier momento de la partida.
    /// </summary>
    public class Enemigo : Unidad
    {
        private readonly int _danio;
        private readonly int _vidaMaxima;

        public TipoEnemigo Tipo { get; }
        public int Alcance { get; }          // 1 = cuerpo a cuerpo
        public int TicksPorCasilla { get; }  // 1 = avanza en cada tick, 2 = avanza cada dos ticks
        public int Recompensa { get; }       // créditos que da al ser derrotado

        // STRATEGY: el enemigo no sabe cómo decide, solo le pregunta a su estrategia.
        public IEstrategiaIA Estrategia { get; set; }

        public override bool EsEnemigo => true;
        public override int Danio => _danio;
        public override int VidaMaxima => _vidaMaxima;
        public override TurnoUnidad Turno { get; } = new TurnoEnemigo();

        private Enemigo(TipoEnemigo tipo, string nombre, string simbolo, Color color, int danio, int vida,
                        int alcance, int ticksPorCasilla, int recompensa, IEstrategiaIA estrategia)
        {
            Tipo = tipo;
            Nombre = nombre;
            Simbolo = simbolo;
            Color = color;
            _danio = danio;
            _vidaMaxima = vida;
            Vida = vida;
            Alcance = alcance;
            TicksPorCasilla = ticksPorCasilla;
            Recompensa = recompensa;
            Estrategia = estrategia;
        }

        public static Enemigo Crear(TipoEnemigo tipo, IEstrategiaIA estrategia)
        {
            switch (tipo)
            {
                case TipoEnemigo.Dron:
                    return new Enemigo(tipo, "Dron", "Dr", Color.FromArgb(210, 120, 50), 6, 35, 3, 1, 12, estrategia);
                case TipoEnemigo.Criatura:
                    return new Enemigo(tipo, "Criatura", "Cr", Color.FromArgb(150, 70, 150), 14, 160, 1, 2, 35, estrategia);
                default:
                    return new Enemigo(tipo, "Pirata", "Pi", Color.FromArgb(200, 60, 60), 8, 60, 1, 1, 15, estrategia);
            }
        }

        public bool PuedeAvanzarEnTick(int tick) => tick % TicksPorCasilla == 0;

        public override string Descripcion()
        {
            return $"{Nombre} | Daño {Danio} | Vida {Vida}/{VidaMaxima} | Alcance {Alcance} | IA: {Estrategia.Nombre}";
        }
    }
}
