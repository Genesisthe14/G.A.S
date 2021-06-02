using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Handle the rope is moved by")]
    private GameObject ropeHandle;

    [SerializeField]
    [Tooltip("Weight hanging on the end of the rope")]
    private Rigidbody2D weight; // make rigidbody and move with physics system

    [Tooltip("Adjusts the stretch error of the rope.")]
    [Range(0.1f, 0.99f)]
    [SerializeField]
    private float errorAdjust = 0.5f;

    [Tooltip("Adjusts how the rope falls. The higher the faster it falls and settles down.")]
    [Range(0.0f,-10.0f)]
    [SerializeField]
    private float yGravity = -1.5f;

    [Tooltip("Adjusts how fast the rope reacts.")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float adjustmentSpeed = 1.0f;

    [Tooltip("Adjusts the length of the rope.")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float ropeSegLen = 0.25f;

    //lineRenderer that renders the rope
    private LineRenderer lineRenderer;

    //List of ropesegemnts that stores the current and former position of a point in the lineRenderer
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    
    //number of segments/points the lineRenderer should have
    private readonly int segmentLength = 35;


    /*
     * Generate LIst of ProxiColliderObjects from a Prefab
     * set Positions of proxis to positions of points in linerenderer
     * move proxis with rope if they didn't collide if they did move linerenderer Points to proxi location
     */



    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        //start point of rope which is at the position of the ropeHandle
        Vector3 ropeStartPoint = ropeHandle.transform.position;

        //Generate segmentLength amount of segemnts/points for the lineRenderer 
        //each having a distance in the y direction of segmentLength (rope goes straight down)
        for (int i = 0; i < segmentLength; i++)
        {
            ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        //set the position of the weight to the position of the last point of the rope
        weight.position = ropeSegments[segmentLength-1].posNow;

    }


    private void FixedUpdate()
    {
        Simulate(); 
        DrawRope();

        AdjustWeightPosition();
        
    }

    //Adjust the position of the weight to be at the end of the rope
    private void AdjustWeightPosition()
    {
        //set weight velocity to reach the end point of the rope
        Vector2 lastPosVelocity = ropeSegments[segmentLength - 1].posNow - ropeSegments[segmentLength - 1].posOld;
        weight.velocity = lastPosVelocity/Time.deltaTime;

        Vector3 lastPotionPos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        weight.MovePosition(new Vector3(lastPotionPos.x, lastPotionPos.y, 0)); //adjust to physics 
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, yGravity);

        for (int i = 1; i < segmentLength; i++)
        {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;

            //calculate the new position of the current segment/point after the movement
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity * adjustmentSpeed;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;

            ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            //apply constraints to the rope to follow the ropeHandle
            ApplyConstraint();
        }

    }

    private void ApplyConstraint()
    {
        //Constraint to ropeHandle
        RopeSegment firstSegment = ropeSegments[0];

        //set the current position of the first segment to the position of the ropeHandle
        firstSegment.posNow = ropeHandle.transform.position;
        ropeSegments[0] = firstSegment;

        for (int i = 0; i < segmentLength - 1; i++)
        {
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            //calculate the error from the movement of the rope and
            //adjust the points/segments of the rope accordingly

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            } else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * errorAdjust;
                ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * errorAdjust;
                ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    //draw the points of the rope with the lineRenderer
    private void DrawRope()
    {
        Vector3[] ropePositions = new Vector3[segmentLength];
        for (int i = 0; i < segmentLength; i++)
        {
            ropePositions[i] = ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    //RopeSegment which stores the current and former position of the segment/ point in the lineRenderer
    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            posNow = pos;
            posOld = pos;
        }
    }
}
