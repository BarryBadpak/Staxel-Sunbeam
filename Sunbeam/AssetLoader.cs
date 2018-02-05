using Plukit.Base;
using Staxel;
using System;
using System.IO;

namespace Sunbeam
{
    public static class AssetLoader
    {
        /// <summary>
        /// Absolute path to the content directory
        /// </summary>
        public static string ContentDirectory { get; private set; }

        /// <summary>
        /// Absolute path to the mod directory
        /// </summary>
        public static string ModDirectory { get; private set; }

        /// <summary>
        /// Initialize the asset loader
        /// </summary>
        public static void Initialize(string modIdentifier)
        {
            string RootPath = "." + Path.DirectorySeparatorChar + "content_root.txt";
            string RelativeContentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), Properties.ContentRoot));
            AssetLoader.SetContentDirectory(RelativeContentDirectory);
            AssetLoader.SetModDirectory(modIdentifier);
        }

        /// <summary>
        /// Retrieve a file's content as text from the mod directory
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static string ReadFileContent(string assetPath)
        {
            assetPath = Path.GetFullPath(Path.Combine(AssetLoader.ModDirectory, assetPath));
            return AtomicFile.ReadText(assetPath, true);
        }

        /// <summary>
        /// Retrieves the absolute path to the content directory based on the relative path
        /// </summary>
        /// <param name="RelativeContentDirectory"></param>
        private static void SetContentDirectory(string RelativeContentDirectory)
        {
            string AbsoluteContentDirectory = Path.GetFullPath(RelativeContentDirectory);
            if (!AbsoluteContentDirectory.EndsWith("\\") && !AbsoluteContentDirectory.EndsWith("/"))
            {
                AbsoluteContentDirectory += "/";
            }

            AssetLoader.ContentDirectory = Path.GetFullPath(AbsoluteContentDirectory);
        }

        /// <summary>
        /// Retrieves the absolute path to the mod directory based on the mod identifier
        /// </summary>
        /// <param name="modIdentifier"></param>
        private static void SetModDirectory(string modIdentifier)
        {
            string path = Path.Combine(AssetLoader.ContentDirectory, "mods", modIdentifier);
            AssetLoader.ModDirectory = Path.GetFullPath(path);
        }
    }
}
