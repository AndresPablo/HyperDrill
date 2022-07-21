using UnityEngine;
using TMPro;

public class UI_FloatText : MonoBehaviour
{
    [SerializeField] Vector2 offset;
    [SerializeField] float reduceSpeed = .5f;
    [SerializeField] float minSize = .1f;
    [SerializeField] float beginTime = .2f;
    bool reducing;
    public TextMeshProUGUI label;


    void Start()
    {
        Invoke("BeginReduce", beginTime);
        transform.position = (Vector2)transform.position + offset;
    }

    void BeginReduce()
    {
        reducing = true;
    }

    void Update()
    {
        if(!reducing)
            return;
        transform.localScale -= new Vector3(reduceSpeed,reduceSpeed,reduceSpeed) * Time.deltaTime;
        if(transform.localScale.x < minSize)
            Destroy(gameObject);
    }
}
