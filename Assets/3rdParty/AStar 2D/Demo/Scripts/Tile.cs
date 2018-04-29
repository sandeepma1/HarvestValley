using UnityEngine;
using System.Collections;


// Import the AStar_2D namespace
using AStar_2D;

// Namespace
namespace AStar_2D.Demo
{
    /// <summary>
    /// This would be the base tile class for a tile based game which contains properties about the appearance etc of the tile
    /// Can inherit any class required but must implement the INode interface to be able to be used by the pathfinding system
    /// </summary>
    public class Tile : MonoBehaviour, IPathNode
    {
        public bool touchingPathFlag = false;
        // Delegates
        /// <summary>

        /// Delegate used when tiles are selected by a mouse click.
        /// </summary>
        /// <param name="tile">The tile that was clicked.</param>
        /// <param name="mouseButton">The mouse button that was pressed</param>
        public delegate void TileSelectedDelegate(Tile tile, int mouseButton);

        /// <summary>
        /// Delegate used when tiles are hovered by the mouse
        /// </summary>
        /// <param name="tile"></param>
        public delegate void TileHoverDelegate(Tile tile);

        // Events
        /// <summary>
        /// Event that triggers when this tile has been selected a mouse click.
        /// Informs the tile manager so that the appropriate action can be taken.
        /// </summary>
        public event TileSelectedDelegate onTileSelected;

        /// <summary>
        /// Event that triggers when this tile is hovered by the mouse.
        /// Informs the tile manager so that a preview path can be shown.
        /// </summary>
        public event TileHoverDelegate onTileHover;

        // Private
        [HideInInspector]
        public bool showDebug = false;
        [SerializeField]
        private bool walkable = true;
        private bool canSend = true;
        private float lastTime = 0;
        private int hardness = 0;
        private bool ladder = false;

        // Public
        /// <summary>
        /// The index into the grid that this tile is located at.
        /// </summary>
        public Index index = new Index();

        // Properties
        /// <summary>
        /// Implement the IsWalkable property in the INode interface
        /// It is up to the user to determine whether or not a tile should be walkable
        /// </summary>
        public bool IsWalkable
        {
            get { return walkable; } // Only need to implement the get but set is useful
            set
            {
                walkable = value;
                if (showDebug)
                {
                    GetComponent<SpriteRenderer>().color = walkable ? Color.white : Color.clear;
                }
                else
                {
                    Destroy(GetComponent<SpriteRenderer>());
                }
            }
        }

        public bool IsLadder
        {
            get { return ladder; } // Only need to implement the get but set is useful
            set { ladder = value; }
        }

        public int Hardness
        {
            get { return hardness; } // Only need to implement the get but set is useful
            set { hardness = value; }
        }

        /// <summary>
        /// Implement the Weighting property in the INode interface
        /// It is up to the user to decide a suitable value between 0 and 1 which represents avoidance of tiles to a certain level
        /// e.g. Grass tiles might have a higher weighting value when there are path tile to walk on
        /// The lower the weighting value, the less resistance there will be
        /// </summary>
        public float Weighting
        {
            get { return index.Equals(Index.zero) ? 1 : 0; }
        }

        /// <summary>
        /// The position in 3D space that this tile is located at.
        /// </summary>
        public Vector3 WorldPosition
        {
            get { return transform.position; }
        }

        // Methods
        /// <summary>
        /// Called by Unity.
        /// Left blank for demonstration.
        /// </summary>
        //public void Start()
        //{
        /*int random = Random.Range (0, 4);
        if (random < 1) {
            walkable = false;
            this.gameObject.GetComponent <SpriteRenderer> ().sprite = tileSheet [Random.Range (0, 4)];
            GameEventManager.numberOfRocksInLevel = GameEventManager.numberOfRocksInLevel + 1;
            //this.gameObject.GetComponent <SpriteRenderer> ().color = new Color (1, 0, 0, 0.5f);
        }*/
        // DO setup code for the tile
        //}

        /// <summary>
        /// Toggle the walkable state of this tile.
        /// </summary>
        public void toggleWalkable()
        {
            walkable = !walkable;
            /*
                        // Get the sprite renderer
                        SpriteRenderer renderer = GetComponent<SpriteRenderer> ();

                        // Check if the tile is walkable
                        if (IsWalkable == true) {
                            renderer.color = new Color (1, 1, 1, 0f);
                        } else {
                            renderer.color = new Color (1, 0, 0, 0f);
                        }*/
        }

        public bool isTouchingPath(Path path)
        {
            // Check if this tile is a node in the specified path
            foreach (PathRouteNode node in path)
                if (node.Index.Equals(index) == true)
                    return true;

            // Not in the path
            return false;
        }
    }
}
