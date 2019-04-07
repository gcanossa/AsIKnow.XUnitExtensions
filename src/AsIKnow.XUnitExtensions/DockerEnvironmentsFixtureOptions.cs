using AsIKnow.DependencyHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.XUnitExtensions
{
    public class DockerEnvironmentsFixtureOptions
    {
        public string DockerComposePath { get; set; } = Environment.CurrentDirectory;
        public string DockerMachineName { get; set; }
        public DependencyCheckerOptions DependencyCheckOptions { get; set; } = new DependencyCheckerOptions() { CheckInterval = 2, CheckTimeout = 30 };
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}
