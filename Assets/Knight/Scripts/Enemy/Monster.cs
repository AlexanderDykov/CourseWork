using UnityEngine;
using System.Collections;
using Assets.Knight.Interfaces;
using Tables;
using System.Collections.Generic;



public class Monster : MonoBehaviour {

    public MonsterType monsterType;
    public Transform monster;
    public Animator animator;
    public GameObject[] targets = new GameObject[2];// 1 - player, 2 - obelisk
    public Transform currTarget;
    public float attackSpeed = 10;
    public float attackDistance = 5f;
    public float boringDistance = 15f;
    public float moveSpeed;
    public float turnSpeed = 10;
    private Vector3 moveDirection;
    private Vector3 moveToward;

    float HP = 100f;
    float maxHP = 100f;
    public float damage = 10f;


    private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
    private Vector3 healthScale;				// The local scale of the health bar initially (with full health).

    public GameObject bullet;
    public List<GameObject> bullets = new List<GameObject>();
    public Transform spawnBulletPosition;
    EventManager em;
    void Awake()
    {
        if (monsterType == MonsterType.Spider)
        {
            bullets = ObjectPool.Instance.Create(bullet, 1);
            spawnBulletPosition = transform.FindChild("BulletSpawnPosition").transform;
        }
        targets[0] = GameObject.Find("Player");
        targets[1] = GameObject.Find("Obelisk");        
        currTarget = targets[1].transform;
        monster = this.transform;
        animator = monster.gameObject.GetComponent<Animator>();
        m_curState = new MovingState();

        healthBar = monster.Find("HealthBar").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;

        em = GameObject.Find("ObserverGO").GetComponent<EventManager>();
        em.mChangeWaveEvent += OnChangeWave;
    }
    public void OnChangeWave(EventManager EM, CustomArg e)
    {
        damage += 2 * e.GetNumber();
        maxHP += 10;
        HP = maxHP;
        UpdateHealthBar();
    }
    void OnEnabled()
    {
        HP = maxHP;
    }
    public void UpdateHealthBar()
    {
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - HP * 0.01f);
        healthBar.transform.localScale = new Vector3(healthScale.x * HP * 0.01f, 1, 1);
    }

    public void ChangeHP(float damage)
    {
        HP += damage;
        if (HP <= 0)
        {
            HP = 0;
            ObjectPool.Instance.Remove(gameObject);
        }
        if (HP > maxHP)
            HP = 100f;
        UpdateHealthBar();
    }

    public void ChangeState(IState newState)
    {
        m_curState = newState;
    }

    public float distanceToTarget = 0;
    private void Update()
    {
        //for rotate
        Vector3 currentPosition = monster.position;
        moveToward = currTarget.transform.position;
        moveDirection = moveToward - currentPosition;
        moveDirection.z = 0;
        moveDirection.Normalize();
        float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        monster.rotation = Quaternion.Slerp(monster.rotation, Quaternion.Euler(0, 0, targetAngle), turnSpeed * Time.deltaTime);

        distanceToTarget = Vector2.Distance(monster.position, currTarget.position);
        m_curState.MonsterToTargetDistance(this, distanceToTarget);

        m_curState.MonsterUpdate(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player" || other.gameObject.name == "Bullet(Clone)")
            m_curState.MonsterOnTriggerEnter(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
            m_curState.MonsterOnTriggerExit(this);
    }

    private IState m_curState;
}