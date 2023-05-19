using DoMulti.Core.Objects;

namespace DoMulti.Core.System;

public class Organizer
{
    private List<Character> allChar;
    private Form1 form;
    private VersionFinder vFinder;
    private CharacterFinder cFinder;
    private string version;

    public Organizer(Form1 f)
    {
        allChar = new List<Character>();
        form = f;
        vFinder = new VersionFinder();
        version = vFinder.GetVersion();

        Update();

        Console.WriteLine("Organizer : "+form);
    }

    public void Update()
    {
        allChar = new List<Character>();
        foreach (var VARIABLE in form.GetData())
        {
            if(VARIABLE.isActivated)
                allChar.Add(new Character(VARIABLE.Name, VARIABLE.Initiative, version));
        }
        Organize();
    }

    private void Organize()
    {
        allChar.Sort((character1, character2) => character1.Initiative.CompareTo(character2.Initiative));
        allChar.Reverse();
    }

    public List<Character> AllChar => allChar;
    public string Version => version;
}