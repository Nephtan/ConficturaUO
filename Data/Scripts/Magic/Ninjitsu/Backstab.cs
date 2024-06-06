using System;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.SkillHandlers;
using Server.Targeting;

namespace Server.Spells.Ninjitsu
{
    public class Backstab : NinjaMove
    {
        public Backstab() { }

        public override int BaseMana
        {
            get { return 30; }
        }
        public override double RequiredSkill
        {
            get { return Core.ML ? 40.0 : 20.0; }
        }
        public override bool BlockedByAnimalForm
        {
            get { return false; }
        }

        public override TextDefinition AbilityMessage
        {
            get { return new TextDefinition(1063089); }
        } // You prepare to Backstab your opponent.

        public override double GetDamageScalar(Mobile attacker, Mobile defender)
        {
            double ninjitsu = attacker.Skills[SkillName.Ninjitsu].Value;

            return 1.0 + (ninjitsu / 360) + Tracking.GetStalkingBonus(attacker, defender) / 100;
        }

        public override bool Validate(Mobile from)
        {
            if (!from.Hidden || from.AllowedStealthSteps <= 0)
            {
                from.SendLocalizedMessage(1063087); // You must be in stealth mode to use this ability.
                return false;
            }

            if (!from.Player)
                return true;

            if (Bushido.HonorableExecution.IsUnderPenalty(from))
            {
                from.SendLocalizedMessage(1063024); // You cannot perform this special move right now.
                return false;
            }

            return CheckSkills(from) && CheckMana(from, false);
        }

        public override bool OnBeforeSwing(Mobile attacker, Mobile defender)
        {
            bool valid = Validate(attacker) && CheckMana(attacker, true);

            if (valid)
            {
                attacker.BeginAction(typeof(Stealth));
                Timer.DelayCall(
                    TimeSpan.FromSeconds(5.0),
                    delegate
                    {
                        attacker.EndAction(typeof(Stealth));
                    }
                );
            }

            return valid;
        }

        public override bool ValidatesDuringHit
        {
            get { return false; }
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            //Validates before swing

            ClearCurrentMove(attacker);

            attacker.SendLocalizedMessage(1063090); // You quickly stab your opponent as you come out of hiding!

            defender.FixedParticles(0x37B9, 1, 5, 0x251D, 0x651, 0, EffectLayer.Waist);

            attacker.RevealingAction();

            CheckGain(attacker);
        }

        public override void OnMiss(Mobile attacker, Mobile defender)
        {
            ClearCurrentMove(attacker);

            attacker.SendLocalizedMessage(1063161); // You failed to properly use the element of surprise.

            attacker.RevealingAction();
        }
    }
}
