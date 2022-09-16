using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeHub
{
    public interface IInitializationData
    {
        void Initialization();
        void CretaClientService();
        void CretaServerService();
        Task CretaEmqxLink();
        void Text();
    }
}
