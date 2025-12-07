using UnityEngine;


[CreateAssetMenu(menuName = "Characters/Character")]
public class Character : ScriptableObject
{
    public string characterName;
    public string realname;
    public string race;

    public Sprite characterSprite;
    public RuntimeAnimatorController battleAnimator;

    public int health;
    public int defense;
    public int mana;

    public int meleeDamageMin;
    public int meleeDamageMax;
    public int rangedDamageMin;
    public int rangedDamageMax;

}
