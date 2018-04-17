using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using AStar_2D.Collections;
using AStar_2D.Pathfinding.Algorithm;

namespace AStar_2D.Pathfinding
{
    /// <summary>
    /// Represents a grid that can be used for pathfinding.
    /// Use this class if you want to achieve pathfinding without relying on Unity components.
    /// </summary>
    public class SearchGrid
    {
        // Private
        private NodeQueue<PathNode> orderedMap = null;
        private OpenNodeMap<PathNode> closedMap = null;
        private OpenNodeMap<PathNode> openMap = null;
        private OpenNodeMap<PathNode> runtimeMap = null;

        private PathNode[,] nodeGrid = null;
        private PathNode[,] searchGrid = null;
        private int width = 0;
        private int height = 0;
        private float nodeSpacing = 0.2f;

        // Public
        /// <summary>
        /// The heuristic method to use.
        /// </summary>
        public HeuristicProvider provider = HeuristicProvider.defaultProvider;

        // Properties
        /// <summary>
        /// Attempts to access an element of the grid at the specified index.
        /// </summary>
        /// <param name="x">The X component of the index</param>
        /// <param name="y">The Y component of the index</param>
        /// <returns>The <see cref="IPathNode"/> at the specified index</returns>
        public IPathNode this[int x, int y]
        {
            get { return nodeGrid[x, y]; }
        }

        /// <summary>
        /// The current width of the grid.
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// The current height of the grid.
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// The distance between 2 nodes. Settings this value can dramaticaly increase the performance of <see cref="findNearestIndex(Vector3)"/>.
        /// As it will prevent an exhaustive search if the method has already found the best matching node.
        /// Only set this value if the user node grid has equal spacing in both the X and Y axis.
        /// </summary>
        public float NodeSpacing
        {
            get { return nodeSpacing; }
            set { nodeSpacing = value; }
        }

        // Methods
        /// <summary>
        /// Allocates the search area based on an input array.
        /// </summary>
        /// <param name="inputGrid">The input array used for searching</param>
        public void constructGrid(IPathNode[,] inputGrid)
        {
            // Make sur ethe input is acceptable
            validateInputGrid(inputGrid);

            // Get sizes
            width = inputGrid.GetLength(0);
            height = inputGrid.GetLength(1);

            // Cache and allocate
            nodeGrid = new PathNode[width, height];
            searchGrid = new PathNode[width, height];

            closedMap = new OpenNodeMap<PathNode>(width, height);
            openMap = new OpenNodeMap<PathNode>(width, height);
            runtimeMap = new OpenNodeMap<PathNode>(width, height);
            orderedMap = new NodeQueue<PathNode>(new PathNode(Index.zero, null));

            // Create the grid wrapper
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Create a wrapper path node over the interface
                    nodeGrid[x, y] = new PathNode(new Index(x, y), inputGrid[x, y]);
                }
            }
        }

        /// <summary>
        /// Attempts to locate the <see cref="Index"/> that is closest to the specified world position.
        /// This method is very expensive and performs a distance check for every node in the grid.
        /// Should not be used for very large grids.
        /// </summary>
        /// <param name="worldPosition">The input position</param>
        /// <returns>A <see cref="Index"/> that best represents the specified world position</returns>
        public Index findNearestIndex(Vector3 worldPosition)
        {
            Index index = new Index(0, 0);
            float closest = float.MaxValue;
            float sqrSpacing = Mathf.Pow(nodeSpacing, 2);

            // Process each node
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 a = nodeGrid[x, y].WorldPosition;

                    float elementX = (a.x - worldPosition.x);
                    float elementY = (a.y - worldPosition.y);
                    float elementZ = (a.z - worldPosition.z);

                    // Calculate the square distance
                    float sqrDistance = (elementX * elementX + elementY * elementY + elementZ * elementZ);

                    // Check for smaller
                    if (sqrDistance < closest)
                    {
                        index = nodeGrid[x, y].Index;
                        closest = sqrDistance;

                        // Check if we can consider this as the best
                        if (sqrDistance < sqrSpacing)
                            return index;
                    }
                }
            }

            return index;
        }

        /// <summary>
        /// Launches the pathfinding algorithm and attempts to find a <see cref="Path"/> between the start and end <see cref="Index"/>.
        /// This overload accepts a <see cref="MonoDelegate"/> as a callback.
        /// </summary>
        /// <param name="start">The start position for the search</param>
        /// <param name="end">The end position for the search</param>
        /// <param name="allowDiagonal">Can diagonal paths be used</param>
        /// <param name="callback">The <see cref="MonoDelegate"/> to invoke on completion</param>
        public void findPath(Index start, Index end, bool allowDiagonal, MonoDelegate callback)
        {
            // Call through
            findPath(start, end, allowDiagonal, (Path path, PathRequestStatus status) =>
            {
                // Invoke the mono delegate
                callback.invoke(new MonoDelegateEvent(path, status));
            });
        }

        /// <summary>
        /// Launches the pathfinding algorithm and attempts to find a <see cref="Path"/> between the start and end <see cref="Index"/>.
        /// </summary>
        /// <param name="start">The start position for the search</param>
        /// <param name="end">The end position for the search</param>
        /// <param name="allowDiagonal">Can diagonal paths be used</param>
        /// <param name="callback">The <see cref="PathRequestDelegate"/> method to call on completion</param>
        public void findPath(Index start, Index end, bool allowDiagonal, PathRequestDelegate callback)
        {
            //Debug.Log (end);			
            // Already at the destination
            if (start.Equals(end))
            {
                callback(null, PathRequestStatus.SameStartEnd);
                return;
            }

            // Get the nodes
            PathNode startNode = nodeGrid[start.X, start.Y];
            PathNode endNode = nodeGrid[end.X, end.Y];

            // Clear all previous data
            clearSearchData();

            // Starting scores
            startNode.g = 0;
            startNode.h = provider.heuristic(startNode, endNode);
            startNode.f = startNode.h;

            // Add the start node
            openMap.add(startNode);
            runtimeMap.add(startNode);
            orderedMap.push(startNode);

            // Allocate a buffer for neghbouring nodes
            PathNode[] adjacentNodes = new PathNode[8];

            while (openMap.Count > 0)
            {
                // Get the front value
                PathNode value = orderedMap.pop();

                if (value == endNode)
                {
                    // We have found the path
                    Path result = constructPath(searchGrid[endNode.Index.X, endNode.Index.Y]);

                    // Last node
                    result.push(endNode, endNode.Index); //Sandy edit
                                                         //Debug.Log (endNode.Index);
                                                         //Debug.Log (result.NodeCount);
                                                         // Trigger the delegate with success
                    callback(result, PathRequestStatus.PathFound);

                    // Exit the method
                    return;
                }
                else
                {
                    openMap.remove(value);
                    closedMap.add(value);

                    // Fill our array with surrounding nodes
                    constructAdjacentNodes(value, adjacentNodes, allowDiagonal);

                    // Process each neighbor
                    foreach (PathNode adjacent in adjacentNodes)
                    {
                        bool isBetter = false;

                        // Skip null nodes
                        if (adjacent == null)
                            continue;

                        // Make sure the node is walkable
                        if (adjacent.IsWalkable == false)
                            continue;

                        // Make sure it has not already been excluded
                        if (closedMap.contains(adjacent) == true)
                            continue;

                        // Check for custom exclusion descisions
                        if (validateConnection(value, adjacent) == false)
                            continue;

                        // Calculate the score for the node
                        float score = runtimeMap[value].g + provider.adjacentDistance(value, adjacent);
                        bool added = false;

                        // Make sure it can be added to the open map
                        if (openMap.contains(adjacent) == false)
                        {
                            openMap.add(adjacent);
                            isBetter = true;
                            added = true;
                        }
                        else if (score < runtimeMap[adjacent].g)
                        {
                            // The score is better
                            isBetter = true;
                        }
                        else
                        {
                            // The score is not better
                            isBetter = false;
                        }

                        // CHeck if a better score has been found
                        if (isBetter == true)
                        {
                            // Update the search grid
                            searchGrid[adjacent.Index.X, adjacent.Index.Y] = value;

                            // Add the adjacent node
                            if (runtimeMap.contains(adjacent) == false)
                                runtimeMap.add(adjacent);

                            // Update the score values for the node
                            runtimeMap[adjacent].g = score;
                            runtimeMap[adjacent].h = provider.heuristic(adjacent, endNode);
                            runtimeMap[adjacent].f = runtimeMap[adjacent].g + runtimeMap[adjacent].h;

                            // CHeck if we added to the open map
                            if (added == true)
                            {
                                // Push the adjacent node to the set
                                orderedMap.push(adjacent);
                            }
                            else
                            {
                                // Refresh the set
                                orderedMap.refresh(adjacent);
                            }
                        }
                    }

                }
            } // End while

            // Failure
            callback(null, PathRequestStatus.PathNotFound);
        }

        /// <summary>
        /// When overriden, allows custom checks to be implemented to determine whether neghboring nodes are able to connect to a specific node.
        /// </summary>
        /// <param name="center">The center node that is currently being validated</param>
        /// <param name="neighbor">The neghboring node that is being checked</param>
        /// <returns>Should return true when the connection between the neighbors is allowed and false whent he connection is not allowed</returns>
        public virtual bool validateConnection(PathNode center, PathNode neighbor)
        {
            // Default behaviour - all nodes are valid and can be included
            return true;
        }

        private int constructAdjacentNodes(PathNode center, PathNode[] nodes, bool allowDiagonal)
        {
            // Get the center node
            Index node = center.Index;

            int index = 0;

            // Check if diagonal movements can be used
            if (allowDiagonal == true)
            {
                // Bottom left
                if ((node.X > 0) && (node.Y > 0))
                    nodes[index++] = nodeGrid[node.X - 1, node.Y - 1];
                else
                    nodes[index++] = null;

                // Top right
                if ((node.X < width - 1) && (node.Y < height - 1))
                    nodes[index++] = nodeGrid[node.X + 1, node.Y + 1];
                else
                    nodes[index++] = null;

                // Top Left
                if ((node.X > 0) && (node.Y < height - 1))
                    nodes[index++] = nodeGrid[node.X - 1, node.Y + 1];
                else
                    nodes[index++] = null;

                // Bottom right
                if ((node.X < width - 1) && (node.Y > 0))
                    nodes[index++] = nodeGrid[node.X + 1, node.Y - 1];
                else
                    nodes[index++] = null;
            }

            // Bottom
            if (node.Y > 0)
                nodes[index++] = nodeGrid[node.X, node.Y - 1];
            else
                nodes[index++] = null;

            // Left
            if (node.X > 0)
                nodes[index++] = nodeGrid[node.X - 1, node.Y];
            else
                nodes[index++] = null;

            // Right
            if (node.X < width - 1)
                nodes[index++] = nodeGrid[node.X + 1, node.Y];
            else
                nodes[index++] = null;

            // Top
            if (node.Y < height - 1)
                nodes[index++] = nodeGrid[node.X, node.Y + 1];
            else
                nodes[index++] = null;

            return index + 1;
        }

        private Path constructPath(PathNode current)
        {
            // Create the path
            Path path = new Path();

            // Call the dee construct method
            deepConstructPath(current, path);

            return path;
        }

        private void deepConstructPath(PathNode current, Path output)
        {
            // Get the node from the search grid
            PathNode node = searchGrid[current.Index.X, current.Index.Y];

            // Make sure we have a valid node
            if (node != null)
            {
                // Call through reccursive
                deepConstructPath(node, output);
            }

            // Push the node to the path
            output.push(current, current.Index);
        }

        private void validateInputGrid(IPathNode[,] grid)
        {
            // CHeck for null arrays
            if (grid == null)
                throw new ArgumentException("A search grid cannot be created from a null reference");

            // Check for 0 lenght arrays
            if (grid.GetLength(0) == 0 || grid.GetLength(1) == 0)
                throw new ArgumentException("A search grid cannot be created because one or more dimensions have a length of 0");
        }

        private void clearSearchData()
        {
            // Reset all data
            closedMap.clear();
            openMap.clear();
            runtimeMap.clear();
            orderedMap.clear();

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    searchGrid[x, y] = null;
        }
    }
}