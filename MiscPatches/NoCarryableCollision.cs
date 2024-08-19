using UnityEngine;
using CoDArchipelago.GlobalGameScene;
using HarmonyLib;
using System;
using System.Linq;

namespace CoDArchipelago.MiscPatches
{
    class NoCarryableCollision : InstantiateOnGameSceneLoad
    {
        [HarmonyPatch(typeof(Player), "PickUpObject")]
        static class OnPickUpCarryable
        {
            static void Postfix(Player __instance, Carryable carryable)
            {
                if (!NoFun.NoJesterBootsCarry.IsFunAllowed() && GlobalHub.Instance.player.WearingHoverBoots()) return;

                SetCollision(carryable.gameObject, true);
            }
        }

        enum CarryableType {
            JesterBoots,
            Medicine,
            Apple,
            BubbleConch,
            SagesGloves,
            KerringtonsWings,
            ShelnertsFish,
            LadyOpalsHead,
        };

        static CarryableType GetCarryableType(GameObject prefab)
        {
            if (prefab.GetComponent<Collectible>() is HoverBoots) return CarryableType.JesterBoots;
            var carryable = prefab.GetComponent<Carryable>();
            if (carryable is HangGlider) return CarryableType.KerringtonsWings;
            if (carryable is Seed) return CarryableType.Apple;
            if (carryable.type == Whackable.WhackType.POTION) return CarryableType.Medicine;
            if (carryable.type == Whackable.WhackType.PAINTING_ITEM_SAGE) return CarryableType.SagesGloves;
            if (carryable.type == Whackable.WhackType.PAINTING_ITEM_KAPPA) return CarryableType.ShelnertsFish;
            if (carryable.type == Whackable.WhackType.PAINTING_ITEM_PRINCESS) return CarryableType.LadyOpalsHead;
            if (carryable.TryGetComponent<Torpedo>(out var _)) return CarryableType.BubbleConch;

            throw new Exception($"Unknown carryable prefab {prefab.transform.GetPath()}");
        }

        public static void SetCollision(GameObject prefab, bool enable)
        {
            SphereCollider sphere;
            var carryableType = GetCarryableType(prefab);
            Debug.LogWarning(carryableType);
            switch (carryableType) {
                case CarryableType.JesterBoots:
                    return;

                case CarryableType.SagesGloves:
                case CarryableType.KerringtonsWings:
                case CarryableType.ShelnertsFish:
                case CarryableType.LadyOpalsHead:
                case CarryableType.Medicine:
                    sphere = prefab.GetComponent<SphereCollider>();
                    sphere.isTrigger = !enable;
                    return;

                case CarryableType.Apple:
                case CarryableType.BubbleConch:
                    sphere = prefab.GetComponents<SphereCollider>().First(x => !x.isTrigger);
                    sphere.enabled = enable;
                    return;
            }
        }

        public NoCarryableCollision()
        {
            RemoveAllTreeComponents();
        }

        ///<summary>
        ///These only serve to drop the item they're holding.
        ///Fynn can grab carryables in the air now, so there's no need.
        ///(Also saves the headache of physics on all items)
        ///</summary>
        static void RemoveAllTreeComponents()
        {
            foreach (Tree tree in GameScene.GetComponentsInChildren<Tree>(true)) {
                Component.DestroyImmediate(tree);
            }
        }
    }
}
