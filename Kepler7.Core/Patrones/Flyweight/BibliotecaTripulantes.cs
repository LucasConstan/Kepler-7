using System;
using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Flyweight
{
    // La tripulación de Kepler-7: 10.000 tripulantes repartidos en 6 subsistemas que comparten
    // solo 5 objetos de especie. En el juego son el escudo de cada núcleo: los enemigos
    // tienen que matarlos antes de poder dañarlo.
    public class BibliotecaTripulantes
    {
        public const int CantidadInicial = 10000;
        private static readonly string[] Rangos = { "Cadete", "Cabo", "Sargento" };

        private readonly FabricaEspecies _fabrica = new FabricaEspecies();
        // Una lista de tripulantes por subsistema (carril).
        private readonly List<TripulanteEstacion>[] _porCarril = new List<TripulanteEstacion>[Grilla.Carriles];
        private int _proximoId = 1;

        // El set es internal para que una partida retomada recupere su cuenta (sale en el resumen final).
        public int TotalGenerados { get; internal set; }

        public int EspeciesEnFabrica
        {
            get { return _fabrica.CantidadEspecies; }
        }

        // Resultados de la comparación con y sin Flyweight (se miden al crear la tripulación).
        public int ObjetosEspecieConFlyweight { get; private set; }
        public long BytesConFlyweight { get; private set; }
        public int ObjetosEspecieSinFlyweight { get; private set; }
        public long BytesSinFlyweight { get; private set; }

        public BibliotecaTripulantes()
        {
            for (int carril = 0; carril < Grilla.Carriles; carril++)
            {
                _porCarril[carril] = new List<TripulanteEstacion>();
            }

            // CON Flyweight: se generan los 10.000 y se mide cuánta memoria y cuántas especies se crearon.
            int especiesAntes = Especie.InstanciasCreadas;
            long bytesAntes = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < CantidadInicial; i++)
            {
                Agregar(i % Grilla.Carriles, 1);
            }
            BytesConFlyweight = GC.GetAllocatedBytesForCurrentThread() - bytesAntes;
            ObjetosEspecieConFlyweight = Especie.InstanciasCreadas - especiesAntes;

            CompararSinFlyweight();
        }

        public int TripulantesEn(int carril)
        {
            return _porCarril[carril].Count;
        }

        // Cuántos tripulantes tiene cada subsistema al empezar (sirve para dibujar la barra).
        public const int InicialesPorCarril = CantidadInicial / Grilla.Carriles;

        public int TotalVivos
        {
            get
            {
                int total = 0;
                for (int i = 0; i < _porCarril.Length; i++)
                {
                    total = total + _porCarril[i].Count;
                }
                return total;
            }
        }

        // Suma tripulantes nuevos a un subsistema. Todos reutilizan las 5 especies de la fábrica.
        public void Agregar(int carril, int cantidad)
        {
            for (int i = 0; i < cantidad; i++)
            {
                int id = _proximoId;
                _proximoId = _proximoId + 1;

                IEspecie especie = _fabrica.ObtenerEspecie(FabricaEspecies.NombresEspecies[id % 5]);
                _porCarril[carril].Add(new TripulanteEstacion(id, "Tripulante " + id, Rangos[id % 3], carril, especie));
            }

            TotalGenerados = TotalGenerados + cantidad;
        }

        // Un golpe enemigo mata tripulantes de un subsistema. Devuelve cuántos murieron.
        public int Eliminar(int carril, int cantidad)
        {
            List<TripulanteEstacion> lista = _porCarril[carril];
            int muertos = Math.Min(cantidad, lista.Count);
            lista.RemoveRange(lista.Count - muertos, muertos);
            return muertos;
        }

        // SIN Flyweight: cada tripulante tiene su propia copia de la especie.
        // Se arma la lista solo para medirla y después se descarta.
        private void CompararSinFlyweight()
        {
            List<TripulanteEstacion> sinCompartir = new List<TripulanteEstacion>();
            int especiesAntes = Especie.InstanciasCreadas;
            long bytesAntes = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 1; i <= CantidadInicial; i++)
            {
                IEspecie modelo = _fabrica.ObtenerEspecie(FabricaEspecies.NombresEspecies[i % 5]);
                Especie copia = new Especie(modelo.Nombre, modelo.Color, modelo.PlanetaOrigen, modelo.Dieta);
                sinCompartir.Add(new TripulanteEstacion(i, "Tripulante " + i, Rangos[i % 3], i % Grilla.Carriles, copia));
            }
            BytesSinFlyweight = GC.GetAllocatedBytesForCurrentThread() - bytesAntes;
            ObjetosEspecieSinFlyweight = Especie.InstanciasCreadas - especiesAntes;
        }
    }
}
