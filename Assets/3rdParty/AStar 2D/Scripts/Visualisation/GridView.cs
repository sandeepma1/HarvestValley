using UnityEngine;

namespace AStar_2D.Visualisation
{
    public sealed class GridView : MonoBehaviour
    {
        // Public
        // Unity complains that this value is never set to anything other than null however it is meant to be set in the inspector.
#pragma warning disable 0649
        public AStarAbstractGrid visualizeGrid;
#pragma warning restore 0649
        public Color colour = Color.green;

        // Methods
        public void Update()
        {
            for (int x = 0; x < visualizeGrid.Width; x++)
            {
                for (int y = 0; y < visualizeGrid.Height; y++)
                {
                    // Get the current node
                    IPathNode current = visualizeGrid[x, y];

                    // Make sure the node is walkable
                    if (current.IsWalkable == false)
                        continue;

                    // Process surrounding tiles
                    IPathNode[] nodes = getSurroundingNodes(x, y);

                    // Process all surrounding tiles
                    foreach (IPathNode node in nodes)
                    {
                        // Check for null
                        if (node == null)
                            continue;

                        // Check for walkable
                        if (node.IsWalkable == false)
                            continue;

                        // Add line
                        Debug.DrawLine(node.WorldPosition, current.WorldPosition, colour);
                    }
                }
            }
        }

        private IPathNode[] getSurroundingNodes(int x, int y)
        {
            // Create the array
            IPathNode[] nodes = new IPathNode[(visualizeGrid.allowDiagonal == true) ? 8 : 4];

            // Left node
            if (x - 1 > 0)
                nodes[0] = visualizeGrid[x - 1, y];

            // Right node
            if (x + 1 < visualizeGrid.Width)
                nodes[1] = visualizeGrid[x + 1, y];

            // Up node
            if (y + 1 < visualizeGrid.Height)
                nodes[2] = visualizeGrid[x, y + 1];

            // Down node
            if (y - 1 > 0)
                nodes[3] = visualizeGrid[x, y - 1];

            // Diagonal neighbors
            if (visualizeGrid.allowDiagonal == true)
            {
                // Top left
                if (x - 1 > 0 && y + 1 < visualizeGrid.Height)
                    nodes[4] = visualizeGrid[x - 1, y + 1];

                // Top right
                if (x + 1 < visualizeGrid.Width && y + 1 < visualizeGrid.Height)
                    nodes[5] = visualizeGrid[x + 1, y + 1];

                // Bottom left
                if (x - 1 > 0 && y - 1 > 0)
                    nodes[6] = visualizeGrid[x - 1, y - 1];

                // Bottom right
                if (x + 1 < visualizeGrid.Width && y - 1 > 0)
                    nodes[7] = visualizeGrid[x + 1, y - 1];
            }
            return nodes;
        }

        public static GridView findViewForGrid(AStarAbstractGrid grid)
        {
            // Find all components
            foreach (GridView view in Component.FindObjectsOfType<GridView>())
            {
                // Check for observed grid
                if (view.visualizeGrid == grid)
                {
                    // Get the component
                    return view;
                }
            }

            // Not found
            return null;
        }
    }
}
