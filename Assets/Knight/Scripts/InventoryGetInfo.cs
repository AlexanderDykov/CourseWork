using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Tables;
using System.Collections.Generic;

public class InventoryGetInfo : MonoBehaviour {

	void Start () 
    {
        using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
               // DataBaseInfo.inventory = (List<Inventory>)manager.ReadByFieldName<Inventory>("PersId",DataBaseInfo.currentPersId);
            }
        }
	}
	
}
