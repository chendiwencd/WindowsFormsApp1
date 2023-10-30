using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static WindowsFormsApp1.TP5510;


namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        public SerialPortManager serialPortManager;

        public Form4()
        {
            InitializeComponent();
            SerialPortManager serialPortManager = SerialPortManager.Instance;
            this.AcceptButton = button1;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            listBox1.KeyDown += ListBox1_KeyDown;
            listBox2.KeyDown += ListBox1_KeyDown2;
        }

        private void ListBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyToClipboard();
            }
        }

        private void ListBox1_KeyDown2(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyToClipboard2();
            }
        }

        private void CopyToClipboard()
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                return;
            }
            var selectedItems = listBox1.SelectedItems.Cast<string>().Where(s => s.Length > 7).Select(s => s.Substring(7));
            var clipboardText = string.Join(Environment.NewLine, selectedItems);

            Clipboard.SetText(clipboardText);
        }

        private void CopyToClipboard2()
        {
            if (listBox1.SelectedItems.Count == 0)
            {
                return;
            }
            var selectedItems = listBox2.SelectedItems.Cast<string>().Where(s => s.Length > 7).Select(s => s.Substring(7));
            var clipboardText = string.Join(Environment.NewLine, selectedItems);

            Clipboard.SetText(clipboardText);
        }

        public void AddItemsToListBox(List<string> items)
        {
            string timestamp = DateTime.Now.ToString("mm:ss");
            foreach (string item in items)
            {
                string itemWithTimestamp = $"[{timestamp}]{item}";

                // 220 像素长度的限制
                int maxPixelWidth = 300;

                // 使用 TextRenderer.MeasureText 方法来测量文本的像素宽度
                int textWidth = TextRenderer.MeasureText(itemWithTimestamp, listBox1.Font).Width;

                if (textWidth <= maxPixelWidth)
                {
                    listBox1.Items.Add(itemWithTimestamp);
                }
                else
                {
                    // 分割文本以适应 220 像素限制
                    string[] splitText = SplitTextByWidth(itemWithTimestamp, maxPixelWidth);

                    // 添加分割后的文本到 ListBox
                    listBox1.Items.AddRange(splitText);
                }
            }

            // 滚动到列表的底部
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }

        public void AddReceivedDataToListBox2(string data)
        {
            // 获取当前时间的时分部分
            string timestamp = DateTime.Now.ToString("mm:ss");
            string receivedData = $"[{timestamp}]{data}";

            // 300 像素长度的限制
            int maxPixelWidth = 300;

            // 使用 TextRenderer.MeasureText 方法来测量文本的像素宽度
            int textWidth = TextRenderer.MeasureText(receivedData, listBox2.Font).Width;

            if (this.listBox2.InvokeRequired)
            {
                this.listBox2.Invoke(new Action<string>(AddReceivedDataToListBox2), data);
            }
            else
            {
                if (textWidth <= maxPixelWidth)
                {
                    listBox2.Items.Add(receivedData);
                }
                else
                {
                    // 分割文本以适应 300 像素限制
                    string[] splitText = SplitTextByWidth(receivedData, maxPixelWidth);

                    // 添加分割后的文本到 ListBox
                    listBox2.Items.AddRange(splitText);
                }
                if (listBox2.Items.Count > 0)
                {
                    listBox2.TopIndex = listBox2.Items.Count - 1;
                }
            }
        }

        private string[] SplitTextByWidth(string text, int maxWidth)
        {
            List<string> splitText = new List<string>();
            string currentLine = string.Empty;

            foreach (char c in text)
            {
                int currentLineWidth = TextRenderer.MeasureText(currentLine, listBox1.Font).Width;

                if (currentLineWidth + TextRenderer.MeasureText(c.ToString(), listBox1.Font).Width <= maxWidth)
                {
                    currentLine += c;
                }
                else
                {
                    splitText.Add(currentLine);
                    currentLine = c.ToString();
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                splitText.Add(currentLine);
            }

            return splitText.ToArray();
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string textBoxContent = textBox1.Text;
            // 检查RadioButton是否被选中
            if (radioButton3.Checked)
            {
                // 尝试将文本框内容转换为16进制
                if (!string.IsNullOrEmpty(textBoxContent))
                {
                    // 尝试将文本框内容转换为16进制（如果是数字）
                    if (int.TryParse(textBoxContent, out int decimalValue))
                    {
                        textBoxContent = decimalValue.ToString("X");
                    }
                    else
                    {
                        // 如果不是数字，将字符转换为ASCII值
                        char[] charArray = textBoxContent.ToCharArray();
                        string asciiString = "";
                        foreach (char c in charArray)
                        {
                            int asciiValue = (int)c;
                            asciiString += asciiValue.ToString("X");
                        }
                        textBoxContent = asciiString;
                    }
                }
            }
            // 创建一个 List<string> 来存储内容
            List<string> BoxItems = new List<string>();

            // 根据RadioButton的状态添加textBoxContent到BoxItems
            if (radioButton3.Checked || radioButton4.Checked)
            {
                if (!string.IsNullOrEmpty(textBoxContent))
                {
                    BoxItems.Add(textBoxContent);
                    SerialPortManager.Instance.SendCommands(textBoxContent);
                }
            }

            if (SharedForms.Form4Instance != null && !SharedForms.Form4Instance.IsDisposed)
            {
                SharedForms.Form4Instance.AddItemsToListBox(BoxItems);
                textBox1.Clear();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
