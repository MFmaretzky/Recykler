using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recykler
{
  public abstract class Hatch : IHatch
  {
    private int _upperSensorPin;
    private int _lowerSensorPin;
    private int _middleSensorPin;
    protected int _bridgeEnablePin;
    public int UpperSensorPin
    {
      get { return _upperSensorPin; }
      set { _upperSensorPin = value; }
    }
    public int LowerSensorPin
    {
      get { return _lowerSensorPin; }
      set { _lowerSensorPin = value; }
    }
    public int MiddleSensorPin
    {
      get { return _middleSensorPin; }
      set { _middleSensorPin = value; }
    }

    public int BridgeEnablePin
    {
      get { return _bridgeEnablePin; }
      set { _bridgeEnablePin = value; }
    }

    public virtual void OpenHatch(GpioController gpio)
    {
      throw new NotImplementedException();
    }

    public virtual void CloseHatch(GpioController gpio)
    {
      throw new NotImplementedException();
    }

    public virtual void Process(GpioController gpio)
    {
      throw new NotImplementedException();
    }

    public void SenInit(GpioController gpio)
    {
      gpio.OpenPin(_upperSensorPin, PinMode.InputPullUp);
      gpio.OpenPin(_lowerSensorPin, PinMode.InputPullUp);
      gpio.OpenPin(_middleSensorPin, PinMode.InputPullUp);
    }

    public void SenDeInit(GpioController gpio)
    {
      gpio.ClosePin(_upperSensorPin);
      gpio.ClosePin(_lowerSensorPin);
      gpio.ClosePin(_middleSensorPin);
    }

    public PinValue GetPinValue(GpioController gpio, int GpioPin)
    {
      return gpio.Read(GpioPin);
    }

    public List<PinValue> GetSenStat(GpioController gpio)
    {
      var statuses = new List<PinValue>();

      PinValue upperStatus = gpio.Read(_upperSensorPin);
      statuses.Add(upperStatus);

      PinValue lowerStatus = gpio.Read(_lowerSensorPin);
      statuses.Add(lowerStatus);

      PinValue middleStatus = gpio.Read(_middleSensorPin);
      statuses.Add(middleStatus);

      return statuses;
    }
  }
}
