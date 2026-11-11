using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Kepler7.Core.Patrones.Bridge
{
    // Cifrado cuántico SIMULADO. La idea real (distribución cuántica de claves) es que la estación y la
    // Tierra comparten una clave al azar del mismo largo que el mensaje, y nadie puede copiarla sin que
    // se note. Acá la clave se genera al azar y se "entrega" a la Tierra guardándola en un diccionario
    // que hace de canal cuántico. Con esa clave se aplica un XOR (one-time pad) y el resultado va en hexadecimal.
    public class CifradoCuantico : ICifrado
    {
        // El canal cuántico simulado: para cada mensaje cifrado, la clave que recibió la Tierra.
        private readonly Dictionary<string, byte[]> _clavesEntregadas = new Dictionary<string, byte[]>();

        public string Nombre
        {
            get { return "Cuántico"; }
        }

        public string Descripcion
        {
            get { return "Simulado: clave al azar del largo del mensaje (one-time pad), entregada por un canal cuántico."; }
        }

        public string Cifrar(string texto)
        {
            byte[] datos = Encoding.UTF8.GetBytes(texto);
            byte[] clave = RandomNumberGenerator.GetBytes(datos.Length);
            byte[] cifrado = Xor(datos, clave);

            string resultado = Convert.ToHexString(cifrado);
            _clavesEntregadas[resultado] = clave;
            return resultado;
        }

        public string Descifrar(string cifrado)
        {
            if (!_clavesEntregadas.ContainsKey(cifrado))
            {
                return "(la Tierra no recibió la clave cuántica de este mensaje)";
            }

            // La clave se usa una sola vez: después se descarta, como en un one-time pad.
            byte[] clave = _clavesEntregadas[cifrado];
            _clavesEntregadas.Remove(cifrado);
            return Encoding.UTF8.GetString(Xor(Convert.FromHexString(cifrado), clave));
        }

        private static byte[] Xor(byte[] datos, byte[] clave)
        {
            byte[] resultado = new byte[datos.Length];
            for (int i = 0; i < datos.Length; i++)
            {
                resultado[i] = (byte)(datos[i] ^ clave[i]);
            }
            return resultado;
        }
    }
}
