using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Net;

using System.Net.Sockets;

using System.Threading;

using Modbus.Data;

using Modbus.Device;
using Modbus.IO;

using Modbus.Utility;

namespace ModbusAsciiSlave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ModbusSlave slave;
        SerialPort slavePort;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = Task.Factory.StartNew(() =>
            {
                slavePort = new SerialPort("COM3");

                slavePort.BaudRate = 9600;

                slavePort.DataBits = 8;

                slavePort.Parity = Parity.None;

                slavePort.StopBits = StopBits.One;

                slavePort.Open();

                byte unitId = 2;

                slave = ModbusSerialSlave.CreateAscii(unitId, slavePort);

                slave.DataStore = DataStoreFactory.CreateDefaultDataStore();
                slavePort.DiscardInBuffer();
                slavePort.DiscardOutBuffer();
                slave.Listen();
            });
        }
        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            slavePort.Close();
            slave.Dispose();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (slave != null && slave.DataStore != null)
            {
                int n;
                if (int.TryParse(tbData.Text, out n))
                {
                    lock (slave.DataStore.SyncRoot)
                    {
                        slave.DataStore.HoldingRegisters[3] = (ushort)n;
                    }
                }
            }
        }
    }
}
