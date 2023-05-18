using DoMulti.Core.System;

namespace DoMulti.Core.Objects;

public class TextBoxNumberBoxPair
{
    public TextBox TextBox { get; set; }
    public TextBox NumberBox { get; set; }
    private bool textBoxModified;
    private bool numberBoxModified;
    private Organizer _organizer;

    public TextBoxNumberBoxPair(TextBox textBox, TextBox numberBox, Organizer o)
    {
        TextBox = textBox;
        NumberBox = numberBox;
        _organizer = o;

        TextBox.TextChanged += (s, e) =>
        {
            textBoxModified = true;
            if (NumberBox.Text != "") numberBoxModified = true;
            CheckBothModified();
        };

        NumberBox.TextChanged += (s, e) =>
        {
            numberBoxModified = true;
            if (TextBox.Text != "") textBoxModified = true;
            CheckBothModified();
        };
    }

    private void CheckBothModified()
    {
        if (textBoxModified && numberBoxModified)
        {
            UpdateCall();
            textBoxModified = false;
            numberBoxModified = false;
        }
    }

    private void UpdateCall()
    {
        _organizer.Update();
    }
}