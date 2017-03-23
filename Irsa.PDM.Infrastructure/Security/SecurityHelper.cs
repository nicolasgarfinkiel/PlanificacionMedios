using System;
using System.Net.Mail;
using System.Security.Cryptography;

namespace Irsa.PDM.Infrastructure.Security
{
    public static class SecurityHelper
    {
        public static String EncodePassword(String clearText, Algorithm hashAlgorithmType = Algorithm.Sha1)
        {
            switch (hashAlgorithmType)
            {
                case Algorithm.Md5:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Md5);

                case Algorithm.Sha1:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Sha1);

                case Algorithm.Sha256:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Sha256);

                case Algorithm.Sha384:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Sha384);

                case Algorithm.Sha512:
                    return Checksum.CalculateStringHash(clearText, Algorithm.Sha512);

                default: throw new Exception("Method not implemented");
            }           
        }

        public static string CreateRandomPassword(int passwordLength)
        {
            var str = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ23456789";
            var data = new byte[passwordLength];
            new RNGCryptoServiceProvider().GetBytes(data);
            var chArray = new char[passwordLength];
            var length = str.Length;

            for (var i = 0; i < passwordLength; i++)
            {
                chArray[i] = str[data[i] % length];
            }

            return new string(chArray);
        }

        public static bool SendEmail(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from, to, subject, body)
            {
                Priority = MailPriority.High
            };

            var client = new SmtpClient();

            try
            {
                client.Send(message);
            }
            catch (Exception exception)
            {
                throw new SmtpException(exception.Message);
            }

            return true;
        }


        //public static T DeserializeNode<T>(String xml) where T : class
        //{
        //    var stm = new MemoryStream();

        //    var stw = new StreamWriter(stm);
        //    stw.Write(xml);
        //    stw.Flush();

        //    stm.Position = 0;

        //    var ser = new XmlSerializer(typeof(T));
        //    var result = (ser.Deserialize(stm) as T);

        //    return result;
        //}

        //public static String  RemoveAcents(this String helper)
        //{
        //    var characters = new Dictionary<char, char>() { { 'á', 'a' }, 
        //                                                    { 'é', 'e' }, 
        //                                                    { 'í', 'i' }, 
        //                                                    { 'ó', 'o' },
        //                                                    { 'ú', 'u' },
        //                                                    { 'Á', 'A' }, 
        //                                                    { 'É', 'E' }, 
        //                                                    { 'Í', 'I' }, 
        //                                                    { 'Ó', 'O' },
        //                                                    { 'Ú', 'U' }};

        //    foreach (var character in characters)
        //        helper = helper.Replace(character.Key, character.Value);
            

        //    return helper;
        //}



    }
}
