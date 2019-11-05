using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constellations
{
    public class Point : MonoBehaviour
    {
        [SerializeField]
        Point[] _neighbourPoints;

        [SerializeField]
        bool[] _isConnected;

        [SerializeField]
        Dictionary<Point, bool> _neighbour = new Dictionary<Point, bool>();

        public Point[] NeighbourList
        {
            get { return _neighbourPoints; }
            set { _neighbourPoints = value; }
        }

        public bool[] IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; }
        }

        public Dictionary<Point, bool> Neighbour
        {
            get { return _neighbour; }
            set { _neighbour = value; }
        }

    }
}
