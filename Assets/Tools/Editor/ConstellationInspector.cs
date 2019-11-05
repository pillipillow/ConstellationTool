using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Constellations.ConstellationsCreator
{
    [CustomEditor(typeof(ConstellationSet))]
    public class ConstellationInspector : Editor
    {
        public static ConstellationSet _myTarget;
           
        Rect buttonRect;

        private void OnEnable()
        {
            _myTarget = (ConstellationSet)target;
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            DrawRenameGUI();
            EditorGUILayout.Space();
            DrawPointsGUI();
            EditorGUILayout.Space();
            DrawSelectedPointGUI();
            EditorGUILayout.Space();
            DrawLineRendererSettingsGUI();

            EditorUtility.SetDirty(_myTarget);

            EditorGUILayout.Space();
            DrawDeleteSetGUI();       
        }

        void DrawRenameGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Constellation Name: ", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(_myTarget.name);

            if (GUILayout.Button("Rename"))
            {
                PopupWindow.Show(buttonRect, new RenamePopUp());
            }
            if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();

            EditorGUILayout.EndHorizontal();

            _myTarget.IsClickDrag = EditorGUILayout.Toggle("Drag Control", _myTarget.IsClickDrag);
            EditorGUILayout.EndVertical();
        }


        void DrawDeleteSetGUI()
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.red;

            if (GUILayout.Button("Delete Constellation",style))
            {
                if(EditorUtility.DisplayDialog
                    ("Delete Constellation Set",
                    "Are you sure you want delete this?\n This action cannot be undone.",
                    "Ok",
                    "Cancel"))
                {

                    DestroyImmediate(_myTarget.gameObject);
                }
            }
        }

        void DrawPointsGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Constellation Points", EditorStyles.boldLabel);

            serializedObject.Update();
            EditorUtils.DrawListGUI(serializedObject.FindProperty("pointList"), false);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Add new point"))
            {
                GameObject pointPrefab = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Point.prefab", typeof(GameObject))) as GameObject;
                pointPrefab.gameObject.name = ("Point [" + _myTarget.pointList.Count + "]");
                pointPrefab.transform.parent = _myTarget.transform;
                pointPrefab.transform.position = _myTarget.transform.localPosition;
                _myTarget.pointList.Add(pointPrefab.GetComponent<Point>());
            }

            if (GUILayout.Button("Delete all points"))
            {
                if(_myTarget.pointList.Count > 0)
                {
                    foreach (Point point in _myTarget.pointList)
                    {
                        DestroyImmediate(point.gameObject);
                    }
                    _myTarget.pointList.Clear();
                }
            }

            EditorGUILayout.EndVertical();
        }

        void DrawSelectedPointGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Selected Points", EditorStyles.boldLabel);

            serializedObject.Update();
            EditorUtils.DrawListGUI(serializedObject.FindProperty("selectedPoints"),false);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Delete selected points"))
            {
                foreach(Point selectGO in _myTarget.selectedPoints)
                {
                    _myTarget.pointList.Remove(selectGO);
                    DestroyImmediate(selectGO.gameObject);
                }
            }

            /*if(_myTarget.selectedPoints.Length == 1)
            {
                EditorGUILayout.LabelField("Neighbours", EditorStyles.boldLabel);

                //NeighbourSelectedPoints();

                serializedObject.Update();
                EditorUtils.DrawListGUI(serializedObject.FindProperty("neighbourPoints"));
                serializedObject.ApplyModifiedProperties();

                _myTarget.selectedPoints[0].NeighbourList = new Point[_myTarget.neighbourPoints.Length];
            }*/


            EditorGUILayout.EndVertical();
        }

        void NeighbourSelectedPoints()
        {
            _myTarget.neighbourPoints = new Point[_myTarget.selectedPoints[0].NeighbourList.Length];

            for(int i = 0; i< _myTarget.selectedPoints[0].NeighbourList.Length;i++)
            {
                _myTarget.neighbourPoints[i] = _myTarget.selectedPoints[0].NeighbourList[i] as Point;
            }
        }


        void DrawLineRendererSettingsGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Line Renderer Settings", EditorStyles.boldLabel);

            _myTarget.LineMaterial = (Material)EditorGUILayout.ObjectField("Line Material",_myTarget.LineMaterial,typeof(Material),false);

            _myTarget.LineWidth = EditorGUILayout.FloatField("Line Width", _myTarget.LineWidth);
           

            EditorGUILayout.EndVertical();
        }


        private void OnSceneGUI()
        {
            EventHandler();
        }


        void EventHandler()
        {
            Object[] selected = Selection.GetFiltered(typeof(Point), SelectionMode.Editable);
            _myTarget.selectedPoints = new Point[selected.Length];
            for (int i = 0; i < selected.Length; i++)
            {
                _myTarget.selectedPoints[i] = selected[i] as Point;
            }
        }

    }

    public class RenamePopUp : PopupWindowContent
    {
        string _newName;
        ConstellationSet _myTarget;

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 25);
        }

        public override void OnGUI(Rect rect)
        {
            _myTarget = ConstellationInspector._myTarget;

            EditorGUILayout.BeginHorizontal();
                _newName = EditorGUILayout.TextField(_newName);
                GUI.enabled = _newName != null;

                if (GUILayout.Button("Change"))
                {
                    _myTarget.name = _newName;
                }
            EditorGUILayout.EndHorizontal();
        }

      
    }
}
