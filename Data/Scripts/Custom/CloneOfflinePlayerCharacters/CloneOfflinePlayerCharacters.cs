/*************************************************************
 * File: CloneOfflinePlayerCharacters.cs
 * Description: Entry point that manages the lifecycle of offline
 *              player character clones.
 *************************************************************/
using Server.Mobiles;
using Server.Regions;
using System;
using System.Collections.Generic;

namespace Server.Custom.Confictura.CloneOfflinePlayerCharacters
{
    /// <summary>
    /// Coordinates clone creation and removal when players log in or out
    /// and offers utility helpers for mass clone synchronization.
    /// </summary>
    public static class CloneOfflinePlayerCharacters
    {
        public static void Initialize()
        {
            EventSink.Logout += OnLogout;
            EventSink.Login += OnLogin;
            CheckFirstRun();
        }

        public static void CreateCloneOf(Mobile mobile)
        {
            PlayerMobile playerMobile = mobile as PlayerMobile;

            if (
                !(mobile is CharacterClone)
                && playerMobile != null
                && playerMobile.Alive
                && playerMobile.AccessLevel == AccessLevel.Player
                && IsValidRegion(playerMobile)
            )
            {
                CharacterClone characterClone = CloneThings.CreateClone(mobile);
                CloneThings.CloneMobileProperties(mobile, characterClone);
                CloneThings.CloneMobileItems(mobile, characterClone);
                CloneThings.CloneMobileBackpack(mobile, characterClone);
                CloneThings.CloneMobileMount(mobile, characterClone);
            }
        }

        public static void DeleteClonesOf(Mobile mobile)
        {
            foreach (Mobile existing in new List<Mobile>(World.Mobiles.Values))
            {
                CharacterClone clone = existing as CharacterClone;

                if (clone != null && clone.Original == mobile)
                {
                    IMount mount = existing.Mount;

                    if (mount != null)
                    {
                        mount.Rider = null;

                        if (mount is EtherealMount)
                        {
                            ((EtherealMount)mount).Delete();
                        }
                        else if (mount is BaseMount)
                        {
                            ((BaseMount)mount).Delete();
                        }
                    }

                    existing.Delete();
                }
            }
        }

        public static void CheckFirstRun()
        {
            int totalClones = 0;
            int processedClones = 0;

            Dictionary<PlayerMobile, CharacterClone> existingClones =
                new Dictionary<PlayerMobile, CharacterClone>();

            foreach (Mobile mobile in new List<Mobile>(World.Mobiles.Values))
            {
                CharacterClone clone = mobile as CharacterClone;

                if (clone != null)
                {
                    PlayerMobile originalPlayer = clone.Original as PlayerMobile;

                    if (originalPlayer != null)
                    {
                        existingClones[originalPlayer] = clone;
                    }

                    if (clone.RaceID != 0)
                    {
                        clone.HueMod = 0;
                    }
                }
            }

            foreach (Mobile mobile in new List<Mobile>(World.Mobiles.Values))
            {
                PlayerMobile playerMobile = mobile as PlayerMobile;

                if (
                    playerMobile != null
                    && playerMobile.Alive
                    && playerMobile.AccessLevel == AccessLevel.Player
                    && !existingClones.ContainsKey(playerMobile)
                )
                {
                    playerMobile.Map = playerMobile.LogoutMap;
                    playerMobile.Location = playerMobile.LogoutLocation;

                    playerMobile.X += 1;
                    playerMobile.X -= 1;

                    if (IsValidRegion(playerMobile))
                    {
                        totalClones++;
                    }
                }
            }

            Console.Write("Cloning Offline Players... ");

            foreach (Mobile mobile in new List<Mobile>(World.Mobiles.Values))
            {
                PlayerMobile playerMobile = mobile as PlayerMobile;

                if (
                    playerMobile != null
                    && playerMobile.Alive
                    && playerMobile.AccessLevel == AccessLevel.Player
                    && !existingClones.ContainsKey(playerMobile)
                )
                {
                    CreateCloneOf(playerMobile);

                    if (IsValidRegion(playerMobile))
                    {
                        processedClones++;
                    }

                    Console.CursorLeft = 27;
                    Console.Write(string.Format("{0}/{1}", processedClones, totalClones));
                }
            }

            Console.WriteLine();

            foreach (Mobile mobile in new List<Mobile>(World.Mobiles.Values))
            {
                CharacterClone clone = mobile as CharacterClone;

                if (
                    clone != null
                    && (
                        mobile.Region.IsPartOf(typeof(StartRegion))
                        || mobile.Region.IsPartOf(typeof(PublicRegion))
                        || mobile.Region.IsPartOf(typeof(CrashRegion))
                        || mobile.Region.IsPartOf(typeof(PrisonArea))
                        || mobile.Region.IsPartOf(typeof(SafeRegion))
                    )
                )
                {
                    mobile.Delete();
                }
            }

            foreach (Mobile mobile in new List<Mobile>(World.Mobiles.Values))
            {
                PlayerMobile playerMobile = mobile as PlayerMobile;

                if (
                    playerMobile != null
                    && playerMobile.Alive
                    && playerMobile.AccessLevel == AccessLevel.Player
                )
                {
                    playerMobile.Location = playerMobile.LogoutLocation;
                    playerMobile.Map = Map.Internal;
                }
            }
        }

        public static bool IsValidRegion(PlayerMobile playerMobile)
        {
            return !(
                playerMobile.Region.IsPartOf(typeof(StartRegion))
                || playerMobile.Region.IsPartOf(typeof(PublicRegion))
                || playerMobile.Region.IsPartOf(typeof(CrashRegion))
                || playerMobile.Region.IsPartOf(typeof(PrisonArea))
                || playerMobile.Region.IsPartOf(typeof(SafeRegion))
            );
        }

        private static void OnLogout(LogoutEventArgs e)
        {
            PlayerMobile playerMobile = e.Mobile as PlayerMobile;

            if (
                !(e.Mobile is CharacterClone)
                && playerMobile != null
                && playerMobile.Alive
                && playerMobile.AccessLevel == AccessLevel.Player
                && IsValidRegion(playerMobile)
            )
            {
                CharacterClone characterClone = CloneThings.CreateClone(e.Mobile);
                CloneThings.CloneMobileProperties(e.Mobile, characterClone);
                CloneThings.CloneMobileItems(e.Mobile, characterClone);
                CloneThings.CloneMobileBackpack(e.Mobile, characterClone);
                CloneThings.CloneMobileMount(e.Mobile, characterClone);
            }
        }

        private static void OnLogin(LoginEventArgs e)
        {
            DeleteClonesOf(e.Mobile);
        }
    }
}
