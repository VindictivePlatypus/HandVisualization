using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandControlHinges : MonoBehaviour {

    public List<GameObject> parts;
    public List<Transform> transfTargets;

    public int indexToShow = 0;

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
        //print(transf[indexToShow].localEulerAngles);
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
            Transform target = transfTargets[i];
            HingeJoint h = joints[i];
            JointMotor m = h.motor;
            float theta = 0;
            float localAngle = 0;
            if (h.axis.Equals(new Vector3(1, 0, 0)))
            {
                localAngle = t.localEulerAngles.x;
                theta = target.localEulerAngles.x;
            }
            else if (h.axis.Equals(new Vector3(0, 1, 0)))
            {
                localAngle = t.localEulerAngles.y;
                theta = target.localEulerAngles.y;
            }
            else
            {
                localAngle = t.localEulerAngles.z;
                theta = target.localEulerAngles.z;
            }
            if (localAngle > 180 && i != 1)
                localAngle -= 360;
            if (theta > 180 && i != 1)
                theta -= 360;
            float a = theta - localAngle;
            if (Mathf.Abs(a) > .5 )
            {
                m.targetVelocity = a;
            } else
            {
                m.targetVelocity = 0;
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
