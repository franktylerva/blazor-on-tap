namespace WasmHosted.Shared
{
    public class DotnetServiceBinding
    {
        private static Dictionary<string, string> _bindingsDictionary;

        public Dictionary<string, string> GetBindings(string type)
        {
            try
            {
                string environmentVariable = Environment.GetEnvironmentVariable("SERVICE_BINDING_ROOT");
                DotnetServiceBinding._bindingsDictionary = new Dictionary<string, string>();
                DotnetServiceBinding.ProcessDirectoryTree(environmentVariable, type);
                return DotnetServiceBinding._bindingsDictionary;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void ProcessDirectoryTree(string directory, string type)
        {
            if (File.Exists(directory + "/type") && File.ReadAllText(directory + "/type") == type)
            {
                foreach (string file in Directory.GetFiles(directory))
                    DotnetServiceBinding.GetFileContents(file);
            }
            foreach (string directory1 in Directory.GetDirectories(directory))
                DotnetServiceBinding.ProcessDirectoryTree(directory1, type);
        }

        private static void GetFileContents(string filename)
        {
            string str = File.ReadAllText(filename);
            string fileName = Path.GetFileName(filename);
            // Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (DotnetServiceBinding._bindingsDictionary.ContainsKey(fileName))
                DotnetServiceBinding._bindingsDictionary["key"] = str;
            else
                DotnetServiceBinding._bindingsDictionary.Add(fileName, str);
        }
    }
}
