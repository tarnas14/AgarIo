namespace AgarIo.Contract.AdminCommands
{
    public class GetSnapshotAdminCommandResponseDto : AdminCommandResponseDto
    {
        public WorldSettingsDto WorldSettings { get; set; }

        public int WorldSize { get; set; }

        public bool IsGameRunning { get; set; }
    }
}