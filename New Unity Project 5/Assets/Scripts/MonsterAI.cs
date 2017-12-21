using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {
    public Transform componentKeeper;
    public Transform character;
    // Use this for initialization
    void Start () {
        componentKeeper = GameObject.FindWithTag("GameController").transform;
        character = componentKeeper.GetComponent<EventController>().userCharacter;
    }
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(character);
	}
}
