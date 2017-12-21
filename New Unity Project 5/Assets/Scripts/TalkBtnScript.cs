using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBtnScript : MonoBehaviour {
    public GameObject bg;
    public GameObject utc;
    public GameObject camera;
    public GameObject thisBtn;

    public void closeAndDestroy()
    {
        Destroy(bg);
        Destroy(utc);
        Destroy(camera);
        Destroy(thisBtn);
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
