using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 rotSpeed;

    void Update()
    {
        transform.Rotate(rotSpeed);    
    }
}
