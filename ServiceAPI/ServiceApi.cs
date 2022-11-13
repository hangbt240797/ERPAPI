using Microsoft.Owin.Hosting;
using System;

namespace ServiceAPI
{
    /// <summary>
    /// Class này khởi tạo các phương thức start, stop(tương tự nhưng trong windown service) cho web api seft host
    /// </summary>
   public class ServiceApi
    {
       IDisposable webServer;
       public void Start()
       {
           this.webServer = WebApp.Start<WebApiConfig>(url: System.Configuration.ConfigurationManager.AppSettings.Get("BaseAddress")); 
       }

       public void Stop()
       {
           this.webServer.Dispose();
       }

    }
}
