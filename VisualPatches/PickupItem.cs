using System.Collections.Generic;
using UnityEngine;
using CoDArchipelago.GlobalGameScene;
using CoDArchipelago.Collecting;

namespace CoDArchipelago.VisualPatches
{
    class PickupItem : ObjectPatcher
    {
        public static readonly new Replaces replaces = new(CollectibleItem.CollectibleType.ITEM);

        readonly GameObject fishFoodObject;
        readonly GameObject[] ladyOpalEggObjects;
        readonly Dictionary<string, GameObject> replacementMap;

        public override GameObject Replace(GameObject toReplace, Item item) =>
            ReplaceWith(toReplace, replacementMap[item.GetFlag()]);

        public override void CollectJingle()
        {
            StockSFX.Instance.jingleGoodShort.Play();
            GlobalHub.Instance.player.curiousSFX.Play();
        }

        public PickupItem()
        {
            fishFoodObject = CreateFishFoodObject();
            ladyOpalEggObjects = CreateLadyOpalEggObjects();

            replacementMap = new() {
                {"ITEM_FISH_FOOD", fishFoodObject},
                {"ITEM_PRINCESS_1", ladyOpalEggObjects[0]},
                {"ITEM_PRINCESS_2", ladyOpalEggObjects[1]},
                {"ITEM_PRINCESS_3", ladyOpalEggObjects[2]},
            };
        }

        GameObject[] CreateLadyOpalEggObjects()
        {
            Transform ladyOpalEggsHolder = GameScene.FindInScene("PALACE", "Valley (Main)/Collectibles/PrincessCollectiblesHolder");
            GameObject[] ladyOpalEggObjects = new GameObject[3];
            Component.DestroyImmediate(ladyOpalEggsHolder.GetComponent<TwoState>());
            for (int i = 0; i < 3; i++) {
                ladyOpalEggObjects[i] = GameObject.Instantiate(ladyOpalEggsHolder.GetChild(i).gameObject, Container);
                ladyOpalEggObjects[i].name = "LadyOpalEgg" + (i + 1);
                Collectible col = ladyOpalEggObjects[i].GetComponent<Collectible>();
                col.cutscene = ItemCutscene;
                col.type = Collectible.CollectibleType.ITEM;
            }

            return ladyOpalEggObjects;
        }

        GameObject CreateFishFoodObject()
        {
            Transform fishFoodHolder = GameScene.FindInScene("LAKE", "Bedroom/Collectibles/FishFoodHolder");
            GameObject fishFoodObject = GameObject.Instantiate(fishFoodHolder.Find("Fish Food").gameObject, Container);
            fishFoodObject.transform.position = new Vector3() {x = 0, y = 0, z = 0};
            fishFoodObject.name = "FishFood";
            CollectibleItem collectibleItem = fishFoodObject.GetComponent<CollectibleItem>();
            collectibleItem.model = fishFoodObject.transform.Find("FishfoodHolder").gameObject;
            collectibleItem.type = Collectible.CollectibleType.ITEM;
            collectibleItem.cutscene = ItemCutscene;
            return fishFoodObject;
        }
    }
}
