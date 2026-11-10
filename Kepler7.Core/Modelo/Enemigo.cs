using System.Drawing;
using Kepler7.Core.Patrones.Strategy;
using Kepler7.Core.Patrones.TemplateMethod;

namespace Kepler7.Core.Modelo
{
    // Unidad enemiga. Cómo decide qué hacer depende de su estrategia de IA,
    // que se puede cambiar en cualquier momento de la partida.
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

        public override bool EsEnemigo
        {
            get { return true; }
        }

        public override int Danio
        {
            get { return _danio; }
        }

        public override int VidaMaxima
        {
            get { return _vidaMaxima; }
        }

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

        // Los números son: daño, vida, alcance, ticks por casilla y créditos que da al morir.
        // Para darle un poco más de dificultad se subió la vida un 30% y se bajó la recompensa un 30%
        // (antes: Dron 35 de vida y 12 cr, Criatura 160 y 35 cr, Pirata 60 y 15 cr).
        public static Enemigo Crear(TipoEnemigo tipo, IEstrategiaIA estrategia)
        {
            switch (tipo)
            {
                case TipoEnemigo.Dron:
                    return new Enemigo(tipo, "Dron", "Dr", Color.FromArgb(210, 120, 50), 6, 45, 3, 1, 8, estrategia);
                case TipoEnemigo.Criatura:
                    return new Enemigo(tipo, "Criatura", "Cr", Color.FromArgb(150, 70, 150), 14, 210, 1, 2, 25, estrategia);
                default:
                    return new Enemigo(tipo, "Pirata", "Pi", Color.FromArgb(200, 60, 60), 8, 78, 1, 1, 10, estrategia);
            }
        }

        public bool PuedeAvanzarEnTick(int tick)
        {
            return tick % TicksPorCasilla == 0;
        }

        public override string Descripcion()
        {
            return Nombre + " | Daño " + Danio + " | Vida " + Vida + "/" + VidaMaxima
                + " | Alcance " + Alcance + " | IA: " + Estrategia.Nombre;
        }
    }
}
