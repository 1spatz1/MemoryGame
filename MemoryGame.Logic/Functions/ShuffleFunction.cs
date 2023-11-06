namespace MemoryGame.Logic;

public static class ShuffleFunction
{
    private static Random random = new();  
    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = random.Next(n + 1);  
            T value = list[k];  // make temporary item
            list[k] = list[n];  // switches item
            list[n] = value;  // switches item
        }  
    }
}