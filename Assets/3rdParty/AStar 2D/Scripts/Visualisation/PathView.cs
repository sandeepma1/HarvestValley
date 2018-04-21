﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AStar_2D.Visualisation
{
    public sealed class PathView : MonoBehaviour
    {
        // Private
        private LineRenderer line = null;

        // Public
        // Unity complains that this value is never set to anything other than null however it is meant to be set in the inspector.
#pragma warning disable 0649
        public AStarAbstractGrid visualizeGrid;
#pragma warning restore 0649
        public Material material;
        public float width = 0.2f;

        // Propeties
        public static Material DefaultMaterial
        {
            get
            {
                Material mat = new Material(Shader.Find("Sprites/Default"));

                mat.color = Color.magenta;

                return mat;
            }
        }

        // Methods
        public void Start()
        {
            // Get the renderer
            line = GetComponent<LineRenderer>();

            // Attatch at runtime
            if (line == null)
                line = gameObject.AddComponent<LineRenderer>();

            // Check for material
            if (material == null)
                material = DefaultMaterial;

            // Setup
            line.material = material;
            line.SetWidth(width, width);
            line.sortingOrder = -1;
        }

        public void setRenderPath(Path path)
        {
            // Set the number of vertices required
            line.SetVertexCount(path.NodeCount);

            int index = 0;

            // Set each vertex
            foreach (PathRouteNode node in path)
            {
                // Set the vertex
                line.SetPosition(index, node.WorldPosition);

                // Increment the index
                index++;
            }
        }

        public static void setRenderPath(AStarAbstractGrid grid, Path path)
        {
            // Make sure the path is valid
            if (path == null)
                return;

            // Find the component 
            PathView view = findViewForGrid(grid);

            // Check for error
            if (view == null)
                return;

            // Udpate the render path
            view.setRenderPath(path);
        }

        public static PathView findViewForGrid(AStarAbstractGrid grid)
        {
            // Find all components
            foreach (PathView view in Component.FindObjectsOfType<PathView>())
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
