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
		private Tile[] rockTilesTemp, rockTiles;
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
		public Sprite[] tileSheet;
		int ladderSelectionNumber = 0, ladderTop, ladderBottom, ladderSpecific, tileRemovedCount = 0;
		// Methods
		/// <summary>
		/// Called by Unity.
		/// Note that the base method is called. This is essential to ensure that the base class initializes correctly.
		/// </summary>
		public override void Awake ()
		{
			m_instance = this;
			base.Awake ();

			GameEventManager.numberOfRocksInLevel = 0;
			tiles = new Tile[gridX, gridY];
			rockTilesTemp = new Tile[gridX * gridY];

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

					int random = Random.Range (0, 4);
					if (random < 1) {
						tiles [i, j].IsWalkable = false;
						tiles [i, j].gameObject.GetComponent <SpriteRenderer> ().sprite = tileSheet [Random.Range (0, 4)];
						rockTilesTemp [GameEventManager.numberOfRocksInLevel] = tiles [i, j];
						GameEventManager.numberOfRocksInLevel = GameEventManager.numberOfRocksInLevel + 1;
					}
				}
			}

			int count = 0;
			rockTiles = new Tile[GameEventManager.numberOfRocksInLevel];
			for (int i = 0; i < rockTilesTemp.Length; i++) {
				if (rockTilesTemp [i] != null) {
					rockTiles [count] = rockTilesTemp [i];
					count++;
				}
			}

			// Pass the arry to the search grid
			constructGrid (tiles);

			//***********************Ladder Logic
			ladderSelectionNumber = Random.Range (0, 3);
			ladderTop = Random.Range (2, GameEventManager.numberOfRocksInLevel / 2);
			ladderBottom = Random.Range (GameEventManager.numberOfRocksInLevel / 2, GameEventManager.numberOfRocksInLevel);
			ladderSpecific = Random.Range (2, GameEventManager.numberOfRocksInLevel);		
			print (GameEventManager.numberOfRocksInLevel + " Tiles**");
			//************************
		}

		public void LadderLogic ()
		{
			switch (ladderSelectionNumber) {
				case 0:
					print ("any top");
					LadderLogic_TOP ();
					break;
				case 1:
					print ("any bottom");
					LadderLogic_BOTTOM ();
					break;
				case 2:
					print ("Specific");
					LadderLogic_SPECIFIC ();
					break;
				case 3:
					print ("enemy");
					LadderLogic_ENEMY ();
					break;
				default:
					break;
			} 
		}

		void LadderLogic_TOP ()
		{
			print (ladderTop);
			if (tileRemovedCount == ladderTop) {
				CreateLadder ();
			}
		}

		void LadderLogic_BOTTOM ()
		{
			print (ladderBottom);
			if (tileRemovedCount == ladderBottom) {
				CreateLadder ();
			}
		}

		void LadderLogic_SPECIFIC ()
		{
			print (ladderSpecific);
			if (tileRemovedCount == ladderSpecific) {
				CreateLadder ();
			}
		}

		void LadderLogic_ENEMY ()
		{

		}

		void CreateLadder ()
		{
			print ("Way to down!!!");
			selectedTile.gameObject.GetComponent <SpriteRenderer> ().sprite = tileSheet [4];
			selectedTile.IsLadder = true;
		}

		private void onTileSelected (Tile tile, int mouseButton)
		{
			StopCoroutine ("RemoveTileAfterDelay");
			selectedTile = null;
			if (!tile.IsWalkable) {
				selectedTile = tile;
			} else {
				selectedTile = null;
			}

			// Check for button
			if (mouseButton == 0) {				
				// Set the destination
				//if stone tile is clicked
				Agent[] agents = Component.FindObjectsOfType<Agent> ();
				if (tile.IsLadder) {
					SceneManager.LoadScene (0);
				}
				if (!tile.IsWalkable) {
					tile.IsWalkable = true;					
					foreach (Agent agent in agents) {				
						agent.clickedBlank = false;
					}
					StopCoroutine (EnableTile (tile));
					StartCoroutine (EnableTile (tile));
				} else {
					foreach (Agent agent in agents) {				
						agent.clickedBlank = true;
					}
				}
				// Set the target for all agents
				foreach (Agent agent in agents) {					
					agent.setDestination (tile.WorldPosition);
				}
				
			} else if (mouseButton == 1) {
				// Toggle the walkable status
				//tile.toggleWalkable ();
				SceneManager.LoadScene (0);
			}
		}

		IEnumerator EnableTile (Tile tile)
		{
			yield return new WaitForSeconds (0.1f);
			tile.IsWalkable = false;
		}

		public override void RemoveTile ()
		{			
			if (selectedTile != null) {
				//start_UniqueSingularCoroutine_RemoveTileAfterDelay ();
				StopCoroutine ("RemoveTileAfterDelay");
				StartCoroutine ("RemoveTileAfterDelay");
			}
		}

		public IEnumerator RemoveTileAfterDelay ()
		{
			yield return new WaitForSeconds (1f);
			selectedTile.toggleWalkable ();
			selectedTile.gameObject.GetComponent <SpriteRenderer> ().sprite = tileSheet [5];
			tileRemovedCount += 1;
			LadderLogic ();
			print ("removed");
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
