using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;
    public Text nameText;
    public SpriteRenderer artworkSprite;
    public Text raceText;
    public Text realnameText;
    public Text healthText;
    public Text defenseText;
    public Text manaText;
    public Text meleeDamageText;
    public Text rangedDamageText;

    public enum PlayerSide { Red, Blue }
    public PlayerSide side = PlayerSide.Red;

    public int selectedOption = 0;

    void Start()
    {
        UpdateCharacter(selectedOption);
    }

    public void NextOption()
    {
        selectedOption++;

        if (selectedOption >= characterDB.characterCount)
            selectedOption = 0;

        UpdateCharacter(selectedOption);
    }

    public void BackOption()
    {
        selectedOption--;

        if (selectedOption < 0)
            selectedOption = characterDB.characterCount - 1;

        UpdateCharacter(selectedOption);
    }

    public void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);

        Debug.Log($"[CharacterManager {side}] index={selectedOption}, char={character?.characterName}");

        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;

        raceText.text = character.race;
        realnameText.text = character.realname;

        healthText.text = character.health.ToString();
        defenseText.text = character.defense.ToString();
        manaText.text = character.mana.ToString();

        meleeDamageText.text = character.meleeDamageMin + " - " + character.meleeDamageMax;
        rangedDamageText.text = character.rangedDamageMin + " - " + character.rangedDamageMax;
    }



    public void ConfirmSelection()
    {
        string key = (side == PlayerSide.Red) ? "RedSelectedOption" : "BlueSelectedOption";
        PlayerPrefs.SetInt(key, selectedOption);
        PlayerPrefs.Save();
    }
}

