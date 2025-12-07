using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("UI")]
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private Text battleText;

    private FighterStats redStats;
    private FighterStats blueStats;

    // true = redHero köre, false = blueHero köre
    private bool isRedTurn = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (battleMenu == null)
            battleMenu = GameObject.Find("ActionMenu");

        if (battleText == null)
            battleText = GameObject.Find("BattleMessage")?.GetComponent<Text>();
    }

    void Start()
    {
        GameObject redHero = GameObject.FindGameObjectWithTag("redHero");
        GameObject blueHero = GameObject.FindGameObjectWithTag("blueHero");

        if (redHero == null || blueHero == null)
        {
            Debug.LogError("[GC] redHero vagy blueHero nincs a scene-ben (Tag alapján)!");
            return;
        }

        redStats = redHero.GetComponent<FighterStats>();
        blueStats = blueHero.GetComponent<FighterStats>();

        if (redStats == null || blueStats == null)
        {
            Debug.LogError("[GC] FighterStats hiányzik valamelyik hősről!");
            return;
        }

        isRedTurn = true;
        UpdateTurnUI();
    }

    private void UpdateTurnUI()
    {
        if (battleText != null)
            battleText.text = isRedTurn ? "Red Hero's Turn" : "Blue Hero's Turn";

        if (battleMenu != null)
            battleMenu.SetActive(true);

        Debug.Log($"[GC] Turn: {(isRedTurn ? "RED" : "BLUE")}");
    }

    public void NextTurn()
    {
        if (redStats != null && redStats.GetDead())
        {
            Debug.Log("[GC] Blue Hero wins!");
            // TODO: win screen
            return;
        }

        if (blueStats != null && blueStats.GetDead())
        {
            Debug.Log("[GC] Red Hero wins!");
            // TODO: win screen
            return;
        }

        isRedTurn = !isRedTurn;
        UpdateTurnUI();
    }

    public GameObject GetCurrentHero()
    {
        if (redStats == null || blueStats == null)
            return null;

        return isRedTurn ? redStats.gameObject : blueStats.gameObject;
    }

    // ==== UI gombok innen hívódnak ====

    public void OnMeleeButton()
    {
        DoAttackFromCurrentHero("melee");
    }

    public void OnRangeButton()
    {
        DoAttackFromCurrentHero("range");
    }

    public void OnRunButton()
    {
        DoAttackFromCurrentHero("run");
    }

    private void DoAttackFromCurrentHero(string attackType)
    {
        GameObject hero = GetCurrentHero();
        if (hero == null)
        {
            Debug.LogError("[GC] Current hero is NULL, attack aborted.");
            return;
        }

        FighterAction action = hero.GetComponent<FighterAction>();
        if (action == null)
        {
            Debug.LogError("[GC] Current hero has no FighterAction.");
            return;
        }

        Debug.Log($"[GC] {hero.name} performs {attackType}");
        action.SelectAttack(attackType);
    }
}
