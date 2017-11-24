using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour {
    int virtualX;
    int virtualZ;

    float tileSize = 10;
    // Use this for initialization
    ShellNumber shellNumber;


    void Start () {
        // print(this.transform.position);
        ShellNumber shellNumber = new ShellNumber();
    }
    struct ShellNumber
    {
      public  int ShellX;
      public  int ShellZ;
    }

    static ShellNumber PointToShellNumber(int virtualX,int virtualZ,ShellNumber shellNumber)
    {
        shellNumber.ShellX = virtualX;
        shellNumber.ShellZ = virtualZ * 10;
        print(shellNumber.ShellZ + shellNumber.ShellX);

        return shellNumber;
    }

    void FixedUpdate()
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        if (Physics.Raycast(ray, out hit))
        {
        virtualX = (int)(hit.point.x/tileSize);

        virtualZ = (int)(hit.point.z/tileSize);

            //print("X:  "+virtualX+"Z:  "+ virtualZ);
            //  print(virtualZ * 10 + virtualX);

            PointToShellNumber(virtualX, virtualZ, shellNumber);

        }

        
           
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
