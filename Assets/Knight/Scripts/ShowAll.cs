using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts;
using Tables;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Specialized;

public class ShowAll : MonoBehaviour {
    public Text all;
    public Text key;
    public Text value;
    List<Progress> allProgress;
    List<Pers> allPers;
    OrderedDictionary info = new OrderedDictionary();
	// Use this for initialization
	void Start () {
        using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
                allProgress = (List < Progress >) manager.ReadAll<Progress>();
                allPers = (List<Pers>)manager.ReadAll<Pers>();
            }
        }
        allProgress.Sort( new Progress.MaxToMin());

        int i = 0;
        foreach (var prog in allProgress)
        {
            i++;
            Pers a = allPers.FirstOrDefault(x => x.Id == prog.PersId);
            info.Add(a.Name, prog.Score);
            all.text += i + "). Name: " + a.Name + " Score: " + prog.Score + "\n";
            
        }
       
	}
	
    public void ShovInfo()
    {
        if (info[key.text] != null)
            value.text = "Score: " + info[key.text].ToString();
    }
}
