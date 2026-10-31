using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Custom.Confictura.Champions;

public static class InfestationStorageEnvelope
{
    private static int Cells,Residents;
    private static long Memory,Bytes;
    private static void Write(string path)
    {
        ArachnidInfestationState state=new ArachnidInfestationState();state.Origin=new Point3D(18,18,0);
        state.Births=Int32.MaxValue;state.Started=state.LastTick=DateTime.UtcNow;state.NextGrowth=state.LastTick.AddSeconds(45);
        for(int y=18;y<3112;y+=12)
            for(int x=18;x<5104;x+=12)
            {
                ArachnidInfestationPatch patch=new ArachnidInfestationPatch();patch.Id=state.Patches.Count;patch.Parent=patch.Id-1;
                patch.Birth=patch.Id;patch.Center=new Point3D(x,y,0);patch.Established=true;patch.Initialized=true;
                for(byte slot=0;slot<13;++slot)
                {
                    ArachnidInfestationResident survivor=new ArachnidInfestationResident();survivor.Slot=slot;survivor.Kind=(byte)(slot%2);survivor.Tier=4;
                    survivor.MaximumHits=(ushort)(slot==0?1400:350);survivor.Hits=(ushort)(survivor.MaximumHits/2);survivor.Stam=20;survivor.Mana=7;
                    survivor.Position=patch.Center;survivor.Identity=new Guid(patch.Id,(short)slot,1,0,0,0,0,0,0,0,1);
                    patch.Residents.Add(survivor);++Residents;
                }
                state.Add(patch);
            }
        state.Epoch=state.Patches.Count+13;Cells=state.Patches.Count;Memory=GC.GetTotalMemory(true);
        using(FileStream stream=new FileStream(path,FileMode.CreateNew,FileAccess.Write))
        { BinaryFileWriter writer=new BinaryFileWriter(stream,true);state.Serialize(writer);writer.Write(441992);writer.Close(); }
        Bytes=new FileInfo(path).Length;
    }
    private static void Read(string path)
    {
        using(FileStream stream=new FileStream(path,FileMode.Open,FileAccess.Read))
        {
            BinaryFileReader reader=new BinaryFileReader(new BinaryReader(stream));ArachnidInfestationState state=new ArachnidInfestationState();state.Deserialize(reader);
            if(state.Patches.Count!=Cells || state.Births!=Int32.MaxValue || reader.ReadInt()!=441992)throw new Exception("Envelope round-trip or boundary failed.");
            int residents=0;foreach(ArachnidInfestationPatch patch in state.Patches)residents+=patch.Residents.Count;
            if(residents!=Residents || state.Patches[state.Patches.Count-1].Residents[12].Mana!=7)throw new Exception("Survivor count/resources changed.");reader.Close();
            Console.WriteLine("PASS synthetic storage envelope: cells="+Cells+"; survivors="+Residents+"; fileBytes="+Bytes+"; writeManagedBytes="+Memory+"; loadedManagedBytes="+GC.GetTotalMemory(true)+"; peakWorkingSet="+Process.GetCurrentProcess().PeakWorkingSet64+"; x86="+(IntPtr.Size==4));
            Console.WriteLine("Synthetic value records only: no terrain validation, live mobiles, copied saves, listener, native simulation or VPS capacity claim.");
        }
    }
    public static int Main(string[] args)
    {
        try
        {
            typeof(World).GetField("m_Items",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Item>());
            typeof(World).GetField("m_Mobiles",BindingFlags.Static|BindingFlags.NonPublic).SetValue(null,new Dictionary<Serial,Mobile>());
            Write(args[0]);GC.Collect();GC.WaitForPendingFinalizers();GC.Collect();Read(args[0]);return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
