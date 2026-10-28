using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Ultima;
class RenderMap
{
    static int Main(string[] args)
    {
        try
        {
            Files.SetMulPath(args[0]);
            Files.MulPath["map0.mul"] = System.IO.Path.Combine(args[0], "map1.mul");
            Files.MulPath["statics0.mul"] = System.IO.Path.Combine(args[0], "statics1.mul");
            Files.MulPath["staidx0.mul"] = System.IO.Path.Combine(args[0], "staidx1.mul");
            Map.UseDiff = false;
            Map.Trammel.Width = 7168;
            using (Bitmap full = Map.Trammel.GetImage(0, 0, 640, 391, true))
            using (Bitmap overview = new Bitmap(1280, 782))
            using (Graphics g = Graphics.FromImage(overview))
            {
                Console.WriteLine("Source bitmap: " + full.Width + "x" + full.Height);
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(full, new Rectangle(0, 0, 1280, 782));

                foreach (string line in System.IO.File.ReadAllLines(args[2]))
                {
                    string[] values = line.Split(',');
                    float x = Single.Parse(values[1]) / 4, y = Single.Parse(values[2]) / 4;
                    float w = Math.Max(2, Single.Parse(values[3]) / 4), h = Math.Max(2, Single.Parse(values[4]) / 4);
                    Color color = values[0] == "house" ? Color.Red : values[0] == "city" ? Color.Orange : values[0] == "nohousing" ? Color.Magenta : Color.Cyan;
                    using (Pen pen = new Pen(color, 1))
                    {
                        g.DrawRectangle(pen, x, y, w, h);
                    }
                }
                using (SolidBrush background = new SolidBrush(Color.FromArgb(220, 0, 0, 0)))
                {
                    g.FillRectangle(background, 0, 0, 1280, 44);
                }
                using (Font font = new Font("Arial", 11))
                {
                    g.DrawString("Sosaria mainland event survey - terrain plus copied save dated 17 Oct 2026", font, Brushes.White, 8,3);
                    g.DrawString("Red: house areas  Orange: city areas  Cyan: portal candidates  Magenta: unconditional outdoor no-housing regions", font, Brushes.White, 8,23);
                }
                overview.Save(args[1], ImageFormat.Png);
                Console.WriteLine("Rendered mainland Sosaria with Pandora's installed Ultima map library: " + args[1]);
            }
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
    }
}
