using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
    public GameObject menu;
    void OnMouseDown()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
        this.gameObject.SetActive(false);
        
    }
}
