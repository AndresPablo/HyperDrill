using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField][Range(0,60f)] float time = 1;

    void Start()
    {
        Destroy(this.gameObject, time);    
    }

}
