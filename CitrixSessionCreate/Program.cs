using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WFICALib;

namespace CitrixSessionCreate
{
    class Program
    {
        public static AutoResetEvent onLogonResetEvent = null;

        static void Main(string[] args)
        {
            ICAClientClass icaClient = new ICAClientClass();
            onLogonResetEvent = new AutoResetEvent(false);

            //use app name here. Empty = desktop session
            icaClient.Application = string.Empty;

            icaClient.Launch = true;
            icaClient.Address = "pwvsmtctx011";
            icaClient.Domain = Environment.UserDomainName;

            //TODO: implement secret server import to username password
            //icaClient.Username = "cperera";
            //icaClient.SetProp("Password", "putpasswordHere");
            // Needed in some cases
            icaClient.EncryptionLevelSession = "EncRC5-128";

            icaClient.DesiredColor = ICAColorDepth.Color24Bit;
            //Can use headless mode also
            icaClient.OutputMode = OutputMode.OutputModeNormal;


            //Windowed or non windowed
           // ica.TWIMode = true;
           icaClient.DesiredHRes = 1024;
           icaClient.DesiredVRes = 786;

            //Scaling           
            icaClient.ScalingMode = ICAScalingMode.ScalingModeDisabled;

            // Register for the OnLogon event
            icaClient.OnLogon += new _IICAClientEvents_OnLogonEventHandler(ica_OnLogon);

            // Launch/Connect to the session
            icaClient.Connect();

            Session test = icaClient.Session;
            if (onLogonResetEvent.WaitOne(new TimeSpan(0, 5, 0)))
            {
                Console.WriteLine("Session Loggedon");
              
            }
            else
                Console.WriteLine("Logon event was not thrown");

            // Do we have access to the client simulation APIs?
            if (icaClient.Session == null)
                throw new Exception("Could not create Ica object");

            Console.WriteLine("\nPress any key to log off");
            Console.Read();

            // Logging off
            Console.WriteLine("Logging off Session");
            icaClient.Logoff();
        }
        static void ica_OnLogon()
        {
            Console.WriteLine("OnLogon event was Handled!");
            onLogonResetEvent.Set();
        }
    }
}
