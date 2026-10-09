using System.Collections.Generic;

namespace Kepler_7.Patrones.Prototype
{
    /// <summary>
    /// PROTOTYPE — Registro de prototipos. El laboratorio guarda un tripulante base por rol
    /// y genera defensores nuevos clonándolos, sin hacer "new Soldado()" cada vez.
    /// </summary>
    public class LaboratorioClonacion
    {
        private readonly Dictionary<string, Tripulante> _prototipos = new Dictionary<string, Tripulante>();

        public IEnumerable<Tripulante> Prototipos => _prototipos.Values;

        public void RegistrarPrototipo(Tripulante prototipo)
        {
            _prototipos[prototipo.Rol] = prototipo;
        }

        public Tripulante ObtenerPrototipo(string rol) => _prototipos[rol];

        public Tripulante Clonar(string rol) => _prototipos[rol].Clonar();

        public static LaboratorioClonacion CrearConPrototiposBase()
        {
            var laboratorio = new LaboratorioClonacion();
            laboratorio.RegistrarPrototipo(new Soldado());
            laboratorio.RegistrarPrototipo(new Medico());
            laboratorio.RegistrarPrototipo(new Francotirador());
            laboratorio.RegistrarPrototipo(new Comandante());
            return laboratorio;
        }
    }
}
