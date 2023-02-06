using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    public static int[] RandomIntExcluding(int toExclude)
    {
        return Enumerable.Range(1, 20).Where(a => a != toExclude).ToArray();
    }
}
