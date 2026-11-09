using System.Collections.Generic;
using Kepler7.Core.Patrones.Strategy;

namespace Kepler7.Core.Modelo
{
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
}
