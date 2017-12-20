using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace screencap
{
    public partial class process_select : Form
    {
        private Main_Form main_form;
        public process_select(Main_Form _form)
        {
            main_form = _form;
            InitializeComponent();

            Process[] processesByName = Process.GetProcessesByName("exefile");
            List<listboxData> listboxDataList = new List<listboxData>();
            foreach (Process process in processesByName)
                listboxDataList.Add(new listboxData()
                {
                    Value = process.Id.ToString(),
                    Text = process.MainWindowTitle
                });
            ListBox_process.DisplayMember = "Text";
            ListBox_process.DataSource = listboxDataList;
        }
        
        private void ListBox_process_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox_process.Image != null)
                pictureBox_process.Dispose();
            Process p = Process.GetProcessById(int.Parse((ListBox_process.SelectedItem as listboxData).Value));
            IntPtr windowHandle = p.MainWindowHandle;
            var Capture = new CaptureLib();
            Bitmap bmp = Capture.CaptureWindow(windowHandle);
            pictureBox_process.Image = bmp;
            pictureBox_process.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void ListBox_process_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListBox_process.IndexFromPoint(e.Location) == -1)
                return;
            main_form.textBox_Name.Text = (ListBox_process.SelectedItem as listboxData).Text;
            main_form.proc = Process.GetProcessById(int.Parse((ListBox_process.SelectedItem as listboxData).Value));
            
            main_form.textBox_Name.ReadOnly = true;
            main_form.button_SetArea.Enabled = true;

            pictureBox_process.Dispose();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Dispose();
            Close();
        }
    }
}