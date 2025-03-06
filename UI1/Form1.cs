using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BLL;
using System.IO;
using System.Threading.Tasks;
using Framework;
using UI1;

namespace UI1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FileBLL fb = new FileBLL();

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.SetOut(new ConsoleWriter(this)); 
            Dictionary<string, string> items = new Dictionary<string, string>
            {
                { "_168x", "shehuangtang" },
                { "JavBusAnalysis", "JavBus" },
                { "Sis001Analysis", "Sis001" },
                { "_52iv", "52iv" },
                { "JavDBAnalysis", "JavDB" },
                { "Hdd600", "Hdd600" },
                
            };
            comboBox1.DataSource = new BindingSource(items, null);
            comboBox1.DisplayMember = "Value";  
            comboBox1.ValueMember = "Key";      

        }
        
        string dataClicked="" ;
        bool flag = false;

        MegFinder megFinder;
        
        public void AppendLog(string message)
        {
            if (textBoxLog.InvokeRequired)
            {
                textBoxLog.Invoke(new Action(() => textBoxLog.AppendText(message + Environment.NewLine)));
            }
            else
            {
                textBoxLog.AppendText(message + Environment.NewLine);
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Config.isCheckSize = this.checkBox2.Checked;
        }

        private void DoConfig()
        {
            Config.timeSpan =Convert.ToInt32( textBox2.Text);
        }
        private void javPopCheck_CheckedChanged(object sender, EventArgs e)
        {
            Config.javPop= javPopCheck.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Config.ifCheckHisSize = checkBox1.Checked;
        }

        private void ifOnlyFindSmallerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.ifOnlyFindSmaller = ifOnlyFindSmallerCheckBox.Checked;
        }

        private void ifBtDig_CheckedChanged(object sender, EventArgs e)
        {
            Config.BtDig = ifBtDig.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Config.isCheck168xC = checkBox3.Checked;
        }

        private async void processButton_Click(object sender, EventArgs e)
        {
            try
            {
                DoConfig();
                processButton.Enabled = false;
                Type type = Type.GetType("BLL."+comboBox1.SelectedValue+", BLL");
                await Task.Run(() => fb.process(textBox1.Text.Replace("\\", "\\\\"),
                    (IAnalysis)Activator.CreateInstance(type), checkBox1.Checked));
            }            
            catch (Exception ex)
            {
                AppendLog("发生错误: " + ex.Message);
            }
            finally
            {
                processButton.Enabled = true;
            }
        }
    }
}

public class ConsoleWriter : TextWriter
{
    private readonly Form1 form;

    public ConsoleWriter(Form1 form)
    {
        this.form = form;
    }

    public override void WriteLine(string value)
    {
        form.AppendLog(value);
    }

    public override void Write(string value)
    {
        form.AppendLog(value);
    }

    public override Encoding Encoding => Encoding.UTF8;
}