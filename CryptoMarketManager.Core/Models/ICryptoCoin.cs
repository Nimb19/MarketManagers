using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CryptoMarketManager.Core.Models
{
    public interface ICryptoCoin
    {
        public string ShortName { get; }
        public string FullName { get; }
    }
}
