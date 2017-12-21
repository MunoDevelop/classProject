using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour {
    [SerializeField]
    Transform componentKeeper;
    [SerializeField]
    public int ToggleNum;

    public void settingPathFind()
    {
        if (this.GetComponent<Toggle>().isOn == true)
        {
            componentKeeper.GetComponent<EventController>().findPathNum = ToggleNum;
        }

        if(this.GetComponent<Toggle>().isOn == false)
        {

        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
