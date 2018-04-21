using UnityEngine;
using System;
using System.Collections;

using AStar_2D.Pathfinding;

namespace AStar_2D.Threading
{
    public sealed class AsyncPathRequest
    {
        // Private
        private SearchGrid grid = null;
        private Index start = Index.zero;
        private Index end = Index.zero;
        private PathRequestDelegate callback = null;
        private bool allowDiagonal = true;
        private long timeStamp = 0;

        // Properties
        public SearchGrid Grid
        {
            get { return grid; }
        }

        public Index Start
        {
            get { return start; }
        }

        public Index End
        {
            get { return end; }
        }

        public bool AllowDiagonal
        {
            get { return allowDiagonal; }
        }

        internal PathRequestDelegate Callback
        {
            get { return callback; }
        }

        internal long TimeStamp
        {
            get { return timeStamp; }
        }

        // Constructor
        public AsyncPathRequest(SearchGrid grid, Index start, Index end, PathRequestDelegate callback)
        {
            this.grid = grid;
            this.start = start;
            this.end = end;
            this.callback = callback;

            // Create a time stamp
            timeStamp = DateTime.Now.Ticks;
        }

        public AsyncPathRequest(SearchGrid grid, Index start, Index end, bool allowDiagonal, PathRequestDelegate callback)
        {
            this.grid = grid;
            this.start = start;
            this.end = end;
            this.allowDiagonal = allowDiagonal;
            this.callback = callback;

            // Create a time stamp
            timeStamp = DateTime.Now.Ticks;
        }
    }
}
