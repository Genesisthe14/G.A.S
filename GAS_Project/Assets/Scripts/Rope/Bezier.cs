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


    private Vector2 point1_2D;
    private Vector2 point2_2D;
    private Vector2 point3_2D;

    private void Start()
    {
        point1_2D = new Vector2(point1.position.x, point1.position.y);
        point2_2D = new Vector2(point2.position.x, point2.position.y);
        point3_2D = new Vector2(point3.position.x, point3.position.y);

        ChangeRope();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 tempPoint1_2D = new Vector2(point1.position.x, point1.position.y);
        Vector2 tempPoint2_2D = new Vector2(point2.position.x, point2.position.y);
        Vector2 tempPoint3_2D = new Vector2(point3.position.x, point3.position.y);

        if(tempPoint1_2D != point1_2D || tempPoint2_2D != point2_2D || tempPoint3_2D != point3_2D)
        {
            point1_2D = tempPoint1_2D;
            point2_2D = tempPoint2_2D;
            point3_2D = tempPoint3_2D;

            ChangeRope();
        }
    }

    private void ChangeRope()
    {
        var pointList = new List<Vector3>();

        // unterteilt den Bezier Spline nach einer gegebenen Anzahl Vertices iteriert von links nach rechts
        for (float ratio = 0.0f; ratio <= 1.0f; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector2.Lerp(point1_2D, point2_2D, ratio);
            var tangentLineVertex2 = Vector2.Lerp(point2_2D, point3_2D, ratio);

            var bezierPoint = Vector2.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);

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

        for(float ratio = 0.5f / vertexCount; ratio < 1.0f; ratio += 1.0f / vertexCount) 
        { 
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(Vector3.Lerp(point1.position, point2.position, ratio), Vector3.Lerp(point2.position, point3.position, ratio));
        }
    }
}
