namespace TilemapTools.Tiled
{
    public struct AnimationFrame
    {
        public AnimationFrame(int id, int duration)
        {
            Id = id;
            Duration = duration;
        }

        public int Id { get; set; }

        public int Duration { get; set; }
    }
}