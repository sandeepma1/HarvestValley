﻿using UnityEngine;
using UnityEditor;
using System;

namespace Devdog.InventorySystem.Editors
{
    [CustomEditor(typeof(VendorTriggerer), true)]
    public class VendorTriggererEditor : Editor
    {
        
        public virtual void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            var t = (VendorTriggerer)target;
            if (t.GetComponent<InventoryItemContainerGenerator>() == null)
            {
                EditorGUILayout.HelpBox("Did you know you can use a generator to generate a set of items?", MessageType.Info);
                GUI.color = Color.green;
                if (GUILayout.Button("Add generator"))
                    t.gameObject.AddComponent<InventoryItemContainerGenerator>();

                GUI.color = Color.white;
            }
        }
    }
}