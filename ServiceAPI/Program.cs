using _3SERP.Web;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using Topshelf;
using BusinessLogic;
using Helper;
namespace ServiceAPI
{
    class Program : IContainerAccessor
    {
        #region Properties

        public static IUnityContainer Container { get; set; }
        IUnityContainer IContainerAccessor.Container
        {
            get { return Container; }
        }

        #endregion

        static void Main(string[] args)
        {
            IUnityContainer container = new UnityContainer();
            Dependency.Register(container);
            Container = container;
            try
            {
                PublicBLL.connectionString = ConfigurationManager.AppSettings["ConnectString"].Convert_ToString();
                PublicBLL.connectionSysString = ConfigurationManager.AppSettings["ConnectString_HDDT"].Convert_ToString();             
                PublicBLL.InitOptions();
            }
            catch
            {

            }

            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ////ghi vào file config
            //string token = System.Guid.NewGuid().ToString();
            //if (config.AppSettings.Settings["APIKey"] == null)
            //{

            //    config.AppSettings.Settings.Add("APIKey", token);
            //}
            //else
            //{
            //    if (config.AppSettings.Settings["APIKey"].Value.Convert_ToString().Equals(""))
            //    {
            //        config.AppSettings.Settings["APIKey"].Value = token;
            //    }

            //}

            //config.Save(ConfigurationSaveMode.Modified);
            //ConfigurationManager.RefreshSection("appSettings");  

            HostFactory.Run(x =>
            {
                x.Service<ServiceApi>(p =>
                {
                    p.ConstructUsing(name => new ServiceApi());
                    p.WhenStarted(tc => tc.Start());
                    p.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("Service APIERP");
                x.SetDisplayName("ServiceAPIERP");
                x.SetServiceName("ServiceAPIERP");
            });
        }
    }
}
