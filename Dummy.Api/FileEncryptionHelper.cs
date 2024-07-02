namespace Dummy.Api
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class FileEncryptionHelper
    {
        public static byte[] EncryptFile(byte[] fileBytes, string password)
        {
            using (Aes aes = Aes.Create())
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                }
                aes.IV = new byte[16]; // Initialization Vector (IV)

                using (MemoryStream ms = new())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(fileBytes, 0, fileBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
            }
        }

        public static byte[] DecryptFile(byte[] encryptedBytes, string password)
        {
            using (Aes aes = Aes.Create())
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    aes.Key = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                }
                aes.IV = new byte[16]; // Initialization Vector (IV)

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                        cs.FlushFinalBlock();
                    }
                    return ms.ToArray();
                }
            }
        }
    }

}
