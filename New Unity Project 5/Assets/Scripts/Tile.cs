﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TilePlugin
{
    public class Tile :MonoBehaviour
    {
        
        
        public int VirtualX;
        
        public int VirtualZ;
        
        public List<Transform> neighbors;
        [HideInInspector]
        public int Navigation_G ;
        [HideInInspector]
        public int Navigation_F ;
        [HideInInspector]
        public bool MoveAble = true;

        [HideInInspector]
        public Transform Building;
        [HideInInspector]
        public int BuildingNum;

        [HideInInspector]
        public Transform NavigationFather;



        // Use this for initialization
        public Tile(int virX,int virZ)
        {
            VirtualX = virX;
            VirtualZ = virZ;
            Navigation_G = 0;
            Navigation_F = 0;
            MoveAble = true;
        }
        public Tile() {
        }

    
    }


}

