using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlLiteHelper
{
    public class GpioOrderModel
    {
        public int Id { get; set; }
        public string Order { get; set; }
        public List<GpioPinModel> Pins { get; set; }
    }
}
