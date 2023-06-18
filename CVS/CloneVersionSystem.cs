using System;
using System.Linq;
using System.Collections.Generic;

namespace Clones.CVS;

public class CloneVersionSystem : ICloneVersionSystem
{
    public List<Clone> Clones = new List<Clone>() { new Clone() };
    public string Execute(string query)
    {
        var a = query.Split(' ');
        if (a[0].CompareTo("learn") == 0)
        {
            Learn(int.Parse(a[1]) - 1, int.Parse(a[2]));
        }

        else if (a[0].CompareTo("rollback") == 0)
        {
            Clones[int.Parse(a[1]) - 1].Skip.Push(Clones[int.Parse(a[1]) - 1].Possibilities.Pop());
        }

        else if (a[0].CompareTo("relearn") == 0)
        {
            Clones[int.Parse(a[1]) - 1].Possibilities.Push(Clones[int.Parse(a[1]) - 1].Skip.Pop());
        }

        else if (a[0].CompareTo("clone") == 0)
        {
            MakeAnother(int.Parse(a[1]) - 1);
        }

        else
        {
            return Check(int.Parse(a[1]) - 1);
        }

        return null;
    }

    public void MakeAnother(int number)
    {
        Clones.Add(new Clone());
        Clones.Last().Possibilities = (LimitedSizeStack)Clones[number].Possibilities.Clone();
        Clones.Last().Skip = (LimitedSizeStack)Clones[number].Skip.Clone();
    }

    public void Learn(int number, int programm)
    {
        Clones[number].Possibilities.Push(programm);
        Clones[number].Skip = new LimitedSizeStack();
    }

    public string Check(int number)
    {
        if (Clones[number].Possibilities.Count == 0) return "basic";
        return Clones[number].Possibilities.tail.Value.ToString();
    }
}
public class Clone : ICloneable
{
    public LimitedSizeStack Possibilities = new LimitedSizeStack();
    public LimitedSizeStack Skip = new LimitedSizeStack();

    object ICloneable.Clone()
    {
        return MemberwiseClone();
    }
}
public class LimitedSizeStack : ICloneable
{
    public Element head { get; set; }

    public Element tail { get; set; }

    public void Push(int item)
    {
        if (head == null && tail == null)
        {
            head = tail = new Element { Value = item };
            Count++;
        }

        else
        {
            var now = new Element { Value = item, Prev = tail };
            tail.Next = now;
            tail = now;
            Count++;
        }
    }

    public int Pop()
    {
        if (head == null) throw new Exception();
        var result = tail.Value;
        if (Count == 1)
        {
            head = null;
            tail = null;
            Count--;
        }

        else
        {
            tail = tail.Prev;
            Count--;
        }

        return result;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }

    public int Count { get; private set; }
}

public class Element
{
    public int Value;
    public Element Next;
    public Element Prev;
}