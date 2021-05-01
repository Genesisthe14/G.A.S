using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject ropeHandle;
    public Rigidbody2D weight; // make rigidbody and move with physics system

    [Tooltip("Adjusts the stretch error of the rope.")]
    [Range(0.1f, 0.99f)]
    [SerializeField]
    private float errorAdjust = 0.5f;

    [Tooltip("Adjusts how the rope falls. The higher the faster it falls and settles down.")]
    [Range(-0.1f,-10.0f)]
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

    [Tooltip("Adjusts the width of the rope.")]
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float lineWidth = 0.1f;


    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    private int segmentLength = 35;


    /*
     * Generate LIst of ProxiColliderObjects from a Prefab
     * set Positions of proxis to positions of points in linerenderer
     * move proxis with rope if they didn't collide if they did move linerenderer Points to proxi location
     */



    // Use this for initialization
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();

        Vector3 ropeStartPoint = ropeHandle.transform.position;//Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        weight.position = ropeSegments[segmentLength-1].posNow;

    }


    private void FixedUpdate()
    {
        this.Simulate(); 
        this.DrawRope();

        AdjustWeightPosition();
    }

    private void AdjustWeightPosition()
    {
        Vector2 lastPosVelocity = ropeSegments[segmentLength - 1].posNow - ropeSegments[segmentLength - 1].posOld;
        weight.velocity = lastPosVelocity/Time.deltaTime;

        Vector3 lastPotionPos = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        weight.MovePosition(new Vector3(lastPotionPos.x, lastPotionPos.y, 0)); //adjust to physics 
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, yGravity);

        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity * adjustmentSpeed;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }

    }

    private void ApplyConstraint()
    {
        //Constrant to Mouse
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = ropeHandle.transform.position;//Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.ropeSegments[0] = firstSegment;

        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
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
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * errorAdjust;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }

    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
