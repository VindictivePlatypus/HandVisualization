using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System;

public class ExecutePython : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(PythonExe());
    }

    // Update is called once per frame
    void Update () {
	
	}

    IEnumerator PythonExe()
    {
        yield return null;
        ProcessStartInfo p = new ProcessStartInfo();
        p.UseShellExecute = false;
        p.FileName = "C:\\Lasagne\\WinPython\\WinPython Command Prompt.exe";
        p.Arguments = "python C:\\Lasagne\\code\\mnist.py mlp 5";
        //(,"C:\\Lasagne\\code\\mnist.py mlp 5");
        Process.Start("C:\\Lasagne\\WinPython\\scripts\\cmd2.bat", "python C:\\Lasagne\\code\\mnist.py mlp 5");
    }
}
