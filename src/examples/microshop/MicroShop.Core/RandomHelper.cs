namespace MicroShop.Core;

public static class RandomHelper
{
    public static Action<T> RandomizeAction<T>(this Random random,
        int percentRatio, Action<T> action1, Action<T> action2)
    {
        int randomFactor = random.Next(1, 100);
        if (randomFactor > percentRatio)
            return action1;
        return action2;
    }
}