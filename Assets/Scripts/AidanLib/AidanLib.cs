using System.Collections.Generic;
using UnityEngine;

public static class AidanLib
{
    public static Queue<T> Shuffle<T>(this Queue<T> queue)
    {
        //return if empty
        if(queue.Count == 0) return queue;

        T[] array = queue.ToArray();
        int n = array.Length;
        
        // Standard Fisher-Yates algorithm
        for (int i = n-1; i > 0; i--)
        {
            // Only pick from unshuffled portion (more efficient)
            int randomIndex = Random.Range(0, i+1);
            
            // Swap
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        
        return new Queue<T>(array);
    }
    
}
