using AsIKnow.DependencyHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace AsIKnow.XUnitExtensions
{
    public abstract class DockerEnvironmentsFixture<T> : DependencyInjectionBaseFixture<T>, IDisposable
    {
        public DockerEnvironmentsFixtureOptions Options { get; protected set; }
        
        protected DependencyChecker DependencyChecker { get; set; }

        public DockerEnvironmentsFixture()
            :base()
        {
        }

        protected override void Init()
        {
            BuildConfigurationRoot();

            Options = Configuration.GetSection($"Test:DockerEnvironments:{typeof(T).Name}").Get<DockerEnvironmentsFixtureOptions>();

            PrepareServicePorvider();

            FindAndExecConfigure();

            StopContainers();
            StartContainers();

            if (DependencyChecker != null)
                DependencyChecker.WaitForDependencies();
        }

        protected void WaitForDependencies(Action<DependencyCheckerBuilder> configuration)
        {
            DependencyCheckerBuilder builder = new DependencyCheckerBuilder(ServiceProvider, Options.DependencyCheckOptions);
            configuration(builder);
            DependencyChecker = builder.Build();
        }
        
        protected virtual void StartContainers()
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                Arguments = 
                    $"-f {Options.DockerComposePath} up -d"
            };
            AddEnvironmentVariables(processStartInfo);

            var process = Process.Start(processStartInfo);

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Unable to start docker environment. \r\nStdOutput: {process.StandardOutput.ReadToEnd()}\r\nStdError: {process.StandardError.ReadToEnd()}");
            }
        }

        protected virtual void StopContainers(bool throwExcetionOnError = false)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                Arguments =
                    $"-f {Options.DockerComposePath} down"
            };
            AddEnvironmentVariables(processStartInfo);

            var process = Process.Start(processStartInfo);

            process.WaitForExit();

            if (throwExcetionOnError && process.ExitCode != 0)
            {
                throw new Exception($"Unable to stop docker environment. \r\nStdOutput: {process.StandardOutput.ReadToEnd()}\r\nStdError: {process.StandardError.ReadToEnd()}");
            }
        }

        protected virtual void AddEnvironmentVariables(ProcessStartInfo processStartInfo)
        {
        }
        
        public virtual void Dispose()
        {
            StopContainers(true);
        }
    }
}
