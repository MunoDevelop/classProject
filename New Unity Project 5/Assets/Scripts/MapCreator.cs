using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PerlinPlugin;
using TilePlugin;
using cakeslice;



public class MapCreator : MonoBehaviour {
    [SerializeField]
    public Transform tilePrefeb;

    float TileRadious = 2.5f;
    [SerializeField]
    float NoiseSeed = 7;

    [SerializeField]
    int MapSize = 7;

    [SerializeField]
    float NoiseLevel = 10;
    [SerializeField]
    int blockRate = 30;
    [SerializeField]
     public Material blockedMat;
    [SerializeField]
    public Material MoveableMat;
    

    enum NeighborType
    { leftTop, midTop,rightTop, right, rightBottom,midBottom, leftBottom, left }

     public Dictionary<Vector2, Transform> map;

    void createMapLoop(Transform centerTile)
    {
        int valX = centerTile.GetComponent<Tile>().VirtualX;
        int valZ = centerTile.GetComponent<Tile>().VirtualZ;
        NeighborType nt;
        if ((Mathf.Abs(valX) >= MapSize || Mathf.Abs(valZ) >= MapSize) )
        {
            return;
        }

        if (!map.ContainsKey(new Vector2(valX - 1, valZ + 1))) {
            nt = NeighborType.leftTop;
            Transform tile = drawTile(new Vector2(valX-1, valZ + 1), centerTile, nt);
            map.Add(new Vector2(valX -1, valZ + 1), tile);

            createMapLoop(tile);
        }
        if (!map.ContainsKey(new Vector2(valX , valZ + 1)))
        {
            nt = NeighborType.midTop;
            Transform tile = drawTile(new Vector2(valX , valZ + 1), centerTile, nt);
            map.Add(new Vector2(valX , valZ + 1), tile);

            createMapLoop(tile);
        }
        if (!map.ContainsKey(new Vector2(valX + 1, valZ + 1)))
        {
            nt = NeighborType.rightTop;
            Transform tile = drawTile(new Vector2(valX + 1, valZ + 1), centerTile, nt);
            map.Add(new Vector2(valX + 1, valZ + 1), tile);

            createMapLoop(tile);
        }
        if (!map.ContainsKey(new Vector2(valX + 1, valZ)))
        {
            nt = NeighborType.right;
            Transform tile = drawTile(new Vector2(valX + 1, valZ), centerTile, nt);
            map.Add(new Vector2(valX + 1, valZ), tile);

            createMapLoop(tile);
        }
        if (!map.ContainsKey(new Vector2(valX + 1, valZ - 1)))
        {
            nt = NeighborType.rightBottom;
            Transform tile = drawTile(new Vector2(valX + 1, valZ - 1), centerTile, nt);
            map.Add(new Vector2(valX + 1, valZ - 1), tile);

            createMapLoop(tile);
        }
        if (!map.ContainsKey(new Vector2(valX , valZ - 1)))
        {
            nt = NeighborType.midBottom;
            Transform tile = drawTile(new Vector2(valX , valZ - 1), centerTile, nt);
            map.Add(new Vector2(valX , valZ - 1), tile);

            createMapLoop(tile);
        }
        if (!map.ContainsKey(new Vector2(valX - 1, valZ - 1)))
        {
            nt = NeighborType.leftBottom;
            Transform tile = drawTile(new Vector2(valX - 1, valZ - 1), centerTile, nt);
            map.Add(new Vector2(valX - 1, valZ - 1), tile);

            createMapLoop(tile);
        }
        if (!map.ContainsKey(new Vector2(valX - 1, valZ )))
        {
            nt = NeighborType.left;
            Transform tile = drawTile(new Vector2(valX - 1, valZ ), centerTile, nt);
            map.Add(new Vector2(valX - 1, valZ ), tile);

            createMapLoop(tile);
        }
        if (!map.ContainsKey(new Vector2(valX - 1, valZ + 1)))
        {
            nt = NeighborType.leftTop;
            Transform tile = drawTile(new Vector2(valX - 1, valZ + 1), centerTile, nt);
            map.Add(new Vector2(valX - 1, valZ + 1), tile);

            createMapLoop(tile);
        }
    }
    Transform drawTile(Vector2 vector, Transform centerTile, NeighborType neighborType)
    {
        Vector3 v3 = new Vector3();

        float xVal = centerTile.position.x;
        float zVal = centerTile.position.z;

        int virtualX = 0;
        int virtualZ = 0;

        switch (neighborType)
        {
            case NeighborType.leftTop:
                v3 = new Vector3( xVal - TileRadious,
               Perlin.Noise(vector.x / NoiseSeed, vector.y / NoiseSeed) * NoiseLevel,
              zVal + TileRadious);
                virtualX = centerTile.GetComponent<Tile>().VirtualX - 1;
                virtualZ = centerTile.GetComponent<Tile>().VirtualZ + 1;
                break;
            case NeighborType.midTop:
                v3 = new Vector3(xVal,
              Perlin.Noise(vector.x / NoiseSeed, vector.y / NoiseSeed) * NoiseLevel,
               TileRadious + zVal);
                virtualX = centerTile.GetComponent<Tile>().VirtualX ;
                virtualZ = centerTile.GetComponent<Tile>().VirtualZ + 1;
                break;
            case NeighborType.rightTop:
                v3 = new Vector3( TileRadious + xVal,
             Perlin.Noise(vector.x / NoiseSeed, vector.y / NoiseSeed) * NoiseLevel,
            zVal + TileRadious);
                virtualX = centerTile.GetComponent<Tile>().VirtualX + 1;
                virtualZ = centerTile.GetComponent<Tile>().VirtualZ + 1;
                break;
            case NeighborType.right:
                v3 = new Vector3(TileRadious + xVal,
             Perlin.Noise(vector.x / NoiseSeed, vector.y / NoiseSeed) * NoiseLevel,
             zVal);
                virtualX = centerTile.GetComponent<Tile>().VirtualX + 1;
                virtualZ = centerTile.GetComponent<Tile>().VirtualZ ;
                break;
            case NeighborType.rightBottom:
                v3 = new Vector3( TileRadious + xVal,
            Perlin.Noise(vector.x / NoiseSeed, vector.y / NoiseSeed) * NoiseLevel,
               zVal - TileRadious);
                virtualX = centerTile.GetComponent<Tile>().VirtualX + 1;
                virtualZ = centerTile.GetComponent<Tile>().VirtualZ - 1;
                break;
            case NeighborType.midBottom:
                v3 = new Vector3(xVal,
            Perlin.Noise(vector.x / NoiseSeed, vector.y / NoiseSeed) * NoiseLevel,
           zVal - TileRadious);
                virtualX = centerTile.GetComponent<Tile>().VirtualX;
                virtualZ = centerTile.GetComponent<Tile>().VirtualZ - 1;
                break;
            case NeighborType.leftBottom:
                v3 = new Vector3(xVal - TileRadious,
            Perlin.Noise(vector.x / NoiseSeed, vector.y / NoiseSeed) * NoiseLevel,
           zVal - TileRadious);
                virtualX = centerTile.GetComponent<Tile>().VirtualX - 1;
                virtualZ = centerTile.GetComponent<Tile>().VirtualZ - 1;
                break;
            case NeighborType.left:
                v3 = new Vector3(xVal - TileRadious,
            Perlin.Noise(vector.x / NoiseSeed, vector.y / NoiseSeed) * NoiseLevel,
           zVal );
                virtualX = centerTile.GetComponent<Tile>().VirtualX - 1;
                virtualZ = centerTile.GetComponent<Tile>().VirtualZ ;
                break;

            
        }

        Transform tile = Instantiate(tilePrefeb, v3, Quaternion.Euler(0, 0, 0));
        tile.GetComponent<Tile>().VirtualX = virtualX;
        tile.GetComponent<Tile>().VirtualZ = virtualZ;
        tile.GetComponent<Tile>().MoveAble = true;
        return tile;
    }

    void FindNeighbors(Transform centerTile)
    {
        int valX = centerTile.GetComponent<Tile>().VirtualX;
        int valZ = centerTile.GetComponent<Tile>().VirtualZ;

        if (map.ContainsKey(new Vector2(valX - 1, valZ + 1)))
        {
            Transform tile = map[new Vector2(valX - 1, valZ + 1)];
            centerTile.GetComponent<Tile>().neighbors.Add(tile);
        }
        if (map.ContainsKey(new Vector2(valX , valZ + 1)))
        {
            Transform tile = map[new Vector2(valX , valZ + 1)];
            centerTile.GetComponent<Tile>().neighbors.Add(tile);
        }
        if (map.ContainsKey(new Vector2(valX + 1, valZ + 1)))
        {
            Transform tile = map[new Vector2(valX + 1, valZ +1)];
            centerTile.GetComponent<Tile>().neighbors.Add(tile);
        }
        if (map.ContainsKey(new Vector2(valX + 1, valZ )))
        {
            Transform tile = map[new Vector2(valX + 1, valZ )];
            centerTile.GetComponent<Tile>().neighbors.Add(tile);
        }
        if (map.ContainsKey(new Vector2(valX + 1, valZ - 1)))
        {
            Transform tile = map[new Vector2(valX + 1, valZ - 1)];
            centerTile.GetComponent<Tile>().neighbors.Add(tile);
        }
        if (map.ContainsKey(new Vector2(valX , valZ - 1)))
        {
            Transform tile = map[new Vector2(valX , valZ - 1)];
            centerTile.GetComponent<Tile>().neighbors.Add(tile);
        }
        if (map.ContainsKey(new Vector2(valX - 1, valZ - 1)))
        {
            Transform tile = map[new Vector2(valX - 1, valZ - 1)];
            centerTile.GetComponent<Tile>().neighbors.Add(tile);
        }
        if (map.ContainsKey(new Vector2(valX - 1, valZ )))
        {
            Transform tile = map[new Vector2(valX - 1, valZ)];
            centerTile.GetComponent<Tile>().neighbors.Add(tile);
        }
        

    }

    void Awake()
    {
        map = new Dictionary<Vector2, Transform>();
    }
    private void Start()
    {
        Transform centerTile = Instantiate(tilePrefeb, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

        map.Add(new Vector2(0, 0), centerTile);
        createMapLoop(centerTile);

        foreach (Transform tile in map.Values)
        {
            FindNeighbors(tile);
             if (tile == centerTile){
                   continue;
               }
            int rand = Random.Range(0, 100);
            if(rand<=blockRate)
            {
                tile.GetComponent<MeshRenderer>().material = blockedMat;
                
                tile.GetComponent<Tile>().MoveAble = false;
            }


            
        }

        
        //Debug.Log(centerTile.GetComponent<Outline>().enabled.ToString());
        //float y = PerlinPlugin.Perlin.Noise(1.0f, 2.0f);
        //Debug.Log(y);
        //Instantiate(prefeb, new Vector3(x,y*10, z), Quaternion.identity);

    }




 


}
    
