using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    void Update()
    {
        Vector3 mousePosition;
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = new Vector3(mousePosition.x,mousePosition.y,0);
    }
}
