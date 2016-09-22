using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

public class generatePressureTexture : MonoBehaviour {
    public string folder;

    private string filePath;
    private float[][][] pressureFrames;
    private int length;
    private MeshRenderer mesh;
    private int idx;
    private float maxPressure;

    private Texture2D tex;

	// Use this for initialization
	void Start () {
        tex = new Texture2D(25, 29);
        tex.filterMode = FilterMode.Point;
        //tex.anisoLevel = 0;
        LoadFile();
        
        mesh = this.GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (idx < length)
        {
            MakeTexture(idx);
            mesh.material.mainTexture = tex;
            idx++;
            //print(tex.GetPixels()[1]);
        }
	}

    void LoadFile()
    {
        filePath = folder + "\\tekscan.dat";
        var lines = File.ReadAllLines(filePath);
        pressureFrames = new float[lines.Length][][];
        length = lines.Length;
        for (int i = 0; i < lines.Length; ++i)
        {
            pressureFrames[i] = new float[25][];
            var data = lines[i].Split(' ');
            for (int j = 0; j < 25; ++j)
            {
                pressureFrames[i][j] = new float[29];
                for (int k = 0; k < 29; k++)
                {
                    pressureFrames[i][j][k] = Convert.ToSingle(data[j*29 + k]);
                }
            }
                
        }
        float[][] t = pressureFrames.SelectMany(x => x).ToArray();
        float[] t1 = t.SelectMany(x => x).ToArray();
        maxPressure = t1.Max();
    }

    private Color GetColor(float v)
    {
        if (v < maxPressure/2) return Color.Lerp(Color.blue, Color.green, 2*v/maxPressure);
        else return Color.Lerp(Color.green, Color.red, -1 + 2*v/maxPressure);
    }

    private void MakeTexture(int index)
    {
        for (int i = 0; i < 25; i++)
        {
            for (int j = 0; j < 29; j++)
            {
                tex.SetPixel(i, j, GetColor(pressureFrames[index][i][j]));
                //tex.SetPixel(i, j, Color.blue);
            }
        }
        tex.Apply();
        //print(Application.dataPath + " /../ SavedScreen.png");
        //File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }
}
