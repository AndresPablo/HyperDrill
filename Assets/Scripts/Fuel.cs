using UnityEngine;

public class Fuel: MonoBehaviour
{
    public bool fullReload;
    public int fuelRestockAmount = 50;
    [SerializeField]GameObject destroyParticlesPrefab;

    #region  EVENTS
    public delegate void Delegate(GameObject gameObject);
    public static event Delegate OnPickup;
    #endregion

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Drill"){
            PickUp();
        }
    }

    public void PickUp(){
        if(!fullReload) 
            GameManager.instance.player.Refuel(fuelRestockAmount);
        if(fullReload) 
            GameManager.instance.player.RefuelFull();
        if(destroyParticlesPrefab)
            Instantiate(destroyParticlesPrefab, transform.position, transform.rotation, null);

        if(OnPickup != null)
            OnPickup(this.gameObject);
        Destroy(this.gameObject);
    }
}
