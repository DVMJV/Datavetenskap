using System.Collections.Generic;

public class SquareCellPriorityQueue
{
    List<SquareCell> queue;

    int count;
    int minimum = int.MaxValue;
    public int Count
    {
        get { return count; }
    }

    public SquareCellPriorityQueue()
    {
        queue = new List<SquareCell>();
    }


    public void Enqueue(SquareCell cell)
    {
        count++;
        int priority = cell.SearchPriority;
        if (priority < minimum)
            minimum = priority;

        while(priority >= queue.Count)
        {
            queue.Add(null);
        }
        cell.NextWithSamePriority = queue[priority];
        queue[priority] = cell;
    }


    public SquareCell Dequeue()
    {
        count--;
        for(; minimum < queue.Count; minimum++)
        {
            SquareCell cell = queue[minimum];
            if(cell != null)
            {
                queue[minimum] = cell.NextWithSamePriority;
                return cell;    
            }
        }
        return null;
    }

    public void Clear()
    {
        count = 0;
        minimum = int.MaxValue;
        queue.Clear();
    }


    public void Change(SquareCell cell, int oldPriority)
    {
        SquareCell current = queue[oldPriority];
        SquareCell next = current.NextWithSamePriority;

        if(current == cell)
        {
            queue[oldPriority] = next;
        }
        else
        {
            while(next != cell)
            {
                current = next;
                next = current.NextWithSamePriority;
            }
            current.NextWithSamePriority = cell.NextWithSamePriority;
        }
        Enqueue(cell);
        count -= 1;

    }

}
