using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Spells.Chivalry;
using Server.Custom.Confictura.Champions;

namespace Server.TestOnly
{
    // Declared forty-point self-injury is a toolkit fixture, never an encounter or pacing reference.
    public static class CloseWoundsClientProbe
    {
        private static readonly string Evidence=Path.GetFullPath(Path.Combine(Core.BaseDirectory,"../evidence"));
        private static readonly BindingFlags Static=BindingFlags.Static|BindingFlags.NonPublic;
        private static string Phase="waiting",Failure=String.Empty;
        private static int Healing,InitialMana,InitialTithing;
        private static DateTime Started;
        private static Timer Poll;
        private static PlayerMobile Actor {get{return (PlayerMobile)typeof(ArachnidArchmagePreparation).GetField("Actor",Static).GetValue(null);}}
        [CallPriority(Int32.MaxValue)]
        public static void Initialize()
        {
            if(!File.Exists(Path.Combine(Evidence,"healing-probe-case.txt")))return;
            if(ArachnidOutbreakController.DeliveryReady || File.ReadAllText(Path.Combine(Evidence,"native-private-root.txt")).Trim()!=Core.BaseDirectory.TrimEnd('\\'))throw new Exception("Guarded copied-world probe required.");
            AutoSave.SavesEnabled=false;EventSink.HealthChanged+=Health;
            Poll=Timer.DelayCall(TimeSpan.FromSeconds(2),TimeSpan.FromMilliseconds(250),Tick);
        }
        private static void Health(HealthChangedEventArgs e)
        {if(Phase=="running" && e.Mobile==Actor && e.Source==Actor && e.Kind==HealthChangeKind.Healing && e.NewHits>e.OldHits)Healing+=e.NewHits-e.OldHits;}
        private static void Tick()
        {
            try
            {
                if(File.Exists(Path.Combine(Evidence,"healing-probe-begin.txt")) && Phase=="waiting")
                {
                    if(Actor==null || Actor.NetState==null || !Actor.Alive || Actor.Blessed || Actor.AccessLevel!=AccessLevel.Player || Actor.Karma<0 || Actor.TithingPoints<10 || Actor.Mana<10)throw new Exception("Actual eligible saved vulnerable client required; no grants.");
                    InitialMana=Actor.Mana;InitialTithing=Actor.TithingPoints;Started=DateTime.UtcNow;Phase="running";
                    Actor.Damage(40,Actor);
                }
                if(Phase=="running" && Healing>0)Phase="healed";
                if(Phase=="running" && (DateTime.UtcNow-Started).TotalSeconds>75)Phase="window-ended";
                Status();
                if(File.Exists(Path.Combine(Evidence,"native-stop.txt")))Poll.Stop();
            }
            catch(Exception e){Failure=e.ToString();Phase="failed";try{Status();}finally{Poll.Stop();File.WriteAllText(Path.Combine(Evidence,"native-stop.txt"),"Toolkit probe failed; retain error and stop without save");}}
        }
        private static void Status()
        {
            string path=Path.Combine(Evidence,"healing-probe-state.xml");
            using(XmlWriter w=XmlWriter.Create(path+".tmp",new XmlWriterSettings{Indent=true}))
            {
                w.WriteStartElement("healingProbe");w.WriteAttributeString("phase",Phase);w.WriteAttributeString("failure",Failure);w.WriteAttributeString("healing",Healing.ToString());w.WriteAttributeString("initialMana",InitialMana.ToString());w.WriteAttributeString("initialTithing",InitialTithing.ToString());
                if(Actor!=null){w.WriteAttributeString("hits",Actor.Hits.ToString());w.WriteAttributeString("mana",Actor.Mana.ToString());w.WriteAttributeString("tithing",Actor.TithingPoints.ToString());w.WriteAttributeString("karma",Actor.Karma.ToString());w.WriteAttributeString("castDelayMs",new CloseWoundsSpell(Actor,null).GetCastDelay().TotalMilliseconds.ToString(System.Globalization.CultureInfo.InvariantCulture));}
                w.WriteEndElement();
            }
            if(File.Exists(path))File.Replace(path+".tmp",path,null);else File.Move(path+".tmp",path);
        }
    }
}
