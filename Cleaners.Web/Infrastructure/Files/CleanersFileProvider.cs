using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Cleaners.Web.Infrastructure.Files
{
    public class CleanersFileProvider : ICleanersFileProvider
    {
        public CleanersFileProvider(string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                throw new ArgumentException($"Parameter {nameof(rootPath)} cannot be null or empty.");
            }

            // Check if rootPath is absolute or map it with ContentRootPath if relative ?

            BasePath = rootPath;
            FileProvider = new PhysicalFileProvider(rootPath);
        }

        public IFileProvider FileProvider { get; }

        public string BasePath { get; }

        public void CreateDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path value cannot be null or empty.", nameof(path));
            }

            Directory.CreateDirectory(MapPath(path));
        }

        public bool IsRootPath(string path)
        {
            return Path.IsPathRooted(path);
        }

        public string MapPath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                // Return base path or throw exception?
                return BasePath;
            }

            if (IsRootPath(relativePath))
            {
                throw new ArgumentException($"Path {relativePath} cannot be rooted.");
            }

            // Replace ~/ for virtual paths
            var formattedPath = relativePath.Replace("~/", "").Replace("/", "\\");

            return Path.Combine(BasePath, formattedPath);
        }
    }
}