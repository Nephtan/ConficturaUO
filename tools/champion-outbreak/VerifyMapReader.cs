using System;
using System.IO;

internal static class VerifyMapReader
{
    public static void Main(string[] args)
    {
        Server.Core.DataDirectories.Add(args[0]);
        Server.Misc.MapDefinitions.Configure();
        Ultima.Files.SetMulPath(args[0]);
        Ultima.Files.MulPath["map0.mul"] = Path.Combine(args[0], "map1.mul");
        Ultima.Files.MulPath["statics0.mul"] = Path.Combine(args[0], "statics1.mul");
        Ultima.Files.MulPath["staidx0.mul"] = Path.Combine(args[0], "staidx1.mul");
        Ultima.Map.UseDiff = false;
        Console.WriteLine("Remapped SDK input: " + Ultima.Files.GetFilePath("map0.mul"));
        int[][] samples = new int[][]
        {
            new int[] { 2979, 351 }, new int[] { 2999, 1060 },
            new int[] { 914, 716 }, new int[] { 2433, 876 }
        };
        foreach (int[] point in samples)
        {
            Server.LandTile server = Server.Map.Sosaria.Tiles.GetLandTile(point[0], point[1]);
            Ultima.Tile sdk = Ultima.Map.Trammel.Tiles.GetLandTile(point[0], point[1]);
            if (server.ID != sdk.ID || server.Z != sdk.Z)
                throw new Exception("Map readers disagree.");
            Console.WriteLine(point[0] + "," + point[1] + " Server=" + server.ID + "," + server.Z + " SDK=" + sdk.ID + "," + sdk.Z);
        }
    }
}
