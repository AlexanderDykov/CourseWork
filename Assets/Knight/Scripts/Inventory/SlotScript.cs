using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Tables;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

public class SlotScript : MonoBehaviour, IPointerDownHandler{

    public Image ItemImage;
    public Text amount;
  //  public Item item;
    public int ItemId = -1;
    public ItemType type;
    public Race race;
    public int lvl;

    void Swap<T>(ref T a, ref T b)
    {
        T temp;
        temp = a;
        a = b;
        b = temp;
    }

    int GetIndex(List<Inventory> inv, int ItemId)
    {
        for(int i=0; i< inv.Count;i++)
        {
            if (inv[i].ItemId == ItemId) return i;
        }
        return -1;
    }

    void Puckik(SlotScript slot1, SlotScript slot2)
    {
        if (InventoryGetInfo.count < 25)
        {
            //Item Image
            if (int.Parse(slot2.amount.text) > 1)
            {
                // DataBaseInfo.currentInventory.Sin
                int index = GetIndex(DataBaseInfo.currentInventory, slot1.ItemId);
                if (index == -1)
                {
                    SlotScript aa = InventoryGetInfo.addingItems[InventoryGetInfo.count].GetComponent<SlotScript>();
                    aa.ItemImage.sprite = slot1.ItemImage.sprite;
                    aa.ItemId = slot1.ItemId;
                    aa.ItemImage.enabled = true;
                    aa.type = slot1.type;
                    aa.amount.enabled = true;
                    InventoryGetInfo.count++;
                }
                else
                {
                    InventoryGetInfo.addingItems[index].GetComponent<SlotScript>().amount.text = (int.Parse(InventoryGetInfo.addingItems[index].GetComponent<SlotScript>().amount.text) + 1).ToString();
                    DataBaseInfo.currentInventory[index].Amount++;
                }
                slot1.ItemId = slot2.ItemId;
                slot1.ItemImage.sprite = slot2.ItemImage.sprite;
                slot2.amount.text = (int.Parse(amount.text) - 1).ToString();
            }
            else
            {
                //ItemID
                Swap<int>(ref this.ItemId, ref EquipedGetInfo.melleWeapone.ItemId);
                Sprite temp = new Sprite();
                temp = ItemImage.sprite;
                ItemImage.sprite = EquipedGetInfo.melleWeapone.ItemImage.sprite;
                EquipedGetInfo.melleWeapone.ItemImage.sprite = temp;
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (type!=null && ItemId != 0 && lvl <= PlayerStats.level && (race == PlayerStats.race || race.RaceName == "No restriction"))
        {
            switch(type.Type)
            {
                case "Melee weapon":
                    if (EquipedGetInfo.melleWeapone.ItemId != this.ItemId)                    
                    {
                        Puckik(EquipedGetInfo.melleWeapone,this);
                    }
                    break;
                case "Ranged weapon":
                   // EquipedGetInfo.rangeWeapone.ItemImage.sprite = this.ItemImage.sprite;
                   // Debug.Log(ItemId);
                    if (EquipedGetInfo.rangeWeapone.ItemId != this.ItemId)
                    {
                        Puckik(EquipedGetInfo.rangeWeapone, this);
                    }
                    break;
                case "Clothes":
                    if (EquipedGetInfo.clothes.ItemId != this.ItemId)
                    {
                        Puckik(EquipedGetInfo.clothes, this);
                    }
                    break;
            }

        }
        else
        {
            Debug.Log("Could not equip");
        }
        //Debug.Log(ItemId);
       // throw new System.NotImplementedException();
    }

}
