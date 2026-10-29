using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Ultima;

class RenderLandmarkAtlas
{
    static int Main(string[] args)
    {
        try
        {
            Files.SetMulPath(args[0]);
            Files.MulPath["map0.mul"] = Path.Combine(args[0], "map1.mul");
            Files.MulPath["statics0.mul"] = Path.Combine(args[0], "statics1.mul");
            Files.MulPath["staidx0.mul"] = Path.Combine(args[0], "staidx1.mul");
            Map.UseDiff = false;
            Map.Trammel.Width = 7168;
            string[] names = { "Central Orc Fort", "Brigand Fort", "Lich Tower", "Ratmen Fort", "Northern Orc Fort", "Swamp Ruins of the Dead" };
            int[][] rectangles = { new int[] {2279,1772,74,64}, new int[] {2979,351,67,71},
                new int[] {3489,2264,41,38}, new int[] {3384,1157,37,37},
                new int[] {559,475,29,27}, new int[] {503,190,38,26} };
            int[] connected = {4041,2487,1322,1186,534,303};
            using (Bitmap result = new Bitmap(1152, 972))
            using (Graphics graphics = Graphics.FromImage(result))
            using (Font title = new Font("Arial", 15, FontStyle.Bold))
            using (Font text = new Font("Arial", 11))
            {
                graphics.Clear(Color.FromArgb(20, 24, 28));
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.DrawString("Sosaria protected landmarks - candidates for different encounter layouts", title, Brushes.White, 12, 8);
                graphics.DrawString("NOT APPROVED: current footprint/buffer does not fit. Magenta: existing permanent housing ban.", text, Brushes.White, 12, 38);
                graphics.DrawString("Red: copied house areas. Cyan: portal candidates. Save dated 17 Oct 2026; current occupancy requires recheck.", text, Brushes.White, 12, 60);
                for (int index = 0; index < names.Length; ++index)
                {
                    int[] r = rectangles[index];
                    int x0 = ((r[0] + r[2] / 2 - 96) / 8) * 8;
                    int y0 = ((r[1] + r[3] / 2 - 96) / 8) * 8;
                    int left = (index % 3) * 384, top = 92 + (index / 3) * 440;
                    using (Bitmap map = Map.Trammel.GetImage(x0 / 8, y0 / 8, 24, 24, true))
                        graphics.DrawImage(map, new Rectangle(left, top, 384, 384));
                    using (Pen border = new Pen(Color.Magenta, 2))
                        graphics.DrawRectangle(border, left + (r[0]-x0)*2, top+(r[1]-y0)*2, r[2]*2, r[3]*2);
                    GraphicsState clip = graphics.Save();
                    graphics.SetClip(new Rectangle(left, top, 384, 384));
                    foreach (string line in File.ReadAllLines(args[2]))
                    {
                        string[] values = line.Split(',');
                        if (values[0] != "house" && values[0] != "portal" && values[0] != "city") continue;
                        int x = Int32.Parse(values[1]), y = Int32.Parse(values[2]);
                        int w = Int32.Parse(values[3]), h = Int32.Parse(values[4]);
                        if (x+w < x0 || y+h < y0 || x >= x0+192 || y >= y0+192) continue;
                        using (Pen pen = new Pen(values[0] == "house" ? Color.Red : values[0] == "city" ? Color.Orange : Color.Cyan, 2))
                            graphics.DrawRectangle(pen, left+(x-x0)*2, top+(y-y0)*2, Math.Max(3,w*2), Math.Max(3,h*2));
                    }
                    graphics.Restore(clip);
                    graphics.DrawString((index+1)+". "+names[index]+" ("+(r[0]+r[2]/2)+","+(r[1]+r[3]/2)+")", text, Brushes.White, left+7, top+389);
                    graphics.DrawString(r[2]+"x"+r[3]+" protected; "+connected[index]+" connected tiles", text, Brushes.White, left+7, top+411);
                }
                result.Save(args[1], ImageFormat.Png);
            }
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
