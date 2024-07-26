using System.Collections.Generic;
using System;

public struct FloatInt : IComparable<FloatInt>
{ 

    public float val;
    public int index;

    public int CompareTo (FloatInt other)
    {

        if (val > other.val) return 1;
        if (val < other.val) return -1; 
        return 0;

    }
}

public struct FloatIntSorter : IComparer<FloatInt>
{

    public int Compare (FloatInt x, FloatInt y)
    {
        
        if (x.val > y.val) return 1;
        if (x.val < y.val) return -1;
        return 0;

    }

}
