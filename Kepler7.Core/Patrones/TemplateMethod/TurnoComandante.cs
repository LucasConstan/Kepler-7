using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    // Le da más daño a todo su carril. Juega primero (prioridad 0) para que el bonus
    // les sirva a los demás en este mismo tick.
    public class TurnoComandante : TurnoUnidad
    {
        private const int BonusCoordinacion = 5;
        private const int AlcanceDisparo = 3;

        public override int Prioridad
        {
            get { return 0; }
        }

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            List<Enemigo> enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, Grilla.Columnas);
            if (enemigos.Count == 0)
            {
                return Accion.Esperar("no hay enemigos en el carril");
            }

            bool hayAliados = false;
            List<Unidad> defensores = contexto.Grilla.Defensores();
            for (int i = 0; i < defensores.Count; i++)
            {
                if (defensores[i] != unidad && defensores[i].Carril == unidad.Carril)
                {
                    hayAliados = true;
                }
            }

            if (hayAliados)
            {
                return Accion.Potenciar("coordina a los aliados del carril");
            }

            List<Enemigo> cercanos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, AlcanceDisparo);
            if (cercanos.Count > 0)
            {
                return Accion.Atacar(cercanos[0], "no tiene aliados, dispara él mismo");
            }

            return Accion.Esperar("el enemigo todavía está lejos");
        }

        protected override string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            if (accion.Tipo == TipoAccion.Potenciar)
            {
                // El propio comandante también entra en el bonus. Partida lo borra al terminar el tick.
                int cantidad = 0;
                List<Unidad> defensores = contexto.Grilla.Defensores();
                for (int i = 0; i < defensores.Count; i++)
                {
                    if (defensores[i].Carril == unidad.Carril)
                    {
                        defensores[i].BonusDanio = defensores[i].BonusDanio + BonusCoordinacion;
                        cantidad = cantidad + 1;
                    }
                }

                return "+" + BonusCoordinacion + " de daño a " + cantidad + " unidades del carril durante este tick";
            }

            return base.Actuar(unidad, accion, contexto);
        }
    }
}
