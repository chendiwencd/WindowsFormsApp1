using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static WindowsFormsApp1.TP5510;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public SerialPortManager serialPortManager;
        private bool isButton4ClickRegistered = false;
        public Form2()
        {
            InitializeComponent();
            get_Message();
            if (!isButton4ClickRegistered)  
            {
                button4.Click += new EventHandler(button4_Click);
                isButton4ClickRegistered = true; 
            }
            button4.Click += new EventHandler(button4_Click);
            SetControlsEnabled(false);
            SerialPortManager serialPortManager = SerialPortManager.Instance;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            string[] ArryPort = SerialPort.GetPortNames();
            for (int i = 0; i < ArryPort.Length; i++)
            {
                comboBox5.Items.Add(ArryPort[i]);
                comboBox5.SelectedIndex = 0;
            }

            this.comboBox13.Items.AddRange(new string[] {"9600", "1200", "2400", "4800","19200", "38400", "57600", "115200"});
            this.comboBox15.Items.AddRange(new string[] { "None", "Even", "Odd" });
            this.comboBox16.Items.AddRange(new string[] { "1", "2" });
            this.comboBox3.Items.AddRange(new string[] { "9600", "1200", "2400", "4800", "19200", "38400", "57600", "115200" });
            this.comboBox4.Items.AddRange(new string[] { "None", "Even", "Odd" });
            this.comboBox6.Items.AddRange(new string[] { "1", "2" });
            this.comboBox5.Items.AddRange(new string[] { "串口未连接" });
            comboBox5.SelectedIndex = 0;
            comboBox1.SelectedIndex = 0;
            comboBox13.SelectedIndex = 0;
            comboBox15.SelectedIndex = 0;
            comboBox16.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
        }

        private void SetControlsEnabled(bool enabled)
        {
            groupBox2.Enabled = enabled;
            groupBox3.Enabled = enabled;
            button5.Enabled = enabled;
            button7.Enabled = enabled;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void menu2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        
        private void button6_Click(object sender, EventArgs e)
        {
            if (SharedForms.Form4Instance == null || SharedForms.Form4Instance.IsDisposed)
            {
                SharedForms.Form4Instance = new Form4();
                SharedForms.Form4Instance.Show();
            }
            else
            {
                if (SharedForms.Form4Instance.Visible)
                {
                    SharedForms.Form4Instance.Hide();
                }
                else
                {
                    if (SharedForms.Form4Instance.WindowState == FormWindowState.Minimized)
                    {
                        SharedForms.Form4Instance.WindowState = FormWindowState.Normal;
                    }
                    SharedForms.Form4Instance.Show();
                    SharedForms.Form4Instance.BringToFront();
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            List<string> ports = new List<string>();
            // 添加combobox5的项
            ports.Add(comboBox5.SelectedItem.ToString());

            // 添加combobox13的项
            ports.Add(comboBox3.SelectedItem.ToString());
            ports.Add("8");
            // 添加combobox15的项
            ports.Add(comboBox4.SelectedItem.ToString());

            // 添加combobox16的项
            ports.Add(comboBox6.SelectedItem.ToString() == "1" ? "One" : comboBox16.SelectedItem.ToString() == "2" ? "Two" : "Default");
            if (button4.Text == "打开串口")
            {
                button4.Text = "关闭串口";
                SetControlsEnabled(true);
                comboBox5.BackColor = ColorTranslator.FromHtml("#8d9e59");
                SerialPortManager.Instance.OpenPort(ports);
                SerialPortManager.Instance.SendCommands("AT+EUI?");
                label9.Text = SerialPortManager.Instance.ReadLineFromPort();
            }
            else if (button4.Text == "关闭串口")
            {
                button4.Text = "打开串口";
                SetControlsEnabled(false);
                comboBox5.BackColor = ColorTranslator.FromHtml("#ed6d58");
                SerialPortManager.Instance.ClosePort();

            }
            get_Message();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> comboBoxItems = new List<string>();
            if (radioButton1.Checked)
            {
                comboBoxItems.Add("3");
            }
            else
            {
                comboBoxItems.Add("0");
            }
            comboBoxItems.Add(comboBox13.SelectedItem.ToString());
            comboBoxItems.Add("8");
            comboBoxItems.Add(comboBox15.SelectedItem.ToString() == "None" ? "N" :
            comboBox15.SelectedItem.ToString() == "Even" ? "E" : comboBox15.SelectedItem.ToString() == "Odd" ? "O" : comboBox15.SelectedItem.ToString());
            comboBoxItems.Add(comboBox16.SelectedItem.ToString());
            string outputString = "AT+UART=" + string.Join(",", comboBoxItems);
            SerialPortManager.Instance.SendCommands(outputString);
            string line1 = null;
            if (SerialPortManager.Instance.IsPortOpen())
            {
                line1 = SerialPortManager.Instance.ReadLineFromPort();
            }
            if (line1 != null && (line1.Contains("OK")))
            {
                button1.Text = "配置成功";
                button4.PerformClick();
                button4.PerformClick();
                Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                {
                    button1.Text = "配置";
                });
            }
            get_Message();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                string comboBoxName = "comboBox" + (i + 1); // 构建comboBox的名称
                ComboBox comboBox = this.Controls.Find(comboBoxName, true).FirstOrDefault() as ComboBox; // 查找comboBox控件

                if (comboBox != null)
                {
                    comboBox.SelectedIndex = 0; // 将SelectedIndex重置为0
                }
            }
            button7_Click(sender, e);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int selectedIndex = comboBox1.SelectedIndex;
            int cycle = selectedIndex / 50;  
            int offsetInCycle = selectedIndex % 50;  
            int parameterValue = cycle * 50 + (offsetInCycle < 44 ? (offsetInCycle / 4) * 4 : 44);
            string line1 = null;
            string line2 = null;
            SerialPortManager.Instance.SendCommands("AT+FREQ=DL,"+parameterValue);
            if (SerialPortManager.Instance.IsPortOpen())
            {
                line1 = SerialPortManager.Instance.ReadLineFromPort();
            }
            if (SerialPortManager.Instance.IsPortOpen())
            {
                line2 = SerialPortManager.Instance.ReadLineFromPort();
            }
            if (line1 != null && line2 != null && (line1.Contains("OK") || line2.Contains("OK")))
            {
                button9.Text = "配置成功";

                Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                {
                    button9.Text = "配置";
                });
            }
            get_Message();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button9.PerformClick();
            button1.PerformClick();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SerialPortManager.Instance.SendCommands("AT+FREQ?");
            if (SerialPortManager.Instance.IsPortOpen())
            {
                List<string> lines = SerialPortManager.Instance.ReadLinesFromPort();
                foreach (string line in lines)
                {
                    string pattern = @"UL:(.*)0MHz";
                    Match match = Regex.Match(line, pattern);
                    if (match.Success)
                    {
                        string extractedData = match.Groups[1].Value;
                        int index = comboBox1.FindString(extractedData);
                        if (index != -1)
                        {
                            comboBox1.SelectedIndex = index;
                        }
                        if (extractedData != null)
                        {
                            button2.Text = "查询成功";
                            Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                            {
                                button2.Text = "查询";
                            });
                        }
                    }
                }
            }
            get_Message();
         }

        private void get_Message()
        {
            int status_code = SerialPortManager.Instance.GetLastStatusCode();
            if (status_code == 0)
            {
                label2.Text = "串口未连接，请连接串口";
            }
            else if (status_code == 1)
            {
                label2.Text = "操作成功";
                Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                {
                    label2.Text = "";
                });
            }
            else if (status_code == 2)
            {
                label2.Text = "返回超时";
            }
        }

            private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] ArryPort = SerialPort.GetPortNames();
            comboBox5.Items.Clear(); // 清除之前的串口列表
            foreach (string portName in ArryPort)
            {
                comboBox5.Items.Add(portName);
            }

            if (ArryPort.Length > 0)
            {
                comboBox5.SelectedIndex = 0; // 设置选定项，如果有可用串口
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SerialPortManager.Instance.SendCommands("AT+UART?");
            if (SerialPortManager.Instance.IsPortOpen())
            {
                string line1 = SerialPortManager.Instance.ReadLineFromPort();
                string line2 = SerialPortManager.Instance.ReadLineFromPort();
                string line3 = SerialPortManager.Instance.ReadLineFromPort();
                string line4 = SerialPortManager.Instance.ReadLineFromPort();
                List<string> lines = new List<string> { line1, line2, line3, line4 };
                Console.WriteLine(lines);
                List<string> dataList1 = new List<string>();
                List<string> dataList2 = new List<string>();
                string pattern = @"RS(\d+): baud: (.*?),\s*dataBit: (\d+),\s*check: (.*?),\s*stop: (\d+)";
                Regex regex = new Regex(pattern);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        if (match.Groups[1].Value == "232") // 检查RS的数字部分是否为232
                        {
                            for (int i = 2; i <= 5; i++) // 添加匹配的组2到组5的值
                            {
                                dataList1.Add(match.Groups[i].Value);
                                Console.WriteLine(match.Groups[i].Value);
                            }
                        }
                        if (match.Groups[1].Value == "485") // 检查RS的数字部分是否为232
                        {
                            for (int i = 2; i <= 5; i++) // 添加匹配的组2到组5的值
                            {
                                dataList2.Add(match.Groups[i].Value);
                                Console.WriteLine(match.Groups[i].Value);
                            }
                        }
                    }
                }
                if (radioButton2.Checked && dataList1.Count >= 4)
                {
                    // 从 dataList1 获取数据
                    string data1 = dataList1[0].Replace(" ", "");
                    string data3 = dataList1[1];
                    string data4 = dataList1[3];

                    // 找到并选择 comboBox13 中的匹配项
                    int index13 = comboBox13.FindString(data1);
                    if (index13 != -1)
                    {
                        comboBox13.SelectedIndex = index13;
                    }

                    // 找到并选择 comboBox15 中的匹配项
                    int index15 = comboBox15.FindString(data3);
                    if (index15 != -1)
                    {
                        comboBox15.SelectedIndex = index15;
                    }

                    // 找到并选择 comboBox16 中的匹配项
                    int index16 = comboBox16.FindString(data4);
                    if (index16 != -1)
                    {
                        comboBox16.SelectedIndex = index16;
                    }
                    button8.Text = "查询成功";
                    Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                    {
                        button8.Text = "查询";
                    });
                }
                if (radioButton1.Checked && dataList1.Count >= 4)
                {
                    string data1 = dataList2[0].Replace(" ", "");
                    string data3 = dataList2[1];
                    string data4 = dataList2[3];
                    int index13 = comboBox13.FindString(data1);
                    if (index13 != -1)
                    {
                        comboBox13.SelectedIndex = index13;
                    }
                    int index15 = comboBox15.FindString(data3);
                    if (index15 != -1)
                    {
                        comboBox15.SelectedIndex = index15;
                    }
                    int index16 = comboBox16.FindString(data4);
                    if (index16 != -1)
                    {
                        comboBox16.SelectedIndex = index16;
                    }
                    button8.Text = "查询成功";
                    Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                    {
                        button8.Text = "查询";
                    });
                }
                get_Message();
            }
        }
        
        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            TP5510 form1 = new TP5510();
            form1.FormClosed += Form1_FormClosed;
            form1.Show();
            this.Hide();
            if (button4.Text == "关闭串口")
            {
                button4.Text = "打开串口";
                comboBox5.BackColor = ColorTranslator.FromHtml("#ed6d58");
                SerialPortManager.Instance.ClosePort();
            }
        }
    }
}
