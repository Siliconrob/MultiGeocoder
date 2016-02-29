using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Geo.Coder
{
  internal static class Program
  {
    private static readonly int ChunkSize = Environment.ProcessorCount;
    private static ILogger _log;

    private static void SetupLog()
    {
      _log = LogManager.GetLogger("AppRunner");
      TestLog();
    }

    private static void TestLog()
    {
      _log.Debug("Debug messages are {0}", _log.IsDebugEnabled);
      _log.Warn("Test Log");
      _log.Info("Test Log");
      _log.Fatal("Test Log");
      _log.Error("Test Log");
    }

    private static void ConsoleInputs()
    {
      Console.WriteLine("Press Q to quit the program");
      Console.Write("Enter free form address to geocode: ");
      var inputText = Console.ReadLine() ?? "";
      while (!inputText.Equals("q", StringComparison.CurrentCultureIgnoreCase))
      {
        var text = inputText;
        var geoTask = new List<Task>
        {
          Task.Run(async () =>
          {
            var results = await Geocode(new List<string> {text.Trim()});
            Console.WriteLine("Result: {0}", results);
          })
        };
        Task.WaitAll(geoTask.ToArray());
        Console.Write("Enter free form address to geocode: ");
        inputText = Console.ReadLine() ?? "";
      }
    }

    private static void WriteExampleUse()
    {
      Console.WriteLine("Incorrect arguments");
      Console.WriteLine("geo [address text file] or");
      Console.WriteLine("geo [address text file] [result text file]");
    }

    private static async Task<string> Geocode(IEnumerable<string> addresses)
    {
      return await Geocode(addresses, ChunkSize);
    }

    private static async Task<string> Geocode(IEnumerable<string> addresses, int segmentSize)
    {
      var geocoders = GeoServices.All();
      var outputText = new StringBuilder();
      foreach (var partialSegment in addresses.Chunk(segmentSize))
      {
        var perGeocoder = Stopwatch.StartNew();
        foreach (var geocoder in geocoders)
        {
          var results = await Batch.Geocode(partialSegment, geocoder);
          outputText.AppendLine(
            $"{string.Concat(Enumerable.Repeat("-", 20))} *[{geocoder.Method.Name} - {perGeocoder.ElapsedMilliseconds} milliseconds |Batch Size {partialSegment.Count()}|]* {string.Concat(Enumerable.Repeat("-", 20))}");
          foreach (var textLine in results)
          {
            outputText.AppendLine(textLine);
          }
        }
      }
      return outputText.ToString();
    }

    private static void AsUtilityApplication(string[] args)
    {
      string inputFile;
      var outputFile = @"results.txt";

      if (args.Any())
      {
        inputFile = args.First().Trim();
        if (args.Count() == 2)
        {
          outputFile = args.Last().Trim();
        }
      }
      else
      {
        WriteExampleUse();
        return;
      }
      var geoTask = new List<Task>
      {
        Task.Run(async () =>
        {
          File.Delete(outputFile);
          var totalTime = Stopwatch.StartNew();
          var textToWrite = await Geocode(File.ReadAllLines(inputFile), ChunkSize);
          Debug.WriteLine("Total geocode time: {0} milliseconds", totalTime.ElapsedMilliseconds);
          File.AppendAllText(outputFile, textToWrite);
        })
      };
      Task.WaitAll(geoTask.ToArray());
    }

    private static void Main(string[] args)
    {
      SetupLog();
      AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
      {
        var exception = (eventArgs.ExceptionObject as Exception) ?? new Exception();
        _log.Error("Unhandled: {0}", exception.Message);
      };

      if (!args.Any())
      {
        // For terminal interaction
        ConsoleInputs();
      }
      else
      {
        // For terminal use as utility application
        AsUtilityApplication(args);
      }
    }
  }
}