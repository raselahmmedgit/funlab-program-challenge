namespace FunlabProgramChallenge.Utility
{
    public class AppDbConfig
    {
        public static string Name = "AppDbConfig";
        public bool IsMSSqlDatabase { get; set; }
        public bool IsMySqlDatabase { get; set; }
        public bool IsDatabaseCreated { get; set; }
        public bool IsMasterDataInserted { get; set; }
    }
}
