using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System.Collections.Generic;
using Tables;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System;


public class ShopForm : MonoBehaviour
{
    List<Item> tempItems = new List<Item>();

    float width = 0;
    float height = 0;
    float widthForImage = 0;
    float heightForImage = 0;
    public Texture2D image;
    
    void Start()
    {
       tempItems = DataBaseInfo.items;

        width = (Screen.width - 10) / 3;
        height = (Screen.height - 10) / 3;



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
    string search = "";
    void OnGUI()
    {
        GUI.TextArea(new Rect((width * 2), 0, width + 10, height * 2.5f), info);

        #region search
        if (GUI.Button(new Rect(250, Screen.height - 120, 150, 30), "Поиск"))
        {
            tempItems = new List<Item>();
            foreach (var item1 in DataBaseInfo.items)
            {
                if (item1.Name.Contains(search))
                {
                    tempItems.Add(item1);
                }
            }
        }
        search = GUI.TextArea(new Rect(250, Screen.height - 150, 150, 30), search);
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
               foreach(var type in DataBaseInfo.types)
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
            }
            
            GUI.DrawTextureWithTexCoords(new Rect((width * j) + 10, (height * i) + heightForImage / 2, widthForImage, heightForImage), image, new Rect((int.Parse(s[0]) - 1) * 0.07143f, (int.Parse(s[1]) - 1) * 0.03333f, 0.07143f, 0.03333f));
            GUI.Label(new Rect((width * j) + 10 + widthForImage, (height * i) + heightForImage / 2, widthForImage + 20, heightForImage), string.Format("Название:\n {0}", tempItems[z + page].Name));
            GUI.Label(new Rect((width * j) + 10, (height * i) + heightForImage * 2 - 10, width, height), string.Format("Цена покупки: {0}\nЦена продажи: {1}", tempItems[z + page].BuyPrice, tempItems[z + page].SellPrice));
            #endregion
            if (GUI.Button(new Rect(Screen.width/2 +50, Screen.height - 50, 50, 50), ">"))
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

            if (GUI.Button(new Rect(Screen.width - 100, Screen.height - 70, 70, 50), "Купить"))
            {
            }

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
        }        
       
    }
    string[] sortItems = {"Не сортировать","По имени","По цене продажи","По цене покупки", "По типу"};
}
