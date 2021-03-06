﻿using System.Collections;
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
    public Transform userCharacter;
   
    [SerializeField]
    Vector3 userCharacterStartPosition = new Vector3(0, 2, 0);

    [SerializeField]
    public int findPathNum = 2;

    [SerializeField]
    public Transform TreePrefeb;
    [SerializeField]
    public Transform StonePrefeb;
    [SerializeField]
    public Transform RiverPrefeb;

    //-------------MoveMent



    public List<Transform> pathBuffer = new List<Transform>();
    //--------------
    private void Awake()
    {
        userCharacter = Instantiate(userCharacter, userCharacterStartPosition, Quaternion.Euler(0, 180, 0));
    }
    void Start () {
        map = Camera.main.GetComponent<MapCreator>().map;
        lineRenderer = this.GetComponent<LineRenderer>();

       
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

    List<Transform> findPath(Transform startTile, Transform destTile)
    {
        switch (findPathNum)
        {
            case 1:
                List<Transform> pathList = new List<Transform>();
                return findPathDFS(startTile, destTile, pathList);
                //return pathList;
                break;
            case 2:
                return findPathBFS(startTile, destTile);
                break;
            case 3:
                return findPathAStar(startTile, destTile);
                break;
        }
        return null;
    }

    List<Transform> findPathDFS(Transform startTile, Transform destTile, List<Transform> pathList)
    {
        //List<Transform> closeList = new List<Transform>();
        if (startTile == destTile)
        {
            return pathList;
        }

        foreach(Transform child in startTile.GetComponent<Tile>().neighbors)
        {
            if (child.GetComponent<Tile>().MoveAble == true)
            {
                findPathDFS(child,destTile,pathList);
            }
        }
        pathList.Add(startTile);
           
        

        return pathList;
    }


    List<Transform> findPathBFS(Transform startTile, Transform destTile)
    {
        List<Transform> pathList = new List<Transform>();

        List<Transform> openList = new List<Transform>();
        List<Transform> closeList = new List<Transform>();

        foreach (Transform tile in map.Values)
        {
            if (tile.GetComponent<Tile>().MoveAble == false)
            {
                closeList.Add(tile);
            }
        }
        openList.Add(startTile);

        while (openList.Count != 0)
        {
            Transform current = openList[0];

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

                
                if (openList.Contains(neibor) == false)
                {
                    openList.Add(neibor);
                }

                // if this is already in the openlist
                
                neibor.GetComponent<Tile>().NavigationFather = current;

                
            }
        }

        return pathList;
    }

     List<Transform> findPathAStar(Transform startTile,Transform destTile)
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

    //public void addLayerTile(Transform tile)
    //{
    //    int numOfLayer = tile.GetComponent<Tile>().layerList.Count;
    //    Vector3 v3;
    //    v3.x = tile.position.x;
    //    v3.y = tile.position.y + 4*(numOfLayer+1);
    //    v3.z = tile.position.z;
    //    Transform ins = Instantiate(LayerTile, v3, Quaternion.Euler(0, 0, 0));
        
    //    ins.GetComponent<LayerTile>().Belong = tile;
    //    tile.GetComponent<Tile>().layerList.Add(ins);
    //}

        public void addSthOnTileToggle(Transform tile)
    {
        Vector3 v3;
            v3.x = tile.position.x;
            v3.y = tile.position.y + 2;
            v3.z = tile.position.z;
        switch (tile.GetComponent<Tile>().BuildingNum)
        {
            case 0:
                v3.y -= 2;
                Transform ins = Instantiate(TreePrefeb, v3, Quaternion.Euler(0, 0, 0));
                tile.GetComponent<Tile>().Building = ins;
                tile.GetComponent<Tile>().BuildingNum = 1;
                tile.GetComponent<Tile>().MoveAble = false;
                break;
            case 1:
                Transform ins1 = Instantiate(StonePrefeb, v3, Quaternion.Euler(0, 0, 0));
                Object.Destroy(tile.GetComponent<Tile>().Building.gameObject);
                tile.GetComponent<Tile>().Building = ins1;
                tile.GetComponent<Tile>().BuildingNum = 2;
                tile.GetComponent<Tile>().MoveAble = false;
                break;
            case 2:
                Transform ins2 = Instantiate(RiverPrefeb, v3, Quaternion.Euler(0, 0, 0));
                Object.Destroy(tile.GetComponent<Tile>().Building.gameObject);
                tile.GetComponent<Tile>().Building = ins2;
                tile.GetComponent<Tile>().BuildingNum = 3;
                tile.GetComponent<Tile>().MoveAble = false;
                break;
            case 3:
                Object.Destroy(tile.GetComponent<Tile>().Building.gameObject);

                tile.GetComponent<Tile>().BuildingNum = 0;
                tile.GetComponent<Tile>().MoveAble = true;
                break;
        }
        


    }

    //public void deleteLayerTile(Transform tile)
    //{
    //    int numOfLayer = tile.GetComponent<Tile>().layerList.Count;
    //    Destroy(tile.GetComponent<Tile>().layerList[numOfLayer - 1].gameObject);
    //    tile.GetComponent<Tile>().layerList.RemoveAt(numOfLayer - 1);
    //  //  Debug.Log(tile.GetComponent<Tile>().layerList.Count);
    //}

    // Update is called once per frame
    private void Update()
    {
       
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector2 virtualPosition = userCharacter.GetComponent<CharacterState>().virtualPosition;
        //float speed = userCharacter.GetComponent<CharacterState>().speed;

        if (Physics.Raycast(ray, out hit))
        {
            //-------------PathFind
            if (hit.transform.tag == "Tile" && (hit.transform != map[userCharacter.GetComponent<CharacterState>().virtualPosition]))
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
                            tile.transform.position.y + laserYPositionCorrection,
                            tile.transform.position.z);

                        v3List.Add(pos);
                    }
                    lineRenderer.positionCount = v3List.Count;
                    lineRenderer.SetPositions(v3List.ToArray());
                }




                //-------OutLine
                if (hit.transform != preTile && preTile != null)
                {
                    preTile.GetComponent<Outline>().enabled = false;
                    hit.transform.GetComponent<Outline>().enabled = true;
                }
                preTile = hit.transform;
                //-------------
            }


            if (hit.transform.tag == "Tile" && Input.GetMouseButtonDown(1))
            {
                addSthOnTileToggle(hit.transform);
                //hit.transform.GetComponent<Tile>().MoveAble = false;

            }
            //// add LayerTile
            //if (hit.transform.tag == "Tile" && Input.GetMouseButtonDown(1))
            //{
            //    if ((hit.transform.GetComponent<Tile>().MoveAble == false) && (hit.transform.GetComponent<Tile>().layerList.Count == 0))
            //    {

                //        hit.transform.GetComponent<Tile>().MoveAble = true;
                //        hit.transform.GetComponent<MeshRenderer>().material= Camera.main.GetComponent<MapCreator>().MoveableMat;

                //    }
                //    else if ((hit.transform.GetComponent<Tile>().MoveAble == true) && (hit.transform.GetComponent<Tile>().layerList.Count == 0))
                //    {
                //        hit.transform.GetComponent<Tile>().MoveAble = false;
                //        addLayerTile(hit.transform);
                //    }
                //    else if ((hit.transform.GetComponent<Tile>().MoveAble == false) && (hit.transform.GetComponent<Tile>().layerList.Count > 0))
                //    {
                //        if (hit.transform.GetComponent<Tile>().layerList.Count < 3)
                //        {
                //            addLayerTile(hit.transform);
                //        }
                //    }
                //}

                //if (hit.transform.tag == "LayerTile" && Input.GetMouseButtonDown(1))
                //{
                //    if (hit.transform.GetComponent<LayerTile>().Belong.GetComponent<Tile>().layerList.Count < 3)
                //    {
                //        addLayerTile(hit.transform.GetComponent<LayerTile>().Belong);
                //    }
                //}

                //// delete LayerTile
                //if (hit.transform.tag == "Tile" && Input.GetMouseButtonDown(2))
                //{
                //    if ((hit.transform.GetComponent<Tile>().MoveAble == true) && (hit.transform.GetComponent<Tile>().layerList.Count == 0))
                //    {
                //        hit.transform.GetComponent<Tile>().MoveAble = false;
                //       hit.transform.GetComponent<MeshRenderer>().material = Camera.main.GetComponent<MapCreator>().blockedMat;

                //        //hit.transform.GetComponent<MeshRenderer>().materials = mats;
                //       // Debug.Log("call");
                //    }
                //    if ((hit.transform.GetComponent<Tile>().MoveAble == false) && (hit.transform.GetComponent<Tile>().layerList.Count > 0))
                //    {


                //        deleteLayerTile(hit.transform);
                //        if(hit.transform.GetComponent<Tile>().layerList.Count == 0)
                //        {
                //            hit.transform.GetComponent<Tile>().MoveAble = true;
                //        }

                //    }

                //}

                //if (hit.transform.tag == "LayerTile" && Input.GetMouseButtonDown(2))
                //{
                //    if (hit.transform.GetComponent<LayerTile>().Belong.GetComponent<Tile>().layerList.Count > 0)
                //    {
                //        deleteLayerTile(hit.transform.GetComponent<LayerTile>().Belong);

                //        if (hit.transform.GetComponent<LayerTile>().Belong.GetComponent<Tile>().layerList.Count == 0)
                //        {
                //            hit.transform.GetComponent<LayerTile>().Belong.GetComponent<Tile>().MoveAble = true;
                //        }
                //    }
                //}


        }


    }
}
