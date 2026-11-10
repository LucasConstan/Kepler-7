using System.Collections.Generic;

namespace Kepler7.Core.Modelo
{
    // Un defensor dentro de una partida guardada.
    public class UnidadGuardada
    {
        // "Heroe", "Nave" o el rol del tripulante ("Soldado", "Médico", "Francotirador", "Comandante").
        public string Tipo { get; set; } = "";

        // Solo para las naves: la receta del Astillero con la que se vuelve a armar ("Exploradora", "Combate", "Carga").
        public string Receta { get; set; } = "";

        // -1 si está en la reserva.
        public int Carril { get; set; } = -1;
        public int Columna { get; set; } = -1;
        public int Vida { get; set; }

        // Solo para los tripulantes: todo su equipo, incluido el que traen de fábrica.
        public List<EquipoGuardado> Equipamiento { get; set; } = new List<EquipoGuardado>();
    }
}
