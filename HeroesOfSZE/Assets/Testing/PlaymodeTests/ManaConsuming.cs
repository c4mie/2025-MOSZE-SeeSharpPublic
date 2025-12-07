using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManager;
using UnityEngine.TestTools;

public class ManaConsuming
{
    [UnityTest]
    public IEnumerator RangedAttack_ConsumesMana()
    {
        
        SceneManager.LoadScene("FightScene", LoadSceneMode.Single);
        yield return new WaitUntil(() => GameController.Instance != null);

        var gc = GameController.Instance;
        Assert.IsNotNull(gc, "GameController.Instance null ");

        var attacker = gc.redHero;
        Assert.IsNotNull(attacker, "gc.redHero null ");

        var stats = attacker.GetComponent<FighterStats>();
        Assert.IsNotNull(stats, "A redHero-n nincs FighterStats.");

        var action = attacker.GetComponent<FighterAction>();
        Assert.IsNotNull(action, "A redHero-n nincs FighterAction.");

        
        stats.CurrentMana = 100f;
        float beforeMana = stats.CurrentMana;

        action.SelectAttack("ranged");

        
        for (int i = 0; i < 5; i++)
            yield return null;

        Assert.Less(
            stats.CurrentMana,
            beforeMana,
            $"A ranged attacknak manába kerül. Előtte: {beforeMana}, utána: {stats.CurrentMana}"
        );
    }
}
