using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Constellations.ConstellationsCreator
{
    public class EditorUtils
    {
        #region List GUI
        public static void DrawListGUI (SerializedProperty list, bool showListSize = true)
        {
            EditorGUILayout.PropertyField(list);

            if(list.arraySize <= 0)
            {
                EditorGUILayout.HelpBox(list.displayName + " currently has nothing in it.", MessageType.Info);
            }


            //Expand list
            if (list.isExpanded)
            {
                if(showListSize)
                {
                    //Array size
                    EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                }
                //Array content
                for (int i = 0; i < list.arraySize; i++)
                {
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none);
                }
            }
        }

        #endregion
    }
}
