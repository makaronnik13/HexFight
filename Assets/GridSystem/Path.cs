using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Path<Cell> : IEnumerable<Cell>
{
    public Cell LastStep { get; private set; }
    public Path<Cell> PreviousSteps { get; private set; }
    public float TotalCost { get; private set; }
    private Path(Cell lastStep, Path<Cell> previousSteps, float totalCost)
    {
        LastStep = lastStep;
        PreviousSteps = previousSteps;
        TotalCost = totalCost;
    }
    public Path(Cell start) : this(start, null, 0) { }
    public Path<Cell> AddStep(Cell step, float stepCost)
    {
        return new Path<Cell>(step, this, TotalCost + stepCost);
    }
    public IEnumerator<Cell> GetEnumerator()
    {
        for (Path<Cell> p = this; p != null; p = p.PreviousSteps)
            yield return p.LastStep;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}