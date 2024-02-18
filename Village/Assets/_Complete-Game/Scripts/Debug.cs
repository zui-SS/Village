using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Completed
{
    using System.Collections.Generic;       //Allows us to use Lists. 
    using UnityEngine.UI;                   //Allows us to use UI.

    public class DebugFunc
    {
        public DebugFunc() { }

        public void PrintMap(int[,] map, int nMaxX, int nMaxY)
        {
            string strDebug = "";
            for (int x = 0; x < nMaxX; x++)
            {
                for (int y = 0; y < nMaxY; y++)
                {
                    string strAdd = strDebug + map[x, y].ToString() + ",";
                    strDebug = strAdd;
                }
                string strPlus = strDebug + "\n";
                strDebug = strPlus;
            }
            Debug.Log("[Test]\n " + strDebug);
        }
    }
}

