using System.Collections.Generic;
using System.Drawing;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.Prototype
{
    // Un defensor que sabe clonarse a sí mismo, con una copia propia de su equipamiento.
    public abstract class Tripulante : Unidad, IPrototipo<Tripulante>
    {
        private List<Equipo> _equipamiento = new List<Equipo>();

        public string Rol { get; private set; }
        public int DanioBase { get; private set; }
        public int VidaBase { get; private set; }

        // Id del tripulante del que se clonó (null si es un original).
        public int? ClonadoDeId { get; private set; }

        public IReadOnlyList<Equipo> Equipamiento
        {
            get { return _equipamiento; }
        }

        public override int Danio
        {
            get
            {
                int total = DanioBase;
                for (int i = 0; i < _equipamiento.Count; i++)
                {
                    total = total + _equipamiento[i].BonusDanio;
                }
                return total;
            }
        }

        public override int VidaMaxima
        {
            get
            {
                int total = VidaBase;
                for (int i = 0; i < _equipamiento.Count; i++)
                {
                    total = total + _equipamiento[i].BonusVida;
                }
                return total;
            }
        }

        protected Tripulante(string rol, string simbolo, Color color, int danioBase, int vidaBase, int costo)
        {
            Rol = rol;
            Nombre = rol;
            Simbolo = simbolo;
            Color = color;
            DanioBase = danioBase;
            VidaBase = vidaBase;
            Costo = costo;
            Vida = vidaBase;
        }

        public void Equipar(Equipo equipo)
        {
            _equipamiento.Add(equipo);
            Vida = Vida + equipo.BonusVida;
        }

        // Para retomar una partida guardada: el tripulante vuelve con el equipo que tenía
        // (que ya incluye el de fábrica), en vez de sumarlo encima del que trae al crearse.
        internal void ReemplazarEquipamiento(List<Equipo> equipo)
        {
            _equipamiento = equipo;
            Vida = VidaMaxima;
        }

        public Tripulante Clonar()
        {
            // Copia superficial: copia todos los campos (rol, stats, color...) de una sola vez.
            Tripulante clon = (Tripulante)MemberwiseClone();

            // Copia profunda de lo mutable: sin esto, el clon y el original compartirían
            // la MISMA lista y equipar al clon también equiparía al original.
            List<Equipo> equipoCopiado = new List<Equipo>();
            for (int i = 0; i < _equipamiento.Count; i++)
            {
                equipoCopiado.Add(_equipamiento[i].Clonar());
            }
            clon._equipamiento = equipoCopiado;

            clon.AsignarNuevoId();
            clon.ClonadoDeId = Id;
            clon.Vida = clon.VidaMaxima;
            return clon;
        }

        public override string Descripcion()
        {
            string equipo = "";
            if (_equipamiento.Count == 0)
            {
                equipo = "sin equipo";
            }
            else
            {
                equipo = _equipamiento[0].Nombre;
                for (int i = 1; i < _equipamiento.Count; i++)
                {
                    equipo = equipo + ", " + _equipamiento[i].Nombre;
                }
            }

            return Rol + " | Daño " + Danio + " | Vida " + Vida + "/" + VidaMaxima + " | Equipo: " + equipo;
        }
    }
}
