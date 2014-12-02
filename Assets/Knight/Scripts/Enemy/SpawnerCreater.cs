using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerCreater : MonoBehaviour {

    public int width;
    public int height;
    public int count = 1;
    public GameObject spawner;
    List<GameObject> spawners = new List<GameObject>();
    EventManager em;

	void Start () {
        spawners = ObjectPool.Instance.Create(spawner, 3);
        em = GameObject.Find("ObserverGO").GetComponent<EventManager>();
        em.mChangeWaveEvent += OnChangeWave;
        Spawn();
    }
    public void OnChangeWave(EventManager EM, CustomArg e)
    {
        if (e.GetNumber() % 5 == 0 && count < 3)
        {
            count++;
            Spawn();
        }
    }
    Random rnd;

    void Spawn()
    {
        int a = Random.Range(0, width);
        int b = Random.Range(0, height);
		
        for (int i = 0; i < count; i++)
        {
            if (!spawners[i].activeInHierarchy)
            {
                ObjectPool.Instance.Spawn(spawners[i], new Vector2(a,b), Quaternion.identity);
                break;
            }
        }
    }
}
