using Owin;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace ServiceAPI
{
  public  class WebApiConfig
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
               name: "Route1_ServiceAPI",
               routeTemplate: "ServiceAPI/api/{controller}/{action}",
               defaults: new { id = RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
                name: "Route2_ServiceAPI",
                routeTemplate: "ServiceAPI/api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Route3_ServiceAPI",
                routeTemplate: "ServiceAPI/api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Services.Add(typeof(IExceptionLogger), new Helper.GlobalErrorLogger());  
            appBuilder.UseWebApi(config);
            
        }
    }
}
