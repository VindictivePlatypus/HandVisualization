using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class FillDropdown : MonoBehaviour {
    
    public string dataFolder;
    public PlayHandPoseFromFile p;
    public HandControlSlerp hcs;
    private Dropdown dd;

	// Use this for initialization
	void Start () {
        dd = this.GetComponent<Dropdown>();
        DirectoryInfo d = new DirectoryInfo(dataFolder);
        DirectoryInfo[] ds = d.GetDirectories();
        List<string> dirnames = new List<string>();
        foreach(DirectoryInfo dir in ds)
        {
            dirnames.Add(dir.Name);
        }
        dd.AddOptions(dirnames);
        SendDirectory();
    }
	
    public void SendDirectory()
    {
        p.ChangeFolder(dataFolder + "\\" + dd.options[dd.value].text);
        p.ChangeFolder(dataFolder + "\\" + dd.options[dd.value].text);
        hcs.ChangeInit();
    }

}
