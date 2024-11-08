using System;
using System.Collections.Generic;
using System.Device.Pwm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recykler
{
  class Rotor
  (
    int chip,
    int channel,
    int frequency,
    double dutyCycle,
    double steepLevel,
    double maxSpeed
  )
  {
    private readonly int _chip = chip;
    private readonly int _channel = channel;
    private readonly int _frequency = frequency;
    private readonly double _dutyCycle = dutyCycle;
    private readonly double _steepLevel = steepLevel;
    private readonly double _maxSpeed = maxSpeed;

    public PwmChannel Init()
    {
      var pwmPin = PwmChannel.Create(_chip, _channel, _frequency, _steepLevel);
      pwmPin.Start();
      return pwmPin;
    }
    public void StartRotate(PwmChannel pwmPin, int Sleep)
    {
      // chip2 channel3 - pin19
      pwmPin.Start();
      while ( pwmPin.DutyCycle < _maxSpeed )
      {
        pwmPin.DutyCycle += _steepLevel;
        Thread.Sleep(Sleep);
      }
    }
    public void Stop(PwmChannel pwmPin)
    {
      pwmPin.DutyCycle = 0;
      pwmPin.Stop();
      pwmPin.Dispose();
    }
  }
}
