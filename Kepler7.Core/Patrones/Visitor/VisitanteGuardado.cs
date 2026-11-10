using Kepler7.Core.Modelo;
using Kepler7.Core.Patrones.ChainOfResponsibility;

namespace Kepler7.Core.Patrones.Visitor
{
    // Pasa un ítem a datos para guardar la partida. Es una operación nueva sobre los ítems
    // y no hizo falta tocar ninguna clase de ítem: para eso está el Visitor.
    public class VisitanteGuardado : IVisitanteItem
    {
        private ItemGuardado _resultado = new ItemGuardado();

        public ItemGuardado ObtenerResultado()
        {
            return _resultado;
        }

        public void Visitar(ItemArma item)
        {
            _resultado = new ItemGuardado();
            _resultado.Tipo = "Arma";
            _resultado.Nombre = item.ObtenerNombre();
            _resultado.Bonus = item.ObtenerBonusDanio();
        }

        public void Visitar(ItemArmadura item)
        {
            _resultado = new ItemGuardado();
            _resultado.Tipo = "Armadura";
            _resultado.Nombre = item.ObtenerNombre();
            _resultado.Bonus = item.ObtenerBonusVida();
        }

        public void Visitar(ItemModuloDefensa item)
        {
            _resultado = new ItemGuardado();
            _resultado.Tipo = "Modulo";
            _resultado.Nombre = item.ObtenerNombre();
            _resultado.Defensa = NombreDefensa(item.ObtenerDefensa());
        }

        // El nombre con el que se guarda cada tipo de defensa. Lo usa también la Partida para la cadena del héroe.
        public static string NombreDefensa(Defensa defensa)
        {
            if (defensa is DefensaEscudo)
            {
                return "Escudo";
            }
            if (defensa is DefensaEvasion)
            {
                return "Evasion";
            }
            return "Blindaje";
        }
    }
}
