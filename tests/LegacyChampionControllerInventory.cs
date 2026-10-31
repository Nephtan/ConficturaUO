using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Engines.CannedEvil;
using Server.Engines.XmlSpawner2;

public static class LegacyChampionControllerInventory
{
    private static void Attr(XmlWriter writer,string key,object value){writer.WriteAttributeString(key,Convert.ToString(value,System.Globalization.CultureInfo.InvariantCulture));}
    private static object Field(object value,string name){return value.GetType().GetField(name,BindingFlags.Instance|BindingFlags.NonPublic).GetValue(value);}
    public static int Main(string[] args)
    {
        try
        {
            if(!Path.GetFullPath(Environment.CurrentDirectory).StartsWith("D:\\ConficturaUOTestTools\\",StringComparison.OrdinalIgnoreCase))throw new Exception("Copied save only.");
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            ScriptCompiler.Assemblies=new Assembly[]{typeof(ChampionSpawn).Assembly};
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();World.Load();
            int present=0,active=0,bosses=0,waves=0,restarting=0,xmlRelated=0;
            using(XmlWriter writer=XmlWriter.Create("legacy-champion-inventory.xml",new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("copiedHistoricalWorld");Attr(writer,"scope","October 17 copied save; matching native World.Load; no Initialize, timer slice, listener, or save; queued timers are not live combat proof.");
                foreach(Item item in World.Items.Values)
                {
                    ChampionSpawn spawn=item as ChampionSpawn;
                    if(spawn!=null && !spawn.Deleted)
                    {
                        ++present;if(spawn.Active)++active;
                        bool boss=spawn.Champion!=null && !spawn.Champion.Deleted && spawn.Champion.Alive;
                        if(boss)++bosses;int living=0;
                        foreach(Mobile mobile in (List<Mobile>)Field(spawn,"m_Creatures"))if(mobile!=null && !mobile.Deleted && mobile.Alive)++living;
                        if(living>0)++waves;Timer timer=Field(spawn,"m_Timer") as Timer;Timer restart=Field(spawn,"m_RestartTimer") as Timer;
                        if(restart!=null && restart.Running)++restarting;
                        writer.WriteStartElement("controller");Attr(writer,"serial",spawn.Serial.Value);Attr(writer,"facet",spawn.Map==null?"none":spawn.Map.Name);Attr(writer,"x",spawn.X);Attr(writer,"y",spawn.Y);
                        Attr(writer,"configuredActive",spawn.Active);Attr(writer,"sliceTimerQueued",timer!=null && timer.Running);Attr(writer,"restartQueued",restart!=null && restart.Running);Attr(writer,"bossPresent",boss);Attr(writer,"livingWaveActors",living);Attr(writer,"type",spawn.Type);Attr(writer,"level",spawn.Level);writer.WriteEndElement();
                    }
                    XmlSpawner xml=item as XmlSpawner;if(xml==null || xml.Deleted)continue;
                    bool offmap=xml.Map==Map.Sosaria && xml.X>=5139 && xml.X<=5147 && xml.Y==3025;
                    List<string> entries=new List<string>();
                    foreach(XmlSpawner.SpawnObject entry in xml.m_SpawnObjects)
                        if(offmap || entry.TypeName.IndexOf("Champion",StringComparison.OrdinalIgnoreCase)>=0)entries.Add(entry.TypeName);
                    if(entries.Count==0)continue;++xmlRelated;
                    writer.WriteStartElement("xmlController");Attr(writer,"serial",xml.Serial.Value);Attr(writer,"facet",xml.Map==null?"none":xml.Map.Name);Attr(writer,"running",xml.Running);Attr(writer,"offmapControl",offmap);
                    foreach(string entry in entries)writer.WriteElementString("entry",entry);writer.WriteEndElement();
                }
                writer.WriteStartElement("counts");Attr(writer,"present",present);Attr(writer,"configuredActive",active);Attr(writer,"bossesPresent",bosses);Attr(writer,"withLivingWaveActors",waves);Attr(writer,"restartTimersQueued",restarting);Attr(writer,"xmlRelated",xmlRelated);writer.WriteEndElement();writer.WriteEndElement();
            }
            Console.WriteLine("PASS native-load inventory: present="+present+"; configured active="+active+"; bosses="+bosses+"; living waves="+waves+"; restart queued="+restarting+"; related XML="+xmlRelated+"; no save/listener.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
