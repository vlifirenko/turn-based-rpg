namespace TurnBasedRPG.Model
{
    public struct CurrentMax
    {
        public int Current;
        public int Max;

        public float Percent => (float) Current / Max;
        
        public string PercentText => $"{Current}/{Max}";
    }
}