using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Teleger
{
    public partial class FormTeleCode : Form
    {
        public FormTeleCode()
        {
            InitializeComponent();
        }
        public string Question
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }
        public string Code { get; private set; }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if(Regex.IsMatch(textBox1.Text, @"\d{5}")){
                Code = textBox1.Text;
                buttonOK.Enabled = true;
            }
        }
    }
}
