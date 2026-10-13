using System.Collections.Generic;

namespace Kepler7.Core.Modelo
{
    /// <summary>
    /// La economía de la estación: créditos y reserva de unidades listas para desplegar.
    /// El tablero en sí vive en la Grilla.
    /// </summary>
    public class Estacion
    {
        // Cada subsistema es un carril del tablero.
        public static readonly string[] Subsistemas =
        {
            "Energía", "Soporte Vital", "Navegación", "Comunicaciones", "Seguridad", "Defensa"
        };

        private readonly List<Unidad> _reserva = new List<Unidad>();

        public int Creditos { get; private set; }

        public IReadOnlyList<Unidad> Reserva => _reserva;

        public Estacion(int creditosIniciales)
        {
            Creditos = creditosIniciales;
        }

        public bool Gastar(int monto)
        {
            if (monto > Creditos) return false;
            Creditos -= monto;
            return true;
        }

        public void Ganar(int monto)
        {
            Creditos += monto;
        }

        public void AgregarAReserva(Unidad unidad)
        {
            _reserva.Add(unidad);
        }

        public bool QuitarDeReserva(Unidad unidad)
        {
            return _reserva.Remove(unidad);
        }
    }
}
