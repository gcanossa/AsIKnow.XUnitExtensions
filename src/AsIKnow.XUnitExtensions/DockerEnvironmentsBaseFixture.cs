using AsIKnow.DependencyHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AsIKnow.XUnitExtensions
{
    public abstract class DockerEnvironmentsBaseFixture<T> : DependencyInjectionBaseFixture<T>, IDisposable
    {
        public DockerEnvironmentsFixtureOptions Options { get; protected set; }
        
        protected DependencyChecker DependencyChecker { get; set; }

        protected Dictionary<string, string> EnvironmentVariables = new Dictionary<string, string>();

        public DockerEnvironmentsBaseFixture()
            :base()
        {
        }

        protected override void Init()
        {
            BuildConfigurationRoot();

            Options = Configuration.GetSection($"Test:DockerEnvironments:{typeof(T).Name}").Get<DockerEnvironmentsFixtureOptions>();

            PrepareServicePorvider();

            FindAndExecConfigure();

            SetMachineEnvironment();

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
        
        protected virtual void SetMachineEnvironment()
        {
            if (!string.IsNullOrEmpty(Options.DockerMachineName))
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "docker-machine",
                    Arguments =
                        $"env --shell sh {Options.DockerMachineName}",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                var process = Process.Start(processStartInfo);

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Unable to setup docker environment. \r\nStdOutput: {process.StandardOutput.ReadToEnd()}\r\nStdError: {process.StandardError.ReadToEnd()}");
                }
                else
                {
                    var output = process.StandardOutput.ReadToEnd();
                    var matches = Regex.Matches(output, "^export ([^=]+)=(.+)$", RegexOptions.Multiline);
                    foreach (Match match in matches)
                    {
                        EnvironmentVariables[match.Groups[1].ToString()] = match.Groups[2].ToString().Trim('"');
                    }
                }
            }
        }

        protected virtual void StartContainers()
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                Arguments =
                    $"-f {Options.DockerComposePath} up -d",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            AddEnvironmentVariables(processStartInfo);

            var process = Process.Start(processStartInfo);

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Unable to start docker environment. \r\nStdOutput: {process.StandardOutput.ReadToEnd()}\r\nStdError: {process.StandardError.ReadToEnd()}");
            }
        }

        protected virtual void StopContainers(bool throwExceptionOnError = false)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "docker-compose",
                Arguments =
                    $"-f {Options.DockerComposePath} down",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };
            AddEnvironmentVariables(processStartInfo);

            var process = Process.Start(processStartInfo);

            process.WaitForExit();

            if (throwExceptionOnError && process.ExitCode != 0)
            {
                throw new Exception($"Unable to stop docker environment. \r\nStdOutput: {process.StandardOutput.ReadToEnd()}\r\nStdError: {process.StandardError.ReadToEnd()}");
            }
        }

        protected virtual void AddEnvironmentVariables(ProcessStartInfo processStartInfo)
        {
            if (EnvironmentVariables.Count > 0)
            {
                foreach (var kv in EnvironmentVariables)
                {
                    processStartInfo.EnvironmentVariables[kv.Key] = kv.Value;
                }
            }
        }
        
        public virtual void Dispose()
        {
            StopContainers(true);
        }
    }
}
