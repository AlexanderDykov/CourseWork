using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CurrentWeapone
{
    Sword,
    Bow
}
public class PlayerStats : MonoBehaviour {

    public List<GameObject> targets;
    public static float fireTime = 0.3f;
    public static float moveSpeed = 5f;
    public static int HP = 100;
    public static float meleeAttackDistance = 1.5f;
    public static float rangedAttackDistance = 5f;
    float maxHP = 100f;
    public static float damageFromSword = 30f;
    public static float damageFromBone = 20f;
    public static CurrentWeapone curWeapone = CurrentWeapone.Sword;

    private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
    private Vector3 healthScale;				// The local scale of the health bar initially (with full health).

}
