namespace SchoolManagement.StartUp
{
    public static class GlobalSetup
    {
        public static void Setup()
        {
            // Map TRIGGER_BY column to TriggerBy in class
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}
