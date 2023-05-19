using System.Runtime.InteropServices;
using DoMulti.Core.Objects;
using DoMulti.Core.System;

namespace DoMulti;

public partial class Form1 : Form
{   
    //TODO : Detecter automatiquement le nombre de perso, mettre une case pour l'activer ou non

    [DllImport("user32.dll")]
    public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
    
    [DllImport("user32.dll")]
    public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private readonly Panel _panel;

    private readonly List<TextBox> _textBoxes = new();
    private readonly List<TextBox> _numberBoxes = new();
    private readonly List<CheckBox> _checkBoxes = new();

    private Organizer _organizer;
    private static WindowSystem _windowSystem;
    
    private const int HOTKEY_ID_NEXT = 1;
    private const int HOTKEY_ID_PREV = 2;

    private CharacterFinder cFinder;
    private List<string> detectedCharacters;

    public Form1()
    {
        var label = new Label
        {
            Text = "Nombre de personnage : ",
            Location = new Point(10, 10),
            AutoSize = true
        };

        var comboBox = new ComboBox
        {
            Location = new Point(label.Right + 40, 7),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        for (int i = 2; i <= 8; i++)
        {
            comboBox.Items.Add(i);
        }
        comboBox.SelectedIndexChanged += (s, e) => CreateTextBoxes((int)comboBox.SelectedItem);
        comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;

        _panel = new Panel
        {
            Location = new Point(10, comboBox.Bottom + 10),
            AutoSize = true
        };

        Controls.Add(label);
        Controls.Add(comboBox);
        Controls.Add(_panel);
        AutoSize = true;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;

        _organizer = new Organizer(this);
        
        cFinder = new CharacterFinder(_organizer);
        detectedCharacters = cFinder.GetCharacters();
        
        _windowSystem = new WindowSystem(_organizer);

        RegisterGlobalHotkey();
    }

    public void RegisterGlobalHotkey()
    {
        uint fsModifiers = 0;
        uint vk = (uint)Keys.F23;

        RegisterHotKey(Handle, HOTKEY_ID_NEXT, fsModifiers, vk);
        
        fsModifiers = 0;
        vk = (uint)Keys.F24;

        RegisterHotKey(Handle, HOTKEY_ID_PREV, fsModifiers, vk);
    }

    public void UnregisterGlobalHotkey()
    {
        UnregisterHotKey(Handle, HOTKEY_ID_NEXT);
        UnregisterHotKey(Handle, HOTKEY_ID_PREV);
    }

    protected override void WndProc(ref Message m)
    {
        const int WM_HOTKEY = 0x0312;
        base.WndProc(ref m);

        if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID_NEXT)
        {
            _windowSystem.Next();
        }
        else if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == HOTKEY_ID_PREV)
        {
            _windowSystem.Previous();
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        UnregisterGlobalHotkey();
        base.OnFormClosing(e);
    }

    private void CreateTextBoxes(int count)
    {
        _panel.Controls.Clear();
        _textBoxes.Clear();
        _numberBoxes.Clear();
        _checkBoxes.Clear();

        Label checkBoxTitle = new Label
        {
            Text = "✓",
            Location = new Point(0, 0),
            AutoSize = true
        };
        
        Label textBoxTitle = new Label
        {
            Text = "Nom Personnage",
            Location = new Point(30, 0),
            AutoSize = true
        };

        Label numberBoxTitle = new Label
        {
            Text = "Initiative",
            Location = new Point(140, 0),
            AutoSize = true
        };

        _panel.Controls.Add(checkBoxTitle);
        _panel.Controls.Add(textBoxTitle);
        _panel.Controls.Add(numberBoxTitle);

        for (int i = 0; i < count; i++)
        {
            CheckBox checkBox = new CheckBox()
            {
                Location = new Point(0, (i + 1) * 30),
                Width = 20
            };
            _checkBoxes.Add(checkBox);
            
            TextBox textBox = new TextBox
            {
                Location = new Point(checkBox.Right + 10, (i+1) * 30),
                Width = 100,
                ReadOnly = true,
            };
            try
            {
                textBox.Text = detectedCharacters[i];
            }
            catch (ArgumentOutOfRangeException)
            {
                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            _textBoxes.Add(textBox);
            
            TextBox numberBox = new TextBox
            {
                Location = new Point(textBox.Right + 10, (i+1) * 30),
                Width = 100,
                Text = "0"
            };
            _numberBoxes.Add(numberBox);

            new TextBoxModificationCheck(numberBox, _organizer);
            new CheckBoxModificationCheck(checkBox, _organizer);
            
            numberBox.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
            _panel.Controls.Add(checkBox);
            _panel.Controls.Add(textBox);
            _panel.Controls.Add(numberBox);
        }

        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
    }

    public List<(bool isActivated, string Name, int Initiative)> GetData()
    {
        var result = new List<(bool, string, int)>();
        for (int i = 0; i < _textBoxes.Count; i++)
        {
            try
            {
                bool isActivated = _checkBoxes[i].Checked;
                string name = _textBoxes[i].Text;
                int initiative = Convert.ToInt32(_numberBoxes[i].Text);
                result.Add((isActivated, name, initiative));
            }
            catch (FormatException e)
            {
                bool isActivated = _checkBoxes[i].Checked;
                string name = _textBoxes[i].Text;
                int initiative = 0;
                result.Add((isActivated, name, initiative));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return result;
    }

    private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        _organizer.Update();
    }
}