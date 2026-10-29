using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Ultima;

class RenderReservationAtlas
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
            string[] lines = File.ReadAllLines(args[3]);
            string[] names = new string[lines.Length];
            int[][] rectangles = new int[lines.Length][];
            int[] connected = new int[lines.Length];
            for (int i=0; i<lines.Length; ++i)
            {
                string[] values = lines[i].Split(',');
                names[i] = values[0];
                rectangles[i] = new int[] {Int32.Parse(values[1])-80,Int32.Parse(values[2])-80,161,161};
                connected[i] = Int32.Parse(values[3]);
            }
            using (Bitmap result = new Bitmap(1344, 1100))
            using (Graphics graphics = Graphics.FromImage(result))
            using (Font title = new Font("Arial", 15, FontStyle.Bold))
            using (Font text = new Font("Arial", 11))
            {
                graphics.Clear(Color.FromArgb(20, 24, 28));
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.DrawString("Proposed Sosaria wilderness reservations - six larger encounter layouts", title, Brushes.White, 12, 8);
                graphics.DrawString("PROPOSAL ONLY. Magenta: 161x161 reservation. Gold: 113x113 combat area. Blue: brood anchors.", text, Brushes.White, 12, 38);
                graphics.DrawString("Red: houses. Cyan: portals. White: services. Copied save dated 17 Oct 2026; native review remains pending.", text, Brushes.White, 12, 60);
                for (int index = 0; index < names.Length; ++index)
                {
                    int[] r = rectangles[index];
                    int x0 = ((r[0] + r[2] / 2 - 112) / 8) * 8;
                    int y0 = ((r[1] + r[3] / 2 - 112) / 8) * 8;
                    int left = (index % 3) * 448, top = 92 + (index / 3) * 504;
                    using (Bitmap map = Map.Trammel.GetImage(x0 / 8, y0 / 8, 28, 28, true))
                        graphics.DrawImage(map, new Rectangle(left, top, 448, 448));
                    using (Pen border = new Pen(Color.Magenta, 2))
                        graphics.DrawRectangle(border, left + (r[0]-x0)*2, top+(r[1]-y0)*2, r[2]*2, r[3]*2);
                    int cx=left+(r[0]+80-x0)*2, cy=top+(r[1]+80-y0)*2;
                    using (Pen combat = new Pen(Color.Gold,2)) graphics.DrawRectangle(combat,cx-112,cy-112,226,226);
                    graphics.FillEllipse(Brushes.Gold,cx-4,cy-4,8,8);
                    foreach (int[] offset in new int[][] {new int[]{-28,-20},new int[]{28,-20},new int[]{0,30}})
                        graphics.FillEllipse(Brushes.DeepSkyBlue,cx+offset[0]*2-4,cy+offset[1]*2-4,8,8);
                    GraphicsState clip = graphics.Save();
                    graphics.SetClip(new Rectangle(left, top, 448, 448));
                    foreach (string line in File.ReadAllLines(args[2]))
                    {
                        string[] values = line.Split(',');
                        if (values[0] != "house" && values[0] != "portal" && values[0] != "city" && values[0] != "service") continue;
                        int x = Int32.Parse(values[1]), y = Int32.Parse(values[2]);
                        int w = Int32.Parse(values[3]), h = Int32.Parse(values[4]);
                        if (x+w < x0 || y+h < y0 || x >= x0+224 || y >= y0+224) continue;
                        using (Pen pen = new Pen(values[0] == "house" ? Color.Red : values[0] == "city" ? Color.Orange : values[0] == "service" ? Color.White : Color.Cyan, 2))
                            graphics.DrawRectangle(pen, left+(x-x0)*2, top+(y-y0)*2, Math.Max(3,w*2), Math.Max(3,h*2));
                    }
                    graphics.Restore(clip);
                    graphics.DrawString((index+1)+". "+names[index]+" ("+(r[0]+r[2]/2)+","+(r[1]+r[3]/2)+")", text, Brushes.White, left+7, top+453);
                    graphics.DrawString(r[2]+"x"+r[3]+" proposed; "+connected[index]+" connected tiles", text, Brushes.White, left+7, top+476);
                }
                result.Save(args[1], ImageFormat.Png);
            }
            return 0;
        }
        catch (Exception error) { Console.Error.WriteLine(error); return 1; }
    }
}
