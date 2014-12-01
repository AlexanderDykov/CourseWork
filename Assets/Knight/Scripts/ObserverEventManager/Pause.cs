using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {

    bool paused = false;
    void OnMouseDown()
    {
        if (!paused)
        {
            paused = !paused;
            Time.timeScale = 0;
        }
        else
        {
            paused = !paused;
            Time.timeScale = 1;
        }

    }
}
