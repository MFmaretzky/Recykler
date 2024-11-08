namespace Recykler
{
  public class Program
  {
    public static void Main()
    {
      int dataPin = 5;     // GPIO pin number connected to HX711 DOUT
      int clockPin = 6;    // GPIO pin number connected to HX711 SCK

      var hx711 = new HX711(dataPin, clockPin);

      // Calibration reference unit 
      double referenceUnit = 1;

      hx711.Tare();
      Console.WriteLine("Tare set. Scale zeroed.");

      while ( true )
      {
        double weight = hx711.GetWeight(referenceUnit);
        Console.WriteLine($"Weight: {weight} grams");
        Thread.Sleep(500);
      }
    }
  }
}