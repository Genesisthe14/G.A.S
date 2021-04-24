using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Erstellt einen Bezier Spline über 3 Punkte diese werden von drei empty Gameobjects definiert
 */
public class Bezier : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public LineRenderer lineRenderer;
    public int vertexCount = 12;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   var pointList = new List<Vector3>();
        // unterteilt den Bezier Spline nach einer gegebenen Anzahl Vertices iteriert von links nach rechts
        for (float ratio = 0.0f / vertexCount; ratio <= 1; ratio += 1.0f / vertexCount)
        {   
            var tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2.position, point3.position, ratio);
            var bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    // Zeichnet Gizmos um die Bezier Kurve auch außerhalb der Laufzeit darzustellen
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1.position, point2.position);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(point2.position, point3.position);

        for(float ratio = 0.5f / vertexCount; ratio<1; ratio += 1.0f / vertexCount) { 
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Vector3.Lerp(point1.position, point2.position, ratio), Vector3.Lerp(point2.position, point3.position, ratio));
        }
    }
}
