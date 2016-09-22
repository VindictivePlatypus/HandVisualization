using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

public class PlayHandPoseFromFile : MonoBehaviour {
    public string folderPath;

    public List<Transform> parts;
    public Quaternion q;
    [Range(-1f, 1f)]
    public float x, y, z, w;

    private string filePath;
    private float[][] poses;
    private int idx;
    private int maxIdx;

    private int[] indexDesJointsSympas, indexDesJointsChiantsY, indexDesJointsChiantsX;


    // Use this for initialization
    void Start() {
        idx = 0;
        maxIdx = -1;
        indexDesJointsSympas = new int[] { 0, 5, 6, 8, 9, 11, 12, 14, 15 };
        indexDesJointsChiantsY = new int[] { 4, 7, 10, 13 };
        LoadFile();
        InvokeRepeating("UpdatePose",0,.03f);
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    void ResetIdx()
    {
        idx = 0;
    }

    void LoadFile()
    {
        filePath = folderPath + "\\shapehand.dat";
        var lines = File.ReadAllLines(filePath);
        poses = new float[lines.Length][];
        for (int i = 0; i < lines.Length; ++i)
        {
            poses[i] = new float[64];
            var data = lines[i].Split(' ');
            for (int j = 0; j < 64; ++j)
                poses[i][j] = Convert.ToSingle(data[j]);
        }
        maxIdx = lines.Length;
    }

    void UpdatePose()
    {
        if (idx < maxIdx)
        {
            //Muthakuckin thumb
            parts[1].localRotation = new Quaternion(poses[idx][1 * 4 + 2], poses[idx][1 * 4 + 3], poses[idx][1 * 4 + 1], poses[idx][1 * 4]);
            q = Quaternion.AngleAxis(180f, new Vector3(1, 0, 0));
            parts[2].localRotation = new Quaternion(poses[idx][2 * 4 + 2], poses[idx][2 * 4 + 1], poses[idx][2 * 4 + 3], poses[idx][2 * 4]) * q;
            parts[3].localRotation = new Quaternion(poses[idx][3 * 4 + 1], poses[idx][3 * 4 + 2], poses[idx][3 * 4 + 3], poses[idx][3 * 4]);

            foreach (int i in indexDesJointsSympas)
            {
                parts[i].localRotation = new Quaternion(poses[idx][i * 4 + 1], poses[idx][i * 4 + 2], -poses[idx][i * 4 + 3], poses[idx][i * 4]);
            }
            q = Quaternion.AngleAxis(180f, new Vector3(0, 1, 0));
            foreach (int i in indexDesJointsChiantsY)
            {
                parts[i].localRotation = new Quaternion(-poses[idx][i * 4 + 1], poses[idx][i * 4 + 2], poses[idx][i * 4 + 3], poses[idx][i * 4]) * q;
            }

            idx++;
        }
    }
}
