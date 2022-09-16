using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteHelper
{
    public class OpcuaNodeModel
    {
        public string OpcuaId { get; set; }
        public Type ActualType { get; set; }
        public Type ExpectedType { get; set; }
        public object Values { get; set; }
    }
}
