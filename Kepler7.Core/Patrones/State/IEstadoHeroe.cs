using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kepler7.Core.Patrones.State
{
    // Patrón State: cada estado responde estas preguntas a su manera,
    // así el héroe no necesita un montón de if según cómo esté.
    public interface IEstadoHeroe
    {
        string ObtenerNombre();
        bool PuedeActuar();                           // ¿juega este turno?
        int ModificarDanio(int danioBase);                   // ¿cuánto pega?
        void AlIniciarTurno(Heroe heroe);                    // ¿cambia al empezar el turno?
        void AlRecibirDanio(Heroe heroe, int danioRecibido); // ¿cambia al recibir un golpe?
    }
}
