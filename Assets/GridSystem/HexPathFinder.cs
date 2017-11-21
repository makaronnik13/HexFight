using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexPathFinder : MonoBehaviour {

    private Path<Cell> FindPath(Cell start, Cell destination)
    //where Cell : IHasNeighbours
    {
        //set of already checked nodes
        var closed = new HashSet<Cell>();
        //queued nodes in open set
        var queue = new PriorityQueue<float, Path<Cell>> ();
        queue.Enqueue(0, new Path<Cell>(start));

        while (!queue.IsEmpty)
        {
            
            var path = queue.Dequeue();

            

            if (closed.Contains(path.LastStep))
                continue;
            if (path.LastStep.Equals(destination))
                return path;

            closed.Add(path.LastStep);

            foreach (Cell n in path.LastStep.Neighbours)
            {
                float d = calcDistance(path.LastStep, n);
                //new step added without modifying current path
                var newPath = path.AddStep(n, d);
                queue.Enqueue(newPath.TotalCost + calcEstimate(n), newPath);
            }
        }

        return null;
    }

    public List<Cell> GetPath(Cell start, Cell aim)
    {
        //We assume that the distance between any two adjacent tiles is 1
        //If you want to have some mountains, rivers, dirt roads or something else which might slow down the player you should replace the function with something that suits better your needs

        return FindPath(start, aim).ToList();
    }

	/*
	public List<Cell> GetAwaliableCells(Cell center, int range)
    {
		List<Cell> awaliableCells = new List<Cell> ();

		foreach(Cell c in GetComponent<HexField>().Cells)
		{
            Path<Cell> path = FindPath(center, c);
            if (path!=null && path.Count()<=range+1)
			{
				awaliableCells.Add (c);
			}
		}

		return awaliableCells;
    }
	*/
	public List<Cell> GetAwaliableCells(Cell start, int movement){
		
		HashSet<Cell> visited = new HashSet<Cell>();
		visited.Add(start);
		List<Cell> fringes = new List<Cell>();
		fringes.Add (start);

		if(movement == 0)
		{
			return fringes;
		}

		for (int i = 1 ; i <= movement; i++)
		{
			
			foreach(Cell c in visited)
			{
				for (int j = 0 ; j < 6; j++)
				{
					Cell n = GetComponent<HexField> ().Neighbour (c.coord, j);
					if(n!=null && n.Passable)
					{
						fringes.Add (n);
					}
				}
			}
			visited.UnionWith (fringes);
		}
		return visited.ToList();
	}

    private float calcEstimate(Cell cell)
    {
        return 1;
    }

    private float calcDistance(Cell tile, Cell destTile)
    {
        //Formula used here can be found in Chris Schetter's article
        float deltaX = Mathf.Abs(destTile.coord.x - tile.coord.x);
        float deltaY = Mathf.Abs(destTile.coord.y - tile.coord.y);
        float z1 = -(tile.coord.x + tile.coord.y);
        float z2 = -(destTile.coord.x + destTile.coord.y);
        float deltaZ = Mathf.Abs(z2 - z1);
        return Mathf.Max(deltaX, deltaY, deltaZ);
    }
}
