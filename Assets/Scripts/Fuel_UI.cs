using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Fuel_UI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject fuelPip_prefab;
    [SerializeField] Transform fuelPip_Container;
    [SerializeField] Image fuelBar;
    [SerializeField] Image costBar;
    [SerializeField] Color costColor;
    [SerializeField] Color insuficientColor;
    GameManager gm;
    Player player;


    void Start()
    {
        gm = GameManager.instance;
        player = gm.player;
        Player.OnPlayerMoves += TooglePanel;
        Player.OnPlayerStop += UpdateFuel;
        Player.OnPlayerRefuel += UpdateFuel;
        GameManager.OnGameStart += UpdateFuel;

        SetupFuelPips();
    }

    void SetupFuelPips()
    {
        for (int i = 0; i < (int)player.maxFuel; i++)
        {
            GameObject go = Instantiate(fuelPip_prefab, fuelPip_Container);
            go.transform.SetParent(fuelPip_Container);
            UI_FuelPip pip = go.GetComponent<UI_FuelPip>();
        }
        UpdateFuel();
    }

    void UpdateFuel(){
        panel.SetActive(true);
        int maxFuel = player.maxFuel;
        //fuelBar.fillAmount =   player.fuel / player.maxFuel; // OLD
        for (int i = 0; i < maxFuel ; i++)
        {
            if(i < player.fuel){
                GameObject go = fuelPip_Container.GetChild(i).gameObject;
                UI_FuelPip pip = go.GetComponent<UI_FuelPip>();
                pip.SetFilled(true);
            }else
            {
                GameObject go = fuelPip_Container.GetChild(i).gameObject;
                UI_FuelPip pip = go.GetComponent<UI_FuelPip>();
                pip.SetFilled(false);
            }
        }
    }

    // DEPRECATED
    void UpdateCost(float _cost){
        costBar.fillAmount =  _cost / player.maxFuel;
        if(_cost > player.fuel)
            costBar.color = insuficientColor;
            else
            costBar.color = costColor;
    }

    void TooglePanel(){
        panel.SetActive(!panel.activeSelf);
    }

    private void Update() {
        UpdateCost(player.jumpCost);
    }

}
