using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour {

	[SerializeField]
	Transform cube;

	int virtualX;
	int virtualZ;


	float tileSize = 10;
	// Use this for initialization
	ShellNumber shellNumber;

	List<Tile> blockTileList;

	void Start () {
		// print(this.transform.position);
	//	ShellNumber shellNumber = new ShellNumber();

		blockTileList = new List<Tile> ();
	}
	struct ShellNumber
	{
		public  int ShellX;
		public  int ShellZ;
	}

	struct Neighber{
		public int virtualX;
		public int virtualY;
	}
	struct Tile{
		public int x;
		public int z;
		public Tile(int X,int Z){
			x = X;
			z = Z;
		}
	}

	static ShellNumber PointToShellNumber(int virtualX,int virtualZ,ShellNumber shellNumber)
	{
		shellNumber.ShellX = virtualX;
		shellNumber.ShellZ = virtualZ * 10;
		print(shellNumber.ShellZ + shellNumber.ShellX);

		return shellNumber;
	}

	static List<Neighber> GetNeighbers(int virtualX,int virtualZ){
		Neighber left;
	    Neighber right;
		Neighber up;
		Neighber down;
		List<Neighber> neighbers = new List<Neighber>();
		if (virtualX > 0) {
			left.virtualX = virtualX - 1;
			left.virtualY = virtualZ;
			neighbers.Add (left);
		}

		if(virtualX<100){
			right.virtualX = virtualX + 1;
			right.virtualY = virtualZ;
			neighbers.Add (right);
		}

		if (virtualZ < 100) {
			up.virtualX = virtualX;
			up.virtualY = virtualZ + 1;
			neighbers.Add (up);
		}

		if (virtualZ > 0) {
			down.virtualX = virtualX;
			down.virtualY = virtualZ - 1;
			neighbers.Add (down);
		}


		return neighbers;
	}



	void FixedUpdate()
	{

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


		if (Physics.Raycast(ray, out hit))
		{
			virtualX = (int)(hit.point.x/tileSize);

			virtualZ = (int)(hit.point.z/tileSize);

            //drawLine()
          //  Vector3 start = new Vector3(virtualX * tileSize, 0, virtualZ * tileSize);
            Vector3 end = new Vector3((virtualX + 1) * tileSize, 0, virtualZ * tileSize);

         //   Gizmos.DrawLine(Vector3.zero, end);
           

            
            List<Neighber> neighbers =  GetNeighbers(virtualX,virtualZ);

			//foreach (Neighber neighber in neighbers) {
				
			//	Debug.Log ("x: "+neighber.virtualX + "Y: "+neighber.virtualY);
			//}

			if (Input.GetMouseButtonDown (0) && (!blockTileList.Contains (new Tile (virtualX, virtualZ)))) {

				Vector3 pos;
				pos.x = virtualX * tileSize;
				pos.y = 0;
				pos.z = virtualZ * tileSize;
				Quaternion rot = new Quaternion ();

				Instantiate (cube, pos, rot);

				Tile tile;
				tile.x = virtualX;
				tile.z = virtualZ;
				blockTileList.Add (tile);
			} else if(Input.GetMouseButtonDown (0)) {
				Debug.Log ("Finding path");
			}

		}
		//----------------------








	}

	// Update is called once per frame
	void Update () {

	}
}
