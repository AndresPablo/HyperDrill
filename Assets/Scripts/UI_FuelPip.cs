using UnityEngine.UI;
using UnityEngine;

public class UI_FuelPip : MonoBehaviour
{
    [SerializeField]Image background;
    [SerializeField]GameObject fillGO;

    public void SetFilled(bool state)
    {
        fillGO.SetActive(state);
    }
}
