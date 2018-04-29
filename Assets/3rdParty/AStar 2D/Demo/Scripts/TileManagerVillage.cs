using UnityEngine;

namespace AStar_2D.Demo
{
    public class TileManagerVillage : AStarGrid
    {
        public static TileManagerVillage Instance = null;
        [SerializeField]
        private bool showDebugPath;
        [SerializeField]
        private Transform allPropsParent;
        // Private
        private Tile[,] tiles;
        Camera mainCamera;
        Vector3 p = new Vector3();
        // Public
        /// <summary>
        /// How many tiles to create in the X axis.
        /// </summary>
        public int gridX = 16;
        /// <summary>
        /// How many tiles to create in the Y axis.
        /// </summary>
        public int gridY = 16;
        /// <summary>
        /// The prefab that represents an individual tile.
        /// </summary>
        public GameObject tilePrefab;
        /// <summary>
        /// When true, a preview path will be shown when the mouse hovers over a tile.
        /// </summary>
        public bool showPreviewPath = false;
        public bool isClickedForBuilding;
        // Methods
        /// <summary>
        /// Called by Unity.
        /// Note that the base method is called. This is essential to ensure that the base class initializes correctly.
        /// </summary>
        public override void Awake()
        {
            Instance = this;
            base.Awake();

            tiles = new Tile[gridX, gridY];

            for (int i = 0; i < gridX; i++)
            {
                for (int j = 0; j < gridY; j++)
                {
                    // Create the tile at its location
                    //GameObject obj = MonoBehaviour.Instantiate(tilePrefab, new Vector3((i - (gridX / 2) + 0.5f), (j - (gridY / 2) + 0.5f)), Quaternion.identity) as GameObject;
                    GameObject obj = MonoBehaviour.Instantiate(tilePrefab, new Vector3(i + 0.5f, j + 0.5f), Quaternion.identity, transform) as GameObject;

                    // Add the tile script
                    tiles[i, j] = obj.GetComponent<Tile>();
                    tiles[i, j].index = new Index(i, j);
                    // Add an event listener
                    tiles[i, j].onTileSelected += onTileSelected;
                    tiles[i, j].showDebug = showDebugPath;
                    //tiles[i, j].gameObject.isStatic = true;
                    // Check for preview
                    if (showPreviewPath == true)
                        tiles[i, j].onTileHover += OnTileHover;
                }
            }

            // Pass the arry to the search grid
            constructGrid(tiles);
            ConstructProps();
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void ConstructProps()
        {
            SpriteRenderer[] allProps = allPropsParent.GetComponentsInChildren<SpriteRenderer>();

            for (int i = 1; i < allProps.Length; i++)
            {
                if (allProps[i].CompareTag("AStarOclude"))
                {
                    BoxCollider2D boxCollider2D = allProps[i].GetComponent<BoxCollider2D>();

                    if (boxCollider2D == null)
                    {
                        // Use Sprite bonds
                        FindSurrounding(allProps[i].bounds.size, allProps[i].transform.localPosition);
                    }
                    else
                    {
                        //Use collider bonds
                        FindSurroundingOnCollision(boxCollider2D, allProps[i].transform.localPosition);
                    }
                }
            }
        }

        private void FindSurrounding(Vector2 bounds, Vector2 position)
        {
            int xPos = Mathf.RoundToInt(position.x - bounds.x / 2);
            int yPos = Mathf.RoundToInt(position.y - bounds.y / 2);
            for (int i = 0; i < bounds.x; i++)
            {
                for (int j = 0; j < bounds.y; j++)
                {
                    tiles[i + xPos, j + yPos].IsWalkable = false;
                }
            }
        }

        private void FindSurroundingOnCollision(BoxCollider2D boxCollider2D, Vector2 position)
        {
            float xPos = (position.x - boxCollider2D.bounds.size.x / 2) + boxCollider2D.offset.x;
            float yPos = (position.y - boxCollider2D.bounds.size.y / 2) + boxCollider2D.offset.y;
            for (int i = 0; i < boxCollider2D.bounds.size.x; i++)
            {
                for (int j = 0; j < boxCollider2D.bounds.size.y; j++)
                {
                    tiles[i + (int)xPos, j + (int)yPos].IsWalkable = false;

                }
            }
        }

        private void onTileSelected(Tile tile, int mouseButton)
        {
            // Check for button
            //if (mouseButton == 0)
            //{
            //    player.setDestination(tile.WorldPosition);
            //}
            //else if (mouseButton == 1)
            //{
            //    // Toggle the walkable status
            //    tile.toggleWalkable();
            //}
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0) && !isClickedForBuilding)
            {
                player.SetDestination(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            }
        }

        public void SetPlayerDestination(Vector3 position)
        {
            player.SetDestination(position);
        }

        private void OnTileHover(Tile tile)
        {
            // Find the first agent
            Agent agent = Component.FindObjectOfType<Agent>();

            if (agent != null)
            {
                // Find the tile index
                Index current = findNearestIndex(agent.transform.position);

                // Request a path but dont assign it to the agent - this will allow the preview to be shown without the agent following it
                findPath(current, tile.index, (Path result, PathRequestStatus status) =>
                {
                    // Do nothing
                });
            }
        }
    }
}