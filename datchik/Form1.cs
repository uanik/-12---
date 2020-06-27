using System;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace datchik
{
    public partial class Form1 : Form
    {
        FileInfo aFile = new FileInfo("WEIGHT.TXT");
        SerialPort SerialPort1 = new SerialPort("COM4");
        SerialPort SerialPort2 = new SerialPort("COM3");
        public Form1()
        {
            InitializeComponent();
            DateTime sysDate = DateTime.Today;
            SerialPort1.BaudRate = 19200; //для vkmodule       
            SerialPort1.Parity = Parity.None;
            SerialPort1.StopBits = StopBits.One;
            SerialPort1.DataBits = 8;
            SerialPort1.Open();
            SerialPort2.BaudRate = 9600; //для A12E  
            SerialPort2.Parity = Parity.None;
            SerialPort2.StopBits = StopBits.One;
            SerialPort2.DataBits = 8;
            SerialPort2.Open();

            if (WindowState == FormWindowState.Minimized) // для трея
            { Hide(); }
            if (aFile.Exists == false) { aFile.Create(); }
            aFile.Attributes = FileAttributes.Normal;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            byte[] vals = { 0x00, 0x02, 0x00, 0x00, 0x00, 0x08, 0x78, 0x1D };   //для vkmodule
            if (SerialPort1.IsOpen) SerialPort1.Write(vals, 0, 8);
          //  SerialPort1.DiscardInBuffer();
            System.Threading.Thread.Sleep(50);
            int byteRecieved = SerialPort1.BytesToRead;
            byte[] messByte = new byte[byteRecieved];
            SerialPort1.Read(messByte, 0, byteRecieved);
            int input;
            input = 64;
            if (byteRecieved == 6)
                input = messByte[3];
            string stroka = input.ToString();
            

            label1.Text = stroka;
            //------------- чтение данных из весопроцессора
            if (SerialPort2.IsOpen) { }
            SerialPort2.DiscardInBuffer();
            System.Threading.Thread.Sleep(150);
            string input1 = SerialPort2.ReadExisting();
         //   label2.Text = input1;
            int idx;
                              //-------- парсер
                idx = input1.IndexOf("WN");

                if (idx > -1)
                {
                    input1 = input1.Substring(idx + 2, 7);
                                    }
                idx = input1.IndexOf("kg");
                if (idx == 5)
                {
                    input1 = input1.Substring(0, 5);
                    string ves = input1;
                }
            stroka = stroka + "   " + input1;
            label2.Text = input1;
            System.IO.File.WriteAllText(@"WEIGHT.TXT", stroka);
        }             
      
        }

    }
