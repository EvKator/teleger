using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teleger
{
    public class Log
    {
        System.Windows.Forms.RichTextBox rtb;
        public Log(ref System.Windows.Forms.RichTextBox rtb)
        {
            this.rtb = rtb;
        }
        public void Wrt(string logStr, bool printDetailedLog = false, string time = null)
        {
            logStr = "\r\n" + DateTime.Now.ToLongTimeString() + " | " + logStr + "\r\n";

            /*if (printDetailedLog)
                logStr = logStr + matrix.LastSortingLog + "\r\n\r\nResult:\r\n" + matrix.ToString() + "\r\n";
            else
                logStr = logStr + matrix.ToString() + "\r\n";*/
            //if (time != null) logStr += "\nTime elapsed: " + time + "\n";
            //logStr += "\r\n-------------------------------------\r\n";
            rtb.AppendText(logStr);
        }
    }
}
