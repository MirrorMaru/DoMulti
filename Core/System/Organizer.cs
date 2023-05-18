using DoMulti.Core.Objects;

namespace DoMulti.Core.System;

public class Organizer
{
    private List<Character> allChar;
    private Form1 form;

    public Organizer(Form1 f)
    {
        allChar = new List<Character>();
        form = f;
        Update();

        Console.WriteLine("Organizer : "+form);
    }

    public void Add()
    {
        if (form.GetData().Count > 0)
        {
            foreach (var VARIABLE in form.GetData())
            {
                if (allChar.Find(obj => obj.Name == VARIABLE.Key) == null)
                {
                    allChar.Add(new Character(VARIABLE.Key, VARIABLE.Value));
                }
            }

            Organize();
        }
    }

    public void Remove()
    {
        if (form.GetData().Count > 0)
        {
            foreach (var VARIABLE in allChar.ToList())
            {
                if (!form.GetData().ContainsKey(VARIABLE.Name))
                {
                    allChar.Remove(VARIABLE);
                }
            }
        }
        
        Organize();
    }

    public void Update()
    {
        Add();
        Remove();
        Organize();
    }

    private void Organize()
    {
        allChar.Sort((character1, character2) => character1.Initiative.CompareTo(character2.Initiative));
        allChar.Reverse();
    }

    public List<Character> AllChar => allChar;
}