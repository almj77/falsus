namespace Falsus.UnitTests.Helpers
{
    using System.Reflection;
    using Newtonsoft.Json;

    public static class ResourceReader
    {
        public static T ReadContentsFromFile<T>(string fileName) where T : class
        {
            var assembly = Assembly.GetCallingAssembly();
            var resourceName = string.Concat("Falsus.UnitTests.Datasets.", fileName);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonFile = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(jsonFile);
            }
        }
    }
}
