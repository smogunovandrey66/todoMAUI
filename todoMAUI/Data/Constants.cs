namespace todoMAUI.Data
{
    public static class Constants
    {
        private const string DatabaseFilename = "todos.db";


        public static string DatabasePath =>
            $"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";
    }
}