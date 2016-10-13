using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandControlSlerp : MonoBehaviour {

    public List<Transform> parts;
    public List<Transform> transfTargets;

    public bool slerp = false;
    public int speed = 1;

    private Quaternion[] initPos = new Quaternion[16];
    private int index = 0;

    // Use this for initialization
    void Start () {
        ChangeInit();
	}
	
	// Update is called once per frame
	void Update () {
	    if (slerp)
        {
            UpdateQuaternions();
            if (index < speed)
            {
                index++;
            } else
            {
                slerp = false;
            }
        }
	}

    void UpdateQuaternions()
    {
        for (int i = 0; i < 16; i++)
        {
            parts[i].localRotation = Quaternion.Slerp(initPos[i], transfTargets[i].localRotation, (float)index / (float)speed);
        }
    }

    public void ChangeInit()
    {
        index = 1;
        for (int i = 0; i < 16; i++)
        {
            initPos[i] = parts[i].localRotation;
        }
    }
}
