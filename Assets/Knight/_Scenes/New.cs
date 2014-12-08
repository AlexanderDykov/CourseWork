using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Tables;
using System.Collections.Generic;

public class New : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
                DataBaseInfo.allInventories = (List<Inventory>)manager.ReadAll<Inventory>();
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
            }
        }
	}
}
