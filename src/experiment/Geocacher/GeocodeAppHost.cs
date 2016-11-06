using System;
using System.Reflection;
using Funq;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Redis;
using ServiceStack.Text;

namespace Geo.Cacher
{
  public class GeocodeAppHost : AppHostBase
  {
    private const string RedisHost = @"RedisServer";
    //Tell Service Stack the name of your application and where to find your web services
    // if this is not the current assembly the routes will not appear properly.
    public GeocodeAppHost()
      : base("Geocode", typeof (Global).Assembly)
    {
    }

    private void RegisterServiceExceptions()
    {
      ServiceExceptionHandlers.Add((httpReq, request, exception) =>
      {
        var currentLog = LogManager.GetLogger(GetType());
        currentLog.ErrorFormat("Http Request DTO {0}", httpReq.Dto.Dump());
        currentLog.ErrorFormat("Request {0}", request.Dump());
        currentLog.ErrorFormat("Exception {0} at {1}", exception.Message, exception.StackTrace);
        return DtoUtils.CreateErrorResponse(request, exception);
      });
    }

    private void RegisterUncaughtExceptions()
    {
      //Custom global uncaught exception handling strategy
      UncaughtExceptionHandlers.Add((req, res, operationName, ex) =>
      {
        var currentLog = LogManager.GetLogger(GetType());
        currentLog.ErrorFormat("Request {0}", req.Dump());
        currentLog.ErrorFormat("Response {0}", res.Dump());
        currentLog.ErrorFormat("OperationName {0} Exception {1} at {2}",
          operationName,
          ex.Message,
          ex.StackTrace);
        res.Write($"UncaughtException {ex.GetType().Name}");
        res.EndRequest(true);
      });
    }

    private void RegisterUnHandledExceptions()
    {
      RegisterUncaughtExceptions();
      RegisterServiceExceptions();
    }

    public override void Configure(Container container)
    {
      Plugins.Add(new ServerEventsFeature());
      Plugins.Add(new CorsFeature());
      RegisterUnHandledExceptions();
      var redisServerUrl = (new AppSettings()).Get(RedisHost, "localhost:6379");
      container.Register<IRedisClientsManager>(c => new PooledRedisClientManager(redisServerUrl));
      container.Register(c => c.Resolve<IRedisClientsManager>().GetCacheClient()).ReusedWithin(ReuseScope.None);
    }
  }

  public static class ICacheClientExtensions
  {
    public static T ToResultUsingCache<T>(this ICacheClient cache,
      string cacheKey,
      Func<T> fn,
      int hours = 168) where T : class
    {
      var cacheResult = cache.Get<T>(cacheKey);
      if (cacheResult != null)
      {
        cache.Log().DebugFormat("Retrieving from cache");
        return cacheResult;
      }

      var result = fn();
      if (result == null)
      {
        return null;
      }
      cache.Set(cacheKey, result, TimeSpan.FromDays(hours));
      return result;
    }
  }

  public static class LogExtensions
  {
    public static ILog Log(this object current)
    {
      if (current == null)
      {
        LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      }
      if (current != null)
      {
        return LogManager.GetLogger(current.GetType());
      }
      return null;
    }
  }
}