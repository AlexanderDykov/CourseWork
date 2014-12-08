using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Tables;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
public class InventoryGetInfo : MonoBehaviour
{

    public GameObject slots;
    int X = -115;
    int Y = 135;
    public Sprite[] sprites;
    public static int count = 0;
    List<GameObject> fff1 = new List<GameObject>();
    public static List<GameObject> addingItems;
    void Start()
    {
        ///remove
        ///
        // DataBaseInfo.currentInventory = DataBaseInfo.shop;
        ///
        sprites = Resources.LoadAll<Sprite>("Item");
        #region Inventory
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {

                GameObject slot = (GameObject)Instantiate(slots);
                fff1.Add(slot);
                if (DataBaseInfo.currentInventory != null && count < DataBaseInfo.currentInventory.Count )
                {
                    if (DataBaseInfo.currentInventory[count].Amount > 0)
                    {
                        SlotScript slscr = slot.GetComponent<SlotScript>();
                        //  slscr.gameObject.SetActive(true);
                        slscr.amount.text = DataBaseInfo.currentInventory[count].Amount.ToString();
                        Item it = DataBaseInfo.items.FirstOrDefault(x => x.Id == DataBaseInfo.currentInventory[count].ItemId);
                        Condition con = DataBaseInfo.conditions.FirstOrDefault(x => x.Id == it.ConditionId);
                        Race rac = DataBaseInfo.races.FirstOrDefault(x => x.Id == con.RaceId);

                        string[] str = it.Image.Split('_');
                        int str1 = 31 - int.Parse(str[1]) - 1;
                        slscr.ItemImage.sprite = sprites[(str1 * 14) - 1 + int.Parse(str[0])];
                        slscr.ItemImage.enabled = true;
                        slscr.race = rac;
                        slscr.lvl = con.Level;
                        slscr.ItemId = it.Id;
                        slscr.type = DataBaseInfo.types.FirstOrDefault(x => x.Id == it.ItemTypeId);
                        slscr.amount.enabled = true;
                    }
                    count++;
                }


                slot.transform.parent = this.transform;
                slot.name = "Slot " + i + "x" + j;
                slot.GetComponent<RectTransform>().localPosition = new Vector3(X, Y, 0);
                X += 60;
                if (j == 4)
                {
                    X = -115;
                    Y -= 60;
                }

            }
        }
        #endregion
        addingItems = fff1;
    }

}
