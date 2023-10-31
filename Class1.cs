using System.Collections.Generic;
using System.IO.Ports;
using System;
using System.Linq;
using static WindowsFormsApp1.TP5510;

public class SerialPortManager
{
    private static SerialPortManager instance;
    public static int ReadTimeout { get; set; } = 200;

    private SerialPortManager()
    {
        
    }

    public static SerialPortManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SerialPortManager();
            }
            return instance;
        }
    }

    public void SetReadTimeout(int timeout)
    {
        ReadTimeout = timeout;
    }

    public SerialPort serialPort;
    public List<string> ComboBoxItems { get; private set; }
    public List<string> ReceivedData { get; private set; }

    public void OpenPort(List<string> serialSettings)
    {
        // 串口未打开，可以进行打开操作
        string portName = serialSettings[0];
        int baudRate = Convert.ToInt32(serialSettings[1]);
        int dataBits = Convert.ToInt32(serialSettings[2]);
        StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), serialSettings[4]);
        Parity parity = (Parity)Enum.Parse(typeof(Parity), serialSettings[3]);
        string[] availablePorts = SerialPort.GetPortNames();
        serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

        ComboBoxItems = new List<string>();
        ComboBoxItems.Add("串口打开");
        string ser;
        try
        {
            serialPort.Open();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }
        catch (Exception ex)
        {
            ser = "串口失败成功" + ex.Message;
        }
        if (SharedForms.Form4Instance != null && !SharedForms.Form4Instance.IsDisposed)
        {
            SharedForms.Form4Instance.AddItemsToListBox(ComboBoxItems);
        }
    }

    public void ClosePort()
    {
        ComboBoxItems = new List<string>();
        ComboBoxItems.Add("关闭串口");
        string clo;
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            clo = "串口已关闭";
        }
        else
        {
            clo = "串口已经关闭";
        }
        if (SharedForms.Form4Instance != null && !SharedForms.Form4Instance.IsDisposed)
        {
            SharedForms.Form4Instance.AddItemsToListBox(ComboBoxItems);
            SharedForms.Form4Instance.AddReceivedDataToListBox2(clo);
        }
    }

    public void SendCommands(string command)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Write(command + "\r\n");
            //Console.WriteLine("Command Sent:" + command);
        }
        else
        {
            if (SharedForms.Form4Instance != null && !SharedForms.Form4Instance.IsDisposed)
            {
                SharedForms.Form4Instance.AddReceivedDataToListBox2("串口未打开");
            }
        }
    }

    private static void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadLine();
        if (SharedForms.Form4Instance != null && !SharedForms.Form4Instance.IsDisposed)
        {
            SharedForms.Form4Instance.AddReceivedDataToListBox2(indata);
        }
    }

    // 状态码
    private int lastStatusCode = 0;

    public string ReadLineFromPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            string line = null;
            try
            {
                serialPort.DataReceived -= DataReceivedHandler;
                serialPort.ReadTimeout = ReadTimeout;
                line = serialPort.ReadLine();
                //Console.WriteLine(line);
                serialPort.DataReceived += DataReceivedHandler;
                lastStatusCode = 1;
                return line;
            }
            catch (TimeoutException)
            {
                if (line == null)
                {
                    serialPort.DataReceived += DataReceivedHandler;
                    lastStatusCode = 2;
                    return "TIME OUT";
                }
                else
                {
                    serialPort.DataReceived += DataReceivedHandler;
                    lastStatusCode = 1;
                    return line;
                }
            }
        }
        else
        {
            lastStatusCode = 0;
            return "Serialport not connected";
        }
    }

    public int GetLastStatusCode()
    {
        return lastStatusCode;
    }

    public bool IsPortOpen()//端口情况
    {
        return serialPort != null && serialPort.IsOpen;
    }

    public List<string> ReadLinesFromPort()
    {
        serialPort.DataReceived -= DataReceivedHandler;
        List<string> lines = new List<string>();
        DateTime startTime = DateTime.Now;
        while ((DateTime.Now - startTime).TotalMilliseconds < 500)
        {
            if (serialPort.BytesToRead > 0)
            {
                string line = serialPort.ReadLine();
                lines.Add(line);
            }
        }
        lastStatusCode = 1;
        serialPort.DataReceived += DataReceivedHandler;
        return lines;
    }
}
