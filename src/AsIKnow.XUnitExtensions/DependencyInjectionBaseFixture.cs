using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AsIKnow.XUnitExtensions
{
    public abstract class DependencyInjectionBaseFixture<T>
    {
        public const string ConfigureMethodName = "Configure";

        public IConfigurationRoot Configuration { get; protected set; }

        protected IServiceProvider ServiceProvider { get; set; }

        public DependencyInjectionBaseFixture()
        {
            Init();
        }

        protected virtual void Init()
        {
            BuildConfigurationRoot();

            PrepareServicePorvider();

            FindAndExecConfigure();
        }

        protected virtual void BuildConfigurationRoot()
        {
            Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        }

        protected virtual void PrepareServicePorvider()
        {
            ServiceCollection sc = new ServiceCollection();
            ConfigureServices(sc);
            ServiceProvider = sc.BuildServiceProvider();
        }

        protected abstract void ConfigureServices(ServiceCollection sc);
        protected void FindAndExecConfigure()
        {
            Type type = GetType();

            if (type.GetMethods().Count(p => p.Name == ConfigureMethodName) > 1)
                throw new Exception("This kinf of fixture must have at most one \'Configure\' method defined.");

            MethodInfo minfo = type.GetMethod(ConfigureMethodName);
            if (minfo != null)
            {
                Dictionary<ParameterInfo, object> pars = minfo.GetParameters().ToDictionary(p => p, p => (object)null);
                foreach (ParameterInfo key in pars.Keys.ToArray())
                {
                    pars[key] = ServiceProvider.GetRequiredService(key.ParameterType);
                }
                minfo.Invoke(this, pars.Values.ToArray());
            }
        }

        public virtual TService GetService<TService>()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }
        public virtual object GetService(Type type)
        {
            return ServiceProvider.GetRequiredService(type);
        }
        public virtual IServiceScope CreateScope()
        {
            return ServiceProvider.CreateScope();
        }
    }
}
