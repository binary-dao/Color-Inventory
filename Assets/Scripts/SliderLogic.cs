using UnityEngine;
using UnityEngine.UI;

public class SliderLogic : MonoBehaviour {
    public Slider slider;

    // Use this for initialization
    void Start ()
    {
        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void ValueChangeCheck()
    {
        InventoryExample.UpdateColorBall();
    }
}
