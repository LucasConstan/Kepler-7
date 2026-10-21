using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.Builder;

namespace Kepler7.Core.Patrones.TemplateMethod
{
    /// <summary>TEMPLATE METHOD concreto - Soldado: dispara al enemigo más cercano.</summary>
    public class TurnoSoldado : TurnoUnidad
    {
        private const int Alcance = 4;

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            var enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, Alcance);
            if (enemigos.Count > 0) return Accion.Atacar(enemigos[0], "dispara al enemigo más cercano");
            return Accion.Esperar("no hay enemigos a su alcance");
        }
    }

    /// <summary>TEMPLATE METHOD concreto - Francotirador: ve todo el carril y apunta al enemigo con más vida.</summary>
    public class TurnoFrancotirador : TurnoUnidad
    {
        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            var enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, Grilla.Columnas);
            if (enemigos.Count == 0) return Accion.Esperar("no hay enemigos en el carril");

            Enemigo masFuerte = enemigos[0];
            foreach (var enemigo in enemigos)
            {
                if (enemigo.Vida > masFuerte.Vida) masFuerte = enemigo;
            }
            return Accion.Atacar(masFuerte, "apunta al enemigo con más vida");
        }
    }

    /// <summary>TEMPLATE METHOD concreto - Médico: cura al aliado más herido de la zona; si no hay, dispara.</summary>
    public class TurnoMedico : TurnoUnidad
    {
        private const int Curacion = 20;
        private const int AlcanceDisparo = 2;

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            Unidad? herido = null;
            foreach (var aliado in contexto.Grilla.Defensores())
            {
                if (aliado == unidad || aliado.Vida >= aliado.VidaMaxima) continue;

                bool cerca = Math.Abs(aliado.Carril - unidad.Carril) <= 1 &&
                             Math.Abs(aliado.Columna - unidad.Columna) <= 2;
                if (cerca && (herido == null || aliado.Vida < herido.Vida)) herido = aliado;
            }
            if (herido != null) return Accion.Curar(herido, "es el aliado más herido cerca");

            var enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, AlcanceDisparo);
            if (enemigos.Count > 0) return Accion.Atacar(enemigos[0], "nadie para curar, se defiende");
            return Accion.Esperar("nadie para curar");
        }

        protected override string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            if (accion.Tipo == TipoAccion.Curar && accion.Objetivo != null)
            {
                int curado = contexto.Curar(accion.Objetivo, Curacion);
                return $"cura a {accion.Objetivo.NombreCompleto}: +{curado} PV";
            }
            return base.Actuar(unidad, accion, contexto);
        }
    }

    /// <summary>TEMPLATE METHOD concreto - Comandante: coordina al carril (+daño a todos) y juega primero.</summary>
    public class TurnoComandante : TurnoUnidad
    {
        private const int BonusCoordinacion = 5;
        private const int AlcanceDisparo = 3;

        public override int Prioridad => 0;

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            var enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, Grilla.Columnas);
            if (enemigos.Count == 0) return Accion.Esperar("no hay enemigos en el carril");

            bool hayAliados = false;
            foreach (var defensor in contexto.Grilla.Defensores())
            {
                if (defensor != unidad && defensor.Carril == unidad.Carril) hayAliados = true;
            }
            if (hayAliados) return Accion.Potenciar("coordina a los aliados del carril");

            var cercanos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, AlcanceDisparo);
            if (cercanos.Count > 0) return Accion.Atacar(cercanos[0], "no tiene aliados, dispara él mismo");
            return Accion.Esperar("el enemigo todavía está lejos");
        }

        protected override string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            if (accion.Tipo == TipoAccion.Potenciar)
            {
                int cantidad = 0;
                foreach (var defensor in contexto.Grilla.Defensores())
                {
                    if (defensor.Carril != unidad.Carril) continue;
                    defensor.BonusDanio += BonusCoordinacion;
                    cantidad++;
                }
                return $"+{BonusCoordinacion} de daño a {cantidad} unidades del carril durante este tick";
            }
            return base.Actuar(unidad, accion, contexto);
        }
    }

    /// <summary>TEMPLATE METHOD concreto - Nave de apoyo: dispara varias veces por tick (según cómo la armó el Builder).</summary>
    public class TurnoNaveApoyo : TurnoUnidad
    {
        private const int Alcance = 5;

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            var enemigos = contexto.Grilla.EnemigosEnRango(unidad.Carril, unidad.Columna, Alcance);
            if (enemigos.Count > 0) return Accion.Atacar(enemigos[0], "abre fuego contra el más cercano");
            return Accion.Esperar("no hay enemigos a su alcance");
        }

        protected override string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            if (accion.Tipo != TipoAccion.Atacar || unidad is not NaveApoyo nave)
                return base.Actuar(unidad, accion, contexto);

            var resultados = new List<string>();
            Unidad? objetivo = accion.Objetivo;
            for (int i = 0; i < nave.DisparosPorTick; i++)
            {
                // Si el objetivo ya cayó, busca el siguiente enemigo a su alcance.
                if (objetivo == null || !objetivo.EstaViva)
                {
                    var enemigos = contexto.Grilla.EnemigosEnRango(nave.Carril, nave.Columna, Alcance);
                    objetivo = enemigos.Count > 0 ? enemigos[0] : null;
                }
                if (objetivo == null) break;

                int real = contexto.AplicarDanio(objetivo, nave.PotenciaDeFuego + nave.BonusDanio);
                resultados.Add($"dispara a {objetivo.NombreCompleto} (-{real} PV)");
            }
            return resultados.Count > 0 ? string.Join("; ", resultados) : "no pudo disparar";
        }
    }

    /// <summary>
    /// TEMPLATE METHOD concreto - Enemigo: el paso "Decidir" se lo pregunta a su estrategia de IA
    /// (acá se juntan Template Method y Strategy).
    /// </summary>
    public class TurnoEnemigo : TurnoUnidad
    {
        protected override string Inicio(Unidad unidad, Contexto contexto)
        {
            var enemigo = (Enemigo)unidad;
            return $"empieza su turno (IA {enemigo.Estrategia.Nombre}, {enemigo.Vida}/{enemigo.VidaMaxima} PV)";
        }

        protected override Accion Decidir(Unidad unidad, Contexto contexto)
        {
            var enemigo = (Enemigo)unidad;
            return enemigo.Estrategia.DecidirAccion(enemigo, contexto);
        }

        protected override string Actuar(Unidad unidad, Accion accion, Contexto contexto)
        {
            var enemigo = (Enemigo)unidad;
            // Los enemigos lentos (la Criatura) solo avanzan cada cierta cantidad de ticks.
            if (accion.Tipo == TipoAccion.Avanzar && !enemigo.PuedeAvanzarEnTick(contexto.Tick))
                return "es lento, no se mueve en este tick";
            return base.Actuar(unidad, accion, contexto);
        }
    }
}
