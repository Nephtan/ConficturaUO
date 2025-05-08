using System;
using System.Collections;
using System.Text;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Spells.Research
{
    public class ResearchRockFlesh : ResearchSpell
    {
        public override int spellIndex
        {
            get { return 10; }
        }
        public int CirclePower = 5;
        public static int spellID = 10; // Matches spellIndex, used for m_Info
        public override TimeSpan CastDelayBase
        {
            get { return TimeSpan.FromSeconds(0.5); }
        }
        public override double RequiredSkill
        {
            get
            {
                return (double)(Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 8)));
            }
        }
        public override int RequiredMana
        {
            get { return Int32.Parse(Server.Misc.Research.SpellInformation(spellIndex, 7)); }
        }

        private static SpellInfo m_Info = new SpellInfo(
            Server.Misc.Research.SpellInformation(spellID, 2),
            Server.Misc.Research.CapsCast(Server.Misc.Research.SpellInformation(spellID, 4)),
            236,
            9011,
            Reagent.MoonCrystal,
            Reagent.Garlic,
            Reagent.PigIron,
            Reagent.BlackPearl
        );

        public ResearchRockFlesh(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info) { }

        // Hashtable to store active effects. Key: Mobile, Value: ResistanceMod[]
        public static Hashtable TableStoneFlesh = new Hashtable();

        // Checks if the mobile has the Rock Flesh effect active.
        // TableStoneFlesh[m] returns null if 'm' is not a key, or if the value for 'm' is null.
        // OnCast always puts a non-null ResistanceMod[] array, so this effectively checks for key presence with a valid mod array.
        public static bool HasEffect(Mobile m)
        {
            return (TableStoneFlesh[m] != null);
        }

        // Redundant with HasEffect if TableStoneFlesh never stores null values for keys.
        // Hashtable.Contains() checks for keys.
        public static bool UnderEffect(Mobile m)
        {
            return TableStoneFlesh.ContainsKey(m); // Or TableStoneFlesh.Contains(m)
        }

        public static void RemoveEffect(Mobile m)
        {
            // FIX: Check if the mobile actually has the effect before trying to remove it.
            // HasEffect(m) checks if TableStoneFlesh[m] is not null.
            if (HasEffect(m))
            {
                // Retrieve the resistance modifications.
                // Since HasEffect(m) is true, TableStoneFlesh[m] should return a valid ResistanceMod[] array.
                ResistanceMod[] mods = (ResistanceMod[])TableStoneFlesh[m];

                // Remove the mobile's entry from the tracking table.
                TableStoneFlesh.Remove(m);

                // Reset visual and gameplay effects on the mobile.
                m.HueMod = -1;
                m.BodyMod = 0; // Reset body to default.
                m.RaceBody(); // This method typically sets the body based on the mobile's race.
                m.SendMessage("Your flesh turns back to normal.");

                // Remove the buff icon.
                BuffInfo.RemoveBuff(m, BuffIcon.RockFlesh);

                // Remove the resistance modifications.
                // This is the loop that was causing the NullReferenceException if mods was null.
                // With the HasEffect(m) check and knowing OnCast stores a non-null array, mods should be valid here.
                // A redundant null check for 'mods' can be added for extreme defensiveness, but should not be necessary
                // if OnCast logic is consistent.
                if (mods != null) // Defensive check, though HasEffect should guarantee non-null from TableStoneFlesh[m]
                {
                    for (int i = 0; i < mods.Length; ++i)
                    {
                        m.RemoveResistanceMod(mods[i]); // This was the original line 72 (approximately)
                    }
                }

                // Visual and sound effects for effect removal.
                Point3D hands = new Point3D((m.X + 1), (m.Y + 1), (m.Z + 8));
                Effects.SendLocationParticles(
                    EffectItem.Create(hands, m.Map, EffectItem.DefaultDuration),
                    0x3837, // Item ID
                    9,      // Speed
                    32,     // Duration
                    Server.Misc.PlayerSettings.GetMySpellHue(true, m, 0xB7F), // Hue
                    0,      // RenderMode
                    5022,   // Effect
                    0       // Unknown
                );
                m.PlaySound(0x65A); // Sound effect.
            }
            // else: The mobile did not have the effect active in TableStoneFlesh. No specific removal actions needed.

            // Always end the action, regardless of whether the effect was found.
            // This is important if RemoveEffect is called from OnCast's Caster.CanBeginAction check,
            // or to clear any lingering action state.
            m.EndAction(typeof(ResearchRockFlesh));
        }

        public override void OnCast()
        {
            if (CheckSequence()) // Standard spell sequence check.
            {
                // If the caster cannot begin this action (e.g., already performing it or another conflicting action),
                // attempt to remove any existing Rock Flesh effect.
                if (!Caster.CanBeginAction(typeof(ResearchRockFlesh)))
                {
                    ResearchRockFlesh.RemoveEffect(Caster);
                }

                // NOTE: The following line attempts to get existing mods, but this 'mods' variable
                // is immediately overwritten. So, its initial value doesn't affect the new application.
                // ResistanceMod[] mods = (ResistanceMod[])TableStoneFlesh[Caster];

                // Define the resistance modification for this spell.
                ResistanceMod[] newMods = new ResistanceMod[1] { new ResistanceMod(ResistanceType.Physical, 90) };

                // Store the new modifications in the tracking table.
                TableStoneFlesh[Caster] = newMods;

                // Apply the resistance modifications to the caster.
                for (int i = 0; i < newMods.Length; ++i)
                    Caster.AddResistanceMod(newMods[i]);

                // Calculate spell duration.
                double totalTime = DamagingSkill(Caster) * 4; // Assuming DamagingSkill returns a relevant skill value.
                
                // Start the timer to remove the effect after duration expires.
                new InternalTimer(Caster, TimeSpan.FromSeconds(totalTime)).Start();

                // Apply visual changes to the caster.
                Caster.BodyMod = 14; // Changes caster's body model.
                Caster.HueMod = 0xB31; // Changes caster's hue.

                // Remove any existing buff icon for RockFlesh and add the new one.
                BuffInfo.RemoveBuff(Caster, BuffIcon.RockFlesh);
                BuffInfo.AddBuff(
                    Caster,
                    new BuffInfo(
                        BuffIcon.RockFlesh, // Icon
                        1063652,            // Title Cliloc
                        1063653,            // Secondary Cliloc (duration/description)
                        TimeSpan.FromSeconds(totalTime), // Duration
                        Caster              // Optional: Mobile associated with the buff
                    )
                );

                // Dismount the caster if they are mounted.
                IMount mount = Caster.Mount;
                if (mount != null)
                {
                    Server.Mobiles.EtherealMount.EthyDismount(Caster); // Handles ethereal mounts.
                    mount.Rider = null; // Standard dismount.
                }

                Caster.SendMessage("Your flesh turns to stone.");

                // Consume scroll if applicable.
                // Assuming 'alwaysConsume' is a class member or appropriately defined.
                // bool alwaysConsume = true; // Example, ensure this is correctly set.
                Server.Misc.Research.ConsumeScroll(Caster, true, spellID, Scroll != null, Scroll); // Adjusted alwaysConsume logic

                // Apply karma modification.
                KarmaMod(Caster, ((int)RequiredSkill + RequiredMana));

                // Visual and sound effects for spell casting.
                Point3D hands = new Point3D((Caster.X + 1), (Caster.Y + 1), (Caster.Z + 8));
                Effects.SendLocationParticles(
                    EffectItem.Create(hands, Caster.Map, EffectItem.DefaultDuration),
                    0x3837,
                    9,
                    32,
                    Server.Misc.PlayerSettings.GetMySpellHue(true, Caster, 0xB7F),
                    0,
                    5022,
                    0
                );
                Caster.PlaySound(0x65A);
            }

            FinishSequence(); // Standard spell sequence finalization.
        }

        // Timer to handle the expiration of the Rock Flesh effect.
        private class InternalTimer : Timer
        {
            private Mobile m_Mobile; // The mobile affected by the spell.
            private DateTime m_ExpireTime; // When the spell effect should expire.

            public InternalTimer(Mobile caster, TimeSpan duration)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.1)) // Timer ticks frequently to check expiration.
            {
                m_Mobile = caster;
                m_ExpireTime = DateTime.Now + duration;
                Priority = TimerPriority.FiftyMS; // Typical priority for spell timers.
            }

            // Called on each timer tick.
            protected override void OnTick()
            {
                if (DateTime.Now >= m_ExpireTime)
                {
                    ResearchRockFlesh.RemoveEffect(m_Mobile); // Remove the effect.
                    Stop(); // Stop the timer.
                }
                // Additional check: if mobile is null, dead, or deleted, stop the timer.
                else if (m_Mobile == null || m_Mobile.Deleted || !m_Mobile.Alive)
                {
                    ResearchRockFlesh.RemoveEffect(m_Mobile); // Attempt to clean up just in case.
                    Stop();
                }
                // Also, ensure the effect is still considered active in the TableStoneFlesh.
                // If not, the timer is orphaned and should stop.
                else if (!ResearchRockFlesh.HasEffect(m_Mobile))
                {
                     // No need to call RemoveEffect again if it's already gone from the table.
                    Stop();
                }
            }
        }
    }
}