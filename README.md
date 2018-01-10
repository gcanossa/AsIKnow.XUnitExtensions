# AsIKnow.XUnitExtensions

XUint extension library.

The library provides two classes to support dependency injection fixtures that enables the configuration of a test collection in a way similar to the configuration of an aspent core application.

The support classes are _DependencyInjectionBaseFixture<T>_ and _DockerEnvironmentsFixture<T>_.

Both reads the configuration file _appsettings.json_, call _ConfigureServices_ method which configures an _IServiceProvider_ instance. After that a public _Configure_ method is searched for and if found is executed trying to resolve any parameter dependency throug the configured service provider.
In the case of _DockerEnvironmentsFixture<T>_ a docker-compose is also triggered in order to give a chance to resolve external service dependencies.

The _T_ parameter is used to separate the configuratio informations between different instances of the classes.

## Examples ##

DatabaseFixture.cs:
<pre>
	using AsIKnow.DependencyHelpers.EF;
	using AsIKnow.XUnitExtensions;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using System;
	using System.Collections.Generic;
	using System.Text;
	
	namespace AsIKnow.Model.XTest
	{
	    public interface Database
	    { }
	
	    public class DatabaseFixture : DockerEnvironmentsFixture&lt;Database>
	    {
	        protected override void ConfigureServices(ServiceCollection sc)
	        {
	            sc.AddDbContext<ModelContext>(optionsBuilder =>
	                optionsBuilder
	                    .UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
	            );
	        }
	
	        public void Configure()
	        {
	            WaitForDependencies(builder => builder.AddEntityFramewrokDbContext<ModelContext>("test", true));
	        }
	    }
	}
</pre>

appsettings.json:
<pre>
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5433;Database=xy;User Id=postgres;Password=root;"
  },
  "Test": {
    "DockerEnvironments": {
      "Database": {
        "DockerComposePath": "./docker-compose.yml",
        "Parameters": {},
        "DependencyCheckOptions": {
          "CheckInterval": 2,
          "CheckTimeout":  40
        }
      }
    }
  },
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
</pre>

docker-compose.yml:
<pre>
version: '3'

services:
  db:
    image: postgres
    ports:
      - 5433:5432
    environment:
      POSTGRES_PASSWORD: root
</pre>