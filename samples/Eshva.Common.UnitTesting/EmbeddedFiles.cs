#region Usings

using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

#endregion


namespace Eshva.Common.UnitTesting
{
    public static class EmbeddedFiles
    {
        public static void SaveToFolder(string targetFolder, params string[] embeddedFileNames)
        {
            var executingAssembly = Assembly.GetCallingAssembly();
            var embeddedFileProvider = new ManifestEmbeddedFileProvider(executingAssembly);

            foreach (var embeddedFileName in embeddedFileNames)
            {
                using var reader = embeddedFileProvider.GetFileInfo(embeddedFileName).CreateReadStream();
                using var writer = new StreamWriter(Path.Combine(targetFolder, Path.GetFileName(embeddedFileName)));
                reader.CopyTo(writer.BaseStream);
            }
        }
    }
}
