using Microsoft.VisualBasic.Devices;

namespace My
{
    internal static class Computer
    {
        internal readonly static Audio Audio;
        internal readonly static Clock Clock;
        internal readonly static ComputerInfo Info;
        internal readonly static Keyboard Keyboard;
        internal readonly static Mouse Mouse;
        internal readonly static string Name;
        internal readonly static Network Network;
        internal readonly static Ports Ports;

        static Computer()
        {
            Audio = new Audio();
            Clock = new Clock();
            Info = new ComputerInfo();
            Keyboard = new Keyboard();
            Mouse = new Mouse();
            Network = new Network();
            Ports = new Ports();

            ServerComputer ThisServerComputer = new ServerComputer();
            Name = ThisServerComputer.Name;
        }
    }
}