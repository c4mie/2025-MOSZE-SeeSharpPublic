using UnityEngine;

public class ChosenPlayer : MonoBehaviour
{
    public CharacterDatabase characterDB;
    public SpriteRenderer artworkSprite;
    public Animator animator;

    public enum PlayerSide { Red, Blue }
    public PlayerSide side = PlayerSide.Red;

    void Start()
    {
        string key = side == PlayerSide.Red ? "RedSelectedOption" : "BlueSelectedOption";
        int index = PlayerPrefs.GetInt(key, 0);

        Character character = characterDB.GetCharacter(index);

        Debug.Log($"[ChosenPlayer {side}] key={key}, index={index}, char={character?.characterName}");

        if (character.characterSprite != null && artworkSprite != null)
            artworkSprite.sprite = character.characterSprite;

        if (character.battleAnimator != null && animator != null)
            animator.runtimeAnimatorController = character.battleAnimator;
    }

}
