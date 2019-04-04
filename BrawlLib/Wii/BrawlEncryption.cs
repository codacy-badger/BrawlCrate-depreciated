using BrawlLib.SSBB.ResourceNodes;
using System.IO;
using System.Security.Cryptography;

namespace BrawlLib.Wii
{
    public unsafe static class BrawlEncryption
    {
        public static readonly byte[] Key = { 0xab, 0x01, 0xb9, 0xd8, 0xe1, 0x62, 0x2b, 0x08, 0xaf, 0xba, 0xd8, 0x4d, 0xbf, 0xc2, 0xa5, 0x5d };
        public static readonly byte[] IV = { 0x4E, 0x03, 0x41, 0xDE, 0xE6, 0xBB, 0xAA, 0x41, 0x64, 0x19, 0xB3, 0xEA, 0xE8, 0xF5, 0x3B, 0xD9 };

        public unsafe static DataSource Encrypt(DataSource d)
        {
            byte[] newRaw;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (UnmanagedMemoryStream old = new UnmanagedMemoryStream((byte*)d.Address, d.Length))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        old.CopyTo(ms);
                        byte[] oldRaw = ms.ToArray();
                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                            {
                                csEncrypt.Write(oldRaw, 0, oldRaw.Length);
                                csEncrypt.CopyTo(msEncrypt);
                                newRaw = msEncrypt.ToArray();
                            }
                        }
                    }
                }
            }
            return new DataSource(new MemoryStream(newRaw));
        }

        public static DataSource Decrypt(DataSource d)
        {
            byte[] newRaw;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (UnmanagedMemoryStream old = new UnmanagedMemoryStream((byte*)d.Address, d.Length))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        old.CopyTo(ms);
                        byte[] oldRaw = ms.ToArray();
                        using (MemoryStream msDecrypt = new MemoryStream())
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                            {
                                csDecrypt.Write(oldRaw, 0, oldRaw.Length);
                                csDecrypt.CopyTo(msDecrypt);
                                newRaw = msDecrypt.ToArray();
                            }
                        }
                    }
                }
            }
            return new DataSource(new MemoryStream(newRaw));
        }
    }
}
