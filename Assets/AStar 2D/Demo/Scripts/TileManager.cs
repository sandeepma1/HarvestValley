using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// Import the AStar_2D namespace
using AStar_2D;
using AStar_2D.Pathfinding;

// Namespace
namespace AStar_2D.Demo
{
	/// <summary>
	/// Inherits the AStar class that allows the user to specify what INode to use for pathfinding, in this case Tile.
	/// By default, AIManager is a singleton which can be accessed anywhere in code using AIManager.Instance.
	/// This allows access to the pathfinding methods within.
	/// </summary>
	public class TileManager : AStarGrid
	{
		
		// Private
		private Tile[,] tiles;
		private Tile selectedTile = null;

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
		public float tileSpacing = 0.5f;
		// Methods
		/// <summary>
		/// Called by Unity.
		/// Note that the base method is called. This is essential to ensure that the base class initializes correctly.
		/// </summary>
		public override void Awake ()
		{
			m_instance = this;
			base.Awake ();

			tiles = new Tile[gridX, gridY];

			for (int i = 0; i < gridX; i++) {
				for (int j = 0; j < gridY; j++) {
					// Create the tile at its location
					GameObject obj = MonoBehaviour.Instantiate (tilePrefab, new Vector3 ((i - (gridX / 2)), (j - (gridY / 2))), Quaternion.identity) as GameObject;

					// Add the tile script
					tiles [i, j] = obj.GetComponent<Tile> ();
					tiles [i, j].index = new Index (i, j);

					// Add an event listener
					tiles [i, j].onTileSelected += onTileSelected;

					// Check for preview
					if (showPreviewPath == true)
						tiles [i, j].onTileHover += onTileHover;

					// Add the tile as a child to keep the scene view clean
					obj.transform.SetParent (transform);
				}
			}

			// Pass the arry to the search grid
			constructGrid (tiles);
		}

		/// <summary>
		/// Called by Unity.
		/// Left blank for demonstration.
		/// </summary>
		public void Update ()
		{
			// Do stuff
		}

		private void onTileSelected (Tile tile, int mouseButton)
		{
			if (!tile.IsWalkable) {
				selectedTile = tile;
			} else {
				selectedTile = null;
			}
			// Check for button
			if (mouseButton == 0) {				
				// Set the destination
				tile.IsWalkable = true;
				Agent[] agents = Component.FindObjectsOfType<Agent> ();

				// Set the target for all agents
				foreach (Agent agent in agents)
					agent.setDestination (tile.WorldPosition);

				
			} else if (mouseButton == 1) {
				// Toggle the walkable status
				//tile.toggleWalkable ();
				SceneManager.LoadScene (0);
			}
			StartCoroutine (EnableTile (tile));
			//tile.IsWalkable = false;
		}

		IEnumerator EnableTile (Tile tile)
		{
			yield return new WaitForSeconds (0.1f);
			tile.IsWalkable = false;
		}

		public override void RemoveTile ()
		{
			
			if (selectedTile != null) {
				selectedTile.toggleWalkable ();
				print ("remove");
			}
		}

		private void onTileHover (Tile tile)
		{
			// Find the first agent
			Agent agent = Component.FindObjectOfType<Agent> ();

			if (agent != null) {
				// Find the tile index
				Index current = findNearestIndex (agent.transform.position);

				// Request a path but dont assign it to the agent - this will allow the preview to be shown without the agent following it
				findPath (current, tile.index, (Path result, PathRequestStatus status) => {
					// Do nothing
				});
			}
		}
	}
}
