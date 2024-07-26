using System.Collections.Generic;

public struct FloatArraySorter : IComparer<float>
{

    public int Compare (float x, float y)
    {

        if (x > y) return 1;
        if (x < y) return -1;
        return 0;

    }

}
