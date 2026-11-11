using System;
using System.Security.Cryptography;
using System.Text;

namespace Kepler7.Core.Patrones.Bridge
{
    // Cifrado AES de verdad (System.Security.Cryptography), con una clave de 256 bits que comparten
    // la estación y la Tierra. Cada mensaje usa un vector de inicialización (IV) nuevo, así el mismo
    // texto nunca da el mismo resultado. El IV viaja delante del texto cifrado, todo en Base64.
    public class CifradoAES : ICifrado
    {
        // La clave sale de una frase acordada. En un sistema real se guardaría en un lugar seguro.
        private static readonly byte[] Clave = SHA256.HashData(Encoding.UTF8.GetBytes("Kepler-7 · enlace Tierra"));

        public string Nombre
        {
            get { return "AES"; }
        }

        public string Descripcion
        {
            get { return "AES de 256 bits con un IV nuevo por mensaje. El resultado va en Base64."; }
        }

        public string Cifrar(string texto)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Clave;
                aes.GenerateIV();
                byte[] datos = Encoding.UTF8.GetBytes(texto);
                byte[] cifrado = aes.EncryptCbc(datos, aes.IV);

                byte[] paquete = new byte[aes.IV.Length + cifrado.Length];
                Buffer.BlockCopy(aes.IV, 0, paquete, 0, aes.IV.Length);
                Buffer.BlockCopy(cifrado, 0, paquete, aes.IV.Length, cifrado.Length);
                return Convert.ToBase64String(paquete);
            }
        }

        public string Descifrar(string cifrado)
        {
            byte[] paquete = Convert.FromBase64String(cifrado);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Clave;
                int largoIV = aes.BlockSize / 8;
                byte[] iv = paquete.AsSpan(0, largoIV).ToArray();
                byte[] datos = paquete.AsSpan(largoIV).ToArray();
                return Encoding.UTF8.GetString(aes.DecryptCbc(datos, iv));
            }
        }
    }
}
