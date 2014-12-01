using UnityEngine;
using System.Collections;

public class GoToShop : MonoBehaviour {
    public GameObject shopPanel;
    public GameObject startPanel;

    public void GoToShopClick()
    {
        startPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void ExitClick()
    {        
        shopPanel.SetActive(false);
        startPanel.SetActive(true);
    }


}
