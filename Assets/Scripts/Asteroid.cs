using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int points = 10;
    [SerializeField]GameObject destroyParticlesPrefab;

    #region  EVENTS
    public delegate void Delegate(GameObject gameObject);
    public static event Delegate OnBreak;
    #endregion

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Drill"){
            Mine();
        }
    }

    public void Mine(){
        GameManager.instance.AddNewScore(points);
        if(destroyParticlesPrefab)
            Instantiate(destroyParticlesPrefab, transform.position, transform.rotation, null);
        if(OnBreak != null)
            OnBreak(gameObject);
        Destroy(this.gameObject);
    }
}
