using System.Collections.Generic;
using Kepler7.Core.Patrones.Strategy;

namespace Kepler7.Core.Modelo
{
    // Las 5 oleadas de la campaña de Kepler-7.
    public static class Campania
    {
        public static List<Oleada> CrearOleadas()
        {
            // Letras cortas para que las listas de abajo se puedan leer de un vistazo.
            const TipoEnemigo P = TipoEnemigo.Pirata;
            const TipoEnemigo D = TipoEnemigo.Dron;
            const TipoEnemigo C = TipoEnemigo.Criatura;

            // El último número de cada oleada son los créditos que da al superarla.
            // Cada Agregar es: tick en que aparece, carril y tipo de enemigo.
            Oleada oleada1 = new Oleada(1, "Primer contacto",
                "Una nave pirata se acerca a la estación. Van directo al núcleo.",
                EstrategiasIA.Agresiva, 80);
            oleada1.Agregar(1, 2, P);
            oleada1.Agregar(3, 0, P);
            oleada1.Agregar(5, 4, P);
            oleada1.Agregar(7, 1, P);
            oleada1.Agregar(9, 3, P);
            oleada1.Agregar(11, 5, P);

            Oleada oleada2 = new Oleada(2, "Drones de interferencia",
                "Drones rebeldes disparan desde lejos y avanzan con cautela.",
                EstrategiasIA.Defensiva, 90);
            oleada2.Agregar(1, 1, D);
            oleada2.Agregar(2, 4, D);
            oleada2.Agregar(4, 0, D);
            oleada2.Agregar(5, 3, D);
            oleada2.Agregar(7, 5, D);
            oleada2.Agregar(8, 2, D);
            oleada2.Agregar(10, 1, P);
            oleada2.Agregar(12, 4, P);

            Oleada oleada3 = new Oleada(3, "Enjambre errático",
                "Un enjambre sin comando claro: nadie sabe qué va a hacer cada uno.",
                EstrategiasIA.Aleatoria, 100);
            oleada3.Agregar(1, 0, P);
            oleada3.Agregar(2, 3, D);
            oleada3.Agregar(4, 5, P);
            oleada3.Agregar(5, 1, D);
            oleada3.Agregar(7, 4, P);
            oleada3.Agregar(8, 2, D);
            oleada3.Agregar(10, 0, P);
            oleada3.Agregar(11, 5, D);
            oleada3.Agregar(13, 3, P);
            oleada3.Agregar(14, 1, D);

            Oleada oleada4 = new Oleada(4, "Inteligencia hostil",
                "Alguien los coordina: buscan el subsistema menos defendido.",
                EstrategiasIA.Calculadora, 110);
            oleada4.Agregar(1, 2, P);
            oleada4.Agregar(2, 4, D);
            oleada4.Agregar(4, 1, P);
            oleada4.Agregar(5, 3, D);
            oleada4.Agregar(7, 0, C);
            oleada4.Agregar(9, 5, P);
            oleada4.Agregar(10, 2, D);
            oleada4.Agregar(12, 4, C);

            Oleada oleada5 = new Oleada(5, "Asalto final",
                "La flota completa ataca Kepler-7. Si resistís, la estación se salva.",
                EstrategiasIA.Calculadora, 130);
            oleada5.Agregar(1, 0, P);
            oleada5.Agregar(1, 3, P);
            oleada5.Agregar(3, 1, D);
            oleada5.Agregar(3, 4, D);
            oleada5.Agregar(5, 2, C);
            oleada5.Agregar(6, 5, P);
            oleada5.Agregar(8, 0, D);
            oleada5.Agregar(9, 3, C);
            oleada5.Agregar(11, 1, P);
            oleada5.Agregar(12, 4, C);
            oleada5.Agregar(14, 2, D);
            oleada5.Agregar(15, 5, C);
            oleada5.Agregar(17, 0, P);
            oleada5.Agregar(18, 3, D);

            // Cada enemigo de la lista viene con un refuerzo (ver Reforzar): así las oleadas traen el doble.
            Reforzar(oleada1);
            Reforzar(oleada2);
            Reforzar(oleada3);
            Reforzar(oleada4);
            Reforzar(oleada5);

            List<Oleada> oleadas = new List<Oleada>();
            oleadas.Add(oleada1);
            oleadas.Add(oleada2);
            oleadas.Add(oleada3);
            oleadas.Add(oleada4);
            oleadas.Add(oleada5);
            return oleadas;
        }

        // Por cada enemigo de la oleada agrega otro igual, un tick después y por el carril de enfrente
        // (el que está 3 carriles más allá). Así la presión se reparte por todo el tablero y no alcanza
        // con defender un solo lado. Se recorre una copia porque la lista original va creciendo.
        private static void Reforzar(Oleada oleada)
        {
            List<SpawnEnemigo> originales = new List<SpawnEnemigo>(oleada.Spawns);
            for (int i = 0; i < originales.Count; i++)
            {
                SpawnEnemigo spawn = originales[i];
                int carrilDeEnfrente = (spawn.Carril + 3) % Grilla.Carriles;
                oleada.Agregar(spawn.Tick + 1, carrilDeEnfrente, spawn.Tipo);
            }
        }
    }
}
