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
        indexDesJointsSympas = new int[] { 0, 1, 3, 5, 6, 8, 9, 11, 12, 14, 15 };
        indexDesJointsChiantsY = new int[] { 4, 7, 10, 13 };
        indexDesJointsChiantsX = new int[] { 2 };
        LoadFile();
    }
	
	// Update is called once per frame
	void Update ()
    {
        q = new Quaternion(x, y, z, w);
	    if (idx < maxIdx)
        {
            parts[0].localRotation = new Quaternion(poses[idx][ 0 * 4 + 1], poses[idx][ 0 * 4 + 2], -poses[idx][ 0 * 4 + 3], poses[idx][ 0 * 4]);
            parts[1].localRotation = new Quaternion(-poses[idx][ 1 * 4 + 2], poses[idx][ 1 * 4 + 3], poses[idx][ 1 * 4 + 1], poses[idx][ 1 * 4]);
            //parts[2].localRotation = new Quaternion(-poses[idx][ 2 * 4 + 2], poses[idx][ 2 * 4 + 1], poses[idx][ 2 * 4 + 3], poses[idx][ 2 * 4]);
            parts[3].localRotation = new Quaternion(poses[idx][ 3 * 4 + 1], -poses[idx][ 3 * 4 + 2], poses[idx][ 3 * 4 + 3], poses[idx][ 3 * 4]);
            //parts[4].localRotation = (new Quaternion(poses[idx][ 4 * 4 + 1], poses[idx][ 4 * 4 + 2], poses[idx][ 4 * 4 + 3], poses[idx][ 4 * 4]));
            parts[5].localRotation = new Quaternion(poses[idx][ 5 * 4 + 1], poses[idx][ 5 * 4 + 2], -poses[idx][ 5 * 4 + 3], poses[idx][ 5 * 4]);
            parts[6].localRotation = new Quaternion(poses[idx][ 6 * 4 + 1], poses[idx][ 6 * 4 + 2], -poses[idx][ 6 * 4 + 3], poses[idx][ 6 * 4]);
            //parts[7].localRotation = new Quaternion(poses[idx][ 7 * 4 + 1], poses[idx][ 7 * 4 + 2], -poses[idx][ 7 * 4 + 3], poses[idx][ 7 * 4]);
            parts[8].localRotation = new Quaternion(poses[idx][ 8 * 4 + 1], poses[idx][ 8 * 4 + 2], -poses[idx][ 8 * 4 + 3], poses[idx][ 8 * 4]);
            parts[9].localRotation = new Quaternion(poses[idx][ 9 * 4 + 1], poses[idx][ 9 * 4 + 2], -poses[idx][ 9 * 4 + 3], poses[idx][ 9 * 4]);
            //parts[10].localRotation = new Quaternion(poses[idx][ 10 * 4 + 1], poses[idx][ 10 * 4 + 2], poses[idx][ 10 * 4 + 3], poses[idx][ 10 * 4]);
            parts[11].localRotation = new Quaternion(poses[idx][ 11 * 4 + 1], poses[idx][ 11 * 4 + 2], -poses[idx][ 11 * 4 + 3], poses[idx][ 11 * 4]);
            parts[12].localRotation = new Quaternion(poses[idx][ 12 * 4 + 1], poses[idx][ 12 * 4 + 2], -poses[idx][ 12 * 4 + 3], poses[idx][ 12 * 4]);
            //parts[13].localRotation = new Quaternion(poses[idx][ 13 * 4 + 1], poses[idx][ 13 * 4 + 2], poses[idx][ 13 * 4 + 3], poses[idx][ 13 * 4]);
            parts[14].localRotation = new Quaternion(poses[idx][ 14 * 4 + 1], poses[idx][ 14 * 4 + 2], -poses[idx][ 14 * 4 + 3], poses[idx][ 14 * 4]);
            parts[15].localRotation = new Quaternion(poses[idx][ 15 * 4 + 1], poses[idx][ 15 * 4 + 2], -poses[idx][ 15 * 4 + 3], poses[idx][ 15 * 4]);
        }
        idx++;
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
        maxIdx = poses.GetLength(0);
    }
}
