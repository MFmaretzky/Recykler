using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recykler
{
  interface IHatch
  {
    void OpenHatch(GpioController gpio);
    void CloseHatch(GpioController gpio);
    void Process(GpioController gpio);
    public void SenInit(GpioController gpio);
    public void SenDeInit(GpioController gpio);
    PinValue GetPinValue(GpioController gpio, int GpioPin);
    List<PinValue> GetSenStat(GpioController gpio);
  }
}
