using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SpawnsCorrectCharacter
{
    [UnityTest]
    public IEnumerator SpawnsOneRedAndOneBlue()
    {
        // Arrange
        PlayerPrefs.SetInt("RedSelectedOption", 1);
        PlayerPrefs.SetInt("BlueSelectedOption", 2);

        SceneManager.LoadScene("FightScene", LoadSceneMode.Single);
        yield return null;
        yield return null;

        
        var reds  = GameObject.FindGameObjectsWithTag("redHero");
        var blues = GameObject.FindGameObjectsWithTag("blueHero");

        int activeRed  = reds.Count(r => r.activeInHierarchy);
        int activeBlue = blues.Count(b => b.activeInHierarchy);

        
        Assert.AreEqual(1, activeRed,  "Pontosan 1 aktív Red hős kell legyen.");
        Assert.AreEqual(1, activeBlue, "Pontosan 1 aktív Blue hős kell legyen.");
    }
}
