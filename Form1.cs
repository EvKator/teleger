using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Teleger
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Manager mngr;
        public string NumsFileName { get; private set; }
        public string ScriptsFileName { get; private set; }
        

        private async void buttonLoadScript_Click(object sender, EventArgs e)
        {
            Log lg = new Log(ref richTextBoxLog);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JSON files|*.json";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Spammer spammer = await Spammer.LoadFromFile(ofd.FileName, this.NumsFileName, lg);
                await spammer.Start();
            }
        }

        private void buttonLoadNums_Click(object sender, EventArgs e)
        {
            buttonLoadScript.Enabled = true;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TXT files|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                NumsFileName = ofd.FileName;
            }
        }
    }
}
