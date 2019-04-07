using AsIKnow.XUnitExtensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest
{
    public interface Database
    { }

    public class DatabaseFixture : DockerEnvironmentsBaseFixture<Database>
    {
        protected override void ConfigureServices(ServiceCollection sc)
        {
        }

        public void Configure()
        {
        }
    }
}
