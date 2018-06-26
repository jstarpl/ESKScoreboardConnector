using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Net;
using Rug.Osc;

namespace ESKScoreboardConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 6250;

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: " + typeof(Program).Assembly.GetName().Name + " <COM Port> <Target IP Address>");
                return;
            }

            IPAddress targetIp = null;
            try
            {
                targetIp = IPAddress.Parse(args[1]);
            } catch (FormatException fe)
            {
                Console.WriteLine("Invalid target IP address: " + args[1]);
                Environment.Exit(2);
            }

            if (args.Length > 2)
            {
                try
                {
                    port = int.Parse(args[2]);
                } catch (FormatException fe2)
                {
                    Console.WriteLine("Invalid port number: " + args[2]);
                    Environment.Exit(3);
                }
            }

            OscSender oscSender = new OscSender(targetIp, port);
            oscSender.Connect();
            SerialPort sp = new SerialPort(args[0], 19200, Parity.None, 8, StopBits.One);
            sp.NewLine = "\r";
            sp.Encoding = Encoding.ASCII;
            sp.Open();

            Console.WriteLine("Reading from " + sp.PortName);
            Console.WriteLine("Sending to " + targetIp.ToString() + ":" + port.ToString());
            Console.WriteLine();

            while (true)
            {
                string dataLine = sp.ReadLine();
                string clockMinutes = dataLine.Substring(4, 2).Trim();
                string clockSeconds = dataLine.Substring(6, 2).Trim();
                string scoreA = dataLine.Substring(8, 3).Trim();
                string scoreB = dataLine.Substring(11, 3).Trim();
                string gamePart = dataLine.Substring(14, 1).Trim();

                oscSender.Send(new OscMessage("/time", clockMinutes, clockSeconds));
                oscSender.Send(new OscMessage("/score", scoreA, scoreB));
                oscSender.Send(new OscMessage("/gamePart", gamePart));

                Console.WriteLine(clockMinutes + ":" + clockSeconds + ", Part " + gamePart + ", Score is: " + scoreA + " - " + scoreB);
            }
        }
    }
}
