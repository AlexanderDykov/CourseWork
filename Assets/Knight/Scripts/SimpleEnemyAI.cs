using UnityEngine;
using System.Collections;

public class SimpleEnemyAI: MonoBehaviour {

	public float moveSpeed;			//Move speed
	public float attackSpeed;		//Attack speed
	public int damage;				//Damage
	private float tmpAttackSpeed;	//Tmp attack speed
	//public GameObject hit;		    //Hit FX
	private Transform player;		//The player
	private int HP = 100;			//Health


	private Vector3 moveDirection;
	public float turnSpeed = 10;
	private Vector3 moveToward;

	void Start ()
	{
		//Find player
		player = GameObject.Find("Player").transform;
	}
	
	void Update ()
	{
		//If the distance between the player position and the our position is over 1 meter
		if (Vector3.Distance(player.position,transform.position) > 1)
		{
			//Update
			MoveUpdate();
		}
		else
		{
			//If tmpAttackSpeed is bigger than attackSpeed
			if (tmpAttackSpeed > attackSpeed)
			{
				//Set tmpAttackSpeed to 0
				tmpAttackSpeed = 0;
				//Hit the player
				//player.GetComponent<PlayerScript>().ChangeHP(-damage);
			}
			else
			{
				//Add 1 to tmpAttackSpeed
				tmpAttackSpeed += 1 * Time.deltaTime;
			}
		}
	}
	
	void MoveUpdate()
	{
		//for rotate
		Vector3 currentPosition = transform.position;
		moveToward = player.transform.position;
		moveDirection = moveToward - currentPosition;
		moveDirection.z = 0; 
		moveDirection.Normalize ();		
		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.Euler( 0, 0,targetAngle ), turnSpeed * Time.deltaTime);

		//move
		transform.Translate(Vector2.right * moveSpeed * Time.smoothDeltaTime);

	}
	
	public void ChangeHP(int health)
	{
		if (damage > 0) //medicine chest
		{
			//Instantiate(hit,transform.position,Quaternion.identity);
		} 
		else //enemy damage
		{
			//Instantiate(hit,transform.position,Quaternion.identity);
		}
		HP += health;
		if (HP <= 0) //death
		{
			//enemySpawn.GetComponent<EnemySpawn>().count--;
			Destroy(gameObject);

		}
	}
}
