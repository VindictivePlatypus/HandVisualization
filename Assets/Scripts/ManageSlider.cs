using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManageSlider : MonoBehaviour {

    public Text t;
    public PlayHandPoseFromFile p;

    private Slider s;

    void Awake()
    {
        s = this.GetComponent<Slider>();
    }

    public void UpdateStuff()
    {
        t.text = s.value.ToString() + "/" + s.maxValue.ToString();
        p.SliderChanged((int)s.value);
    }
}
