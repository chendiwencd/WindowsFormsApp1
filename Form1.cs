using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace WindowsFormsApp1
{
    public partial class TP5510 : Form
    {
        static bool isApplicationResponsive = true;
        private DateTime _lastInteractionTime;

        public TP5510()
        {
            InitializeComponent();
            button4.Click += new EventHandler(button4_Click);
            SetControlsEnabled(false);

            SerialPortManager serialPortManager = SerialPortManager.Instance;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            this.menu2ToolStripMenuItem.Click += menu2ToolStripMenuItem_Click;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.listBox2.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBox2.DrawItem += listBox2_DrawItem;

            string[] ArryPort = SerialPort.GetPortNames();
            for (int i = 0; i < ArryPort.Length; i++)
            {
                comboBox5.Items.Add(ArryPort[i]);
                comboBox5.SelectedIndex = 0;
            }

            this.comboBox2.Items.AddRange(new string[] { "2.4kbps 传得更远", "19.2kbps 传得更稳定", "76.8kbps 传得更快" });
            this.comboBox13.Items.AddRange(new string[] { "9600", "1200", "2400", "4800", "19200", "38400", "57600", "115200" });
            this.comboBox15.Items.AddRange(new string[] { "None", "Even", "Odd" });
            this.comboBox16.Items.AddRange(new string[] { "1", "2" });
            this.comboBox3.Items.AddRange(new string[] { "9600", "1200", "2400", "4800", "19200", "38400", "57600", "115200" });
            this.comboBox4.Items.AddRange(new string[] { "None", "Even", "Odd" });
            this.comboBox6.Items.AddRange(new string[] { "1", "2" });
            this.comboBox5.Items.Add("串口未连接");
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox13.SelectedIndex = 0;
            comboBox15.SelectedIndex = 0;
            comboBox16.SelectedIndex = 0;
            get_Message();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            form3.FormClosed += (s, ev) =>
            {
                get_Message();
                button3_Click(sender, e);
                button3_Click(sender, e);
            };
        }



        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            listBox2.ItemHeight = 25;
            if (e.Index >= 0)
            {
                e.DrawBackground();
                string itemText = listBox2.Items[e.Index].ToString();

                using (StringFormat sf = new StringFormat())
                {
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;
                    e.Graphics.DrawString(itemText, e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                }

                e.DrawFocusRectangle();
            }
            if (e.Index >= 0)
            {
                Color backColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected ? Color.Gray : listBox2.BackColor;
                using (SolidBrush backBrush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(backBrush, e.Bounds);
                }

                string itemText = listBox2.Items[e.Index].ToString();

                using (StringFormat sf = new StringFormat())
                {
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center; 
                    using (SolidBrush textBrush = new SolidBrush((e.State & DrawItemState.Selected) == DrawItemState.Selected ? Color.Black : e.ForeColor))
                    {
                        e.Graphics.DrawString(itemText, e.Font, textBrush, e.Bounds, sf);
                    }
                }
                e.DrawFocusRectangle();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void menu2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Show();
        }

        private void SetControlsEnabled(bool enabled)
        {
            groupBox1.Enabled = enabled;
            groupBox2.Enabled = enabled;
            groupBox3.Enabled = enabled;
            button5.Enabled = enabled;
            button8.Enabled = enabled;
        }

        public static class SharedForms
        {
            public static Form4 Form4Instance { get; set; }
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
                    // 如果窗口是可见的，隐藏它
                    SharedForms.Form4Instance.Hide();
                }
                else
                {
                    // 如果窗口已经最小化，将其恢复
                    if (SharedForms.Form4Instance.WindowState == FormWindowState.Minimized)
                    {
                        SharedForms.Form4Instance.WindowState = FormWindowState.Normal;
                    }
                    // 如果窗口是隐藏的，显示它并将其带到前台
                    SharedForms.Form4Instance.Show();
                    SharedForms.Form4Instance.BringToFront();
                }
            }

        }

        private void button8_Click(object sender, EventArgs e)
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
            button5_Click(sender, e);
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void menu2ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.FormClosed += Form2_FormClosed;
            form2.Show();
            if (button4.Text == "关闭串口")
            {
                button4.Text = "打开串口";
                comboBox5.BackColor = ColorTranslator.FromHtml("#ed6d58");
                SerialPortManager.Instance.ClosePort();
            }
            this.Hide();
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            string comm1 = "AT+FREQ=" + comboBox1.SelectedIndex;
            string comm3 = "AT+SYMBOL=03,"+(comboBox2.SelectedIndex == 0 ? "00" : comboBox2.SelectedIndex == 1 ? "03" : comboBox2.SelectedIndex == 2 ? "05" : "default_value");
            string line1 = null;
            string line2 = null;
            string line3 = null;
            SerialPortManager.Instance.SendCommands(comm1);
            if (SerialPortManager.Instance.IsPortOpen())
            {
                line1 = SerialPortManager.Instance.ReadLineFromPort();
                Console.WriteLine(line1);
            }

            System.Threading.Thread.Sleep(200);
            SerialPortManager.Instance.SendCommands(comm3);
            if (SerialPortManager.Instance.IsPortOpen())
            {
                line2 = SerialPortManager.Instance.ReadLineFromPort();
                Console.WriteLine(line2);
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

        private void button5_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            button9_Click(sender, e);
            button10_Click(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
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
            // 添加combobox6的项
            comboBoxItems.Add(comboBox13.SelectedItem.ToString());
            comboBoxItems.Add("8");

            // 添加combobox7的项
            comboBoxItems.Add(comboBox15.SelectedItem.ToString() == "None" ? "N" :
                comboBox15.SelectedItem.ToString() == "Even" ? "E" : comboBox15.SelectedItem.ToString() == "Odd" ? "O" : comboBox15.SelectedItem.ToString());

            // 添加combobox9的项
            comboBoxItems.Add(comboBox16.SelectedItem.ToString());
            string outputString = "AT+UART=" + string.Join(",", comboBoxItems);
            SerialPortManager.Instance.SendCommands(outputString);
            string line1 = null;
            if (SerialPortManager.Instance.IsPortOpen())
            {
                line1 = SerialPortManager.Instance.ReadLineFromPort();
                Console.WriteLine(line1);
            }
            if (line1 != null && (line1.Contains("OK")))
            {
                button10.Text = "配置成功";
                Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                {
                    button10.Text = "配置";
                });
            }
            get_Message();
        }
        

        private void button4_Click_1(object sender, EventArgs e)
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
            button3_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                // 从 listbox2 中获取被选中的项
                string selectedValue = listBox2.SelectedItem.ToString();
                string modifiedString = "AT+DEV=1," + selectedValue + ",3";
                SerialPortManager.Instance.SendCommands(modifiedString);
            }
            get_Message();
            button3_Click(sender, e);
            button3_Click(sender, e);
        }



        private void button3_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear(); // 清空 listBox2
            SerialPortManager.Instance.SendCommands("AT+DEV?");
            if (SerialPortManager.Instance.IsPortOpen())
            {
                List<string> lines = SerialPortManager.Instance.ReadLinesFromPort();

                foreach (string line in lines)
                {
                    if (line.Contains("OK"))
                    {
                        continue;
                    }
                    int firstCommaIndex = line.IndexOf(',');
                    string modifiedLine = firstCommaIndex >= 0 ? line.Substring(0, firstCommaIndex) : line;
                    listBox2.Items.Add(modifiedLine);
                }
            }
            SerialPortManager.Instance.SendCommands("AT+EUI?");
            label9.Text = SerialPortManager.Instance.ReadLineFromPort();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SerialPortManager.Instance.SendCommands("AT+FREQ?");
            if (SerialPortManager.Instance.IsPortOpen())
            {
                List<string> lines = SerialPortManager.Instance.ReadLinesFromPort();
                foreach (string line in lines)
                {
                    string pattern = @"DL:(.*)0MHz"; // 正则表达式模式
                    Match match = Regex.Match(line, pattern);
                    if (match.Success)
                    {
                        string extractedData = match.Groups[1].Value;
                        // 查找匹配的项
                        int index = comboBox1.FindString(extractedData);
                        // 如果找到匹配项，选择它
                        if (index != -1)
                        {
                            comboBox1.SelectedIndex = index;
                        }
                        
                    }
                }
            }

            System.Threading.Thread.Sleep(200);
            SerialPortManager.Instance.SendCommands("AT+SYMBOL?");
            if (SerialPortManager.Instance.IsPortOpen())
            {
                string line1 = SerialPortManager.Instance.ReadLineFromPort();
                string line2 = SerialPortManager.Instance.ReadLineFromPort();
                string line3 = SerialPortManager.Instance.ReadLineFromPort();
                string pattern = @"UL:(.*) kbps"; // 正则表达式模式
                Match match = Regex.Match(line2, pattern);

                if (match.Success)
                {
                    string extractedData = match.Groups[1].Value;
                    var matchingItems = comboBox2.Items.Cast<string>() .Where(item => item.Contains(extractedData)).ToList();
                    if (matchingItems.Count > 0)
                    {
                        comboBox2.SelectedIndex = comboBox2.Items.IndexOf(matchingItems.First());
                    }
                    if (extractedData != null)
                    {
                        button7.Text = "查询成功";
                        Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                        {
                            button7.Text = "查询";
                        });
                    }
                }
            }
            get_Message();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
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

        private void button13_Click(object sender, EventArgs e)
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
                    button13.Text = "查询成功";
                    Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                    {
                        button13.Text = "查询";
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
                    button13.Text = "查询成功";
                    Task.Delay(1000).GetAwaiter().OnCompleted(() =>
                    {
                        button13.Text = "查询";
                    });
                }
                get_Message();
            }

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

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox6_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void label2_Click_2(object sender, EventArgs e)
        {

        }
    }
}
