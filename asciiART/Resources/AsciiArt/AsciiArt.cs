/*======================================================================/

Copyright (C) 2004 Daniel Fisher(lennybacon).  All rights reserved.

THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
PARTICULAR PURPOSE.

For more information email: info@lennybacon.com

12/27/04 Enhancements by Steven Fowler (steven_m_fowler@yahoo.com)
=======================================================================*/

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StaticDust
{
  public interface IAsciiArtProgress
  {
    /// <summary>
    /// callback to report progress
    /// </summary>
    /// <param name="percentComplete">percentage complete in the range [0, 100]</param>
    void Progress(int percentComplete);
  }

  /// <summary>
  /// Summary description for image2html.
  /// </summary>
  public class AsciiArt
  {
    /// <summary>
    /// converts an image to an ascii string
    /// </summary>
    /// <param name="imgStream">opened stream containing an image</param>
    /// <param name="fileStream">opened file to write HTML</param>
    /// <param name="imgBlockSize">
    /// used to determine the width in pixels of one ascii character
    /// which is then used to determine the block size in pixels
    /// of one ascii character
    /// <remarks>
    /// acceptable values are [1, 2, 6, 8, 16, 24, 32]
    /// </remarks>
    /// </param>
    /// <param name="fontSize">
    /// fixed width font size in pixels
    /// <remarks>5 pixels seems to be a good value</remarks>
    /// </param>
    /// <param name="quick">
    /// true to perform a quick brightness calculation
    /// <param name="monitor"></param>
    /// <remarks>
    /// quick brightness calculation is only done on first row of pixels in a block
    /// </remarks>
    /// </param>
    /// <param name="colour">true to generate a colour ascii image</param>
    /// <param name="monitor">
    /// optional callback to monitor progress, <seealso cref="IAsciiArtProgress"/>
    /// <remarks>pass in null to not monitor progress</remarks>
    /// </param>
    /// <returns></returns>
    public static void ConvertImage(Stream imgStream, StreamWriter fileStream, int imgBlockSize, int fontSize, bool quick, bool colour, IAsciiArtProgress monitor)
    {
      const string WebPage1 =
        "<html>" +
          "<body>" +
          "<pre>" +
            "<span style=\"font-size: {0}px\">";

      fileStream.Write(WebPage1, fontSize);

      Bitmap bmp = null;

      try
      {
        #region load image

        using (var img = Image.FromStream(imgStream))
        {
          bmp = new Bitmap(img, new Size(img.Width, img.Height));
        }

        var bounds = new Rectangle(0, 0, bmp.Width, bmp.Height);

        #endregion

        #region greyscale image

        var matrix = new ColorMatrix();

        matrix[0, 0] = 1 / 3f;
        matrix[0, 1] = 1 / 3f;
        matrix[0, 2] = 1 / 3f;
        matrix[1, 0] = 1 / 3f;
        matrix[1, 1] = 1 / 3f;
        matrix[1, 2] = 1 / 3f;
        matrix[2, 0] = 1 / 3f;
        matrix[2, 1] = 1 / 3f;
        matrix[2, 2] = 1 / 3f;

        var attributes = new ImageAttributes();
        attributes.SetColorMatrix(matrix);

        if (!colour)
        {
          using (var gphGrey = Graphics.FromImage(bmp))
          {
            gphGrey.DrawImage(bmp, bounds, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
          }
        }

        #endregion

        #region ascii image

        var pixWidth = imgBlockSize;
        var pixHeight = pixWidth * 2;
        var pixSeg = pixWidth * pixHeight;
        var numHeightIter = bmp.Height / pixHeight;
        var numWidthIter = bmp.Width / pixWidth;
        var percentIter = (numHeightIter * numWidthIter / 100) + 1;
        var currIter = 0;

        for (var h = 0; h < numHeightIter; h++, currIter++)
        {
          // segment height
          var startY = (h * pixHeight);

          // segment width
          for (var w = 0; w < numWidthIter; w++, currIter++)
          {
            if (monitor != null && (currIter % percentIter) == 0)
            {
              monitor.Progress(currIter / percentIter);
            }

            var startX = (w * pixWidth);
            var allBrightness = 0;
            var allAlpha = 0;
            var allRed = 0;
            var allGreen = 0;
            var allBlue = 0;

            if (quick)
            {
              // each pix of this segment
              for (var y = 0; y < pixWidth; y++)
              {
                try
                {
                  var clr = bmp.GetPixel(startX, y + startY);
                  allAlpha += clr.A;
                  allRed += clr.R;
                  allGreen += clr.G;
                  allBlue += clr.B;
                  var brt = (int)(clr.GetBrightness() * 100);
                  allBrightness = allBrightness + brt;
                }
                catch
                {
                  allBrightness = (allBrightness + 50);
                }
              }
            }
            else
            {
              // each pix of this segment
              for (var y = 0; y < pixWidth; y++)
              {
                for (var x = 0; x < pixHeight; x++)
                {
                  var cY = y + startY;
                  var cX = x + startX;
                  try
                  {
                    if (cX < bmp.Width)
                    {
                      var clr = bmp.GetPixel(cX, cY);
                      allAlpha += clr.A;
                      allRed += clr.R;
                      allGreen += clr.G;
                      allBlue += clr.B;
                      var brt = (int)(clr.GetBrightness() * 100);
                      allBrightness = allBrightness + brt;
                    }
                  }
                  catch
                  {
                    allBrightness = (allBrightness + 50);
                  }
                }
              }
            }

            var avgAlpha = (allAlpha / pixSeg);
            var avgRed = (allRed / pixSeg);
            var avgGreen = (allGreen / pixSeg);
            var avgBlue = (allBlue / pixSeg);
            var avgClr = Color.FromArgb(avgAlpha, avgRed, avgGreen, avgBlue);

            var sbrt = (allBrightness / pixSeg);
            string asciiChar;
            if (sbrt < 10)
            {
              asciiChar = "#";
            }
            else if (sbrt < 17)
            {
              asciiChar = "@";
            }
            else if (sbrt < 24)
            {
              asciiChar = "&";
            }
            else if (sbrt < 31)
            {
              asciiChar = "$";
            }
            else if (sbrt < 38)
            {
              asciiChar = "%";
            }
            else if (sbrt < 45)
            {
              asciiChar = "|";
            }
            else if (sbrt < 52)
            {
              asciiChar = "!";
            }
            else if (sbrt < 59)
            {
              asciiChar = ";";
            }
            else if (sbrt < 66)
            {
              asciiChar = ":";
            }
            else if (sbrt < 73)
            {
              asciiChar = "'";
            }
            else if (sbrt < 80)
            {
              asciiChar = "`";
            }
            else if (sbrt < 87)
            {
              asciiChar = ".";
            }
            else
            {
              asciiChar = " ";
            }

            if (colour)
            {
              const string ColourCharElement = "<code style=\"color:{0}\">{1}</code>";

              var clrHex = string.Format("#{0:x}{1:x}{2:x}", avgClr.R, avgClr.G, avgClr.B);
              var asciiStr = string.Format(ColourCharElement, clrHex, asciiChar);

              fileStream.Write(asciiStr);
            }
            else
            {
              fileStream.Write(asciiChar);
            }
          }
          fileStream.Write("\n");
        }

        #endregion
      }
      finally
      {
        //clean up
        bmp.Dispose();
      }

      const string WebPage3 =
            "</span>" +
          "</pre>" +
          "</body>" +
        "</html>";


      fileStream.Write(WebPage3);
    }
  }
}