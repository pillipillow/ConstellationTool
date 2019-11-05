using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Constellations.ConstellationsCreator
{
    public class MenuItems
    {
        [MenuItem("Tools/Constellation Creator/Create new set")]
        static void CreateNewSet()
        {
            GameObject GO = new GameObject("Constellation");
            GO.transform.position = Vector3.zero;
            GO.AddComponent<ConstellationSet>();
        }
    }
}
