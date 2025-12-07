using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    public CharacterManager redSelector;
    public CharacterManager blueSelector;

    public void ConfirmAndStartFight()
    {
        PlayerPrefs.SetInt("RedSelectedOption", redSelector.selectedOption);
        PlayerPrefs.SetInt("BlueSelectedOption", blueSelector.selectedOption);

        SceneManager.LoadScene("FightScene");
    }
}
