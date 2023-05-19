using DoMulti.Core.System;

namespace DoMulti.Core.Objects;

public class CheckBoxModificationCheck
{
    public CheckBox CheckBox { get; }
    private Organizer _organizer;

    public CheckBoxModificationCheck(CheckBox checkBox, Organizer o)
    {
        CheckBox = checkBox;
        _organizer = o;

        CheckBox.CheckedChanged += (s, e) =>
        {
            UpdateCall();
        };
    }

    private void UpdateCall()
    {
        _organizer.Update();
    }
}