using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, this.transform.position.z);
        transform.position =   newPos;  
    }
}
