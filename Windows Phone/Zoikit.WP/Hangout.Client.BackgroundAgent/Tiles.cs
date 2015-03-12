using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media;

namespace Hangout.Client.BackgroundAgent
{
    class Tiles
    {
        private static Version TargetedVersion = new Version(7, 10, 8858);
        public static bool IsTargetedVersion { get { return Environment.OSVersion.Version >= TargetedVersion; } }


        public static void UpdateIconTile(
            string widecontent1,
            string widecontent2,
            string widecontent3,
            int count)
        {
            if (IsTargetedVersion)
            {
                Type tileDataType = Type.GetType("Microsoft.Phone.Shell.IconicTileData, Microsoft.Phone");
                ShellTileData IconicTileData = (ShellTileData)tileDataType.GetConstructor(new Type[] { }).Invoke(null); // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                var UpdateTileData = tileDataType.GetConstructor(new Type[] { }).Invoke(null);
                SetProperty(IconicTileData, "Title", "Zoik it!");
                SetProperty(IconicTileData, "Count", count);
                SetProperty(IconicTileData, "BackgroundColor", Color.FromArgb(0, 255, 175, 4));//Color.FromArgb(255, 200, 10, 30)
                SetProperty(IconicTileData, "IconImage", new Uri("/Images/HatTileIcon.png", UriKind.RelativeOrAbsolute));
                SetProperty(IconicTileData, "SmallIconImage", new Uri("/Images/HatSmallTileIcon.png", UriKind.RelativeOrAbsolute));
                SetProperty(IconicTileData, "WideContent1", widecontent1);
                SetProperty(IconicTileData, "WideContent2", widecontent2);
                SetProperty(IconicTileData, "WideContent3", widecontent3);
                Create(new Uri("/Dashboard.xaml", UriKind.RelativeOrAbsolute), IconicTileData, true);

            }

        }

        public static void Create(Uri uri, ShellTileData tiledata, bool usewide)
        {
            Type shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");
            MethodInfo createmethod = shellTileType.GetMethod("Create", new[] { typeof(Uri), typeof(ShellTileData), typeof(bool) });
            createmethod.Invoke(null, new object[] { uri, tiledata, usewide });
        }



        public static void UpdateFlipTile(
            string title,
            string backTitle,
            string backContent,
            string wideBackContent,
            int count,
            Uri tileId,
            Uri smallBackgroundImage,
            Uri backgroundImage,
            Uri backBackgroundImage,
            Uri wideBackgroundImage,
            Uri wideBackBackgroundImage)
        {
            if (IsTargetedVersion)
            {
                // Get the new FlipTileData type.
                Type flipTileDataType = Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");

                // Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.
                Type shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");

                // Loop through any existing Tiles that are pinned to Start.
                foreach (var tileToUpdate in ShellTile.ActiveTiles)
                {
                    // Look for a match based on the Tile's NavigationUri (tileId).
                    if (tileToUpdate.NavigationUri.ToString() == tileId.ToString())
                    {
                        // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                        var UpdateTileData = flipTileDataType.GetConstructor(new Type[] { }).Invoke(null);

                        // Set the properties. 
                        SetProperty(UpdateTileData, "Title", title);
                        SetProperty(UpdateTileData, "Count", count);
                        SetProperty(UpdateTileData, "BackTitle", backTitle);
                        SetProperty(UpdateTileData, "BackContent", backContent);
                        SetProperty(UpdateTileData, "SmallBackgroundImage", smallBackgroundImage);
                        SetProperty(UpdateTileData, "BackgroundImage", backgroundImage);
                        SetProperty(UpdateTileData, "BackBackgroundImage", backBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackgroundImage", wideBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackBackgroundImage", wideBackBackgroundImage);
                        SetProperty(UpdateTileData, "WideBackContent", wideBackContent);

                        // Invoke the new version of ShellTile.Update.
                        shellTileType.GetMethod("Update").Invoke(tileToUpdate, new Object[] { UpdateTileData });
                        break;
                    }
                }
            }

        }

        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }
    }
}
