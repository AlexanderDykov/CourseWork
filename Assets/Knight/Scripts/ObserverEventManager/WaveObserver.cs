using UnityEngine;
using System.Collections;

public class WaveObserver : MonoBehaviour {
    public EventManager em;
    float time = 2f;
    int level = 0;
    public float timeLine = 3f;

	void Start () {
        em = GameObject.Find("ObserverGO").GetComponent<EventManager>();
        InvokeRepeating("Time",1f,1f);
	}
	
    void Time()
    {
        timeLine--;
        if(timeLine <= 0)
        {
            timeLine = 3;
            level++;
            em.ChangeLevelEvent(level);
        }
    }
    void OnGUI()
    {
        GUI.Box(new Rect(10,10, timeLine + 10, 10),"");
    }

}
