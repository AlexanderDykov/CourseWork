using UnityEngine;
using System.Collections;

public class ForAdmin : MonoBehaviour {
    public GameObject insertItem;
    public GameObject updateItem;

	// Use this for initialization
	void Start () {

	    if(DataBaseInfo.isAdmin)
        {
            insertItem.SetActive(true);
            updateItem.SetActive(true);
        }
        else
        {
            insertItem.SetActive(false);
            updateItem.SetActive(false);
        }
	}
}
