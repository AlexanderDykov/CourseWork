using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawn : MonoBehaviour {

	public GameObject enemies;			//Enemy prefab
	public float spawnTime;				//Spawn time
	private float tmpSpawnTime;			//Tmp spawn time
	public int count = 0;
    List<GameObject> aaa = new List<GameObject>();
	void Start ()
	{
        aaa = ObjectPool.Instance.Create(enemies, 10);
	}
	
	void Update ()
	{
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
        for (int i = 0; i < aaa.Count; i++)
        {

            if (!aaa[i].activeInHierarchy)
            {
                ObjectPool.Instance.Spawn(aaa[i], this.transform.position, Quaternion.identity );
                break;
            }
        }
		count--;
	}
}
