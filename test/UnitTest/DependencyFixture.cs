using AsIKnow.XUnitExtensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest
{
    public interface Dependency
    { }
    public class DependencyFixture : DependencyInjectionBaseFixture<Dependency>
    {
        protected override void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<TestTypes.Type1>(new TestTypes.Type1());
            services.AddSingleton<TestTypes.Type2>(new TestTypes.Type2());
        }
        public void Configure(TestTypes.Type1 a, TestTypes.Type2 b)
        {

        }
    }
}
