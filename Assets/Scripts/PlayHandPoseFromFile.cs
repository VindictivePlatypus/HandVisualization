using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayHandPoseFromFile : MonoBehaviour {
    private string folderPath;

    public List<Transform> parts;
    
    public Slider slide;

    private string filePath;
    private float[][] poses;
    private int idx;
    private int maxIdx;

    private int[] indexDesJointsSympas= new int[] { 0, 5, 6, 8, 9, 11, 12, 14, 15 }, indexDesJointsChiantsY = new int[] { 4, 7, 10, 13 };

    private bool isPLaying;


    // Use this for initialization
    void Start() {
        isPLaying = false;
        slide.minValue = 0;
        slide.wholeNumbers = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void ResetIdx()
    {
        idx = 0;
        slide.value = 0;
        slide.GetComponent<ManageSlider>().UpdateStuff();
    }

    public void ChangeFolder(string s)
    {
        folderPath = s;
        LoadFile();
        ResetIdx();
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
        slide.maxValue = maxIdx-1;
    }

    void UpdatePose()
    {
        if (idx < maxIdx)
        {
            SetPose();
            idx++;
            slide.value = idx;
        }
    }

    public void PlayPausePose()
    {
        if (isPLaying)
        {
            CancelInvoke("UpdatePose");
        } else
        {
            InvokeRepeating("UpdatePose", 0, .03f);
        }
        isPLaying = !isPLaying;
    }

    public void SetPose()
    {
        //Muthakuckin thumb
        parts[1].localRotation = new Quaternion(poses[idx][1 * 4 + 2], poses[idx][1 * 4 + 3], poses[idx][1 * 4 + 1], poses[idx][1 * 4]);
        Quaternion q = Quaternion.AngleAxis(180f, new Vector3(1, 0, 0));
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
    }

    public void SliderChanged(int i)
    {
        idx = i;
        SetPose();
    }
}
