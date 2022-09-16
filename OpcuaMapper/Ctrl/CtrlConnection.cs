using Ctrl2MqttBridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpcuaMapper.Ctrl
{
    public class CtrlConnection : BridgeConnection
    {
        [Obsolete]
        public CtrlConnection()
        {
            ConnectionHandler += CtrlConnection_ConnectionHandler;
            ConnectSync("192.168.214.241", 51883, new Guid().ToString());
        }


        private void CtrlConnection_ConnectionHandler(object sender, bool e)
        {
            if (e)
                SubsClientThread();
        }

        void SubsClientThread()
        {
            if (subsc_Main == null)
            {
                subsc_Main = new SubscriptionHelper();
                subsc_Main.DataChanged += Subsc_Main_DataChanged;
            }
            AddMonitoredOPCItem("/Channel/Parameter/r[u1,1]", subsc_Main);
        }

        private void Subsc_Main_DataChanged(object sender, MonitoredItem monitoredItem)
        {
            //Callback when R1 changed...
        }
    }

}
