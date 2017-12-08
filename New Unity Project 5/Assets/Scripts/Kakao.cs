using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;

using System.IO;



public class Kakao : MonoBehaviour
{
    [SerializeField]
    int blockMapXSize = 10;
    [SerializeField]
    int blockMapYSize = 12;
    [SerializeField]
    Transform blockPrefeb;
    [SerializeField]
    float blockRadious = 2.0f;
    [SerializeField]
    Vector3 createPosition = new Vector3(0,0,0);
    [SerializeField]
    Vector3 movePosition;
    [SerializeField]
    float moveSpeed =0.02f;

    [SerializeField]
    Material mat1;
    [SerializeField]
    Material mat2;
    [SerializeField]
    Material mat3;
 

    List<List<Transform>> Map;
    
    List<Vector2> requiredBlocks;

    enum State { calculating, moving, deletding,final }

    State kakaoState;

    float moveIndex = 0;

    void calculateRequiredBlocks()
    {
       for(int i = 0;i< blockMapXSize; i++)
        {
            for(int j = 0; j < blockMapYSize; j++)
            {
                if(j>= Map[i].Count)
                {
                    requiredBlocks.Add(new Vector2(i,j));
                }
            }
        }
    }

    void createRequiredBlocks()
    {
        foreach (Vector2 v2 in requiredBlocks)
        {
            Vector3 v3;
            v3.x = createPosition.x + blockRadious * v2.x;
            v3.y = createPosition.y + blockRadious * v2.y;
            v3.z = createPosition.z;
            Transform trans = Instantiate(blockPrefeb, v3, Quaternion.Euler(0, 0, 0));
            trans.GetComponent<Block>().virtualPositionX = (int)v2.x;
            trans.GetComponent<Block>().virtualPositionY = (int)v2.y;
            trans.GetComponent<Block>().recentPosition = trans.position;

            int rand = UnityEngine.Random.Range(1, 3);
            trans.GetComponent<Block>().type = rand;
            switch (rand)
            {
                case 1:
                    trans.GetComponent<MeshRenderer>().material = mat1;
                    break;
                case 2:
                    trans.GetComponent<MeshRenderer>().material = mat2;
                    break;
                case 3:
                    trans.GetComponent<MeshRenderer>().material = mat3;
                    break;
               
            }
            


            Map[(int)v2.x].Add(trans);
        }
    }

    void moveRequiredBlocks()
    {
        moveIndex += Time.deltaTime * moveSpeed;
        foreach (Vector2 v2 in requiredBlocks)
        {
            Transform trans = Map[(int)v2.x][(int)v2.y];
            
            trans.position = Vector3.Lerp(trans.GetComponent<Block>().recentPosition, 
                trans.GetComponent<Block>().recentPosition + movePosition,
                moveIndex);

            if (Mathf.Abs(trans.GetComponent<Block>().recentPosition.y + movePosition.y - trans.position.y) < 0.5)
            {
                
                kakaoState = State.deletding;
                
            }
        }
        //requiredBlocks.Clear();
        //moveIndex = 0;
    }

    void deleteBlocks()
    {
        List<Vector2> deleteList = new List<Vector2>();

        List<Vector2> noSameDeleteList = new List<Vector2>();
        //int deleteCounter = 0;

        for (int i = 0; i < Map.Count - 1; i++)
        {
            for (int j = 0; j < Map[i].Count - 1; j++)
            {
                if (j >= Map[i + 1].Count)
                {
                    continue;
                }

                int ca = Map[i].ElementAt(j).GetComponent<Block>().type;
                if (
                    (ca == Map[i].ElementAt(j + 1).GetComponent<Block>().type) &&
                    (ca == Map[i + 1].ElementAt(j).GetComponent<Block>().type) &&
                    (ca == Map[i + 1].ElementAt(j + 1).GetComponent<Block>().type)
                )
                {
                    Vector2 deleteV;
                    deleteV.x = i;
                    deleteV.y = j;
                    deleteList.Add(deleteV);
                    deleteV.x = i + 1;
                    deleteV.y = j;
                    deleteList.Add(deleteV);
                    deleteV.x = i;
                    deleteV.y = j + 1;
                    deleteList.Add(deleteV);
                    deleteV.x = i + 1;
                    deleteV.y = j + 1;
                    deleteList.Add(deleteV);
                }

            }

        }

        for (int i = 0; i < deleteList.Count; i++)
        {
            if (!noSameDeleteList.Contains(deleteList.ElementAt(i)))
            {
                noSameDeleteList.Add(deleteList.ElementAt(i));
            }
        }
        Debug.Log(noSameDeleteList.Count);
        foreach (Vector2 v2 in noSameDeleteList)
        {
            Debug.Log(v2);
        }

        for (int i = 0; i < Map.Count; i++)
        {
            for (int j = Map[i].Count; j >= 0; j--)
            {
                
                if ((noSameDeleteList.Contains(new Vector2(i,j))))
                {
                    Explode(new Vector2(i, j));
                    Map[i].RemoveAt(j);
                }
            }
        }
        //deleteCounter += noSameDeleteList.Count;

        //-----------------TEST
        
        //----------------
        deleteList.Clear();
        noSameDeleteList.Clear();

        kakaoState = State.final;

       // yield return new WaitForSeconds(111);

    }

    void Start()
    {

        Map = new List<List<Transform>>();
        requiredBlocks = new List<Vector2>();

        for(int i= 0; i < blockMapXSize; i++)
        {
            Map.Add(new List<Transform>());
        }

        movePosition = new Vector3(0, -blockRadious * blockMapYSize, 0);

        kakaoState = State.calculating;

    }

    IEnumerator Explode(Vector2 v2)
    {
        StartCoroutine(Map[(int)v2.x][(int)v2.y].GetComponent<TriangleExplosion>().SplitMesh(true));
        //yield return new WaitForSeconds(111);
        return null;
    }
    
    private void Update()
    {
        switch (kakaoState)
        {
            case State.calculating:
                calculateRequiredBlocks();

                createRequiredBlocks();
                kakaoState = State.moving;
                break;
            case State.moving:
                moveRequiredBlocks();
               
                 
                break;

            case State.deletding:



                deleteBlocks();
               // Debug.Log(" x");
                
                break;
            case State.final:
                break;
        }
        



    }

    // Update is called once per frame
   
}
