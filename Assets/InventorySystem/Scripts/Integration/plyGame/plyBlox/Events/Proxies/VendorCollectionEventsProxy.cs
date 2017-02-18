﻿#if PLY_GAME

using System;
using System.Collections.Generic;
using Devdog.InventorySystem.Integration.plyGame.plyBlox;
using UnityEngine;

namespace Devdog.InventorySystem.Integration.plyGame.plyBlox
{

    /// <summary>
    /// Relays all events to plyGame's plyBox
    /// </summary>
    public partial class VendorCollectionEventsProxy : MonoBehaviour
    {
        private VendorEventHandler eventHandler { get; set; }
        private VendorUI window { get; set; }
        
        // <inheritdoc />
        public void Start()
        {
            eventHandler = GetComponent<VendorEventHandler>();
            window = InventoryManager.instance.vendor;

            if (window != null)
            {
                window.OnSoldItemToVendor += OnSoldItemToVendor;
                window.OnBoughtItemFromVendor += OnBoughtItemFromVendor;
                window.OnBoughtItemBackFromVendor += OnBoughtItemBackFromVendor;
            }
        }


        public void OnDestroy()
        {
            if (window != null)
            {
                window.OnSoldItemToVendor -= OnSoldItemToVendor;
                window.OnBoughtItemFromVendor -= OnBoughtItemFromVendor;
                window.OnBoughtItemBackFromVendor -= OnBoughtItemBackFromVendor;
            }
        }


        public void OnSoldItemToVendor(InventoryItemBase item, uint amount, VendorTriggerer vendor)
        {
            if (eventHandler != null)
                eventHandler.OnSoldItemToVendor(item, amount, vendor);
        }

        public void OnBoughtItemFromVendor(InventoryItemBase item, uint amount, VendorTriggerer vendor)
        {
            if (eventHandler != null)
                eventHandler.OnBoughtItemFromVendor(item, amount, vendor);
        }

        public void OnBoughtItemBackFromVendor(InventoryItemBase item, uint amount, VendorTriggerer vendor)
        {
            if (eventHandler != null)
                eventHandler.OnBoughtItemBackFromVendor(item, amount, vendor);
        }
    }
}

#endif