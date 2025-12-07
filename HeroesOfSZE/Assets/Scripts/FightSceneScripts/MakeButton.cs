using UnityEngine;
using UnityEngine.UI;

public class MakeButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();

        if (btn == null)
        {
            Debug.LogError("[MakeButton] Nincs Button komponens az objecten!");
            return;
        }

        switch (gameObject.name)
        {
            case "MeleeBtn":
                btn.onClick.AddListener(() => GameController.Instance?.OnMeleeButton());
                break;

            case "RangeBtn":
                btn.onClick.AddListener(() => GameController.Instance?.OnRangeButton());
                break;

            default: // pl. RunBtn
                btn.onClick.AddListener(() => GameController.Instance?.OnRunButton());
                break;
        }
    }
}
