﻿using UnityEngine;
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

    public List<RawImage> pressImg;
    public Button playPauseButton;

    private string filePathPose, filePathPressure;
    private float[][] poses, pressures;
    private int idx, maxIdx;
    private float maxPressure;

    private Color orange = new Color(1, 0.65f, 0);

    private int[] indexDesJointsSympas= new int[] { 0, 5, 6, 8, 9, 11, 12, 14, 15 }, indexDesJointsChiantsY = new int[] { 4, 7, 10, 13 };
    private int[][] indexPressureImages = new int[][]
    {
        new int[] { 522,523,524,525,547,548,549,550,572,573,574,575 },
        new int[] { 397,398,399,400,422,423,424,425,447,448,449,450,472,473,474,475},
        new int[] { 241,242,243,244,266,267,268,269,291,292,293,294},
        new int[] { 141,142,143,144,166,167,168,169,191,192,193,194},
        new int[] { 16,17,18,19,41,42,43,44,66,67,68,69,91,92,93,94},
        new int[] { 236,237,238,239,261,262,263,264,286,287,288,289},
        new int[] { 136,137,138,139,161,162,163,164,186,187,188,189},
        new int[] { 11,12,13,14,36,37,38,39,61,62,63,64,86,87,88,89},
        new int[] { 231,232,233,234,256,257,258,259,281,282,283,284},
        new int[] { 131,132,133,134,156,157,158,159,181,182,183,184},
        new int[] { 6,7,8,9,31,32,33,34,56,57,58,59,81,82,83,84},
        new int[] { 226,227,228,229,251,252,253,254,276,277,278,279},
        new int[] { 126,127,128,129,151,152,153,154,176,177,178,179},
        new int[] { 1,2,3,4,26,27,28,29,51,52,53,54,76,77,78,79},
        new int[] { 326,327,328,329,330,331,332,333,334,335,336,337,338,
            339,340,341,342,343,344,351,352,353,354,355,356,357,358,359,
            360,361,362,363,364,365,366,367,368,369,376,377,378,379,380,
            381,382,383,384,385,386,387,388,389,390,391,392,393,394,401,
            402,403,404,405,406,407,408,409,410,411,412,413,414,415,416,417,418,419},
        new int[] { 612,613,614,615,616,617,618,619,620,637,638,639,640,
            641,642,643,644,645,662,663,664,665,666,667,668,669,670,687,
            688,689,690,691,692,693,694,695,712,713,714,715,716,717,718,719,720},
        new int[] { 501,502,503,504,526,527,528,529,551,552,553,554,576,577,578,579,601,602,603,604},
        new int[] { 626,627,628,629,630,631,632,633,651,652,653,654,655,656,657,
            658,676,677,678,679,680,681,682,683,701,702,703,704,705,706,707,708}
    };
    private Texture2D[] textures;

    private bool isPLaying;

    void Awake()
    {
        textures = new Texture2D[]
        {
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,4,TextureFormat.ARGB32,false),
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,4,TextureFormat.ARGB32,false),
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,4,TextureFormat.ARGB32,false),
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,4,TextureFormat.ARGB32,false),
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,3,TextureFormat.ARGB32,false),
            new Texture2D(4,4,TextureFormat.ARGB32,false),
            new Texture2D(19,4,TextureFormat.ARGB32,false),
            new Texture2D(9,5,TextureFormat.ARGB32,false),
            new Texture2D(4,5,TextureFormat.ARGB32,false),
            new Texture2D(8,4,TextureFormat.ARGB32,false)
        };
        foreach (Texture2D t in textures)
        {
            t.filterMode = FilterMode.Point;
        }
    }


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
        filePathPose = folderPath + "\\shapehand.dat";
        filePathPressure = folderPath + "\\tekscan.dat";
        var linesPose = File.ReadAllLines(filePathPose);
        var linesPressure = File.ReadAllLines(filePathPressure);
        poses = new float[linesPose.Length][];
        pressures = new float[linesPressure.Length][];
        for (int i = 0; i < linesPressure.Length; ++i)
        {
            poses[i] = new float[64];
            pressures[i] = new float[725];
            var data1 = linesPose[i].Split(' ');
            var data2 = linesPressure[i].Split(' ');
            for (int j = 0; j < 64; ++j)
                poses[i][j] = Convert.ToSingle(data1[j]);
            for (int j = 0; j < 725; ++j)
                pressures[i][j] = Convert.ToSingle(data2[j]);
        }
        maxIdx = linesPressure.Length;
        slide.maxValue = maxIdx-1;
        float[] t = pressures.SelectMany(x => x).ToArray();
        maxPressure = t.Max();
    }

    void UpdatePose()
    {
        if (idx < maxIdx)
        {
            SetPose();
            SetPressure();
            idx++;
            slide.value = idx;
        }
    }

    public void PlayPausePose()
    {
        if (isPLaying)
        {
            CancelInvoke("UpdatePose");
            playPauseButton.GetComponentInChildren<Text>().text = "Play";
        } else
        {
            InvokeRepeating("UpdatePose", 0, .03f);
            playPauseButton.GetComponentInChildren<Text>().text = "Pause";
        }
        isPLaying = !isPLaying;
    }

    public void SetPose()
    {
        /*
        //Muthakuckin thumb
        parts[1].localRotation = new Quaternion(poses[idx][1 * 4 + 2], poses[idx][1 * 4 + 3], poses[idx][1 * 4 + 1], poses[idx][1 * 4]);
        parts[2].localRotation = new Quaternion(poses[idx][2 * 4 + 2], poses[idx][2 * 4 + 1], poses[idx][2 * 4 + 3], poses[idx][2 * 4]) * q;
        parts[3].localRotation = new Quaternion(poses[idx][3 * 4 + 1], poses[idx][3 * 4 + 2], poses[idx][3 * 4 + 3], poses[idx][3 * 4]);

        foreach (int i in indexDesJointsSympas)
        {
            parts[i].localRotation = new Quaternion(poses[idx][i * 4 + 1], poses[idx][i * 4 + 2], -poses[idx][i * 4 + 3], poses[idx][i * 4]);
        }
        
        foreach (int i in indexDesJointsChiantsY)
        {
            parts[i].localRotation = new Quaternion(-poses[idx][i * 4 + 1], poses[idx][i * 4 + 2], poses[idx][i * 4 + 3], poses[idx][i * 4]) * q;
        }
        */
        //this is a test of another hand model
        Quaternion q = Quaternion.AngleAxis(180f, new Vector3(0,0,1));
        int i = 0;
        //parts[i].localRotation = (new Quaternion(poses[idx][i * 4 + 1], poses[idx][i * 4 + 2], poses[idx][i * 4 + 3], poses[idx][i * 4]))*q;
        i = 1;
        parts[i].localRotation = (new Quaternion(poses[idx][i * 4 + 1], poses[idx][i * 4 + 2], poses[idx][i * 4 + 3], poses[idx][i * 4]))*q;
        i = 2;
        parts[i].localRotation = (new Quaternion(poses[idx][i * 4 + 1], poses[idx][i * 4 + 2], poses[idx][i * 4 + 3], poses[idx][i * 4]));
        i = 3;
        parts[i].localRotation = (new Quaternion(poses[idx][i * 4 + 1], -poses[idx][i * 4 + 2], poses[idx][i * 4 + 3], poses[idx][i * 4]));
        for (i =4; i < 16; i++)
        {
            parts[i].localRotation = (new Quaternion(poses[idx][i * 4 + 1], poses[idx][i * 4 + 2], -poses[idx][i * 4 + 3], poses[idx][i * 4]));
        }
    }

    public void SetPressure()
    {
        for (int i = 0; i < 18; i++)
        {
            for (int j = 0; j < textures[i].width; j++)
            {
                for (int k = 0; k < textures[i].height; k++)
                {
                    textures[i].SetPixel(j, k, GetColor(pressures[idx][indexPressureImages[i][textures[i].height * j + k]]));
                }
            }
            textures[i].Apply();
            pressImg[i].texture = textures[i];
        }
    }

    public void SliderChanged(int i)
    {
        idx = i;
        SetPose();
        SetPressure();
    }

    private Color GetColor(float w)
    {
        float v = Mathf.Pow(w/maxPressure,.5f);
        if (v < 1f / 4f) return Color.Lerp(Color.blue, Color.green, Mathf.InverseLerp(0f, 1f/4f, v));
        else if (v < 2f / 4f) return Color.Lerp(Color.green, Color.yellow, Mathf.InverseLerp(1f/4f, 1f/2f, v));
        else if (v < 3f / 4f) return Color.Lerp(Color.yellow, orange, Mathf.InverseLerp( 1f/2f,  3f/4f, v));
        else return Color.Lerp(orange, Color.red, Mathf.InverseLerp(3f/4f, 1f, v));
    }


}
