using UnityEngine;

public class FighterAction : MonoBehaviour
{
    [Header("Damage config")]
    [SerializeField] private bool usesMagicForRange = true;
    [SerializeField] private float rangeMagicCost = 10f;
    [SerializeField] private float minAttackMultiplier = 0.8f;
    [SerializeField] private float maxAttackMultiplier = 1.2f;
    [SerializeField] private float minDefenseMultiplier = 0.2f;
    [SerializeField] private float maxDefenseMultiplier = 1.0f;

    public void SelectAttack(string attackType)
    {
        // 1) Ellenőrzés: tényleg én vagyok soron?
        GameController gc = GameController.Instance;
        if (gc != null)
        {
            GameObject current = gc.GetCurrentHero();
            if (current != gameObject)
            {
                Debug.Log($"[FighterAction] Nem a te köröd: {name}");
                return;
            }
        }

        // 2) Ki vagyok én?
        FighterStats self = GetComponent<FighterStats>();
        if (self == null)
        {
            Debug.LogError("[FighterAction] FighterStats missing on hero!");
            return;
        }

        // 3) Kit támadok?
        GameObject victimGO = null;
        if (CompareTag("redHero"))
            victimGO = GameObject.FindGameObjectWithTag("blueHero");
        else if (CompareTag("blueHero"))
            victimGO = GameObject.FindGameObjectWithTag("redHero");

        if (victimGO == null)
        {
            Debug.LogWarning("[FighterAction] Victim hero not found (maybe dead).");
            return;
        }

        FighterStats target = victimGO.GetComponent<FighterStats>();
        if (target == null)
        {
            Debug.LogError("[FighterAction] Victim has no FighterStats.");
            return;
        }

        // 4) RUN = kör eldobás
        if (attackType == "run")
        {
            Debug.Log("[FighterAction] Run / skip turn.");
            GameController.Instance?.NextTurn();
            return;
        }

        bool isRange = attackType == "range";

        // 5) mana check range-re
        if (isRange && usesMagicForRange && self.magic < rangeMagicCost)
        {
            Debug.Log("[FighterAction] Not enough mana for range attack, skipping.");
            GameController.Instance?.NextTurn();
            return;
        }

        // 6) sebzés kalkuláció
        float baseAttack = isRange ? self.magicRange : self.melee;
        float baseDefense = target.defense;

        float attackMul = Random.Range(minAttackMultiplier, maxAttackMultiplier);
        float defenseMul = Random.Range(minDefenseMultiplier, maxDefenseMultiplier);

        float damageF = attackMul * baseAttack - defenseMul * baseDefense;
        int damage = Mathf.Max(1, Mathf.RoundToInt(damageF));

        Debug.Log($"[FighterAction] {name} -> {victimGO.name}, dmg: {damage}");

        // 7) támadó anim (melee / range)
        Animator anim = GetComponentInChildren<Animator>();
        if (anim != null)
        {
            string stateName = isRange ? "range" : "melee";
            int hash = Animator.StringToHash(stateName);
            bool hasState = anim.HasState(0, hash);

            Debug.Log($"[FighterAction] {name} TRY PLAY '{stateName}', HasState={hasState}");

            if (hasState)
                anim.Play(hash, 0, 0f);
            else
                anim.Play(stateName, 0, 0f); // fallback
        }
        else
        {
            Debug.LogWarning($"[FighterAction] {name} has no Animator in children.");
        }

        // 8) sebzés + hurt/death anim a targeten
        target.ReceiveDamage(damage);

        // 9) mana levonás
        if (isRange && usesMagicForRange && rangeMagicCost > 0f)
            self.UpdateMagicFill(rangeMagicCost);

        // 10) körváltás
        GameController.Instance?.NextTurn();
    }
}
