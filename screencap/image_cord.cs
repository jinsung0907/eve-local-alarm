using System;
using System.Drawing;
using System.Windows.Forms;

namespace screencap
{
    public partial class image_cord : Form
    {
        private bool Drawing = false;
        private Main_Form main_form;
        private Bitmap OriginalImage;
        private Bitmap DisplayImage;
        private Graphics DisplayGraphics;
        private Point StartPoint;
        private Point EndPoint;

        public image_cord(Main_Form _form, Bitmap src)
        {
            main_form = _form;
            InitializeComponent();
            OriginalImage = src;
            DisplayImage = OriginalImage.Clone() as Bitmap;
            DisplayGraphics = Graphics.FromImage(DisplayImage);
            pictureBox1.Image = DisplayImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        // 영역 선택 시작
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Drawing = true;
            StartPoint = e.Location;
            DrawSelectionBox(e.Location);
        }

        // 움직이는 동안 빨간 상자 그리기
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Drawing)
                return;
            DrawSelectionBox(e.Location);
        }

        // 영역 선택 끝
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Drawing)
                return;
            Drawing = false;
            main_form.isareaset = 1;
            main_form.status = 1;
            main_form.timer1.Start();
            main_form.button_SetArea.Enabled = false;

            OriginalImage.Dispose();
            DisplayImage.Dispose();
            pictureBox1.Dispose();
            DisplayGraphics.Dispose();
            Dispose();
            Close();
        }

        public void DrawSelectionBox(Point end_point)
        {
            label1.Text = StartPoint.X.ToString() + " " + EndPoint.X.ToString() + " " + StartPoint.Y.ToString() + " " + EndPoint.Y.ToString();
            
            EndPoint = end_point;

            if (EndPoint.X < StartPoint.X)
                EndPoint.X = StartPoint.X + 10;
            if (EndPoint.X < 0)
                EndPoint.X = 0;
            if (EndPoint.X >= OriginalImage.Width)
                EndPoint.X = OriginalImage.Width - 1;

            if (EndPoint.Y < StartPoint.Y)
                EndPoint.Y = StartPoint.Y + 10;
            if (EndPoint.Y < 0)
                EndPoint.Y = 0;
            if (EndPoint.Y >= OriginalImage.Height)
                EndPoint.Y = OriginalImage.Height - 1;

            main_form.text_xstart = StartPoint.X;
            main_form.text_xend = EndPoint.X;
            main_form.text_ystart = StartPoint.Y;
            main_form.text_yend = EndPoint.Y;

            DisplayGraphics.DrawImageUnscaled(OriginalImage, 0, 0);
            DisplayGraphics.DrawRectangle(Pens.Red, Math.Min(StartPoint.X, EndPoint.X), Math.Min(StartPoint.Y, EndPoint.Y), Math.Abs(StartPoint.X - EndPoint.X), Math.Abs(StartPoint.Y - EndPoint.Y));
            pictureBox1.Refresh();
        }
    }
}
