using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TilePlugin;

public class RandomSpawnLer : MonoBehaviour {
    public Transform monsterType1;
    public Transform monsterType2;

    int counter = 0;

    Dictionary<Vector2, Transform> map;

    void spawn(Transform monsterType)
    {
        List<Transform> openTiles = new List<Transform>();
        foreach( Transform tile in map.Values)
        {
            if (tile.GetComponent<Tile>().MoveAble == true)
            {
                openTiles.Add(tile);
            }
           
        }

        int randNum = Random.Range(0, openTiles.Count - 1);


        openTiles[randNum].GetComponent<Tile>().MoveAble = false;

        Vector3 v3 = openTiles[randNum].position;
        v3.y += 2;

        Instantiate(monsterType, v3, Quaternion.Euler(0, 180, 0));

    }

    
	// Use this for initialization
	void Start () {
        map = Camera.main.GetComponent<MapCreator>().map;
    }
	
	// Update is called once per frame
	void Update () {
		if( ( (int)(Time.time%10) == 0)&&(counter<((Time.time / 10)-1))){
            counter++;
            spawn(monsterType1);
        }
	}
}
