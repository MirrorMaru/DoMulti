using DoMulti.Core.System;

namespace DoMulti.Core.Objects;

public class TextBoxModificationCheck
{
    public TextBox NumberBox { get; set; }
    private Organizer _organizer;

    public TextBoxModificationCheck(TextBox numberBox, Organizer o)
    {
        NumberBox = numberBox;
        _organizer = o;

        NumberBox.TextChanged += (s, e) =>
        {
            UpdateCall();
        };
    }

    private void UpdateCall()
    {
        _organizer.Update();
    }
}