using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MBTrade.Tools
{

    // C++: class LogisticRegression
    //javadoc: LogisticRegression

    public class LogisticRegression
    {
        public static void teste()
        {
            using (var src = new Mat(new Size(128, 128), MatType.CV_8U, Scalar.All(255)))
            using (var dst = new Mat())
            {
                for (var y = 0; y < src.Height; y++)
                {
                    for (var x = 0; x < src.Width; x++)
                    {
                        var color = src.Get<Vec3b>(y, x);
                        var temp = color.Item0;
                        color.Item0 = color.Item2; // B <- R
                        color.Item2 = temp;        // R <- B
                        src.Set(y, x, color);
                    }
                }

                src.CopyTo(dst);

                using (var window = new Window("window", image: dst, flags: WindowMode.AutoSize))
                {
                    Cv2.WaitKey();
                }
            }
        }
    }
}