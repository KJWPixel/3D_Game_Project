using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AES : MonoBehaviour
{
    public static string EncryptAES(string textToEncrypt, string key)
    {
        using var rijndael = GetRijndaelCipher(key);
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
        byte[] encrypted = rijndael.CreateEncryptor().TransformFinalBlock(plainText, 0, plainText.Length);
        return Convert.ToBase64String(encrypted);
    }

    public static string DecryptAES(string textToDecrypt, string key)
    {
        using var rijndael = GetRijndaelCipher(key);
        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] plainText = rijndael.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }

    private static RijndaelManaged GetRijndaelCipher(string key)
    {
        byte[] keyBytes = new byte[16];
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        int len = Math.Min(pwdBytes.Length, keyBytes.Length);
        Array.Copy(pwdBytes, keyBytes, len);

        return new RijndaelManaged
        {
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7,
            KeySize = 128,
            BlockSize = 128,
            Key = keyBytes,
            IV = keyBytes
        };
    }

}
