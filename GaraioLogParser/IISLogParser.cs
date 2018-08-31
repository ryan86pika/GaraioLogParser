using GaraioLogParser.Model;
using GaraioLogParser.Query;
using GaraioLogParser.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GaraioLogParser
{
    public class IISLogParser
    {
        private readonly string _logPathFileName;

        public IISLogParser(String logPathFileName)
        {
            _logPathFileName = logPathFileName;
        }

        public List<IPDataResult> ParseW3CLog()
        {
            ILogRecordSet rsLP = null;
            ILogRecord rowLP = null;

            var result = new List<IPDataResult>();

            rsLP = IISLogQuery.Execute(_logPathFileName, new string[] { "c-ip", "s-ip", "cs(Referer)" });

            do
            {
                rowLP = rsLP.Element;

                var clientIp = ExtractClientIP(rowLP.GetValue(0));
                var fqdn = ExtractFQDN(rowLP.GetValue(2), rowLP.GetValue(1));
                var nCalls = ExtractNCalls(rowLP.GetValue(3));

                SetupResult(clientIp, fqdn, nCalls, ref result);

                rsLP = rsLP.NextElement;
            } while (rsLP != null);

            return result;
        }

        private string ExtractClientIP(string str)
        {
            if (str == "0" || str == string.Empty) throw new NullReferenceException(Resource.NoValueClientIP);
            else if (str.Contains("::1")) return "localhost";
            return str;
        }

        private string ExtractFQDN(string str, string str2)
        {
            if ((str == "0" || str == string.Empty) && (str2 == "0" || str2 == string.Empty)) throw new NullReferenceException(Resource.NoValueFQDN);
            else if (str == "0" || str == string.Empty) return str2.Contains("::1") ? "localhost" : str2;
            else return (new Uri(str.Contains("::1") ? "localhost" : str)).Host;
        }

        private long ExtractNCalls(string str)
        {
            long result = 0;
            if (str == "0" || str == string.Empty) throw new NullReferenceException(Resource.NoValueCalls);
            else if (!Int64.TryParse(str, out result)) throw new FormatException(Resource.InvalidValueCalls);
            return result;
        }

        private void SetupResult(string clientIp, string fqdn, long nCalls, ref List<IPDataResult> result)
        {
            if (result.Count == 0) result.Add(new IPDataResult(clientIp, fqdn, nCalls));
            else
            {
                var resultFirstOrDefault = result.FirstOrDefault(f => f.ClientIp == clientIp);
                if (resultFirstOrDefault == null) result.Add(new IPDataResult(clientIp, fqdn, nCalls));
                else resultFirstOrDefault.AddParams(fqdn, nCalls);
            }
        }
    }
}
