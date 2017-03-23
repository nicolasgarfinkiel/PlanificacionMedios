using System;
using System.Security.Cryptography;
using System.Text;

namespace Irsa.PDM.Infrastructure.Security
{
    public class Checksum
    {
        // Methods
        private static string ArrayToString(byte[] byteArray)
        {
            StringBuilder builder = new StringBuilder(byteArray.Length);
            for (int i = 0; i < byteArray.Length; i++)
            {
                builder.Append(byteArray[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public static string CalculateStringHash(string cadena, Algorithm alg)
        {
            HashAlgorithm hashProvider = GetHashProvider(alg);
            byte[] bytes = Encoding.ASCII.GetBytes(cadena);
            return ArrayToString(hashProvider.ComputeHash(bytes));
        }

        private static HashAlgorithm GetHashProvider(Algorithm alg)
        {
            switch (alg)
            {
                case Algorithm.Md5:
                    return new MD5CryptoServiceProvider();

                case Algorithm.Sha1:
                    return new SHA1Managed();

                case Algorithm.Sha256:
                    return new SHA256Managed();

                case Algorithm.Sha384:
                    return new SHA384Managed();

                case Algorithm.Sha512:
                    return new SHA512Managed();
            }
            throw new Exception("Invalid Provider.");
        }
    }


}
