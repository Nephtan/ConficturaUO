using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Custom.Confictura.Champions;

public static class ArachnidSavedBuildSurvey
{
    public static int Main(string[] args)
    {
        try
        {
            if (!Path.GetFullPath(Environment.CurrentDirectory).StartsWith("D:\\ConficturaUOTestTools\\",StringComparison.OrdinalIgnoreCase)) throw new Exception("Copied-save survey only");
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            ScriptCompiler.Assemblies=new Assembly[]{typeof(ArachnidOutbreakController).Assembly};
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();World.Load();
            int count=0,novices=0,intermediates=0;
            using(XmlWriter writer=XmlWriter.Create("saved-builds.private.xml",new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("players");
                foreach(Mobile mobile in World.Mobiles.Values)
                {
                    PlayerMobile actor=mobile as PlayerMobile;if(actor==null || actor.Deleted || actor.AccessLevel!=AccessLevel.Player)continue;
                    ++count;double highest=0,total=0;
                    for(int i=0;i<actor.Skills.Length;++i){Skill skill=actor.Skills[i];highest=Math.Max(highest,skill.Base);total+=skill.Base;}
                    if(highest<=50)++novices;else if(highest<110)++intermediates;
                    writer.WriteStartElement("player");
                    Attr(writer,"name",actor.Name);Attr(writer,"serial",actor.Serial.Value);Attr(writer,"alive",actor.Alive);Attr(writer,"blessed",actor.Blessed);
                    Attr(writer,"highestBase",highest);Attr(writer,"totalBase",total);Attr(writer,"str",actor.RawStr);Attr(writer,"dex",actor.RawDex);Attr(writer,"int",actor.RawInt);
                    Attr(writer,"hits",actor.Hits);Attr(writer,"hitsMax",actor.HitsMax);Attr(writer,"mana",actor.Mana);Attr(writer,"stam",actor.Stam);
                    Attr(writer,"x",actor.X);Attr(writer,"y",actor.Y);Attr(writer,"z",actor.Z);Attr(writer,"map",actor.Map);
                    for(int i=0;i<actor.Skills.Length;++i){Skill skill=actor.Skills[i];if(skill.Base>0){writer.WriteStartElement("skill");Attr(writer,"name",skill.SkillName);Attr(writer,"base",skill.Base);Attr(writer,"effective",skill.Value);Attr(writer,"cap",skill.Cap);writer.WriteEndElement();}}
                    foreach(Item item in actor.Items){writer.WriteStartElement("equipped");Attr(writer,"serial",item.Serial.Value);Attr(writer,"type",item.GetType().FullName);Attr(writer,"amount",item.Amount);writer.WriteEndElement();}
                    if(actor.Backpack!=null)foreach(Item item in actor.Backpack.FindItemsByType(typeof(Item),true)){writer.WriteStartElement("carried");Attr(writer,"serial",item.Serial.Value);Attr(writer,"type",item.GetType().FullName);Attr(writer,"amount",item.Amount);writer.WriteEndElement();}
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            Console.WriteLine("Copied saved-build survey: players="+count+"; highest-base<=50="+novices+"; highest-base50-110="+intermediates+". Details remain private. No listeners, save or gameplay acceptance.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
    private static void Attr(XmlWriter writer,string name,object value){writer.WriteAttributeString(name,Convert.ToString(value,System.Globalization.CultureInfo.InvariantCulture));}
}
