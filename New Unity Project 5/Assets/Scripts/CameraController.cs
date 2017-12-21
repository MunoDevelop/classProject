using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform componentKeeper;
    public Transform character;

    [SerializeField]
    float cameraRadious = 11;
    [SerializeField]
    float cameraRotateSpeed = 0.1f;

    Vector3 preDistance;
	// Use this for initialization
	void Start () {
        character = componentKeeper.GetComponent<EventController>().userCharacter;
        preDistance = Camera.main.transform.position - character.position;
    }
	
    bool isViewPortRside()
    {
        if(Camera.main.transform.position.z - character.position.z > 0)
        {
            return true;
        }
        return false;
    }

    float getCameraZPositionByCircle(float x,bool isRSide)
    {
        float z;
       
        float a = character.position.x;
        float b = character.position.z;
        if (!isRSide)
        {
            z = b - Mathf.Sqrt(Mathf.Abs((cameraRadious - x + a) * (cameraRadious + x - a)));
        }
        else
        {
            z = b + Mathf.Sqrt(Mathf.Abs((cameraRadious - x + a) * (cameraRadious + x - a)));
        }
        return z;
    }
	// Update is called once per frame
	void Update () {
        //base move
        this.transform.position = character.position + preDistance;

        // ps4 view control 
        float maxX = character.position.x + cameraRadious;
        float minX = character.position.x - cameraRadious;

        float cameraPositionX = this.transform.position.x;

        if (Input.GetKey(KeyCode.Q))
        {
            if (isViewPortRside())
            {
                Vector3 v3 = this.transform.position;
                v3.x += cameraRotateSpeed;
                v3.z = getCameraZPositionByCircle(v3.x,true);
                this.transform.position = v3;
                
            }
            else
            {
                Vector3 v3 = this.transform.position;
                v3.x -= cameraRotateSpeed;
                v3.z = getCameraZPositionByCircle(v3.x,true);
                this.transform.position = v3;
            }
        }


        if (Input.GetKey(KeyCode.E) )
        {
            if (isViewPortRside())
            {
                Vector3 v3 = this.transform.position;
                v3.x += cameraRotateSpeed;
                v3.z = getCameraZPositionByCircle(v3.x, false);
                this.transform.position = v3;
            }
            else
            {
                Vector3 v3 = this.transform.position;
                v3.x -= cameraRotateSpeed;
                v3.z = getCameraZPositionByCircle(v3.x, false);
                this.transform.position = v3;
            }
        }






        this.transform.LookAt(character);
        preDistance = Camera.main.transform.position - character.position;
    }
}
