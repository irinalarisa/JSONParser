using System;
using System.Deployment.Application;
using System.Windows.Forms;

namespace JSON_Parser
{
    class Program
    {
        public static Version Version { get; } = new Version(Application.ProductVersion);

        static void Main(string[] args)
        {

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                Console.WriteLine("Your application name - v{0}",
                    ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
            }

            Console.WriteLine("Hello world!");
            Console.WriteLine(Program.Version);
            Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            //Person p = new Person { Id = 3, Name = "Ana", Age = 42};
            //string a = JSONParserSafe<Person>.ObjectToJSON(p);
            //Console.WriteLine(a);

            //string json = "{\"Id\":\"3\",\",\"Age\":\"42\"}";
            //Person p2 = JSONParserSafe<Person>.JSONToObject(json);
            //Console.WriteLine(p2.Id + " " + p2.Name + " " + p2.Age);

            //Person p3 = JSONParserSafe<Person>.JSONToObject(readStringFromFile: true, filePath: @"C:\Users\irina.barbos\Desktop\231\APP\JSON Parser\JSONParser\JSON Parser\text.txt");
            //Console.WriteLine(p3.Id + " " + p3.Name + " " + p3.Age);
        }
    }
}
