namespace TurnBasedRPG.Model
{
    public struct CurrentMax
    {
        public int Current;
        public int Max;

        public CurrentMax(int value)
        {
            Current = value;
            Max = value;
        }

        public float Percent => (float) Current / Max;
        
        public string PercentText => $"{Current}/{Max}";

        public void SetMax() => Current = Max;
    }
}