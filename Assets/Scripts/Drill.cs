using UnityEngine;

public class Drill : MonoBehaviour
{
    [SerializeField] AudioClip[] SFX_AsteroidBreak;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Asteroid"){
            other.GetComponentInParent<Asteroid>().Mine();
            AudioManager.instance.PlayRandomOneShot(SFX_AsteroidBreak);
        }

        if( other.GetComponentInParent<Fuel>()){
            other.GetComponentInParent<Fuel>().PickUp();
        }

        if(other.tag == "Hazard"){
            GameManager.instance.KillPlayer();
        }
    }
}
