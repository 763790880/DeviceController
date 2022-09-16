using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteHelper
{
    public class OpcuaDeviceModel
    {
        public string? DeviceName { get; set; } = default;
        public string? Depict { get; set; } = default;
        public List<OpcuaNodeModel> Nodes { get; set; }
    }
}
