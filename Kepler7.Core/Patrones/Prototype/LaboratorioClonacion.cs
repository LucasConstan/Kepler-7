using System.Collections.Generic;

namespace Kepler7.Core.Patrones.Prototype
{
    // Guarda un tripulante base por rol y genera defensores nuevos clonándolos,
    // sin tener que hacer "new Soldado()" cada vez.
    public class LaboratorioClonacion
    {
        private readonly Dictionary<string, Tripulante> _prototipos = new Dictionary<string, Tripulante>();

        public IEnumerable<Tripulante> Prototipos
        {
            get { return _prototipos.Values; }
        }

        // Si ya había un prototipo con ese rol, lo reemplaza.
        public void RegistrarPrototipo(Tripulante prototipo)
        {
            _prototipos[prototipo.Rol] = prototipo;
        }

        public Tripulante ObtenerPrototipo(string rol)
        {
            return _prototipos[rol];
        }

        public Tripulante Clonar(string rol)
        {
            return _prototipos[rol].Clonar();
        }

        public static LaboratorioClonacion CrearConPrototiposBase()
        {
            LaboratorioClonacion laboratorio = new LaboratorioClonacion();
            laboratorio.RegistrarPrototipo(new Soldado());
            laboratorio.RegistrarPrototipo(new Medico());
            laboratorio.RegistrarPrototipo(new Francotirador());
            laboratorio.RegistrarPrototipo(new Comandante());
            return laboratorio;
        }
    }
}
