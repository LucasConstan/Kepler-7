using System.Collections.Generic;
using Kepler7.Core.Patrones.Strategy;

namespace Kepler7.Core.Modelo
{
    /// <summary>Un enemigo que aparece en un tick determinado de la oleada.</summary>
    public class SpawnEnemigo
    {
        public int Tick { get; }
        public int Carril { get; }
        public TipoEnemigo Tipo { get; }

        public SpawnEnemigo(int tick, int carril, TipoEnemigo tipo)
        {
            Tick = tick;
            Carril = carril;
            Tipo = tipo;
        }
    }

    public class Oleada
    {
        public int Numero { get; }
        public string Nombre { get; }
        public string Historia { get; }
        public int Recompensa { get; }
        public List<SpawnEnemigo> Spawns { get; } = new List<SpawnEnemigo>();

        // IA que usan los enemigos de esta oleada (se puede cambiar durante la partida).
        public IEstrategiaIA Estrategia { get; set; }

        public Oleada(int numero, string nombre, string historia, IEstrategiaIA estrategia, int recompensa)
        {
            Numero = numero;
            Nombre = nombre;
            Historia = historia;
            Estrategia = estrategia;
            Recompensa = recompensa;
        }

        public Oleada Agregar(int tick, int carril, TipoEnemigo tipo)
        {
            Spawns.Add(new SpawnEnemigo(tick, carril, tipo));
            return this;
        }
    }

    /// <summary>Las 5 oleadas de la campaña de Kepler-7.</summary>
    public static class Campania
    {
        public static List<Oleada> CrearOleadas()
        {
            const TipoEnemigo P = TipoEnemigo.Pirata;
            const TipoEnemigo D = TipoEnemigo.Dron;
            const TipoEnemigo C = TipoEnemigo.Criatura;

            var oleada1 = new Oleada(1, "Primer contacto",
                "Una nave pirata se acerca a la estación. Van directo al núcleo.",
                EstrategiasIA.Agresiva, 100)
                .Agregar(1, 2, P).Agregar(3, 0, P).Agregar(5, 4, P)
                .Agregar(7, 1, P).Agregar(9, 3, P).Agregar(11, 5, P);

            var oleada2 = new Oleada(2, "Drones de interferencia",
                "Drones rebeldes disparan desde lejos y avanzan con cautela.",
                EstrategiasIA.Defensiva, 120)
                .Agregar(1, 1, D).Agregar(2, 4, D).Agregar(4, 0, D).Agregar(5, 3, D)
                .Agregar(7, 5, D).Agregar(8, 2, D).Agregar(10, 1, P).Agregar(12, 4, P);

            var oleada3 = new Oleada(3, "Enjambre errático",
                "Un enjambre sin comando claro: nadie sabe qué va a hacer cada uno.",
                EstrategiasIA.Aleatoria, 140)
                .Agregar(1, 0, P).Agregar(2, 3, D).Agregar(4, 5, P).Agregar(5, 1, D).Agregar(7, 4, P)
                .Agregar(8, 2, D).Agregar(10, 0, P).Agregar(11, 5, D).Agregar(13, 3, P).Agregar(14, 1, D);

            var oleada4 = new Oleada(4, "Inteligencia hostil",
                "Alguien los coordina: buscan el subsistema menos defendido.",
                EstrategiasIA.Calculadora, 160)
                .Agregar(1, 2, P).Agregar(2, 4, D).Agregar(4, 1, P).Agregar(5, 3, D)
                .Agregar(7, 0, C).Agregar(9, 5, P).Agregar(10, 2, D).Agregar(12, 4, C);

            var oleada5 = new Oleada(5, "Asalto final",
                "La flota completa ataca Kepler-7. Si resistís, la estación se salva.",
                EstrategiasIA.Calculadora, 200)
                .Agregar(1, 0, P).Agregar(1, 3, P).Agregar(3, 1, D).Agregar(3, 4, D).Agregar(5, 2, C)
                .Agregar(6, 5, P).Agregar(8, 0, D).Agregar(9, 3, C).Agregar(11, 1, P).Agregar(12, 4, C)
                .Agregar(14, 2, D).Agregar(15, 5, C).Agregar(17, 0, P).Agregar(18, 3, D);

            return new List<Oleada> { oleada1, oleada2, oleada3, oleada4, oleada5 };
        }
    }
}
