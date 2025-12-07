using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public GameObject owner;

    [Header("Config")]
    [SerializeField] private string animationName;    // "melee" vagy "range"
    [SerializeField] private bool magicAttack = false;
    [SerializeField] private float magicCost = 0f;
    [SerializeField] private float minAttackMultiplier = 0.8f;
    [SerializeField] private float maxAttackMultiplier = 1.2f;
    [SerializeField] private float minDefenseMultiplier = 0.2f;
    [SerializeField] private float maxDefenseMultiplier = 1.0f;

    [SerializeField] private Animator ownerAnimatorOverride;

    private FighterStats attackerStats;
    private FighterStats targetStats;

    public void Attack(GameObject victim)
    {
        if (owner == null || victim == null)
        {
            Debug.LogError("[Attack] owner vagy victim null!");
            Destroy(gameObject);
            return;
        }

        attackerStats = owner.GetComponent<FighterStats>();
        targetStats = victim.GetComponent<FighterStats>();

        if (attackerStats == null || targetStats == null)
        {
            Debug.LogError("[Attack] FighterStats hiányzik attacker/victim-rõl!");
            Destroy(gameObject);
            return;
        }

        // mana check
        if (magicAttack && attackerStats.magic < magicCost)
        {
            Debug.Log($"[Attack] {owner.name}: nincs elég mana, kör eldobva.");
            Invoke(nameof(FinishAndNextTurn), 0.3f);
            return;
        }

        // sebzés
        float attackMul = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        float defenseMul = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);

        float rawAttack = magicAttack ? attackerStats.magicRange : attackerStats.melee;
        float rawDefense = targetStats.defense;

        float damage = attackMul * rawAttack - defenseMul * rawDefense;
        damage = Mathf.Max(1f, damage);

        Debug.Log($"[Attack] {owner.name} -> {victim.name}, damage: {damage}");

        // anim az attacker-en
        Animator anim = ownerAnimatorOverride;
        if (anim == null)
            anim = owner.GetComponentInChildren<Animator>();

        if (anim != null && !string.IsNullOrEmpty(animationName))
        {
            int hash = Animator.StringToHash(animationName);
            if (anim.HasState(0, hash))
                anim.Play(hash);
        }

        // damage
        targetStats.ReceiveDamage(Mathf.CeilToInt(damage));

        // mana
        if (magicAttack && magicCost > 0f)
            attackerStats.UpdateMagicFill(magicCost);

        // kis delay után körváltás
        Invoke(nameof(FinishAndNextTurn), 0.6f);
    }

    private void FinishAndNextTurn()
    {
        if (GameController.Instance != null)
            GameController.Instance.NextTurn();

        Destroy(gameObject);
    }
}
