using System;

using StreamlabsEventReceiver;

// Test envoirement for StreamlabsEventReceiver DLL
namespace TestEnv
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = "";
            var SER = new StreamlabsEventClient();

            SER.StreamlabsSocketConnected += (o, e) => {
                Console.WriteLine("Connected");
            };

            SER.StreamlabsSocketDisconnected += (o, e) =>
            {
                Console.WriteLine("Disconnected");
            };

            SER.StreamlabsSocketEvent += (o, e) =>
            {
                Console.WriteLine(e.Data);
            };

            SER.Connect(token);

            Console.ReadKey();

        }
    }
}
