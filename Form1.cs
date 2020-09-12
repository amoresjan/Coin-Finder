using ImageProcess2;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Reflection;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

namespace Amores_Coin
{
    public partial class Form1 : Form
    {
        Bitmap loaded;
        Bitmap processed;

        public Form1()
        {
            InitializeComponent();
            loaded = new Bitmap(@"C:\Users\dell\Downloads\coins1.jpg");
            BitmapFilter.Scale(ref loaded, ref processed, 251, 332);
            loaded = processed;
            pictureBox1.Image = loaded;
        }

        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            BitmapFilter.NegativeThreshold(ref loaded, ref processed, trackBar1.Value);
            pictureBox1.Image = processed;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            double sum = 0;
            //locate objects
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.ProcessImage(processed);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            Graphics g = Graphics.FromImage(processed);
            Pen redPen = new Pen(Color.Red, 2);

            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            Console.WriteLine(blobs.Length);

            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);

                AForge.Point center;
                float radius;

                if (shapeChecker.IsCircle(edgePoints, out center, out radius))
                {
                    g.DrawEllipse(redPen,
                        (int)(center.X - radius),
                        (int)(center.Y - radius),
                        (int)(radius * 2),
                        (int)(radius * 2));

                    if (blobs[i].Area >= 240 && blobs[i].Area <= 290)
                    {
                        sum += 0.05;
                    }
                    if (blobs[i].Area >= 315 && blobs[i].Area <= 360)
                    {
                        sum += 0.1;
                    }
                    if (blobs[i].Area >= 420 && blobs[i].Area <= 500)
                    {
                        sum += 0.25;
                    }
                    if (blobs[i].Area >= 610 && blobs[i].Area <= 680)
                    {
                        sum += 1;
                    }
                    if (blobs[i].Area >= 760 && blobs[i].Area <= 840)
                    {
                        sum += 5;
                    }
                }

            }


            redPen.Dispose();
            g.Dispose();

            
            MessageBox.Show("Total: " + sum);
        }
    }
}
