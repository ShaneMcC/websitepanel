using System;
using System.Security.Cryptography;
using System.Text;

namespace WebsitePanel.Providers.Common
{
    public class PasswdHelper
    {
        protected static readonly string MD5_MAGIC_PREFIX = "$apr1$";
        protected const int MD5_DIGESTSIZE = 16;
        protected static readonly string SHA_MAGIC_PREFIX = "{SHA}";

        private static readonly string itoa64 =         /* 0 ... 63 => ASCII - 64 */
            "./0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private static Random _random;

        static PasswdHelper()
        {
            _random = new Random();

        }

        public static string to64(ulong v, int n)
        {
            StringBuilder sb = new StringBuilder();
            while (--n >= 0)
            {
                sb.Append(itoa64[(int)v & 0x3f]);
                v >>= 6;
            }
            return sb.ToString();
        }


        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in ba)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }


        public static byte[] getMD5HashHex(string s)
        {
            MD5 md5 = MD5.Create();
            return md5.ComputeHash(Encoding.ASCII.GetBytes(s));
        }


        public static string MD5Encode(string pw, string salt)
        {
            // для проверки приходит такое:
            // $apr1$Vs5.....$iSQlpTkND9RjL7iAMTjDt.

            // для генерирования пароля приходит случайная строка - соль

            string password;
            byte[] final;

            // найдём соль, если в качестве соли пришёл уже хэш
            //  1. Уберём магический $apr1$
            if (salt.StartsWith(MD5_MAGIC_PREFIX))
            {
                salt = salt.Substring(MD5_MAGIC_PREFIX.Length);
            }

            //  2. Найдём соль до первого '$' Или 8 символов
            int sp = salt.IndexOf('$');
            if (sp < 0 || sp > 8) sp = 8;

            salt = salt.Substring(0, sp);
            //Debug.WriteLine(string.Format("salt [{0}]", salt));

            ByteVector s = new ByteVector();
            ByteVector s1 = new ByteVector();

            s.Add(pw);
            s.Add(MD5_MAGIC_PREFIX);
            s.Add(salt);

            s1.Add(pw);
            s1.Add(salt);
            s1.Add(pw);

            final = s1.GetMD5Hash();

            for (int i = pw.Length; i > 0; i -= MD5_DIGESTSIZE)
            {
                s.Add(final, 0, (i > MD5_DIGESTSIZE) ? MD5_DIGESTSIZE : i);
            }

            for (int i = 0; i < final.Length; i++)
                final[i] = 0;

            for (int i = pw.Length; i != 0; i >>= 1)
            {
                // (i & 1)  в апаче
                if ((i & 0x01) == 1)
                {
                    s.Add(final, 0, 1);
                }
                else
                {
                    s.Add(pw.Substring(0, 1));
                }
            }

            final = s.GetMD5Hash();

            for (int i = 0; i < 1000; i++)
            {
                s1.Clear();
                if ((i & 1) != 0)
                {
                    s1.Add(pw);
                }
                else
                {
                    s1.Add(final);
                }
                if ((i % 3) != 0)
                {
                    s1.Add(salt);
                }

                if ((i % 7) != 0)
                {
                    s1.Add(pw);
                }

                if ((i & 1) != 0)
                {
                    s1.Add(final);
                }
                else
                {
                    s1.Add(pw);
                }
                final = s1.GetMD5Hash();
            }

            password = "";
            ulong l;

            l = ((ulong)final[0] << 16) | ((ulong)final[6] << 8) | ((ulong)final[12]);
            password += PasswdHelper.to64(l, 4);
            l = ((ulong)final[1] << 16) | ((ulong)final[7] << 8) | ((ulong)final[13]);
            password += PasswdHelper.to64(l, 4);
            l = ((ulong)final[2] << 16) | ((ulong)final[8] << 8) | ((ulong)final[14]);
            password += PasswdHelper.to64(l, 4);
            l = ((ulong)final[3] << 16) | ((ulong)final[9] << 8) | ((ulong)final[15]);
            password += PasswdHelper.to64(l, 4);
            l = ((ulong)final[4] << 16) | ((ulong)final[10] << 8) | ((ulong)final[5]);
            password += PasswdHelper.to64(l, 4);
            l = ((ulong)final[11]);
            password += PasswdHelper.to64(l, 2);

            password = string.Format("{0}{1}${2}", MD5_MAGIC_PREFIX, salt, password);

            return password;
        }


        public static string SHA1Encode(string clear)
        {
            if (clear.StartsWith(SHA_MAGIC_PREFIX))
            {
                clear = clear.Substring(SHA_MAGIC_PREFIX.Length);
            }

            SHA1 sha = new SHA1CryptoServiceProvider();

            string cr = Convert.ToBase64String(
                sha.ComputeHash(Encoding.Default.GetBytes(clear))
                );
            return SHA_MAGIC_PREFIX + cr;
        }


        public static string GetRandomSalt()
        { 
            return to64((ulong)_random.Next(), 8);
        }


        public static string DigestEncode(string username, string realm, string passwd)
        {
            MD5 md5 = MD5.Create();

            byte[] b = md5.ComputeHash(Encoding.ASCII.GetBytes(
                                           string.Format("{0}:{1}:{2}", username, realm, passwd)
                                           ));

            StringBuilder sb = new StringBuilder(b.Length*2);
            for (int i = 0; i < b.Length; ++i)
            {
                sb.Append( String.Format("{0:x2}", b[i]) );
            }

            return sb.ToString();
        }
    }
}
