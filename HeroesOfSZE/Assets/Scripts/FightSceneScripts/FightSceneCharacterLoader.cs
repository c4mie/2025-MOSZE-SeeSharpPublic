using UnityEngine;

public class FightSceneCharacterLoader : MonoBehaviour
{
    public CharacterDatabase characterDB;

    public FighterStats redStats;
    public SpriteRenderer redSprite;
    public Animator redAnimator;

    public FighterStats blueStats;
    public SpriteRenderer blueSprite;
    public Animator blueAnimator;

    void Start()
    {
        int redIndex = PlayerPrefs.GetInt("RedSelectedOption", 0);
        int blueIndex = PlayerPrefs.GetInt("BlueSelectedOption", 0);

        ApplyCharacter(characterDB.GetCharacter(redIndex), redStats, redSprite, redAnimator);
        ApplyCharacter(characterDB.GetCharacter(blueIndex), blueStats, blueSprite, blueAnimator);
    }

    void ApplyCharacter(Character ch, FighterStats stats, SpriteRenderer sprite, Animator animator)
    {
        if (ch == null || stats == null) return;

        // statok másolása
        stats.health = ch.health;
        stats.magic = ch.mana;
        stats.melee = ch.meleeDamageMax;
        stats.magicRange = ch.rangedDamageMax;
        stats.defense = ch.defense;

        // sprite
        if (sprite != null)
            sprite.sprite = ch.characterSprite;

        // animator
        if (animator != null)
            animator.runtimeAnimatorController = ch.battleAnimator;

        // HP/MP csík init
        stats.ReInitFromCurrent();
    }
}
