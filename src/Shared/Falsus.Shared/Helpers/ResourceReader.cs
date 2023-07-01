namespace Falsus.Shared.Helpers
{
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;

    /// <summary>
    /// Helper class to assist in reading DLL embedded resources.
    /// </summary>
    public static class ResourceReader
    {
        /// <summary>
        /// Reads the contents from the specified resouce file as JSON and deserializes as 
        /// an instance of the specified type parameter.
        /// </summary>
        /// <typeparam name="T">The type to deserilize the JSON.</typeparam>
        /// <param name="resourcePath">The path for the embedded resource.</param>
        /// <returns>The contents of the specified resource file as 
        /// an instnace of the specified type parameter.</returns>
        public static T ReadContentsFromFile<T>(string resourcePath) where T : class
        {
            var assembly = Assembly.GetCallingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonFile = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(jsonFile);
            }
        }
    }
}
