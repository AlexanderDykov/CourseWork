using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float moveSpeed = 30f;	//Move Speed
	public float damage;			//Damage
	public float destroyTime = 1;	//Destroy Time

	void OnEnable()
	{
        StartCoroutine(Destroy(destroyTime));
	}
	
	void FixedUpdate ()
	{
		transform.Translate(Vector2.right * moveSpeed * Time.smoothDeltaTime);
	}


	void OnCollisionEnter2D(Collision2D other)
	{
        if (other.gameObject.tag == "Enemy" && this.gameObject.tag != "EnemyBullet")
		{
			//Hit the enemy
			other.gameObject.GetComponent<Monster>().ChangeHP(-PlayerStats.damageFromBone);			
			StartCoroutine(Destroy(0));
		}
        else if (other.gameObject.tag == "Player")
		{
            //
			//Destroy the bullet
			StartCoroutine(Destroy(0));
		}
        else
        {
            StartCoroutine(Destroy(0));
        }
	}

	
	IEnumerator Destroy(float _time)
	{
		//Wait destroyTime
		yield return new WaitForSeconds(_time);
		//Destroy the bullet
        ObjectPool.Instance.Remove(gameObject);
		//Destroy(gameObject);
	}
}
