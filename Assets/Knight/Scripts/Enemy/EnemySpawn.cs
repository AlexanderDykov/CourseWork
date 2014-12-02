using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour {
	public GameObject[] enemy = new GameObject[2];			//Enemy prefab
	public float spawnTime;				//Spawn time
	private float tmpSpawnTime;			//Tmp spawn time
    private int count = 0;
    List<GameObject> enemies = new List<GameObject>();
    EventManager em;
    public void OnChangeWave(EventManager EM, CustomArg e)
    {
        count++;
        //Debug.Log("");
    }
	void Start ()
    {
        em = GameObject.Find("ObserverGO").GetComponent<EventManager>();
        em.mChangeWaveEvent += OnChangeWave;
        int a = Random.Range(0,2);
        enemies = ObjectPool.Instance.Create(enemy[a], 5);
	}
	
	void Update ()
	{
        if (count < enemies.Count)
		    if (count != 0)
		    {
			    //tmpSpawnTime is bigger than spawnTime
			    if (tmpSpawnTime > spawnTime) {
					    //Set tmpSpawnTime to 0
					    tmpSpawnTime = 0;
					    //Spawn enemy
					    InstantiateEnemy ();
			    } else {
					    //Add 1 to tmpSpawnTime
					    tmpSpawnTime += 1 * Time.deltaTime;
			    }
		    }
	}
	
	void InstantiateEnemy()
	{
        
        for (int i = 0; i < count ; i++)
        {

            if (!enemies[i].activeInHierarchy)
            {
                ObjectPool.Instance.Spawn(enemies[i], this.transform.position, Quaternion.identity);
        
            }
        }
	}
}
