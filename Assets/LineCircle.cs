using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineCircle : MonoBehaviour
{
    [Range(0,50)]
    public int segments = 50;
    [Range(0,20)]
    public float xradius = 5;
    [Range(0,20)]
    public float yradius = 5;
    LineRenderer line;

    void Start ()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.positionCount = segments+1;
        line.useWorldSpace = false;
        line.enabled = false;

        CreatePoints();
    }

    void CreatePoints ()
    {
        float x = 0;
        float y = 0;
        //float z = 0;

        float angle = 20f;
        line.positionCount = 0;
        line.positionCount = segments+1;

        line.enabled = true;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition (i,new Vector3(x,y,0) );

            angle += (360f / segments);
        }
    }

    public void SetColor(Color newColor)
    {
        line.startColor = newColor;
        line.endColor = newColor;
    }
}
