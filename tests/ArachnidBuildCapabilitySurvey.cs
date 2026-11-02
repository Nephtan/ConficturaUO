using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Custom.Confictura;
using Server.Custom.Confictura.Champions;

public static class ArachnidBuildCapabilitySurvey
{
    private static void Attr(XmlWriter writer,string key,object value)
    { writer.WriteAttributeString(key,Convert.ToString(value,System.Globalization.CultureInfo.InvariantCulture)); }
    private static void FoodTraits(XmlWriter writer,Item item)
    {
        Food food=item as Food;if(food!=null){Attr(writer,"food",true);Attr(writer,"fill",food.FillFactor);}
        BaseBeverage drink=item as BaseBeverage;if(drink!=null){Attr(writer,"drink",drink.Content);Attr(writer,"quantity",drink.Quantity);}
    }
    public static int Main(string[] args)
    {
        try
        {
            if(!Path.GetFullPath(Environment.CurrentDirectory).StartsWith("D:\\ConficturaUOTestTools\\",StringComparison.OrdinalIgnoreCase))throw new Exception("Private copied-save survey required");
            Core.Assembly=typeof(Core).Assembly;Core.Expansion=Expansion.SA;Core.DataDirectories.Add(args[0]);
            ScriptCompiler.Assemblies=new Assembly[]{typeof(ArachnidOutbreakController).Assembly};
            MapDefinitions.Configure();RaceDefinitions.Configure();PoisonImpl.Configure();Server.Movement.MovementImpl.Configure();Corpse.Initialize();World.Load();
            int count=0;
            using(XmlWriter writer=XmlWriter.Create("capability-survey.private.xml",new XmlWriterSettings{Indent=true}))
            {
                writer.WriteStartElement("players");
                foreach(Mobile mobile in World.Mobiles.Values)
                {
                    PlayerMobile player=mobile as PlayerMobile;if(player==null || player.Deleted || player.AccessLevel!=AccessLevel.Player)continue;
                    CharacterCapabilityProfile profile=CharacterCapabilityService.Inspect(player);CharacterLevelDiagnostics nominal=CharacterLevelService.GetDiagnostics(player);++count;
                    writer.WriteStartElement("player");Attr(writer,"serial",player.Serial.Value);Attr(writer,"name",player.Name);Attr(writer,"alive",player.Alive);Attr(writer,"blessed",player.Blessed);
                    Attr(writer,"level",profile.NominalLevel);Attr(writer,"training",profile.TrainingBand);Attr(writer,"highestBase",profile.HighestBaseSkill);Attr(writer,"bestNominal",nominal.BestArchetype);Attr(writer,"roles",String.Join(",",profile.Roles.ToArray()));
                    Attr(writer,"hits",player.Hits);Attr(writer,"maxHits",player.HitsMax);Attr(writer,"mana",player.Mana);Attr(writer,"tithing",player.TithingPoints);Attr(writer,"weaponSkill",profile.WeaponSkill);Attr(writer,"weaponSkillValue",profile.WeaponSkillValue);
                    Attr(writer,"hunger",player.Hunger);Attr(writer,"thirst",player.Thirst);
                    Attr(writer,"bandages",profile.Bandages);Attr(writer,"heals",profile.HealingPotions);Attr(writer,"cures",profile.CurePotions);Attr(writer,"immortality",profile.DeathPreventionItems);Attr(writer,"nearbyPets",profile.NearbyLivingCompanions);
                    Attr(writer,"stabledPets",player.Stabled.Count);Attr(writer,"autoReturnPets",player.AutoStabled==null ? 0 : player.AutoStabled.Count);
                    Attr(writer,"x",player.X);Attr(writer,"y",player.Y);Attr(writer,"z",player.Z);Attr(writer,"map",player.Map);
                    foreach(KeyValuePair<CharacterArchetype,int> lane in nominal.ArchetypeLevels){writer.WriteStartElement("lane");Attr(writer,"name",lane.Key);Attr(writer,"level",lane.Value);writer.WriteEndElement();}
                    for(int i=0;i<player.Skills.Length;++i)if(player.Skills[i].Base>0){writer.WriteStartElement("skill");Attr(writer,"name",player.Skills[i].SkillName);Attr(writer,"base",player.Skills[i].Base);Attr(writer,"effective",player.Skills[i].Value);writer.WriteEndElement();}
                    foreach(Item item in player.Items){writer.WriteStartElement("equipped");Attr(writer,"serial",item.Serial.Value);Attr(writer,"type",item.GetType().FullName);Attr(writer,"amount",item.Amount);writer.WriteEndElement();}
                    foreach(Item item in player.Items)
                    {
                        Container container=item as Container;if(container==null || container==player.Backpack || container is BankBox)continue;
                        foreach(Item held in container.FindItemsByType(typeof(Item),true)){writer.WriteStartElement("equippedCarried");Attr(writer,"serial",held.Serial.Value);Attr(writer,"type",held.GetType().FullName);Attr(writer,"amount",held.Amount);Attr(writer,"parent",container.Serial.Value);writer.WriteEndElement();}
                    }
                    if(player.Backpack!=null)foreach(Item item in player.Backpack.FindItemsByType(typeof(Item),true)){writer.WriteStartElement("carried");Attr(writer,"serial",item.Serial.Value);Attr(writer,"type",item.GetType().FullName);Attr(writer,"amount",item.Amount);FoodTraits(writer,item);writer.WriteEndElement();}
                    foreach(string uncertainty in profile.Uncertainty)writer.WriteElementString("uncertainty",uncertainty);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            Console.WriteLine("PASS private capability survey: "+count+" actual saved Player builds; no listeners, script global initialization, save or gameplay acceptance.");return 0;
        }
        catch(Exception error){Console.Error.WriteLine(error);return 1;}
    }
}
