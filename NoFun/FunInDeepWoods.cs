using HarmonyLib;
using System;
using UnityEngine;
using CoDArchipelago.GlobalGameScene;

namespace CoDArchipelago.NoFun
{
    class FunInDeepWoods : InstantiateOnGameSceneLoad
    {
        public static bool insideDeepWoods = false;

        static bool wasInDeepWoodsLastFrame = false;

        class DeepWoodsFunChecker : MonoBehaviour
        {
            void Update()
            {
                var player = GlobalHub.Instance.player;

                Vector3 position = player.transform.localPosition;
                insideDeepWoods = position.z > 35 && position.x < -50 && position.y < 11;

                if (insideDeepWoods == wasInDeepWoodsLastFrame) return;
                wasInDeepWoodsLastFrame = insideDeepWoods;

                if (insideDeepWoods) {
                    player.curiousSFX.Play();
                } else {
                    player.whimperSFX.Play();
                }
            }
        }

        [HarmonyPatch(typeof(WarpTrigger), "Warp")]
        static class OnAreaLoad
        {
            static void Postfix(WarpTrigger __instance)
            {
                if (wasInDeepWoodsLastFrame) {
                    insideDeepWoods = false;
                    wasInDeepWoodsLastFrame = false;
                    GlobalHub.Instance.player.whimperSFX.Play();
                }
            }
        }

        [LoadOrder(Int32.MaxValue)]
        public FunInDeepWoods()
        {
            if (APClient.Client.SlotData.allowFun) return;

            var sign = PlaceFunSign();

            sign.AddComponent<DeepWoodsFunChecker>();

            MiscPatches.DialogPatches.RegisterDynamicDialogPatch(
                "/DialogBaseCutscene(Clone)/DialogEvent",
                DeepWoodsFunSign
            );
        }

        static string DeepWoodsFunSign(Dialog dialog)
        {
            if (isNoFunSign) {
                isNoFunSign = false;
                return "Hey, Fynn! In this area, and this area only, you can carry items while wearing the Jester Boots. Got it?";
            }
            return dialog.GetText();
        }

        static InteractSign noFunSign = null;
        static bool isNoFunSign = false;

        [HarmonyPatch(typeof(InteractSign), nameof(InteractSign.Interact))]
        static class SignInteractPatch
        {
            static void Prefix(InteractSign __instance)
            {
                if (__instance == noFunSign) {
                    isNoFunSign = true;
                }
            }
        }

        static GameObject PlaceFunSign()
        {
            var container = GameScene.FindInScene("LAKE", "Lake (Main)/lake2");

            var signOriginal = GameScene.FindInScene("GALLERY", "Foyer (Main)/foyer4/Chandelier/SignGallerySecret");
            var sign = GameObject.Instantiate(signOriginal.gameObject, container);

            noFunSign = sign.GetComponent<InteractSign>();
            sign.transform.localPosition = new(-45f, -3f, -50.25f);
            sign.transform.localRotation = Quaternion.Euler(0, 320, 0);

            sign.transform.Find("sign_secret/Plane").GetComponent<MeshRenderer>().material.mainTexture = APResources.apMinorTexture;

            return sign;
        }
    }
}
