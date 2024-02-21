using System;

public static class ArrayExtensions
{
    private static Random random = new Random();

    public static void Shuffle<T>(this T[] array) {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--) {
            int j = random.Next(0, i + 1);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}