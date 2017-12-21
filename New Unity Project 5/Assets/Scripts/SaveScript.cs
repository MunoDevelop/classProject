using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using TilePlugin;

public class SaveScript : MonoBehaviour {

    Dictionary<Vector2, Transform> map;

    List<jsonTile> simpleMap;

    public void saveMap()
    {
        foreach(Transform tile in map.Values)
        {
            jsonTile jTile = new jsonTile(tile.GetComponent<Tile>().VirtualX, tile.GetComponent<Tile>().VirtualZ, tile.GetComponent<Tile>().BuildingNum);
            simpleMap.Add(jTile);
        }


        JsonData jsonMap = JsonMapper.ToJson(simpleMap);

        File.WriteAllText(Application.dataPath + "/Resource/mapData.json", jsonMap.ToString());
        //Debug.Log("ss  ");
       //s return true;
    }
	// Use this for initialization
	void Start () {
        map = Camera.main.GetComponent<MapCreator>().map;
        simpleMap = new List<jsonTile>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class jsonTile
{
    public int virtualX;
    public int virtualZ;

    public int BuildingNum;

    public jsonTile(int X, int Z, int BNum)
    {
        virtualX = X;
        virtualZ = Z;
        BuildingNum = BNum;
    }
}