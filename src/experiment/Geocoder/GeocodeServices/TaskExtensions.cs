using System;
using System.Threading;
using System.Threading.Tasks;

namespace Geo.Coder.GeocodeServices
{
  public static class TaskExtensions
  {
    /// <summary>
    /// Task result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <see cref="http://stackoverflow.com/questions/15316613/real-life-scenarios-for-using-taskcompletionsourcet"/>
    /// <returns></returns>
    public static Task<T> GetTaskResult<T>(this T result)
    {
      var tcs = new TaskCompletionSource<T>();
      ThreadPool.QueueUserWorkItem(_ =>
      {
        try
        {
          tcs.SetResult(result);
        }
        catch (Exception exc)
        {
          tcs.SetException(exc);
        }
      });
      return tcs.Task;
    }
  }
}