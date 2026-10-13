using System;
using System.Collections.Generic;

namespace Kepler7.Core.Modelo
{
    /// <summary>
    /// El tablero: 6 carriles (uno por subsistema) x 9 columnas. Una celda guarda una sola unidad.
    /// La columna 0 es la más cercana al núcleo; los enemigos aparecen en la última y avanzan hacia la 0.
    /// </summary>
    public class Grilla
    {
        public const int Carriles = 6;
        public const int Columnas = 9;
        // Las primeras columnas son zona de defensa; las últimas, zona de aproximación enemiga.
        public const int ColumnasDefensa = 6;

        private readonly Unidad?[,] _celdas = new Unidad?[Carriles, Columnas];

        public bool EsZonaDefensa(int columna) => columna < ColumnasDefensa;

        public bool EsValida(int carril, int columna) =>
            carril >= 0 && carril < Carriles && columna >= 0 && columna < Columnas;

        public Unidad? ObtenerUnidad(int carril, int columna) =>
            EsValida(carril, columna) ? _celdas[carril, columna] : null;

        public bool EstaLibre(int carril, int columna) =>
            EsValida(carril, columna) && _celdas[carril, columna] == null;

        // --- Operaciones básicas (el Command de la persona 2 puede usarlas y deshacerlas) ---

        public bool Colocar(Unidad unidad, int carril, int columna)
        {
            if (!EstaLibre(carril, columna)) return false;
            _celdas[carril, columna] = unidad;
            unidad.Carril = carril;
            unidad.Columna = columna;
            return true;
        }

        public void Quitar(Unidad unidad)
        {
            if (!EsValida(unidad.Carril, unidad.Columna)) return;
            if (_celdas[unidad.Carril, unidad.Columna] == unidad)
                _celdas[unidad.Carril, unidad.Columna] = null;
            unidad.Carril = -1;
            unidad.Columna = -1;
        }

        public bool Mover(Unidad unidad, int carril, int columna)
        {
            if (!EstaLibre(carril, columna)) return false;
            Quitar(unidad);
            return Colocar(unidad, carril, columna);
        }

        // --- Consultas ---

        public List<Unidad> Unidades()
        {
            var lista = new List<Unidad>();
            foreach (var unidad in _celdas)
                if (unidad != null) lista.Add(unidad);
            return lista;
        }

        public List<Unidad> Defensores()
        {
            var lista = new List<Unidad>();
            foreach (var unidad in Unidades())
                if (!unidad.EsEnemigo) lista.Add(unidad);
            return lista;
        }

        public List<Enemigo> Enemigos()
        {
            var lista = new List<Enemigo>();
            foreach (var unidad in Unidades())
                if (unidad is Enemigo enemigo) lista.Add(enemigo);
            return lista;
        }

        // Defensores a la izquierda de la columna dada (hacia el núcleo), del más cercano al más lejano.
        public List<Unidad> DefensoresEnRango(int carril, int columna, int alcance)
        {
            var lista = new List<Unidad>();
            for (int c = columna - 1; c >= Math.Max(0, columna - alcance); c--)
            {
                var unidad = ObtenerUnidad(carril, c);
                if (unidad != null && !unidad.EsEnemigo) lista.Add(unidad);
            }
            return lista;
        }

        // Enemigos a la derecha de la columna dada, del más cercano al más lejano.
        public List<Enemigo> EnemigosEnRango(int carril, int columna, int alcance)
        {
            var lista = new List<Enemigo>();
            for (int c = columna + 1; c <= Math.Min(Columnas - 1, columna + alcance); c++)
            {
                if (ObtenerUnidad(carril, c) is Enemigo enemigo) lista.Add(enemigo);
            }
            return lista;
        }

        // Un enemigo puede avanzar si la celda de adelante está libre (o si ya está pegado al núcleo).
        public bool PuedeAvanzar(Enemigo enemigo)
        {
            return enemigo.Columna == 0 || EstaLibre(enemigo.Carril, enemigo.Columna - 1);
        }

        // Qué tan defendido está un carril: suma de daño y un poco de vida de sus defensores.
        public int PoderDefensivo(int carril)
        {
            int poder = 0;
            for (int c = 0; c < Columnas; c++)
            {
                var unidad = ObtenerUnidad(carril, c);
                if (unidad != null && !unidad.EsEnemigo)
                    poder += unidad.DanioTotal + unidad.Vida / 10;
            }
            return poder;
        }
    }
}
