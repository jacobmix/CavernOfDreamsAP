using UnityEngine;
using System.Collections.Generic;
using System;
using CoDArchipelago.Collecting;
using CoDArchipelago.GlobalGameScene;

namespace CoDArchipelago.LocationSplitPatches
{
    static class GratitudeTeleports
    {
        public static bool IsSunCavernPortal(Transform t)
        {
            var path = t.GetPath().Substring(1);

            foreach (var info in gratitudeTeleportInfos) {
                if (info.sunCavernTeleportPath == path) return true;
                if (info.otherTeleportPath == path) return true;
            }

            return false;
        }

        readonly struct GratitudeTeleportInfo
        {
            public readonly string gratitudeFlag;
            public readonly string teleportFlag;
            public readonly string sunCavernTeleportPath;
            public readonly string otherTeleportPath;

            public GratitudeTeleportInfo(
                string gratitudeFlag,
                string teleportFlag,
                string teleportName
            ) {
                this.gratitudeFlag = gratitudeFlag;
                this.teleportFlag = teleportFlag;
                this.sunCavernTeleportPath = $"CAVE/Sun Cavern (Main)/Fellas/Nest FellaHatchable {teleportName}/Portal";
                this.otherTeleportPath = $"CAVE/{teleportName} Lobby/Warps/Portal";
            }
        }

        static readonly List<GratitudeTeleportInfo> gratitudeTeleportInfos = new() {
            new("GRATITUDE1", "TELEPORT_LAKE",    "Lake"),
            new("GRATITUDE2", "TELEPORT_MONSTER", "Monster"),
            new("GRATITUDE3", "TELEPORT_PALACE",  "Palace"),
            new("GRATITUDE4", "TELEPORT_GALLERY", "Gallery"),
        };

        public static class LinkGratitudeWithTeleports
        {
            static Action<bool> SetTeleportFlagFactory(string teleportFlag) =>
                gratitudeRandomized => new MyItem(teleportFlag, randomized: true).Collect();

            static void RegisterGratitudeTeleportLink(string gratitudeFlag, string teleportFlag) =>
                MyItem.RegisterTrigger(gratitudeFlag, SetTeleportFlagFactory(teleportFlag));

            public static void RegisterLinks()
            {
                foreach (var info in gratitudeTeleportInfos) {
                    RegisterGratitudeTeleportLink(info.gratitudeFlag, info.teleportFlag);
                }
            }
        }

        class PatchTeleports : InstantiateOnGameSceneLoad
        {
            static Action<bool> ActivatePortalFactory(Transform nestPortal, Transform otherPortal)
            {
                return randomized => {
                    Area area = GlobalHub.Instance.GetArea();

                    if (area.name == "Sun Cavern (Main)") {
                        ShowPortal(nestPortal);
                    } else if (area.ContainsComponentInChildren(otherPortal.GetComponent<WarpTrigger>(), includeInactive: true)) {
                        ShowPortal(otherPortal);
                    }
                };
            }

            static void InitializePortal(Transform portal, string flag)
            {
                portal.GetComponent<TwoState>().flag = flag;
                Transform modelHolder = portal.Find("PortalModelHolder");
                modelHolder.GetComponent<TwoState>().flag = flag;
            }

            static void ShowPortal(Transform portal)
            {
                portal.gameObject.SetActive(true);
                Transform modelHolder = portal.Find("PortalModelHolder");
                modelHolder.GetComponent<Activation>().Activate();
            }

            [LoadOrder(Int32.MinValue+1)]
            public PatchTeleports()
            {
                foreach (var info in gratitudeTeleportInfos) {
                    var nestPortal = GameScene.FindInSceneFullPath(info.sunCavernTeleportPath);
                    var otherPortal = GameScene.FindInSceneFullPath(info.otherTeleportPath);

                    InitializePortal(nestPortal, info.teleportFlag);
                    InitializePortal(otherPortal, info.teleportFlag);

                    MyItem.RegisterTrigger(info.teleportFlag, ActivatePortalFactory(nestPortal, otherPortal));
                }

                if (!APClient.Client.SlotData.splitGratitudeAndTeleports) {
                    LinkGratitudeWithTeleports.RegisterLinks();
                }
            }
        }
    }
}
