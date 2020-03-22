using System;
using System.Drawing;
using System.Windows.Forms;
using StaticDust;
using System.IO;

namespace AsciiArt_Demo
{
    public partial class Main : Form, IAsciiArtProgress
    {
        public Main()
        {
            InitializeComponent();

            mAsciiImageSize.SelectedIndex = 0;
        }

        private void CmdSelectImage_Click(object sender, EventArgs e)
        {
            if (fdOpen.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            txtImgPath.Text = fdOpen.FileName;
            Browser.DocumentText = string.Empty;
        }

        private int GetBlockSize(Image img)
        {
            var targetWidth = 320;
            var targetHeight = 240;
            switch (mAsciiImageSize.SelectedIndex)
            {
                // micro = 320x240
                case 0:
                    targetWidth = 320;
                    targetHeight = 240;
                    break;

                // tiny = 480x360
                case 1:
                    targetWidth = 480;
                    targetHeight = 360;
                    break;

                // small = 640x480
                case 2:
                    targetWidth = 640;
                    targetHeight = 480;
                    break;

                // medium = 800x600
                case 3:
                    targetWidth = 800;
                    targetHeight = 600;
                    break;

                // large = 1024x768
                case 4:
                    targetWidth = 1024;
                    targetHeight = 768;
                    break;

                // extra large = 1280x1024
                case 5:
                    targetWidth = 1280;
                    targetHeight = 1024;
                    break;

                // super large = 1600x1200
                case 6:
                    targetWidth = 1600;
                    targetHeight = 1200;
                    break;
            }
            var blockWidth = img.Width / targetWidth;
            var blockHeight = img.Height / targetHeight;
            const int FudgeFactor = 5;

            return FudgeFactor * Math.Max(blockWidth, blockHeight);
        }

        private void CmdConvert_Click(object sender, EventArgs e)
        {
            Browser.DocumentText = string.Empty;
            pbAsciify.Value = 0;

            using (var imgStream = File.OpenRead(txtImgPath.Text))
            {
                using (var img = Image.FromStream(imgStream))
                {
                    // IE browser can't handle *huge* document text string, so load from a temp file
                    var tmpFile = Path.GetTempFileName();

                    // delete zero byte file
                    File.Delete(tmpFile);

                    using (var fileStream = new StreamWriter(tmpFile))
                    {
                        var blockSize = GetBlockSize(img);
                        var sw = System.Diagnostics.Stopwatch.StartNew();
                        AsciiArt.ConvertImage(imgStream, fileStream, blockSize, 5, false, chkColour.Checked, this);
                        System.Diagnostics.Debug.WriteLine("ConvertImage in " + sw.ElapsedMilliseconds + " ms");
                    }

                    Browser.Navigate(@"file:///" + tmpFile);
                    Browser.DocumentCompleted += delegate
                      {
                          File.Delete(tmpFile);
                      };

                    // reset progress bar
                    pbAsciify.Value = 0;
                }
            }
        }

        public void Progress(int percentComplete)
        {
            pbAsciify.Value = percentComplete;
        }
    }
}
