using System.Collections.Generic;

namespace Kepler7.Core.Modelo
{
    // La economía de la estación: créditos y reserva de unidades listas para desplegar.
    // El tablero en sí vive en la Grilla.
    public class Estacion
    {
        // Cada subsistema es un carril del tablero.
        public static readonly string[] Subsistemas =
        {
            "Energía", "Soporte Vital", "Navegación", "Comunicaciones", "Seguridad", "Defensa"
        };

        private readonly List<Unidad> _reserva = new List<Unidad>();

        public int Creditos { get; private set; }

        public IReadOnlyList<Unidad> Reserva
        {
            get { return _reserva; }
        }

        public Estacion(int creditosIniciales)
        {
            Creditos = creditosIniciales;
        }

        // Devuelve false si no alcanza, y en ese caso no descuenta nada.
        public bool Gastar(int monto)
        {
            if (monto > Creditos)
            {
                return false;
            }

            Creditos = Creditos - monto;
            return true;
        }

        public void Ganar(int monto)
        {
            Creditos = Creditos + monto;
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
