// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using BigBrother.UILogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace BigBrother.UILayer.Services
{
    // Documentation on working with tiles can be found at http://go.microsoft.com/fwlink/?LinkID=288821&clcid=0x409
    public class SecondaryTileService : ISecondaryTileService
    {
        private Uri _squareLogoUri = new Uri("ms-appx:///Assets/Logo.png");
        private Uri _wideLogoUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png");

        public bool SecondaryTileExists(string tileId) {
            return SecondaryTile.Exists(tileId);
        }

        public async Task<bool> PinSquareSecondaryTile(string tileId, string shortName, string displayName, string arguments) {
            if (!SecondaryTileExists(tileId)) {
                var secondaryTile = new SecondaryTile(tileId, shortName, displayName, arguments, TileOptions.ShowNameOnLogo, _squareLogoUri, null);
                bool isPinned = await secondaryTile.RequestCreateAsync();

                return isPinned;
            }

            return true;
        }

        public async Task<bool> PinWideSecondaryTile(string tileId, string shortName, string displayName, string arguments) {
            if (!SecondaryTileExists(tileId)) {
                var secondaryTile = new SecondaryTile(tileId, shortName, displayName, arguments, TileOptions.ShowNameOnWideLogo, _squareLogoUri, _wideLogoUri);
                bool isPinned = await secondaryTile.RequestCreateAsync();

                return isPinned;
            }

            return true;
        }

        public async Task<bool> UnpinTile(string tileId) {
            if (SecondaryTileExists(tileId)) {
                var secondaryTile = new SecondaryTile(tileId);
                bool isUnpinned = await secondaryTile.RequestDeleteAsync();
                return isUnpinned;
            }

            return true;
        }

        public void ActivateTileNotifications(string tileId, Uri tileContentUri, PeriodicUpdateRecurrence recurrence) {
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
            tileUpdater.StartPeriodicUpdate(tileContentUri, recurrence);
        }
    }
}
