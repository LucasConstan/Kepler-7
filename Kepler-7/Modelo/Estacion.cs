using System.Collections.Generic;
using System.Linq;
using Kepler_7.Patrones.Builder;

namespace Kepler_7.Modelo
{
    /// <summary>
    /// Estado del juego: 6 carriles (uno por subsistema), las unidades desplegadas,
    /// la reserva de unidades listas para colocar y los créditos disponibles.
    /// </summary>
    public class Estacion
    {
        public const int Carriles = 6;
        public const int Columnas = 9;
        // Las primeras columnas son zona de defensa; las últimas, zona de aproximación enemiga.
        public const int ColumnasDefensa = 6;

        public static readonly string[] Subsistemas =
        {
            "Energía", "Soporte Vital", "Navegación", "Comunicaciones", "Seguridad", "Defensa"
        };

        private readonly Unidad[,] _celdas = new Unidad[Carriles, Columnas];
        private readonly List<Unidad> _reserva = new List<Unidad>();

        public int Creditos { get; private set; }

        public IReadOnlyList<Unidad> Reserva => _reserva;

        public Estacion(int creditosIniciales)
        {
            Creditos = creditosIniciales;
        }

        public Unidad ObtenerUnidad(int carril, int columna) => _celdas[carril, columna];

        public bool EsZonaDefensa(int columna) => columna < ColumnasDefensa;

        public bool Gastar(int monto)
        {
            if (monto > Creditos) return false;
            Creditos -= monto;
            return true;
        }

        public void AgregarAReserva(Unidad unidad)
        {
            _reserva.Add(unidad);
        }

        public bool Desplegar(Unidad unidad, int carril, int columna)
        {
            if (!_reserva.Contains(unidad) || !EsZonaDefensa(columna) || _celdas[carril, columna] != null)
                return false;

            _celdas[carril, columna] = unidad;
            _reserva.Remove(unidad);
            return true;
        }

        public IEnumerable<Unidad> UnidadesDesplegadas()
        {
            foreach (var unidad in _celdas)
                if (unidad != null) yield return unidad;
        }

        /// <summary>Créditos extra que darán las naves de carga desplegadas en cada oleada.</summary>
        public int BonusRecursosPorOleada =>
            UnidadesDesplegadas().OfType<NaveApoyo>().Sum(n => n.BonusRecursos);
    }
}
