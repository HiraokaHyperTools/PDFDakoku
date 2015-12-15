using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFDakoku {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            var rows = tb.Text.Replace("\r\n", "\n").Split('\n');

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.pdf|*.pdf";
            sfd.FileName = rows[0] + ".pdf";
            if (sfd.ShowDialog(this) != System.Windows.Forms.DialogResult.OK) return;

            iTextSharp.text.io.StreamUtil.AddToResourceSearch(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iTextAsian.dll"));

            FontFactory.Register(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "msgothic.ttc"));
            //FontFactory.Register(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "msmincho.ttc"));

            using (var fs = sfd.OpenFile()) {
                Document doc = new Document();

                var wr1 = PdfWriter.GetInstance(doc, fs);

                var A4 = new iTextSharp.text.Rectangle(0, 0, 595, 842);
                if (comboBox1.Text.Length != 0) {
                    var cols = comboBox1.Text.Split(':')[1].Split(',');

                    A4 = new iTextSharp.text.Rectangle(0, 0, float.Parse(cols[0]), float.Parse(cols[1]));
                }

                var f1 = iTextSharp.text.FontFactory.GetFont("MS-UIGothic", BaseFont.IDENTITY_H, 24 / 595.0f * A4.Width);

                doc.SetPageSize(A4);
                doc.SetMargins(0, 0, 0, 0);
                doc.Open();

                int y = 1;
                foreach (String row in rows) {
                    float cx1 = f1.BaseFont.GetWidthPoint(" ", f1.Size);
                    float cy1 = f1.BaseFont.GetAscentPoint(row, f1.Size) - f1.BaseFont.GetDescentPoint(row, f1.Size);

                    ColumnText.ShowTextAligned(
                        wr1.DirectContent,
                        Element.ALIGN_LEFT,
                        new Phrase(row, f1),
                        30,
                        A4.Top - 30 - cy1 * y * 2f,
                        0
                        );
                    y++;
                }

                doc.Close();
            }
        }
    }
}
