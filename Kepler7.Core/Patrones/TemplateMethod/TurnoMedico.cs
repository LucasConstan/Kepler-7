using System;
using System.Collections.Generic;
using Kepler7.Core.Modelo;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    // Primero intenta curar al aliado más herido que tenga cerca. Si no hay ninguno, se defiende.
    public class TurnoMedico : TurnoUnidad
    {
        private const int Curacion = 20;
        private const int AlcanceDisparo = 2;

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            Unidad? herido = null;
            List<Unidad> defensores = contexto.Grilla.Defensores();
            for (int i = 0; i < defensores.Count; i++)
            {
                Unidad aliado = defensores[i];
                if (aliado != unidad && aliado.Vida < aliado.VidaMaxima)
                {
                    // "Cerca" es como mucho un carril de diferencia y hasta dos columnas.
                    bool cerca = Math.Abs(aliado.Carril - unidad.Carril) <= 1 &&
                                 Math.Abs(aliado.Columna - unidad.Columna) <= 2;
                    if (cerca && (herido == null || aliado.Vida < herido.Vida))
                    {
                        herido = aliado;
                    }
                }
            }

            if (herido != null)
            {
                return Accion.Curar(herido, "es el aliado más herido cerca");
            }

            List<Enemigo> enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, AlcanceDisparo);
            if (enemigos.Count > 0)
            {
                return Accion.Atacar(enemigos[0], "nadie para curar, se defiende");
            }

            return Accion.Esperar("nadie para curar");
        }

        protected override string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            // Curar es propio del médico: Contexto.Ejecutar no sabe hacerlo, por eso se resuelve acá.
            if (accion.Tipo == TipoAccion.Curar && accion.Objetivo != null)
            {
                int curado = contexto.Curar(accion.Objetivo, Curacion);
                return "cura a " + accion.Objetivo.NombreCompleto + ": +" + curado + " PV";
            }

            return base.Actuar(unidad, accion, contexto);
        }
    }
}
