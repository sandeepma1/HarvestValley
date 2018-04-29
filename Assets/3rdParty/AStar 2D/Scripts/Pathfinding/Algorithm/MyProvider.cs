using UnityEngine;
using System.Collections;

namespace AStar_2D.Pathfinding.Algorithm
{
    /// <summary>
    /// Provides a Euclidean heuristic.
    /// </summary>
    public class MyProvider : HeuristicProvider
    {
        // Methods
        /// <summary>
        /// Calcualtes the Euclidean heuristic.
        /// </summary>
        /// <param name="start">The first node</param>
        /// <param name="end">The second node</param>
        /// <returns>The heuristic between the 2 nodes</returns>
        public override float heuristic(PathNode start, PathNode end)
        {
            float dx = Mathf.Abs(start.Index.X - end.Index.X);
            float dy = Mathf.Abs(start.Index.Y - end.Index.Y);
            return 2 * (dx + dy);
            //return Mathf.Abs(start.Index.X - end.Index.X) + Mathf.Abs(start.Index.Y - end.Index.Y);
        }


    }
}

//**
//  return Mathf.Abs(start.Index.X - end.Index.X) + Mathf.Abs(start.Index.Y - end.Index.Y);
//This goes a little bit good than diagonal
//**

//** Manhattan distance
//float dx = Mathf.Abs(start.Index.X - end.Index.X);
//float dy = Mathf.Abs(start.Index.Y - end.Index.Y);
//return 2 * (dx + dy);
//** This goes a far end to end movement which I need