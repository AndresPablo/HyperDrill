using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField][Range(1f, 50f)] float radius = 20f;
    [SerializeField][Range(0f, 2f)] float playerExcludeZone = .5f;
    [SerializeField] Transform entityContainer;
    [Space]
    public PrefabSettings asteroid_settings;
    public PrefabSettings anomaly_settings;
    public PrefabSettings fuelCell_settings;
    [Space]
    public Pools pools;

    void Start()
    {
        GameManager.OnGameStart += SpawnInitialWave;
        Player.OnPlayerStop += SpawnFillerWave;
        Asteroid.OnBreak += RemoveAsteroid;
        Fuel.OnPickup += RemoveFuel;
    }

    void RemoveAsteroid(GameObject GO)
    {
        pools.asteroids.Remove(GO);
    }

    void RemoveFuel(GameObject GO)
    {
         pools.fuelCells.Remove(GO);
    }

    public void SpawnInitialWave(){
        Clear();
        for(int i=0; i < asteroid_settings.maxAmount;i++){
            SpawnAsteroid();
        }
        for(int i=0; i < anomaly_settings.maxAmount;i++){
            //SpawnFuel();
        }
        for(int i=0; i < fuelCell_settings.maxAmount;i++){
            //SpawnAnomaly();
        }
    }

    public void Clear(){
        foreach(GameObject go in pools.asteroids)
        {
            Destroy(go);
        }
        foreach(GameObject go in pools.specialAsteroids)
        {
            Destroy(go);
        }
        foreach(GameObject go in pools.anomalies)
        {
            Destroy(go);
        }
        foreach(GameObject go in pools.fuelCells)
        {
            Destroy(go);
        }
        pools.asteroids.Clear();
        pools.anomalies.Clear();
        pools.specialAsteroids.Clear();
        pools.fuelCells.Clear();
    }

    public void SpawnFillerWave(){
        for(int i=0; i < asteroid_settings.amountPerRound;i++){
            if(pools.asteroids.Count > asteroid_settings.maxAmount)
                break;
            SpawnAsteroid();
        }
        for(int i=0; i < anomaly_settings.amountPerRound;i++){
            if(pools.anomalies.Count < anomaly_settings.maxAmount)
            {
                if(Random.value < anomaly_settings.spawnChance)
                    SpawnAnomaly();
            }
            else
                break; 
        }
        for(int i=0; i < fuelCell_settings.amountPerRound;i++){
            if(pools.fuelCells.Count < fuelCell_settings.maxAmount)
            {
                if(Random.value < fuelCell_settings.spawnChance)
                    SpawnFuel();
            }
            else
                break;
        }
    }

    public void SpawnAsteroid(){
        GameObject go = Instantiate
            (asteroid_settings.prefabs[Random.Range(0, asteroid_settings.prefabs.Length)]);
        Vector3 euler = transform.eulerAngles;
        go.transform.SetParent(entityContainer);
        pools.asteroids.Add(go);
        euler.z = Random.Range(0f, 360f);
        go.transform.position = GetRandomCoordinate();
        go.transform.eulerAngles = euler;
    }

    public void SpawnAnomaly()
    {
        Vector2 spawnPos = GetRandomCoordinate();
        Transform[] anomaliesOverlapping = AnomaliesInRadius(spawnPos);
        float extraRadius = 1f;
        Vector3 largestAnomalyScale = new Vector3(0,0,0);
        if(anomaliesOverlapping != null && anomaliesOverlapping.Length > 0)
        {
            float totalX = 0f;
            float totalY = 0f;
            foreach(Transform t in anomaliesOverlapping)
            {
                if(t.localScale.magnitude > largestAnomalyScale.magnitude)
                    largestAnomalyScale = t.localScale;       
                totalX += t.position.x;
                totalY += t.position.y;
                extraRadius += .65f;
                //extraRadius += t.localScale.magnitude; // Demasiado grande
                pools.anomalies.Remove(t.gameObject);
                Destroy(t.gameObject);
            }
            float centerX = totalX / anomaliesOverlapping.Length;
            float centerY = totalY / anomaliesOverlapping.Length;
            spawnPos = new Vector2(centerX, centerY);
        }
        GameObject go = Instantiate(anomaly_settings.Prefab, spawnPos, Quaternion.identity);
        go.transform.localScale = (Vector2.one * extraRadius);
        go.transform.SetParent(entityContainer);
        pools.anomalies.Add(go);
       // go.transform.SetParent(entityContainer);
    }

    public void SpawnFuel(){
        Vector2 spawnPos = GetRandomCoordinate();
        GameObject go = Instantiate(fuelCell_settings.Prefab, spawnPos, Quaternion.identity);
        go.transform.SetParent(entityContainer);
        pools.fuelCells.Add(go);
        //go.transform.SetParent(entityContainer);
    }

    public Vector2 GetRandomCoordinate(){
        Vector2 pos = Random.insideUnitCircle * radius;
        Vector2 playerPos = GameManager.instance.player.transform.position;
        // Vuelve a generar otra posicion si esta sobre el jugador, hasta que encuentre un lugar libre
        while(Vector2.Distance(pos, playerPos) < playerExcludeZone)
        {
            pos = Random.insideUnitCircle * radius;
        }
        return pos;
    }

    Transform[] AnomaliesInRadius(Vector2 pos)
    {
        List<Transform> anomaliesToReturn = new List<Transform>();
        foreach(GameObject go in pools.anomalies)
        {
            if(Vector2.Distance(pos, go.transform.position) <= (.5f + go.transform.localScale.magnitude))
            {
                anomaliesToReturn.Add(go.transform);
            }
        }
        return anomaliesToReturn.ToArray();
    }
}

[System.Serializable]
public class PrefabSettings{
    public GameObject Prefab {get {return prefabs[0];}}
    public GameObject[] prefabs;
    [Range(0, 200)]public int maxAmount = 10;
    public int amountPerRound = 1;
    [Range(0, 1f)]public float spawnChance = 1f;

}

[System.Serializable]
public class Pools
{
    public List<GameObject> asteroids = new List<GameObject>();
    public List<GameObject> specialAsteroids = new List<GameObject>();
    public List<GameObject> anomalies = new List<GameObject>();
    public List<GameObject> fuelCells = new List<GameObject>();
}