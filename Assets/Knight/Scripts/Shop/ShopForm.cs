﻿using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using Tables;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;



public class ShopForm : MonoBehaviour
{
    List<Item> tempItems = new List<Item>();
    List<Shop> tempShop = new List<Shop>();
    float width = 0;
    float height = 0;
    float widthForImage = 0;
    float heightForImage = 0;
    public Texture2D image;
    public Text money;
    void Start()
    {
        /*using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
                manager.UpdateFieldInRecord<Shop>("Id","1","Amount","8");
            }
        }
        */

        money.text = "Money: " + DataBaseInfo.currentProgress.Money;
        foreach (var it in DataBaseInfo.shop)
            foreach (var item1 in DataBaseInfo.items)
            {
                if (item1.Id == it.ItemId && it.Amount > 0)
                {
                    tempItems.Add(item1);
                    tempShop.Add(it);
                }
            }
        //        DataBaseInfo.currentInventory =(List<Inventory>) DataBaseInfo.allInventories.Select(x => x.PersId == DataBaseInfo.currentPersId);

        width = (Screen.width - 10) / 3;
        height = (Screen.height - 10) / 3;
        // DataBaseInfo.currentInventory.Add(new Inventory() {  Id = 0, Amount = 10, ItemId = 2, PersId = 1});

        widthForImage = width / 3;
        heightForImage = height / 3;
    }
    int page = 0;
    int temp = 0;
    string info = "";
    int countItemsOnPage = 4;


    private string sortStr = "Сортировка";
    private bool isOpen = false;
    private Vector2 scrollPos = Vector2.zero;
    string searchByCost = "";
    string searchByName = "";
    void OnGUI()
    {
        GUI.TextArea(new Rect((width * 2), 0, width + 10, height * 2.5f), info);

        #region search
        if (GUI.Button(new Rect(250, Screen.height - 120, 150, 30), "Поиск"))
        {
                tempItems = new List<Item>();

                if (searchByCost != "")
                {
                    foreach (var item1 in DataBaseInfo.items)
                    {
                        if (item1.Name.Contains(searchByName) && item1.BuyPrice == int.Parse(searchByCost))
                        {
                            tempItems.Add(item1);
                        }
                    }
                }
                else
                {
                    foreach (var item1 in DataBaseInfo.items)
                    {
                        if (item1.Name.Contains(searchByName))
                        {
                            tempItems.Add(item1);
                        }
                    }
                }
                
        }
        searchByName = GUI.TextArea(new Rect(250, Screen.height - 150, 150, 30), searchByName);
        searchByCost = GUI.TextArea(new Rect(400, Screen.height - 150, 150, 30), searchByCost);
        #endregion

        #region sort
        GUILayout.BeginArea(new Rect(20, Screen.height - 150, 200, 150));
        {
            if (GUILayout.Button(sortStr))
            {
                isOpen = !isOpen;
            }
            if (isOpen)
            {
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                {
                    for (int sort = 0; sort < sortItems.Length; sort++)
                    {
                        if (GUILayout.Button(sortItems[sort], GUILayout.ExpandWidth(true)))
                        {
                            switch (sort)
                            {
                                case 0:
                                    tempItems.Sort(new Item.DontSort());
                                    break;
                                case 1:
                                    tempItems.Sort(new Item.SortByName());
                                    break;
                                case 2:
                                    tempItems.Sort(new Item.SortBySellPrice());
                                    break;
                                case 3:
                                    tempItems.Sort(new Item.SortByBuyPrice());
                                    break;
                                case 4:
                                    tempItems.Sort(new Item.SortByType());
                                    break;
                            }
                            page = 0;
                            info = "";
                            sortStr = sortItems[sort];
                            isOpen = false;
                        }
                    }
                }
                GUILayout.EndScrollView();
            }
        }
        GUILayout.EndArea();
        #endregion

        for (int i = 0, j = 0, z = 0; z + page < tempItems.Count && z < countItemsOnPage; j++, z++)
        {
            #region itemsInfo

            if (j == countItemsOnPage / 2)
            {
                i++;
                j = 0;
            }
            string[] s = tempItems[z + page].Image.Split('_');
            if (GUI.Button(new Rect((width * j), (height * i), width, height), ""))
            {

                temp = z + page;
                info = string.Format("Название: {0}\nСвойство: +{1}\nОписание: {2}",
                    tempItems[z + page].Name,
                    tempItems[z + page].Stats,
                    tempItems[z + page].Description
                    );
                foreach (var type in DataBaseInfo.types)
                {
                    if (type.Id == tempItems[z + page].ItemTypeId && type.Type == "Оружие")
                    {
                        info += "\nТип предмета: " + type.Type;
                        info += "\nСкорость атаки: " + tempItems[z + page].SpeedAttack + " удар./сек.";
                        break;
                    }
                }
                info += "\nОграничения:";
                foreach (var con in DataBaseInfo.conditions)
                {
                    if (con.Id == tempItems[z + page].ConditionId)
                    {
                        info += "\n   Уровень использования: " + con.Level;
                        foreach (var rac in DataBaseInfo.races)
                        {
                            if (rac.Id == con.RaceId)
                            {
                                info += "\n   Раса: " + rac.RaceName;
                                break;
                            }
                        }
                        break;
                    }

                }
                info += "\nКоличество " + tempShop[temp].Amount;
            }

            GUI.DrawTextureWithTexCoords(new Rect((width * j) + 10, (height * i) + heightForImage / 2, widthForImage, heightForImage), image, new Rect((int.Parse(s[0]) - 1) * 0.07143f, (int.Parse(s[1]) - 1) * 0.03333f, 0.07143f, 0.03333f));
            GUI.Label(new Rect((width * j) + 10 + widthForImage, (height * i) + heightForImage / 2, widthForImage + 20, heightForImage), string.Format("Название:\n {0}", tempItems[z + page].Name));
            GUI.Label(new Rect((width * j) + 10, (height * i) + heightForImage * 2 - 10, width, height), string.Format("Цена покупки: {0}\nЦена продажи: {1}", tempItems[z + page].BuyPrice, tempItems[z + page].SellPrice));
            #endregion

            if (GUI.Button(new Rect(Screen.width / 2 + 50, Screen.height - 50, 50, 50), ">"))
            {
                if (z + page + countItemsOnPage < tempItems.Count)
                {
                    info = "";
                    page += countItemsOnPage;
                }
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height - 50, 50, 50), "<"))
            {
                if (page - countItemsOnPage >= 0)
                {
                    info = "";
                    page -= countItemsOnPage;

                }
            }
            #region Buy
            if (info != "" && DataBaseInfo.currentProgress.Money >= tempItems[temp].BuyPrice)
            {
                if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 70, 70, 50), "Купить"))
                {
                    // Debug.Log(temp);

                    DataBaseInfo.currentProgress.Money -= tempItems[temp].BuyPrice;

                    info = "";
                    tempShop[temp].Amount--;
                    using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
                    {
                        if (manager.ConnectToDatabase())
                        {
                            if (DataBaseInfo.currentInventory != null)
                            {
                                Debug.Log(tempItems[temp].Id);
                                Inventory inv = DataBaseInfo.currentInventory.FirstOrDefault(x => x.ItemId == tempItems[temp].Id);
                               
                                if (inv != null)
                                {
                                    Debug.Log(inv.Id);
                                    inv.Amount++;
                                    manager.UpdateRecordByFieldName<Inventory>("Id", inv.Id.ToString(), inv);
                                }
                                else
                                {
                                    manager.InsertRecord<Inventory>(new Inventory() { ItemId = tempItems[temp].Id, Amount = 1, PersId = DataBaseInfo.currentPersId });
                                }
                            }
                            else
                            {
                                manager.InsertRecord<Inventory>(new Inventory() { ItemId = tempItems[temp].Id, Amount = 1, PersId = DataBaseInfo.currentPersId });
                            }
                            DataBaseInfo.allInventories =(List<Inventory>) manager.ReadAll<Inventory>();
                            Debug.Log(DataBaseInfo.allInventories.Count);
                            DataBaseInfo.currentInventory = new List<Inventory>();
                            foreach (var inv in DataBaseInfo.allInventories)
                            {
                                if (inv.PersId == DataBaseInfo.currentPersId)
                                {
                                    DataBaseInfo.currentInventory.Add(inv);
                                }
                            }
                            Debug.Log(DataBaseInfo.allInventories.Count);
                            manager.UpdateRecordByFieldName<Shop>("Id", tempShop[temp].Id.ToString(), tempShop[temp]);
                        }
                    }

                    if (tempShop[temp].Amount <= 0)
                    {
                        tempItems.RemoveAt(temp);
                    }
                    money.text = "Money: " + DataBaseInfo.currentProgress.Money;
                    PlayerStats.QuitScene();
                }
            }
            #endregion
            #region PDF
            if (GUI.Button(new Rect(Screen.width - 230, Screen.height - 70, 120, 50), "Генерация PDF"))
            {
                var doc = new Document();
                PdfWriter.GetInstance(doc, new FileStream(Application.dataPath + @"\Document.pdf", FileMode.Create));
                doc.Open();
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Application.dataPath + @"/Resources/Item.png");
                jpg.Alignment = Element.ALIGN_CENTER;
                doc.Add(jpg);
                PdfPTable table = new PdfPTable(3);
                PdfPCell cell = new PdfPCell(new Phrase("Products",
                    new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16,
                  iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));
                cell.BackgroundColor = new BaseColor(255, 255, 255);
                cell.Padding = 3;
                cell.Colspan = 5;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                for (int it = 0; it < tempItems.Count; it++)
                {
                    table.AddCell("Name: " + tempItems[it].Name);
                    table.AddCell("Sell Price = " + tempItems[it].SellPrice);
                    table.AddCell("Buy Price = " + tempItems[it].BuyPrice);
                }
                if (doc.Add(table))
                    Debug.Log("Success!");
                else Debug.Log("Error!");
                doc.Close();
            }
            #endregion
        }

    }
    string[] sortItems = { "Не сортировать", "По имени", "По цене продажи", "По цене покупки", "По типу" };
}
