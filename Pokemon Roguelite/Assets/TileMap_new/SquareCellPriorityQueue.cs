using System.Collections.Generic;

public class SquareCellPriorityQueue
{
    List<SquareCell> list = new List<SquareCell>();

    int count = 0;
    public int Count { get { return count; } }
    int minimum = int.MaxValue;


    public void Enqueue(SquareCell cell) 
    {
        count++;
        //  int priority = cell.SearchPriority;
        int priority = 0;

        if (priority < minimum)
            minimum = priority;

        while (priority >= list.Count)
            list.Add(null);

        cell.NextWithSamePriority = list[priority];
        list[priority] = cell;
    }

    public SquareCell Dequeue() 
    {
        count--;
        for (; minimum < list.Count; minimum++)
        {
            SquareCell cell = list[minimum];
            if (cell != null)
            {
                list[minimum] = cell.NextWithSamePriority;
                return cell;
            }
        }
        return null;
    }

    public void Change(SquareCell cell, int oldPriority) 
    {
        SquareCell current = list[oldPriority];
        SquareCell next = current.NextWithSamePriority;
     
        if (current == cell)
            list[oldPriority] = next;
        else
        {
            while (next != cell)
            {
                current = next;
                next = current.NextWithSamePriority;
            }
            current.NextWithSamePriority = cell.NextWithSamePriority;
        }
        Enqueue(cell);
        count -= 1;
    }

    public void Clear()
    { 
        list.Clear();
        count = 0;
        minimum = int.MaxValue;
    }

}
