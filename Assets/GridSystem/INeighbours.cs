using System.Collections;
using System.Collections.Generic;
//interface that should be implemented by grid nodes used in E. Lippert's generic path finding implementation
public interface IHasNeighbours<T>
{
    IEnumerable<T> Neighbours { get; }
}