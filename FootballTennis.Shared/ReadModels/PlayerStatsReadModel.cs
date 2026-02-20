namespace FootballTennis.Shared.ReadModels
{
    public sealed class PlayerStatsReadModel
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; } = default!;
        public int TournamentsCount { get; set; }
        public int FirstCount { get; set; }
        public int SecondCount { get; set; }
        public int ThirdCount { get; set; }
    }
}
