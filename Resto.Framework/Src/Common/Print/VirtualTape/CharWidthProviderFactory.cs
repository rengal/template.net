using System.Collections.Generic;
using System.Text;

namespace Resto.Framework.Common.Print.VirtualTape
{
    public static class CharWidthProviderFactory
    {
        public static readonly ICharWidthProvider DefaultCharWidthProvider;
        private static readonly Dictionary<Encoding, ICharWidthProvider> Providers;

        static CharWidthProviderFactory()
        {
            DefaultCharWidthProvider = new CharWidthProviderBase64();
            Providers = new Dictionary<Encoding, ICharWidthProvider>
            {
                {Encoding.GetEncoding("GB2312"), new CharWidthProviderGb2312()},
                {Encoding.GetEncoding("cp866"), new CharWidthProviderBase64()}
            };
        }

        public static ICharWidthProvider GetByEncoding(Encoding encoding)
        {
            return Providers.ContainsKey(encoding)
                ? Providers[encoding]
                : DefaultCharWidthProvider;
        }

        private sealed class CharWidthProviderGb2312 : ICharWidthProvider
        {
            public int GetCharWidth(char c)
            {
                return c < 127 ? 1 : 2;
            }
        }
        
        private sealed class CharWidthProviderBase64 : ICharWidthProvider
        {
            public int GetCharWidth(char c)
            {
                return 1;
            }
        }
    }
}
