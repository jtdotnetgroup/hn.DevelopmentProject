using System.IO;
using System.Text;
using System.Windows.Forms;

namespace hn.Common
{
    public class TextBoxWriter:TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        private TextBox textBox;
        delegate void WriteFunc(string value);
        WriteFunc write;
        WriteFunc writeLine;

        public TextBoxWriter(TextBox text)
        {
            this.textBox = text;
            write = Write;
            writeLine = WriteLine;
        }

        public override void Write(string value)
        {
            if (textBox.InvokeRequired)
                textBox.BeginInvoke(write, value);
            else
                textBox.AppendText(value);
        }

        public override void WriteLine(string value)
        {
            if (textBox.InvokeRequired)
                textBox.BeginInvoke(writeLine, value);
            else
            {
                textBox.AppendText(value);
                textBox.AppendText(this.NewLine);
            }
           
        }
    }
}