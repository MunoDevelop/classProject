using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TilePlugin;
using cakeslice;
using CharacterStatePlugIn;

public class EventController : MonoBehaviour {

    Transform preTile;

    Dictionary<Vector2, Transform> map;
    
    LineRenderer lineRenderer;
    [SerializeField]
    float laserYPositionCorrection = 5;

    [SerializeField]
    Transform userCharacter;
    [SerializeField]
    Vector3 userCharacterStartPosition = new Vector3(0, 2, 0);
    

    //-------------MoveMent

    

    public List<Transform> pathBuffer = new List<Transform>();
    //--------------
  
    void Start () {
        map = Camera.main.GetComponent<MapCreator>().map;
        lineRenderer = this.GetComponent<LineRenderer>();

       userCharacter =  Instantiate(userCharacter, userCharacterStartPosition, Quaternion.Euler(0, 180, 0));
    }

    public Transform lowest(List<Transform> openlist)
    {
        if (openlist.Count == 0)
        {
            Debug.Log("openlist is empty");

        }
        Transform reTransform = null;

        float val = openlist[0].GetComponent<Tile>().Navigation_F;

        foreach (Transform tile in openlist)
        {
            if (tile.GetComponent<Tile>().Navigation_F <= val)
            {
                val = tile.GetComponent<Tile>().Navigation_F;
                reTransform = tile;
            }
        }
        return reTransform;
    }

    //--------------leave for a while
    public int terrainCost(Transform tile)
    {
        return 1;
    }
    public int distance(Transform startTile,Transform destTile)
    {
        int startTileX = startTile.GetComponent<Tile>().VirtualX;
        int startTileZ = startTile.GetComponent<Tile>().VirtualZ;
        int startTileY = -startTileX - startTileZ;

        int destTileX = destTile.GetComponent<Tile>().VirtualX;
        int destTileZ = destTile.GetComponent<Tile>().VirtualZ;
        int destTileY = -destTileX - destTileZ;

        int reVal = (Mathf.Abs(startTileX - destTileX) + Mathf.Abs(startTileY - destTileY) + Mathf.Abs(startTileZ - destTileZ)) / 2;

        return reVal;

    }

    List<Transform> findPath(Transform startTile,Transform destTile)
    {
        List<Transform> pathList = new List<Transform>();

        List<Transform> openList = new List<Transform>();
        List<Transform> closeList = new List<Transform>();

        foreach (Transform tile in map.Values)
        {
            if (tile.GetComponent<Tile>().MoveAble==false)
            {
                closeList.Add(tile);
            }
        }
        openList.Add(startTile);

        while (openList.Count != 0)
        {
            Transform current = lowest(openList);

            if (current == destTile)
            {
                while (current.GetComponent<Tile>().NavigationFather != null)
                {
                    pathList.Add(current.GetComponent<Tile>().NavigationFather);
                    current.GetComponent<Tile>().NavigationFather = current.GetComponent<Tile>().NavigationFather.GetComponent<Tile>().NavigationFather;
                }
               
                pathList.Reverse();
                pathList.Add(destTile);
                
                return pathList;
            }
            openList.Remove(current);
            closeList.Add(current);
            foreach (Transform neibor in current.GetComponent<Tile>().neighbors)
            {

                if (closeList.Contains(neibor))
                {
                    continue;
                }

                int tentative_gScore = current.GetComponent<Tile>().Navigation_G + terrainCost(neibor);
                if (openList.Contains(neibor)==false)
                {
                    openList.Add(neibor);
                }

                // if this is already in the openlist
                else if (tentative_gScore > neibor.GetComponent<Tile>().Navigation_G)
                {
                    continue;
                }
                neibor.GetComponent<Tile>().NavigationFather = current;
                
                neibor.GetComponent<Tile>().Navigation_G = tentative_gScore;
                neibor.GetComponent<Tile>().Navigation_F = tentative_gScore + distance(neibor, destTile);
            }
        }

        return pathList;
    }

    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector2 virtualPosition = userCharacter.GetComponent<CharacterState>().virtualPosition;
        //float speed = userCharacter.GetComponent<CharacterState>().speed;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Tile"&&(hit.transform!= map[userCharacter.GetComponent<CharacterState>().virtualPosition]))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    

                     pathBuffer = findPath(map[virtualPosition], hit.transform);
                    foreach (Transform tile in map.Values)
                    {
                        tile.GetComponent<Tile>().NavigationFather = null;
                    }

                    //MovementState moveState = userCharacter.GetComponent<CharacterState>().moveState;



                    //------------drawLine Part
                    List<Vector3> v3List = new List<Vector3>();
                    foreach (Transform tile in pathBuffer)
                    {
                        
                        Vector3 pos = new Vector3(tile.transform.position.x,
                            tile.transform.position.y+ laserYPositionCorrection,
                            tile.transform.position.z);
                        
                        v3List.Add(pos);
                    }
                    lineRenderer.positionCount = v3List.Count;
                    lineRenderer.SetPositions(v3List.ToArray());
                }




                //-------OutLine
                if (hit.transform != preTile&&preTile!=null)
                {
                    preTile.GetComponent<Outline>().enabled = false;
                    hit.transform.GetComponent<Outline>().enabled = true;
                }
                preTile = hit.transform;
                //-------------
            }
        }

        //--for while just
       
        


    }
}
