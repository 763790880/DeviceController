using System.Device.Gpio;

namespace Gpio
{
    public class GpioHelper : IGpioHelper
    {
        private GpioController _ledController;
        private int _count = 0;
        public GpioHelper()
        {
            _ledController = new GpioController(PinNumberingScheme.Logical);
        }
        //打开引脚
        public void OpenPin(int[] pins, PinMode pinMode)
        {
            foreach (var item in pins)
            {
                _ledController.OpenPin(item, pinMode);
            }
        }
        /// <summary>
        /// 控制引脚的高低压
        /// </summary>
        /// <param name="h">高电压的引脚值</param>
        /// <param name="l">低电压的引脚值</param>
        public void PinTransition(List<int> h, List<int> l)
        {
            foreach (var item in h)
            {
                if (!_ledController.IsPinOpen(item))
                    _ledController.OpenPin(item, PinMode.Output);
                _ledController.Write(item, PinValue.High);
            }
            foreach (var item in l)
            {
                if (!_ledController.IsPinOpen(item))
                    _ledController.OpenPin(item, PinMode.Output);
                _ledController.Write(item, PinValue.Low);
            }
            Console.WriteLine("命令执行成功！");
        }
        /// <summary>
        /// 监听指定引脚高低压变化
        /// </summary>
        /// <param name="monitorPin">引脚值（gpio）</param>
        /// <param name="pinChangeEvent">回调事件</param>
        public void MonitorPin(int[] monitorPin, PinChangeEventHandler pinChangeEvent)
        {
            foreach (var item in monitorPin)
            {
                //_ledController.SetPinMode(item, PinMode.Input);
                _ledController.RegisterCallbackForPinValueChangedEvent(item, PinEventTypes.Falling, pinChangeEvent);
            }

        }
        /// <summary>
        /// 默认回调事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pinValueChangedEventArgs"></param>
        public void TurnColour(object sender, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            _count++;
            Console.WriteLine("按钮被按下了一次");
        }

        public void Dispose()
        {
            _ledController.Dispose();
            _ledController = null;
        }
    }
}