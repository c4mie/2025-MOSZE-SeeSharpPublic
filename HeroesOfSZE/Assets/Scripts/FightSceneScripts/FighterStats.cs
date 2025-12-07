using UnityEngine;
using System;

public class FighterStats : MonoBehaviour, IComparable
{
    [Header("Refs")]
    public Animator animator;
    public GameObject healthFill;
    public GameObject magicFill;

    [Header("Stats")]
    public float health;
    public float magic;
    public float melee;
    public float magicRange;
    public float defense;

    [Header("Animation State Names")]
    public string idleStateName = "idle";
    public string hurtStateName = "hurt";
    public string deathStateName = "death";

    private float startHealth;
    private float startMagic;
    private Vector2 healthScale;
    private Vector2 magicScale;
    private bool dead = false;

    [HideInInspector] public int nextActTurn;

    void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (healthFill != null)
            healthScale = healthFill.transform.localScale;

        if (magicFill != null)
            magicScale = magicFill.transform.localScale;
    }

    public void ReInitFromCurrent()
    {
        startHealth = health;
        startMagic = magic;

        if (healthFill != null)
        {
            float ratio = Mathf.Clamp01(health / startHealth);
            healthFill.transform.localScale = new Vector2(healthScale.x * ratio, healthScale.y);
        }

        if (magicFill != null)
        {
            float ratio = Mathf.Clamp01(magic / startMagic);
            magicFill.transform.localScale = new Vector2(magicScale.x * ratio, magicScale.y);
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (dead) return;

        damage = Mathf.Max(0, damage);
        health -= damage;

        Debug.Log($"[FighterStats {name}] Damage: {damage}, new HP: {health}");

        if (healthFill != null)
        {
            float ratio = Mathf.Clamp01(health / startHealth);
            healthFill.transform.localScale = new Vector2(healthScale.x * ratio, healthScale.y);
        }

        if (health <= 0f)
        {
            dead = true;
            animator?.Play(deathStateName, 0, 0f);
            gameObject.tag = "Dead";
        }
        else
        {
            animator?.Play(hurtStateName, 0, 0f);
        }
    }
    public void UpdateMagicFill(float cost)
    {
        if (magicFill == null || startMagic <= 0f)
            return;

        magic -= cost;
        magic = Mathf.Max(0f, magic);

        Debug.Log($"[FighterStats {name}] Magic cost: {cost}, new MP: {magic}");

        float ratio = Mathf.Clamp01(magic / startMagic);
        magicFill.transform.localScale = new Vector2(magicScale.x * ratio, magicScale.y);
    }

    public bool GetDead() => dead;

    public int CompareTo(object other)
    {
        return nextActTurn.CompareTo(((FighterStats)other).nextActTurn);
    }
}
