using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Constellations
{
    public class ConstellationSet : MonoBehaviour
    {
        #region Variables
        //Controls
        [SerializeField]
        bool _isClickDrag;

        //Points
        public List<Point> pointList = new List<Point>();
        Point _selectedPoint; //For in-game only

        //For Inspector
        public Point[] selectedPoints;
        public Point[] neighbourPoints;

        //Lines
        [SerializeField]
        Material _lineMaterial;
        [SerializeField]
        float _lineWidth;
        GameObject _lineActive;

        public bool IsClickDrag
        {
            get {return _isClickDrag; }
            set {_isClickDrag = value; }
        }

        public Material LineMaterial
        {
            get { return _lineMaterial; }
            set {_lineMaterial = value; }
        }

        public float LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        #endregion


        private void Update()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (_isClickDrag)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.collider != null && hit.collider.GetComponent<Point>() != null)
                    {
                        _selectedPoint = hit.collider.GetComponent<Point>();
                    }
                }

                if (Input.GetMouseButton(0))
                {
                    if (_selectedPoint)
                    {
                        DrawLine(mousePos);
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (hit)
                    {
                        if (hit.collider.GetComponent<Point>() != null &&
                        hit.collider.gameObject != _selectedPoint.gameObject)
                        {
                            ConnectLine(hit.collider.GetComponent<Point>());
                        }
                    }
                    else
                    {
                        ReleaseLine();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (_selectedPoint)
                    {
                        if (hit)
                        {
                            if (hit.collider.GetComponent<Point>() != null &&
                            hit.collider.gameObject != _selectedPoint.gameObject)
                            {
                                ConnectLine(hit.collider.GetComponent<Point>());
                            }
                        }
                        else
                        {
                            ReleaseLine();
                        }
                    }
                    else
                    {
                        if (hit.collider != null && hit.collider.GetComponent<Point>() != null)
                        {
                            _selectedPoint = hit.collider.GetComponent<Point>();
                        }

                    }
                }

                //For the end line updating to stick to the pointer
                if (_selectedPoint)
                {
                    DrawLine(mousePos);
                }
            }
        }

        void SpawnLineGO()
        {
            GameObject lineGO = new GameObject("Line");
            lineGO.AddComponent<LineRenderer>();

            LineRenderer lr = lineGO.GetComponent<LineRenderer>();
            lr.startWidth = _lineWidth;
            lr.endWidth = _lineWidth;
            lr.material = _lineMaterial;
            lr.sortingOrder = 2;
            

            lineGO.transform.parent = this.transform;
            lineGO.transform.position = _selectedPoint.transform.position;

            _lineActive = lineGO;
        }

        void DrawLine(Vector3 clickPos)
        {
            if (!_lineActive)
            {
                SpawnLineGO();
            }

            LineRenderer lr = _lineActive.GetComponent<LineRenderer>();
            lr.SetPosition(0, _selectedPoint.transform.position);
            lr.SetPosition(1, clickPos);
        }

        void ConnectLine(Point point)
        {
            LineRenderer lr = _lineActive.GetComponent<LineRenderer>();
            lr.SetPosition(1, point.transform.position);
            _lineActive = null;
            _selectedPoint = null;
        }

        void ReleaseLine()
        {
            Destroy(_lineActive);
            _lineActive = null;
            _selectedPoint = null;
        }

    }
}
