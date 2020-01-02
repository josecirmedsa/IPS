using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Utils
{
    public class TripleDESUtil
    {
        #region [Properties]
        public byte[] IV { get; set; }
        public byte[] Key { get; set; }
        #endregion
        #region [Constructors]
        public TripleDESUtil()
        {
        }
        public TripleDESUtil(string pKey)
        {
            var encoding = new UTF8Encoding();
            Key = encoding.GetBytes(pKey);
        }
        #endregion
        #region [Methods]
        /// <summary>
        /// Desencripta el arreglo de bytes que recibe como parametro utilizando
        /// el algoritmo TripleDES
        /// </summary>
        /// <param name="pText"></param>
        /// <returns>Texto desencriptado</returns>
        public string DesEncrypt(byte[] pText)
        {
            var cryptoProvider = new
                TripleDESCryptoServiceProvider();
            cryptoProvider.KeySize = 192;
            cryptoProvider.Mode = CipherMode.ECB;
            cryptoProvider.Padding = PaddingMode.Zeros;
            var cryptoTransform = cryptoProvider.CreateDecryptor(Key,
                IV);
            var memoryStream = new MemoryStream(pText);
            var cryptoStream = new CryptoStream(memoryStream,
                cryptoTransform, CryptoStreamMode.Read);
            var sr = new StreamReader(cryptoStream, true);
            return sr.ReadToEnd();
        }
        /// <summary>
        /// Encripta el texto que recibe como parametro utilizando el algoritmo
        /// tripleDES
        /// </summary>
        /// <param name="pText"></param>
        /// <returns>Arreglo de bytes correspondiente al texto encriptado</returns>
        public byte[] Encrypt(string pText)
        {
            var encoding = new UTF8Encoding();
            var message = encoding.GetBytes(pText);
            var cryptoProvider = new
                TripleDESCryptoServiceProvider();
            cryptoProvider.KeySize = 192;
            cryptoProvider.Mode = CipherMode.ECB;
            cryptoProvider.Padding = PaddingMode.Zeros;
            IV = cryptoProvider.IV;
            var criptoTransform = cryptoProvider.CreateEncryptor(Key,
                IV);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream,
                criptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(message, 0, message.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }
        #endregion
    }
}
