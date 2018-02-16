using Plukit.Base;
using Staxel;
using System.IO;

namespace Sunbeam.Core
{
    public class AssetLoader
    {
        /// <summary>
        /// Absolute path to the content directory
        /// </summary>
        public string ContentDirectory { get; private set; }

        /// <summary>
        /// Absolute path to the mod directory
        /// </summary>
        public string ModDirectory { get; private set; }

        /// <summary>
        /// Initialize the asset loader
        /// </summary>
        public AssetLoader(string modIdentifier)
        {
            string RootPath = "." + Path.DirectorySeparatorChar + "content_root.txt";
            string RelativeContentDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), Properties.ContentRoot));
            this.SetContentDirectory(RelativeContentDirectory);
            this.SetModDirectory(modIdentifier);
        }

        /// <summary>
        /// Retrieve a file's content as text from the mod directory
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public string ReadFileContent(string assetPath)
        {
            assetPath = Path.GetFullPath(Path.Combine(this.ModDirectory, assetPath));
            return AtomicFile.ReadText(assetPath, true);
        }

        /// <summary>
        /// Retrieves the absolute path to the content directory based on the relative path
        /// </summary>
        /// <param name="RelativeContentDirectory"></param>
        private void SetContentDirectory(string RelativeContentDirectory)
        {
            string AbsoluteContentDirectory = Path.GetFullPath(RelativeContentDirectory);
            if (!AbsoluteContentDirectory.EndsWith("\\") && !AbsoluteContentDirectory.EndsWith("/"))
            {
                AbsoluteContentDirectory += "/";
            }

            this.ContentDirectory = Path.GetFullPath(AbsoluteContentDirectory);
        }

        /// <summary>
        /// Retrieves the absolute path to the mod directory based on the mod identifier
        /// </summary>
        /// <param name="modIdentifier"></param>
        private void SetModDirectory(string modIdentifier)
        {
            string path = Path.Combine(this.ContentDirectory, "mods", modIdentifier);
            this.ModDirectory = Path.GetFullPath(path);
        }
    }
}
