using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevicePOC
{
    public class GPIO
    {
        public readonly GpioController _ledController;
        public int _count=0;
        public GPIO(GpioController gpioController)
        {
            _ledController= gpioController;
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
        public void PinTransition(List<int> h,List<int> l)
        {
            foreach (var item in h)
            {
                _ledController.Write(item, PinValue.High);
            }
            foreach (var item in l)
            {
                _ledController.Write(item, PinValue.Low);
            }
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
            if (_count > 2)
                _count = 0;
            _count++;
            Console.WriteLine("按钮被按下了一次");
            bian();
        }
        public void bian()
        {
            var ledPin = new List<int>() { 13, 19, 26 };
            var m = _count - 1;
            Console.WriteLine($"我是魔值：{m}");
            var h = new List<int>() { ledPin[m] };
            var l= ledPin.Remove(ledPin[m]);
            PinTransition(h, ledPin);
        }
    }
}
