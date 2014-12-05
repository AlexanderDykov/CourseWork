using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Tables;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
public class InventoryGetInfo : MonoBehaviour {

    public GameObject slots;
    int X = -115;
    int Y = 135;
    public Sprite[] sprites;
    int count = 0;


    public Image ItemImage;
    public Text amount;
    public Item item;
	void Start () 
    {
        sprites = Resources.LoadAll<Sprite>("Item");
        for (int i = 0; i <5; i++)
        {
            for (int j = 0; j < 5; j++)
            {

                GameObject slot = (GameObject)Instantiate(slots);
                if (count < DataBaseInfo.shop.Count)
                {
                    SlotScript slscr = slot.GetComponent<SlotScript>();
                    //  slscr.gameObject.SetActive(true);
                    slscr.amount.text = DataBaseInfo.shop[count].Amount.ToString();
                    Item it = DataBaseInfo.items.Single(x => x.Id == DataBaseInfo.shop[count].ItemId);
                    string[] str = it.Image.Split('_');
                    int str1 = 31 - int.Parse(str[1]) - 1;
                    slscr.ItemImage.sprite = sprites[(str1 * 14) - 1 + int.Parse(str[0])];
                    slscr.ItemImage.enabled = true;
                    slscr.amount.enabled = true;
                    Debug.Log(count);
                }
                count++;
                
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
            
   
        
           /* using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
            {
                if (manager.ConnectToDatabase())
                {
                    // DataBaseInfo.inventory = (List<Inventory>)manager.ReadByFieldName<Inventory>("PersId",DataBaseInfo.currentPersId);
                }
            }*/
	}
	
}
