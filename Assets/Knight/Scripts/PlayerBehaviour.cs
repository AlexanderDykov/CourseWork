using UnityEngine;
using System.Collections;
using Assets.Scripts;
using System;
using System.Collections.Generic;

public class PlayerBehaviour: MonoBehaviour
{
    public Joystick leftJoystick;
   // public Joystick rightJoystick;
    int curentIdJoy = -1;
    int curentIdBut = -1;

    public Camera guiCam;

    public int count = 5;
    private float tmpFireTime;
    public Transform spawnBulletPosition;
    //for fire
    //	public AudioClip audioFire;

    public GameObject bullet;
    List<GameObject> bullets = new List<GameObject>();
    GameObject player;
    void Start()
    {
        player = this.gameObject;
        bullets = ObjectPool.Instance.Create(bullet, count);
    }

    IEnumerator StartFire(float timer)
    {
        yield return new WaitForSeconds(timer);
        for (int i = 0; i < bullets.Count; i++)
        {

            if (!bullets[i].activeInHierarchy)
            {
                ObjectPool.Instance.Spawn(bullets[i], spawnBulletPosition.position, spawnBulletPosition.rotation);
                break;
            }
        }
    }
    void FireBone()
    {
        if (tmpFireTime >= PlayerStats.fireTime)
        {
            tmpFireTime = 0;
            StartCoroutine("StartFire", PlayerStats.fireTime);
            

            /*   if (!audio.isPlaying)
               {
                   //Play sound
                   audio.clip = audioFire;
                   audio.PlayOneShot(audioFire);
               }*/
        }       
    }
    void FireSword()
    {
        if (tmpFireTime >= PlayerStats.fireTime)
        {
            tmpFireTime = 0;
            RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, new Vector2(spawnBulletPosition.position.x - player.transform.position.x, spawnBulletPosition.position.y - player.transform.position.y), PlayerStats.meleeAttackDistance);
            foreach(var hit in hits)
                if (!hit.collider.isTrigger && hit.collider != null && hit.transform.gameObject.tag == "Enemy")
                {
                    hit.transform.gameObject.gameObject.GetComponent<Monster>().ChangeHP(-PlayerStats.damageFromSword);	
                }
        }
    }


    void CheckTouchMob()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Moved || Input.GetTouch(i).phase == TouchPhase.Stationary)
            {
                Vector3 wp = guiCam.ScreenToWorldPoint(Input.GetTouch(i).position);
                Vector2 touchPos = new Vector2(wp.x, wp.y);
                Collider2D hit = Physics2D.OverlapPoint(touchPos);

                if (hit)
                {
                    switch (hit.gameObject.name)
                    {
                        case "JoystickL":
                            curentIdJoy = Input.GetTouch(i).fingerId;
                            break;
                        case "ButtonBone":
                            curentIdBut = Input.GetTouch(i).fingerId;
                            PlayerStats.curWeapone = CurrentWeapone.Bone;
                            break;
                        case "ButtonSword":
                            PlayerStats.curWeapone = CurrentWeapone.Sword;
                            break;
                    }
                }
                if (curentIdJoy == Input.GetTouch(i).fingerId)
                {
                    if (leftJoystick.Dist() > 0.25f)
                        player.rigidbody2D.velocity = leftJoystick.Move(touchPos) * PlayerStats.moveSpeed;
                    else player.rigidbody2D.velocity = Vector2.zero;
                    player.transform.rotation = leftJoystick.Rotation(touchPos);
                }
                if(curentIdBut == Input.GetTouch(i).fingerId)
                {
                    switch(PlayerStats.curWeapone)
                    {
                        case CurrentWeapone.Sword:
                            FireSword();
                            break;
                        case CurrentWeapone.Bone:
                            FireBone();
                            break;
                    }
                    
                }
            }
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                if (curentIdJoy == Input.GetTouch(i).fingerId)
                {
                    curentIdJoy = -1;
                    leftJoystick.JoystickReset();
                    player.rigidbody2D.velocity = Vector2.zero;
                }
                if(curentIdBut == Input.GetTouch(i).fingerId)
                {
                    curentIdBut = -1;
                }
            }
        }
       
    }
    void Update()
    {
        if (tmpFireTime < PlayerStats.fireTime)
        {
            tmpFireTime += 1 * Time.deltaTime;
        }

#if UNITY_ANDROID
        CheckTouchMob();       
#endif

#if UNITY_EDITOR
        TouchToWindowsPlatform();
#endif
    }



    bool move = false;
    bool fire = false;
    void CheckTouch(Vector3 pos, string phase)
    {
        Vector3 wp = guiCam.ScreenToWorldPoint(pos);
        Vector2 touchPos = new Vector2(wp.x, wp.y);
        Collider2D hit = Physics2D.OverlapPoint(touchPos);

        if (hit && phase == "began")
        {
            switch (hit.gameObject.name)
            {

                case "JoystickL":
                    move = true;
                    break;
                case "ButtonBone":
                    fire = true;
                    PlayerStats.curWeapone = CurrentWeapone.Bone;
                    break;
                case "ButtonSword":
                    fire = true;
                    PlayerStats.curWeapone = CurrentWeapone.Sword;
                    break;

            }
        }
        if (move)
        {
            if (leftJoystick.Dist() > 0.5f)
                player.rigidbody2D.velocity = leftJoystick.Move(touchPos) * PlayerStats.moveSpeed;
            else player.rigidbody2D.velocity = Vector2.zero;
            player.transform.rotation = leftJoystick.Rotation(touchPos);
          //  animator.SetBool("move", move);
        }
           // playerMove.Move(leftJoystick.Move(touchPos), leftJoystick.Rotation(touchPos));
        if (fire)
            switch (PlayerStats.curWeapone)
            {
                case CurrentWeapone.Sword:
                    FireSword();
                    break;
                case CurrentWeapone.Bone:
                    FireBone();
                    break;
            }

        if (phase == "ended")
        {
            leftJoystick.JoystickReset();
            move = false;
            fire = false;
            player.rigidbody2D.velocity = Vector2.zero;
        }

    }

    private void TouchToWindowsPlatform()
    {
        if (Input.GetMouseButton(0))
            CheckTouch(Input.mousePosition, "began");

        if (Input.GetMouseButtonUp(0))
            CheckTouch(Input.mousePosition, "ended");
    }

}
