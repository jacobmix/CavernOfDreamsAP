using HarmonyLib;
using System;
using UnityEngine;
using CoDArchipelago.GlobalGameScene;

namespace CoDArchipelago.NoFun
{
    class NoFunInDeepWoods : InstantiateOnGameSceneLoad
    {
        public static bool enabled = true;

        static bool wasInDeepWoodsLastFrame = false;

        class DeepWoodsFunChecker : MonoBehaviour
        {
            void Update()
            {
                var player = GlobalHub.Instance.player;

                Vector3 position = player.transform.localPosition;
                bool isInDeepWoods = position.z > 35 && position.x < -50;

                if (isInDeepWoods == wasInDeepWoodsLastFrame) return;
                wasInDeepWoodsLastFrame = isInDeepWoods;

                if (isInDeepWoods) {
                    player.curiousSFX.Play();
                    NoFunInDeepWoods.enabled = false;
                } else {
                    player.whimperSFX.Play();
                    NoFunInDeepWoods.enabled = true;
                }
            }
        }

        [HarmonyPatch(typeof(WarpTrigger), "Warp")]
        static class OnAreaLoad
        {
            static void Postfix(WarpTrigger __instance)
            {
                if (wasInDeepWoodsLastFrame) {
                    NoFunInDeepWoods.enabled = false;
                    wasInDeepWoodsLastFrame = false;
                    GlobalHub.Instance.player.whimperSFX.Play();
                }
            }
        }

        [LoadOrder(Int32.MaxValue)]
        public NoFunInDeepWoods()
        {
            if (!NoFun.enabled) return;

            var sign = PlaceNoFunSign();

            sign.AddComponent<DeepWoodsFunChecker>();

            MiscPatches.DialogPatches.RegisterDynamicDialogPatch(
                "/DialogBaseCutscene(Clone)/DialogEvent",
                DeepWoodsNoFunSign
            );
        }

        static string DeepWoodsNoFunSign(Dialog dialog)
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

        static GameObject PlaceNoFunSign()
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

        // static readonly string[] noteRingShrooms = new string[] {
        //     "NoteLake",
        //     "NoteLake (1)",
        //     "NoteLake (2)",
        //     "NoteLake (3)",
        //     "NoteLake (4)",
        //     "NoteLake (5)",
        // };

        // static void PlaceNoFunPlatforms()
        // {
        //     var container = GameScene.FindInScene("LAKE", "Lake (Main)/lake2");
        //     var platform = GameObject.Instantiate(container.Find("DeepWoods_Gate"));
        //     Component.DestroyImmediate(platform.GetComponent<RotateActivation>());
        //     Component.DestroyImmediate(platform.GetComponent<InteractText>());

        //     platform.localRotation = Quaternion.Euler(0, 0, 270);
        //     platform.localScale = new(0.5f, 0.5f, 0.5f);

        //     var shroomsContainer = GameScene.FindInScene("LAKE", "Lake (Main)/Collectibles/Notes/NoteRingCircle");
        //     foreach (var shroomName in noteRingShrooms) {
        //         var shroom = shroomsContainer.Find(shroomName);
        //         var shroomPlatform = GameObject.Instantiate(platform.gameObject, shroomsContainer);
        //         shroomPlatform.transform.localPosition = new(
        //             shroom.localPosition.x,
        //             shroom.localPosition.y - 0.5f,
        //             shroom.localPosition.z + 0.5f
        //         );
        //     }

        //     GameObject.DestroyImmediate(platform.gameObject);
        // }
    }
}
