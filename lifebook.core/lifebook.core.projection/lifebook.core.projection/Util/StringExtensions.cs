using System;
using System.Security.Cryptography;
using System.Text;

namespace lifebook.core.projection.Util
{
    public static class StringExtensions
    {
        public static Guid ToGuid(this string str)
        {
            Guid guid;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(str));
                guid = new Guid(hash);
            }
            return guid;
        }
    }
}
