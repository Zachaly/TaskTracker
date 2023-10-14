using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace TaskTracker.Application.Authorization.Service
{
    public interface IHashService
    {
        Task<string> HashStringAsync(string str);
        Task<bool> CompareStringWithHashAsync(string str, string hash);
    }

    public class HashService : IHashService
    {
        private readonly string _hashKey;

        public HashService(IConfiguration configuration)
        {
            _hashKey = configuration["HashKey"]!;
        }

        public Task<bool> CompareStringWithHashAsync(string str, string hash)
        {
            string decryptedHash = "";

            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_hashKey);
                aes.IV = new byte[16];
                var buffer = Convert.FromBase64String(hash);

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            decryptedHash = streamReader.ReadToEnd();
                        }
                    }
                }
            }

            return Task.FromResult(str == decryptedHash);
        }

        public Task<string> HashStringAsync(string str)
        {
            byte[] bytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_hashKey);
                aes.IV = new byte[16];

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(str);
                        }

                        bytes = memoryStream.ToArray();
                    }
                }
            }

            return Task.FromResult(Convert.ToBase64String(bytes));
        }
    }
}
