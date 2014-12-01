using UnityEngine;
using System.Collections;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

public class PDFTest : MonoBehaviour {

    void OnGUI()
    {
        if (GUI.Button(new Rect(40, 40,150, 40), "Купить"))
        {

        }
        if (GUI.Button(new Rect(40, 100,150, 40), "Продать"))
        { 

        }
        if (GUI.Button(new Rect(40,160, 150,40), "Добавить вещь"))
        { 

        }
        if (GUI.Button(new Rect(40, 220, 150, 40), "Обновить вещь"))
        {

        }
    }

    string a = "";
/*   void OnGUI()
    {

        if (GUI.Button(new Rect(32, 16, 64, 64), "Click"))
        {
            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(Application.dataPath + @"\Document.pdf", FileMode.Create));
            doc.Open();
            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(Application.dataPath + @"/Resources/Item.png");
            jpg.Alignment = Element.ALIGN_CENTER;
            doc.Add(jpg);
            PdfPTable table = new PdfPTable(5);
            PdfPCell cell = new PdfPCell(new Phrase("Simple table",
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 16,
              iTextSharp.text.Font.NORMAL, new BaseColor(0, 0, 0))));
            cell.BackgroundColor = new BaseColor(255, 255, 255);
            cell.Padding = 3;
            cell.Colspan = 5;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
            table.AddCell("Col 1 Row 1");
            table.AddCell("Col 2 Row 1");
            table.AddCell("Col 3 Row 1");
            table.AddCell("Col 4 Row 1");
            table.AddCell("Col 1 Row 2");
            table.AddCell("Col 2 Row 2");
            table.AddCell("Col 3 Row 2");
            table.AddCell("Col 4 Row 2");
            if (doc.Add(table))
                Debug.Log("Success!");
            else Debug.Log("Error!");
            doc.Close();
        }
        GUI.Label(new Rect(100, 100, 100,100), a);
    }*/
}
