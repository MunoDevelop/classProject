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
        [SerializeField]
        public float rotateYieldTime = 0;

        Dictionary<Vector2, Transform> map;



        //---------Movement 
        public MovementState moveState;


        List<Transform> path;

        public int pathIndex = 0;

        float startTime;

        float journeyLength;

        IEnumerator characterRotateCoroutine(Vector3 lookAt,Vector3 endPos,float yieldTime)
        {
            float startTime = Time.time;
            
            float i = 0;
            while (i < 1)
            {
                i += Time.deltaTime/10;
                Vector3 v3 = Vector3.Lerp(lookAt, endPos,i);
                transform.LookAt(v3);
                yield return new WaitForSeconds(0.0001f);
            }
            Debug.Log(Time.time-startTime);
            yield return null;
        }

        
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

                        this.GetComponent<Animator>().SetBool("isRunning", true);
                        //Vector3 forwardVec = endPosition - startPosition;

                        transform.LookAt(endPosition);

                        //StartCoroutine(characterRotateCoroutine(transform.forward, endPosition, rotateYieldTime));
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

                            //StartCoroutine(characterRotateCoroutine(transform.forward,EndPosition, rotateYieldTime));
                            transform.LookAt(EndPosition);
                        }

                        
                        if (pathIndex+1 == path.Count-1)//end Sequence
                        {
                            
                            virtualPosition = new Vector2(x,z);
                            moveState = MovementState.standing;
                            this.GetComponent<Animator>().SetBool("isRunning", false);
                            break;
                        }
                        //Sequence not end
                        pathIndex++;
                        startTime = Time.time;
                        virtualPosition = new Vector2(x, z);
                        Vector3 startPosition = map[virtualPosition].position + characterPositionCorrection;
                        Vector3 endPosition = path[pathIndex + 1].transform.position + characterPositionCorrection;

                        journeyLength = Vector3.Distance(startPosition, endPosition);

                        //StartCoroutine(characterRotateCoroutine(transform.forward, endPosition, rotateYieldTime));
                        transform.LookAt(endPosition);
                    }

                    break;
            }




        }
    }
}

