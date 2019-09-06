using System;
using System.Text;

namespace lifebook.core.eventstore.domain.models
{
    public sealed class Data
    {
        private byte[] _bytes;

        public Data(byte[] bytes)
        {
            _bytes = bytes;
        }

        public T TransformDataFromByte<T>(Func<byte[], T> transformer)
        {
            return transformer(_bytes);
        }

        public T TransformDataFromString<T>(Func<string, T> transformer)
        {
            return transformer(Encoding.UTF8.GetString(_bytes));
        }
    }
}
