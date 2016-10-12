using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandControlHinges : MonoBehaviour {

    public List<GameObject> parts;

    private List<Transform> transf = new List<Transform>();
    private List<HingeJoint> joints = new List<HingeJoint>();

    public float[] targetAngles = new float[16];

    public bool useMotors = false;

	// Use this for initialization
	void Start () {
	    foreach (GameObject g in parts)
        {
            transf.Add(g.transform);
            joints.Add(g.GetComponent<HingeJoint>());
        }
        LoadTargetAngles();
	}
	
	// Update is called once per frame
	void Update () {
        if (useMotors)
            UpdateSpeed();
        else
            StopMotors();
	}

    void UpdateSpeed()
    {
        for (int i = 0; i < 16; i++)
        {
            Transform t = transf[i];
            HingeJoint h = joints[i];
            JointMotor m = h.motor;
            float theta = targetAngles[i];
            if (theta < 0)
            {
                theta = 180 - theta;
            }
            float localAngle = 0;
            if (h.axis.Equals(new Vector3(1, 0, 0)))
            {
                localAngle = t.localEulerAngles.x;
            }
            else if (h.axis.Equals(new Vector3(0, 1, 0)))
            {
                localAngle = t.localEulerAngles.y;
            }
            else
            {
                localAngle = t.localEulerAngles.z;
            }
            if (localAngle - theta > .1)
            {
                m.targetVelocity = Mathf.Min(25, Mathf.Abs(localAngle - theta));
            }
            else if (localAngle - theta < -.1)
            {
                m.targetVelocity = -Mathf.Min(25, Mathf.Abs(localAngle - theta));
            }
            h.motor = m;
        }
    }

    public void StopMotors()
    {
        //useMotors = false;
        for (int i = 0; i < 16; i++)
        {
            HingeJoint h = joints[i];
            JointMotor m = h.motor;
            m.targetVelocity = 0;
            h.motor = m;
        }
    }

    void LoadTargetAngles()
    {

    }
}
