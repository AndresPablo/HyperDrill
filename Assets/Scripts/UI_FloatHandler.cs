using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UI_FloatHandler : MonoBehaviour
{
    [SerializeField] GameObject scorePrefab;
    [SerializeField] GameObject refuelPrefab;
    [SerializeField] GameObject comboPrefab;


    void Start()
    {
        GameManager.OnScoreAdd += SpawnScore;
        Fuel.OnPickup += SpawnRefuel;
    }

    void SpawnScore(int newScore)
    {
        GameObject go = Instantiate(scorePrefab);

        UI_FloatText txt = go.GetComponent<UI_FloatText>();
        txt.label.text = "+"+newScore;
        go.transform.position = GameManager.instance.player.transform.position;
    }

    void SpawnRefuel(GameObject gameObject = null)
    {
        GameObject go = Instantiate(refuelPrefab);
        go.transform.position = GameManager.instance.player.transform.position;
    }
}
