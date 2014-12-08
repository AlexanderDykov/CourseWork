using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {

    public GameObject inventory;
    public GameObject menu;
    public GameObject pause;
    public PlayerStats ps;
    public void Pause()
    {
        
        menu.SetActive(true);
        pause.SetActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pause.SetActive(true);
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowInventory()
    {
        ps.enabled = false;
        menu.SetActive(false);
        inventory.SetActive(true);
    }
    public void HideInventory()
    {
        ps.enabled = true;
        menu.SetActive(true);
        inventory.SetActive(false);
    }
    public void ExitToMenu()
    {
        PlayerStats.QuitScene();
        Application.LoadLevel(1);
    }
}
