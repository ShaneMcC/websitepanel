using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers.VirtualizationForPC
{
    public static class MacAddressHelper
    {
        const string _allPossibleCharacters = "0123456789ABCDEF";
        readonly static Random _randomGenerator = new Random();

        public static string GetNewMacAddress()
        {
            StringBuilder macAddress = new StringBuilder("00:1D:D8:");

            for (int i = 0; i < 8; i++)
            {
                macAddress.Append(((i + 1) % 3 == 0) ? ':' : _allPossibleCharacters[_randomGenerator.Next(16)]);
            }

            return macAddress.ToString();
        }
    }
}
