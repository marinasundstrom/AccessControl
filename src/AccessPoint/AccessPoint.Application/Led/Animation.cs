using System;
using System.Drawing;
using System.Text;

namespace AccessPoint.Application.Led
{
    /// <summary>
    /// Holds the animation.
    /// </summary>
    public static class Animation
    {
        public static Color InterpolateColors(int seconds)
        {
            if (seconds < 0 || seconds > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), "Must not be less than 0 or greater than 30.");
            }

            Color resultColor;
            Color from = Color.Red;
            Color to = Color.Green;

            int step = 0;

            if (seconds < 10)
            {
                step = seconds;

                from = Color.Red;
                to = Color.Green;
            }
            else if (seconds < 20)
            {
                step = seconds - 10;

                from = Color.Green;
                to = Color.Blue;
            }
            else if (seconds <= 30)
            {
                step = seconds - 20;

                from = Color.Blue;
                to = Color.Red;
            }

            resultColor = Interpolate(from, to, step, 10);
            return resultColor;
        }

        public static Color Interpolate(Color from, Color to, int stepNumber, int lastStepNumber)
        {
            var bytes = new byte[]
            {
                (byte) Algorithms.Interpolate(from.R, to.R, stepNumber, lastStepNumber),
                (byte) Algorithms.Interpolate(from.G, to.G, stepNumber, lastStepNumber),
                (byte) Algorithms.Interpolate(from.B, to.B, stepNumber, lastStepNumber),
            };

            var str = ByteArrayToString(bytes);

            Console.WriteLine(str);

            return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
