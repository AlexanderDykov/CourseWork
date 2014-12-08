using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System.Linq;
using Assets.Scripts;

public class InsertItem : MonoBehaviour {


    private string selected = "none";
    private string raceStr = "none";
    private bool isOpen = false;
    private bool isOpen2 = false;
    private Vector2 scrollPos = Vector2.zero;
    int itemID = 0;
    public Texture2D image;
    float width = 0;
    float height = 0;
    void Start()
    {
        width = (Screen.width - 30) / 3;
        height = (Screen.height - 40) / 4;


    }

    int i = 0;
    int j = 0;
    string name = "";
    string description = "";
    string buyPrice = "";
    string sellPrice = "";
    string lvl = "";
    int raceId = 0;
    string stats = "";
    private void OnGUI()
    {
        GUI.Label(new Rect(width / 6, height * 0.25f, width, height * 0.5f), "Название");
        name = GUI.TextField(new Rect(width / 6, height * 0.5f, (Screen.width) / 4, height * 0.5f), name);
        GUI.Label(new Rect(width / 6, height * 1f, width, height * 0.5f), "Описание");
        description = GUI.TextArea(new Rect(width / 6, height * 1.25f, (Screen.width) / 4, 2.5f * height), description);


        GUI.DrawTextureWithTexCoords(new Rect(width + width / 6, height * 0.25f, width / 2, height), image, new Rect(i * 0.07143f, j * 0.03333f, 0.07143f, 0.03333f));
        if (GUI.Button(new Rect(width + (width) * 2 / 3, height * 0.25f, width / 4, height / 2), "-"))
        {
            if (i == 0)
            {
                if (j != 0)
                {
                    j--;
                    i = 13;
                }
            }
            else i--;
        }
        if (GUI.Button(new Rect(width + (width) * 2 / 3, height * 0.75f, width / 4, height / 2), "+"))
        {
            if (i == 13)
            {
                if (j != 29)
                {
                    j++;
                    i = 0;
                }
            }
            else i++;
        }
        GUI.Label(new Rect(width + width / 6, height * 1.5f, width, height * 0.5f), "Цена покупки");
        buyPrice = GUI.TextField(new Rect(width + width / 6, height * 1.75f, (Screen.width) / 4, height * 0.5f), buyPrice);
        GUI.Label(new Rect(width + width / 6, height * 2.25f, width, height * 0.5f), "Цена продажи");
        sellPrice = GUI.TextField(new Rect(width + width / 6, height * 2.5f, (Screen.width) / 4, height * 0.5f), sellPrice);
        GUI.Label(new Rect(width + width / 6, height * 3f, width, height * 0.5f), "Статы");
        stats = GUI.TextField(new Rect(width + width / 6, height * 3.25f, (Screen.width) / 4, height * 0.5f), stats);

        GUI.Label(new Rect(width * 2 + width / 6, height * 0.25f, width, height * 0.5f), "Уровень использования");
        lvl = GUI.TextField(new Rect(width * 2 + width / 6, height * 0.5f, (Screen.width) / 4, height * 0.5f), lvl);

        GUI.Label(new Rect(width * 2 + width / 6, height , width, height * 0.5f), "Ограничение по расе");
        GUILayout.BeginArea(new Rect(width * 2 + width / 6, height * 1.25f, (Screen.width) / 4, height * 0.75f));
        {
            if (GUILayout.Button(raceStr))
            {
                isOpen2 = !isOpen2;
            }
            if (isOpen2)
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                {
                    foreach (var rac in DataBaseInfo.races)
                    {
                        if (GUILayout.Button(rac.RaceName, GUILayout.ExpandWidth(true)))
                        {
                            raceStr = rac.RaceName;
                            raceId = rac.Id;
                            isOpen2 = false;
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
        }
        GUILayout.EndArea();

        GUI.Label(new Rect(width * 2 + width / 6, height*2, width, height * 0.5f), "Тип вещи");
        GUILayout.BeginArea(new Rect(width * 2 + width / 6, height * 2.25f, (Screen.width) / 4, height * 0.75f));
        {
            if (GUILayout.Button(selected))
            {
                isOpen = !isOpen;
            }
            if (isOpen)
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                {
                    foreach (var itemType in DataBaseInfo.types)
                    {
                        if (GUILayout.Button(itemType.Type, GUILayout.ExpandWidth(true)))
                        {
                            selected = itemType.Type;
                            itemID = itemType.Id;
                            isOpen = false;
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
        }
        GUILayout.EndArea();

        if(GUI.Button(new Rect(width * 2 + width / 6, height * 3f, (Screen.width) / 4, height), "Добавить"))
        {
            if(name != "" && sellPrice != "" && buyPrice !=""  && itemID!= 0 && stats != "")
            {
                using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
                {
                    if (manager.ConnectToDatabase())
                    {
                        Condition con = DataBaseInfo.conditions.FirstOrDefault(x => x.Level == int.Parse(lvl) && x.RaceId == raceId);
                        if (con == null)
                        {
                            manager.InsertRecord<Condition>(new Condition() { Level = int.Parse(lvl), RaceId = raceId });
                            DataBaseInfo.conditions = (List<Condition>)manager.ReadAll<Condition>();
                            con = DataBaseInfo.conditions[DataBaseInfo.conditions.Count - 1];
                        }
                        manager.InsertRecord<Item>(new Item() {  Name = name, Description = description, SellPrice = int.Parse(sellPrice), AttackDistance = 0, BuyPrice = int.Parse(buyPrice), ItemTypeId = itemID, SpeedAttack = 0, Stats = int.Parse(stats) , Image = string.Format("{0}_{1}", i + 1, j + 1), ConditionId = con.Id });
                        DataBaseInfo.shop = (List<Shop>) manager.ReadAll<Shop>();
                    }
                }
                Application.LoadLevel(1);
            }

           
           // Condition con = new Condition() { Level = int.Parse( lvl), RaceId = raceId };
            //int racIdTest = 0;
           // newItem = new Item() { BuyPrice = buyPrice, Level = lvl, ConditionId =  };
        }
    }
}
