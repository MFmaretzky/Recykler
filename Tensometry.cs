using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recykler
{
  public class HX711
  {
    private readonly int dataPin;
    private readonly int clockPin;
    private readonly GpioController gpio;
    private double tareOffset = 0;

    public HX711(int dataPin, int clockPin)
    {
      this.dataPin = dataPin;
      this.clockPin = clockPin;
      gpio = new GpioController();

      // Open the GPIO pins
      gpio.OpenPin(dataPin, PinMode.Input);
      gpio.OpenPin(clockPin, PinMode.Output);
    }

    public int Read()
    {
      // Wait for the chip to be ready
      while ( gpio.Read(dataPin) == PinValue.High ) { }

      int count = 0;

      // Read 24 bits of data
      for ( int i = 0; i < 24; i++ )
      {
        gpio.Write(clockPin, PinValue.High);
        count = count << 1;

        gpio.Write(clockPin, PinValue.Low);

        if ( gpio.Read(dataPin) == PinValue.High )
        {
          count++;
        }
      }

      // Set gain to 128 by sending one more clock pulse
      gpio.Write(clockPin, PinValue.High);
      gpio.Write(clockPin, PinValue.Low);

      // Convert the 24-bit result to a signed 32-bit integer
      count ^= 0x800000;  // Convert to a signed integer
      return count;
    }

    public double ReadAverage(int times = 10)
    {
      double sum = 0;
      for ( int i = 0; i < times; i++ )
      {
        sum += Read();
        Thread.Sleep(10);  // Small delay between reads
      }
      return sum / times;
    }

    public void Tare()
    {
      tareOffset = ReadAverage();  // Set tare offset to current average reading
    }

    public double GetWeight(double referenceUnit)
    {
      double weight = (ReadAverage() - tareOffset) / referenceUnit;
      return Math.Round(weight, 2);
    }

    public void Dispose()
    {
      gpio.ClosePin(dataPin);
      gpio.ClosePin(clockPin);
      gpio.Dispose();
    }
  }
}
