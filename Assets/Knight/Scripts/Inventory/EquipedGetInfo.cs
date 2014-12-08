using UnityEngine;
using System.Collections;
using Tables;
using System.Linq;

public class EquipedGetInfo : MonoBehaviour
{
    public SlotScript clothes1;
    public SlotScript melleWeapone1;
    public SlotScript rangeWeapone1;

    public static SlotScript clothes;
    public static SlotScript melleWeapone;
    public static SlotScript rangeWeapone;
    public Sprite[] sprites;
    void Start()
    {
        clothes = clothes1;
        melleWeapone = melleWeapone1;
        rangeWeapone = rangeWeapone1;
        ///remove
        ///
        // DataBaseInfo.currentInventory = DataBaseInfo.shop;
        ///
        sprites = Resources.LoadAll<Sprite>("Item");
        #region EqupedItems
        Item it1 = DataBaseInfo.items.FirstOrDefault(x => x.Id == DataBaseInfo.currentEquipedItems.Armor);
        string[] str = it1.Image.Split('_');
        int str1 = 31 - int.Parse(str[1]) - 1;
        clothes.ItemImage.sprite = sprites[(str1 * 14) - 1 + int.Parse(str[0])];
        clothes.ItemImage.enabled = true;
        clothes.ItemId = it1.Id;
        clothes.type = DataBaseInfo.types.FirstOrDefault(x => x.Id == it1.ItemTypeId);

        it1 = DataBaseInfo.items.FirstOrDefault(x => x.Id == DataBaseInfo.currentEquipedItems.Sword);
        str = it1.Image.Split('_');
        str1 = 31 - int.Parse(str[1]) - 1;
        melleWeapone.ItemImage.sprite = sprites[(str1 * 14) - 1 + int.Parse(str[0])];
        melleWeapone.ItemImage.enabled = true;
        melleWeapone.ItemId = it1.Id;
        melleWeapone.type = DataBaseInfo.types.FirstOrDefault(x => x.Id == it1.ItemTypeId);


        it1 = DataBaseInfo.items.FirstOrDefault(x => x.Id == DataBaseInfo.currentEquipedItems.Bow);
        str = it1.Image.Split('_');
        str1 = 31 - int.Parse(str[1]) - 1;
        rangeWeapone.ItemImage.sprite = sprites[(str1 * 14) - 1 + int.Parse(str[0])];
        rangeWeapone.ItemImage.enabled = true;
        rangeWeapone.ItemId = it1.Id;
        rangeWeapone.type = DataBaseInfo.types.FirstOrDefault(x => x.Id == it1.ItemTypeId);
        #endregion

    }
}
