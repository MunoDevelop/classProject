using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TilePlugin;

namespace CharacterStatePlugIn

{
    public enum MovementState
    {
        standing, running
    }
    public class CharacterState : MonoBehaviour
    {
        [SerializeField]
        GameObject controllerKeeper;

        [SerializeField]
        Vector3 characterPositionCorrection = new Vector3(0, 2, 0);

        [HideInInspector]
        public Vector2 virtualPosition;
        
        [SerializeField]
        public float speed;

        Dictionary<Vector2, Transform> map;



        //---------Movement 
        public MovementState moveState;


        List<Transform> path;

        public int pathIndex = 0;

        float startTime;

        float journeyLength;

        
        void Start()
        {
            map = Camera.main.GetComponent<MapCreator>().map;
            moveState = MovementState.standing;
            controllerKeeper = GameObject.FindWithTag("GameController");
            speed = 10f;
        }

        
        void Update()
        {
            //
            List<Transform> pathBuffer = controllerKeeper.GetComponent<EventController>().pathBuffer;
            
            switch (moveState)
            {
                case MovementState.standing:
                    if (pathBuffer.Count > 0)
                    {
                        path = new List<Transform>(pathBuffer);
                        controllerKeeper.GetComponent<EventController>().pathBuffer.Clear();
                        pathIndex = 0;
                        startTime = Time.time;
                        Vector3 startPosition = map[virtualPosition].position + characterPositionCorrection;
                        Vector3 endPosition = path[pathIndex+1].transform.position + characterPositionCorrection;

                        journeyLength = Vector3.Distance(startPosition, endPosition);
                        moveState = MovementState.running;
                    }

                    break;
                case MovementState.running:
                    

                    float distCovered = (Time.time - startTime) * speed;
                    
                    float fracJourney = distCovered / journeyLength;
                   
                    Vector3 startPos = map[virtualPosition].position + characterPositionCorrection;
                    Vector3 endPos = path[pathIndex+1].transform.position + characterPositionCorrection;

                    transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

                    if (Vector3.Distance(transform.position,endPos)<0.1)
                    {
                        int x = path[pathIndex + 1].GetComponent<Tile>().VirtualX;
                        int z = path[pathIndex + 1].GetComponent<Tile>().VirtualZ;
                        if (pathBuffer.Count > 0)
                        {
                            pathIndex = 0;
                            startTime = Time.time;
                            virtualPosition = new Vector2(x, z);
                            path.Clear();
                            path = new List<Transform>(pathBuffer);
                            pathBuffer.Clear();

                            Vector3 StartPosition = map[virtualPosition].position + characterPositionCorrection;
                            Vector3 EndPosition = path[pathIndex + 1].transform.position + characterPositionCorrection;

                            journeyLength = Vector3.Distance(StartPosition, EndPosition);
                        }

                        
                        if (pathIndex+1 == path.Count-1)//end Sequence
                        {
                            
                            virtualPosition = new Vector2(x,z);
                            moveState = MovementState.standing;
                            break;
                        }
                        //Sequence not end
                        pathIndex++;
                        startTime = Time.time;
                        virtualPosition = new Vector2(x, z);
                        Vector3 startPosition = map[virtualPosition].position + characterPositionCorrection;
                        Vector3 endPosition = path[pathIndex + 1].transform.position + characterPositionCorrection;

                        journeyLength = Vector3.Distance(startPosition, endPosition);
                    }

                    break;
            }




        }
    }
}

