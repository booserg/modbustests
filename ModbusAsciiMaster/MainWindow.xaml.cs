using Modbus.Device;
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

namespace ModbusAsciiMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IModbusSerialMaster master;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var masterPort = new SerialPort("COM4");
            masterPort.BaudRate = 9600;
            masterPort.DataBits = 8;
            masterPort.Parity = Parity.None;
            masterPort.StopBits = StopBits.One;
            masterPort.ReadTimeout = 100;
            masterPort.Open();

            master = ModbusSerialMaster.CreateAscii(masterPort);
            master.Transport.ReadTimeout = 1000;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            master.Dispose();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var res = await GetData().ConfigureAwait(true);

            tbData.Text = res[0].ToString();
        }

        private async Task<ushort[]> GetData()
        {
            var res = await Task<ushort[]>.Factory.StartNew(() => {
                byte slaveId = 2;

                ushort[] registers = master.ReadHoldingRegisters(slaveId, 2, 1);

                return registers;
            });
            return res;
        }
    }
}
