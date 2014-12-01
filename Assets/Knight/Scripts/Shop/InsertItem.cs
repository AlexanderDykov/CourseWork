using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using Assets.Scripts;

public class InsertItem : MonoBehaviour {

    private List<ItemType> types = new List<ItemType>();
    List<Race> race = new List<Race>();
    List<Condition> condition = new List<Condition>();
    Item newItem = new Item();

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
        types.Add(new ItemType() { Id = 0, Type = "Оружие", CanUse = false, CanWear = false });
        types.Add(new ItemType() { Id = 1, Type = "Одежда", CanUse = false, CanWear = false });
        types.Add(new ItemType() { Id = 2, Type = "Зелье", CanUse = false, CanWear = false });
        width = (Screen.width - 30) / 3;
        height = (Screen.height - 40) / 4;
        race.Add(new Race() { Id = 0, RaceName = "Орк" });
        race.Add(new Race() { Id = 1, RaceName = "Ельф" });
        race.Add(new Race() { Id = 2, RaceName = "Человек" });


    }

    int i = 0;
    int j = 0;
    string name = "";
    string description = "";
    string buyPrice = "";
    string sellPrice = "";
    string lvl = "";
    int raceId = 0;

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
                    foreach (var rac in race)
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
                    foreach (var itemType in types)
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
           // newItem = new Item() { BuyPrice = buyPrice, Level = lvl, ConditionId =  };
        }
    }
}
