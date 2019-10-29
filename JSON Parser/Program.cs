using System;
using System.Deployment.Application;

namespace JSON_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello world!");
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                string version = string.Format("Your application name - v{0}",
                    ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
                Console.WriteLine(version);
            }
        }
    }
}
