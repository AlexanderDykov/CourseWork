using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
using System.Linq;
using Assets.Scripts;

public enum CurrentWeapone
{
    Sword,
    Bow
}
public class PlayerStats : MonoBehaviour
{
    public GameObject end;
    public static GameObject endPicture;
    public List<GameObject> targets;
    public static float fireTime = 0.3f;
    public static float moveSpeed = 5f;
    public static float HP = 100;
    public static float meleeAttackDistance = 1.5f;
    public static float rangedAttackDistance = 5f;
    public static float exp = 0;
    static float maxHP = 100f;
    public static float damageFromSword = 30f;
    public static float damageFromBone = 20f;
    public static CurrentWeapone curWeapone = CurrentWeapone.Sword;

    private static SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
    private static Vector3 healthScale;				// The local scale of the health bar initially (with full health).
    public static Race race;
    public static int level;
    Item[] equipedItems = new Item[3];

    void Reset()
    {
        damageFromSword = 30f;
        damageFromBone = 20f;
        HP = 100;
    }

    public static void UpdateHealthBar()
    {
        healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - HP * 0.01f);
        healthBar.transform.localScale = new Vector3(healthScale.x * HP * 0.01f, 1, 1);
    }

    public static void ChangeHP(float damage)
    {
        HP += damage;
        if (HP <= 0)
        {
            endPicture.SetActive(true);
            Time.timeScale = 0;
            HP = 0;
            
        }
        if (HP > maxHP)
            HP = 100f;
        UpdateHealthBar();
    }
    public static void QuitScene()
    {
        Debug.Log("QIUT");
        using (DatabaseManager manager = new DatabaseManager("gamedata.db"))
        {
            if (manager.ConnectToDatabase())
            {
                manager.UpdateRecordByFieldName<Progress>("Id", DataBaseInfo.currentProgress.Id.ToString(), new Progress() { CurrentLevel = DataBaseInfo.currentProgress.CurrentLevel, PersId = DataBaseInfo.currentPersId, Money = DataBaseInfo.currentProgress.Money, Score = DataBaseInfo.currentProgress.Score });
            }
        }

    }

    void Start()
    {
        endPicture = end;
        endPicture.SetActive(false);
        healthBar = transform.Find("HealthBar").GetComponent<SpriteRenderer>();
        healthScale = healthBar.transform.localScale;
    }

    void OnEnable()
    {
        Reset();
        equipedItems[0] = DataBaseInfo.items.FirstOrDefault(x => x.Id == DataBaseInfo.currentEquipedItems.Sword);
        equipedItems[1] = DataBaseInfo.items.FirstOrDefault(x => x.Id == DataBaseInfo.currentEquipedItems.Bow);
        equipedItems[2] = DataBaseInfo.items.FirstOrDefault(x => x.Id == DataBaseInfo.currentEquipedItems.Armor);
        HP += equipedItems[2].Stats;
        damageFromSword += equipedItems[0].Stats;
        damageFromBone += equipedItems[1].Stats;
    }
}
