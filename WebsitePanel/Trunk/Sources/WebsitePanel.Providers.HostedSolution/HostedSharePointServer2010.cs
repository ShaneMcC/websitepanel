using System;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Providers.HostedSolution
{
    public class HostedSharePointServer2010 : HostedSharePointServer
    {
        public HostedSharePointServer2010()
        {
            this.Wss3RegistryKey = @"SOFTWARE\Microsoft\Shared Tools\Web Server Extensions\14.0";
            this.Wss3Registry32Key = @"SOFTWARE\Wow6432Node\Microsoft\Shared Tools\Web Server Extensions\14.0";
            this.LanguagePacksPath = @"%commonprogramfiles%\microsoft shared\Web Server Extensions\14\HCCab\";
        }
    }
}
