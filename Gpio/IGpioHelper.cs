using System.Device.Gpio;

namespace Gpio
{
    public interface  IGpioHelper :IDisposable
    {
        void OpenPin(int[] pins, PinMode pinMode);
        void PinTransition(List<int> h, List<int> l);
        void MonitorPin(int[] monitorPin, PinChangeEventHandler pinChangeEvent);
        void TurnColour(object sender, PinValueChangedEventArgs pinValueChangedEventArgs);
    }
}
