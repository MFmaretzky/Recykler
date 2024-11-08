using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recykler
{
  public sealed class FrontHatch : Hatch
  {
    private int chip;
    private int channel;
    private int freq;
    private double dutyCycle = 0;
    private double steepLevel = 0;
    private double maxSpeed = 0.95;

    public FrontHatch(int bridgeEnablePin, int upperSensorPin, int lowerSensorPin, int middleSensorPin)
    {
      BridgeEnablePin = bridgeEnablePin;
      UpperSensorPin = upperSensorPin;
      LowerSensorPin = lowerSensorPin;
      MiddleSensorPin = middleSensorPin;
    }

    public override void OpenHatch(GpioController gpio)
    {
      chip = 2;
      channel = 2;
      freq = 400;
      dutyCycle = 0.0;
      steepLevel = 0.19;
      maxSpeed = 0.95;

      gpio.OpenPin(_bridgeEnablePin, PinMode.Output);
      Rotor rotor = new(chip, channel, freq, dutyCycle, steepLevel, maxSpeed);
      var pwmPin = rotor.Init();

      List<PinValue> statuses = GetSenStat(gpio);

      if ( statuses[1] == PinValue.Low )
      {
        gpio.Write(_bridgeEnablePin, PinValue.High);
        rotor.StartRotate(pwmPin, 50);
        do
        {
          statuses = GetSenStat(gpio);
        } while ( statuses[0] == PinValue.High );
        rotor.Stop(pwmPin);
      }
      else
      {
        Console.WriteLine("Error, exiting now.");
      }
      gpio.Write(_bridgeEnablePin, PinValue.Low);
      gpio.ClosePin(_bridgeEnablePin);
    }

    public override void CloseHatch(GpioController gpio)
    {
      chip = 2;
      channel = 3;
      freq = 400;
      dutyCycle = 0.0;
      steepLevel = 0.05;
      maxSpeed = 0.95;

      gpio.OpenPin(_bridgeEnablePin, PinMode.Output);
      Rotor rotor = new(chip, channel, freq, dutyCycle, steepLevel, maxSpeed);
      var pwmPin = rotor.Init();

      List<PinValue> statuses = GetSenStat(gpio);

      if ( statuses[0] == PinValue.Low )
      {
        gpio.Write(_bridgeEnablePin, PinValue.High);
        rotor.StartRotate(pwmPin, 5);
        do
        {
          statuses = GetSenStat(gpio);
        } while ( statuses[1] == PinValue.High || statuses[2] == PinValue.High );
        rotor.Stop(pwmPin);
      }
      else
      {
        Console.WriteLine("Error, exiting now.");
      }

      gpio.Write(_bridgeEnablePin, PinValue.Low);
      gpio.ClosePin(_bridgeEnablePin);
    }

    public override void Process(GpioController gpio)
    {
      Console.Write("(O)pen or (c)lose?: ");
      string? input = Console.ReadLine();

      if ( input != null )
      {
        input = input.Trim().ToLower();
      }

      if ( input == "o" )
      {
        this.OpenHatch(gpio);
      }
      else if ( input == "c" )
      {
        this.CloseHatch(gpio);
      }
    }
  }
}
