using Kepler7.Core.Patrones.ChainOfResponsibility;

namespace Kepler7.Core.Patrones.Visitor
{
    /// <summary>
    /// VISITOR - Elemento. Un ítem solo guarda sus datos y sabe aceptar a un visitante;
    /// las operaciones (describir, tasar, equipar) viven en los visitantes.
    /// </summary>
    public abstract class ItemInventario
    {
        private string _nombre;

        protected ItemInventario(string nombre)
        {
            _nombre = nombre;
        }

        public string ObtenerNombre()
        {
            return _nombre;
        }

        public abstract void Aceptar(IVisitanteItem visitante);
    }

    /// <summary>VISITOR elemento concreto - Arma: aumenta el daño del héroe.</summary>
    public class ItemArma : ItemInventario
    {
        private int _bonusDanio;

        public ItemArma(string nombre, int bonusDanio) : base(nombre)
        {
            _bonusDanio = bonusDanio;
        }

        public int ObtenerBonusDanio()
        {
            return _bonusDanio;
        }

        public override void Aceptar(IVisitanteItem visitante)
        {
            visitante.Visitar(this);
        }
    }

    /// <summary>VISITOR elemento concreto - Armadura: aumenta la vida máxima del héroe.</summary>
    public class ItemArmadura : ItemInventario
    {
        private int _bonusVida;

        public ItemArmadura(string nombre, int bonusVida) : base(nombre)
        {
            _bonusVida = bonusVida;
        }

        public int ObtenerBonusVida()
        {
            return _bonusVida;
        }

        public override void Aceptar(IVisitanteItem visitante)
        {
            visitante.Visitar(this);
        }
    }

    /// <summary>VISITOR elemento concreto - Módulo: agrega una defensa a la cadena del héroe.</summary>
    public class ItemModuloDefensa : ItemInventario
    {
        private Defensa _defensa;

        public ItemModuloDefensa(string nombre, Defensa defensa) : base(nombre)
        {
            _defensa = defensa;
        }

        public Defensa ObtenerDefensa()
        {
            return _defensa;
        }

        public override void Aceptar(IVisitanteItem visitante)
        {
            visitante.Visitar(this);
        }
    }
}
