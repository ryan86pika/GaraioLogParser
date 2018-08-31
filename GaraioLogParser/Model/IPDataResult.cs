using System;
using System.Collections.Generic;
using System.Linq;

namespace GaraioLogParser.Model
{
    public class IPDataResult
    {
        private List<string> _fQDNs;

        public String ClientIp { get; private set; }
        public List<String> FQDNs { get => _fQDNs = _fQDNs ?? new List<string>(); private set => _fQDNs = value; }
        public Int64 NCalls { get; private set; }

        public IPDataResult(String clientIp, String fqdn, Int64 nCalls)
        {
            ClientIp = clientIp;

            if (!string.IsNullOrEmpty(fqdn)) AddFQDN(fqdn);

            NCalls = nCalls;
        }

        public void AddFQDN(String fqdn)
        {
            if (string.IsNullOrEmpty(fqdn)) return;
            else if (!FQDNs.Any(f => f == fqdn)) FQDNs.Add(fqdn);
        }

        public void AddNCalls(Int64 nCalls) => NCalls += nCalls;

        public void AddParams(String fqdn, Int64 nCalls)
        {
            AddFQDN(fqdn);
            AddNCalls(nCalls);
        }
    }
}
