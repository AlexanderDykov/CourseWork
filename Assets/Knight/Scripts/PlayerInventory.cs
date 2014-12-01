using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class PlayerInventory : MonoBehaviour {
    List<Item> items = new List<Item>();
    List<Inventory> inventory = new List<Inventory>();
    List<Item> inventoryItems = new List<Item>();
    Item[] equiped = new Item[5];
    bool showInventory = false;

    PlayerBehaviour player;
	// Use this for initialization
	void Start () 
    {
        player = gameObject.GetComponent<PlayerBehaviour>();
        foreach(var item in items)
        {
            foreach(var inv in inventory)
            {
                if (item.Id == inv.Id)
                    inventoryItems.Add(item);
            }
        }
        inventoryItems.Add(new Item() { Id = 0, Name = "Sword1", Stats = 5, SpeedAttack = 3f, Description = "", Image = "1_25", ItemTypeId = 0, BuyPrice = 10, SellPrice = 8, ConditionId = 0 });
        inventoryItems.Add(new Item() { Id = 1, Name = "Sword2", Stats = 10, SpeedAttack = 3f, Description = "", Image = "2_25", ItemTypeId = 0, BuyPrice = 15, SellPrice = 12, ConditionId = 1 });
        inventoryItems.Add(new Item() { Id = 2, Name = "Sword3", Stats = 15, SpeedAttack = 3f, Description = "", Image = "3_25", ItemTypeId = 0, BuyPrice = 12, SellPrice = 10, ConditionId = 2 });
        inventoryItems.Add(new Item() { Id = 3, Name = "Меч4", Stats = 20, SpeedAttack = 3f, Description = "", Image = "4_25", ItemTypeId = 0, BuyPrice = 5, SellPrice = 2, ConditionId = 0 });
       
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public Texture2D image;
    Vector2 scrollPos;
    void OnGUI()
    {
        if(GUI.Button(new Rect(10,10,50,50),"Inventory"))
        {
            showInventory = !showInventory;
        }
        if(showInventory)
        {

            GUILayout.BeginArea(new Rect(0, 0, 200, 150));
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos,false,true);
                {
                    for (int i = 0; i < inventoryItems.Count; i++)
                    {
                        string[] s = inventoryItems[i].Image.Split('_');
                        if (GUI.Button(new Rect(68, i * 34 + 34, 100, 34), inventoryItems[i].Name))
                        {

                        }
                        GUI.DrawTextureWithTexCoords(new Rect(34, i * 34 + 34, 34, 34), image, new Rect((int.Parse(s[0]) - 1) * 0.07143f, (int.Parse(s[1]) - 1) * 0.03333f, 0.07143f, 0.03333f));
                    }
                }
                GUILayout.EndScrollView();                
            }
            GUILayout.EndArea();
        }
    }
}
