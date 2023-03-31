using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.VisualStyles;
using Malom.Model;
using Malom.Persistence;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace Malom.WinForms;

public partial class GameForm : Form
{
    private IMalomDataAccess _dataAccess = null!;
    private MalomGameModel _model = null!;
    private RoundButton[] _outerButtons = new RoundButton[8];
    private RoundButton[] _middleButtons = new RoundButton[8];
    private RoundButton[] _innerButtons = new RoundButton[8];
    private Button skipButton = new();
    private Label _turn = new();
    private bool _isMill = false;
    private bool _isMove = false;
    private int _moveIndex = 0;


    public GameForm()
    {
        InitializeComponent();

        _dataAccess = new MalomFileDataAccess();

        _model = new MalomGameModel(_dataAccess);
        _model.GameOver += Game_GameOver;
        _model.GameAdvanced += GameAdvanced;
        _model.OnMill += GameMill;
        _model.Move += MovePiece;

        toolStripSaveGame.Click += SaveGameClick;
        toolStripLoadGame.Click += LoadGameClick;

        GenerateTable();


        //_turn.Width = 500;
        _turn.BorderStyle = BorderStyle.None;
        _turn.Font = new Font(FontFamily.GenericSansSerif, 15);
        _turn.Text = "It's " + (_model.Table.CurrentPlayer == Values.Player1 ? "red's" : "blue's") + " turn";
        _turn.Location = new Point(460, 600);
        _turn.Width = 300;
        _turn.Height = 50;
        _turn.TextAlign = ContentAlignment.BottomCenter;
        _turn.AutoSize = true;


        Controls.Add(_turn);
    }

    private void Game_GameOver(object? sender, MalomEventArgs e)
    {
        if (e.Winner == Values.Empty)
            MessageBox.Show("It's a draw!", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show("The winner is " + (e.Winner == Values.Player1 ? "red" : "blue"), "Winner",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        ;
        foreach (var button in _innerButtons) button.Enabled = false;
        foreach (var button in _outerButtons) button.Enabled = false;
        foreach (var button in _middleButtons) button.Enabled = false;

        toolStripSaveGame.Enabled = false;
    }

    private void GameMill(object? sender, MalomEventArgs e)
    {
        _turn.Text = "You have a mill! Click on a piece you want to remove.";
        _isMill = true;
    }

    private void GameAdvanced(object? sender, MalomEventArgs e)
    {
        if (e.GameState == 0)
            _turn.Text = "It's " + (_model.Table.CurrentPlayer == Values.Player1 ? "red's" : "blue's") + " turn";
        else
            _turn.Text = "Move a " + (_model.Table.CurrentPlayer == Values.Player1 ? "red" : "blue") +
                         " piece to an adjacent tile";
    }
    private void NewGameClick(object? sender, EventArgs e)
    {
        toolStripSaveGame.Enabled = true;
        _model.NewGame();
        GenerateTable();
        _isMill = false;
        _isMove = false;
    }

    private async void SaveGameClick(object? sender, EventArgs e)
    {
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
            try
            {
                await _model.SaveGameAsync(saveFileDialog.FileName);
            }
            catch (MalomDataException)
            {
                MessageBox.Show(
                    "Save was unsuccessful!" + Environment.NewLine +
                    "Either the path is invalid or the folder cannot be written.", "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
    }

    private async void LoadGameClick(object? sender, EventArgs e)
    {
        if (openFileDialog.ShowDialog() == DialogResult.OK)
            try
            {
                _isMill = false;
                _isMove = false;
                await _model.LoadGameAsync(openFileDialog.FileName);
                GenerateTable();
                RedrawTable();
                toolStripSaveGame.Enabled = true;
                if (!_model.CanMove())
                {
                    skipButton.Enabled = true;
                    skipButton.Visible = true;
                }
            }
            catch (MalomDataException)
            {
                MessageBox.Show(
                    "Loading of the game was unsuccessfull!" + Environment.NewLine +
                    "The path is incorrect or the extension of the file is wrong.",
                    "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _model.NewGame();
                toolStripSaveGame.Enabled = true;
            }
    }

    private void RedrawTable()
    {
        for (var i = 0; i < _model.Table.FieldOuterValues.Length; i++)
            if (_model.Table.FieldOuterValues[i] == Values.Player1)
            {
                _outerButtons[i].BackColor = Color.Red;
                // _outerButtons[i].Text = "afafa";
                _outerButtons[i].Update();
            }
            else if (_model.Table.FieldOuterValues[i] == Values.Player2)
            {
                _outerButtons[i].BackColor = Color.Blue;
                _outerButtons[i].Update();
            }
            else
            {
                _outerButtons[i].BackColor = Color.Black;
                _outerButtons[i].Update();
            }

        for (var i = 0; i < _model.Table.FieldMiddleValues.Length; i++)
            if (_model.Table.FieldMiddleValues[i] == Values.Player1)
                _middleButtons[i].BackColor = Color.Red;
            else if (_model.Table.FieldMiddleValues[i] == Values.Player2)
                _middleButtons[i].BackColor = Color.Blue;
            else
                _middleButtons[i].BackColor = Color.Black;

        for (var i = 0; i < _model.Table.FieldInnerValues.Length; i++)
            if (_model.Table.FieldInnerValues[i] == Values.Player1)
                _innerButtons[i].BackColor = Color.Red;
            else if (_model.Table.FieldInnerValues[i] == Values.Player2)
                _innerButtons[i].BackColor = Color.Blue;
            else
                _innerButtons[i].BackColor = Color.Black;
    }


    private void SkipButtonClick(object? sender, MouseEventArgs e)
    {
        _model.Skip();
        skipButton.Enabled = false;
        skipButton.Visible = false;
    }

    private void ButtonClick(object? sender, MouseEventArgs e)
    {
        if (sender is RoundButton button)
        {
            if (_isMill)
            {
                RemovePiece(button);
                return;
            }

            try
            {
                if (_model.GameState == 0)
                {
                    _model.Step(button.TabIndex);
                    if (_model.Table.CurrentPlayer == Values.Player2)
                        button.BackColor = Color.Red;
                    else
                        button.BackColor = Color.Blue;
                }
                else
                {
                    if (!_isMove)
                    {
                        _moveIndex = button.TabIndex;
                        _isMove = true;
                    }
                    else
                    {
                        _model.MovePiece(_moveIndex, button.TabIndex);
                        _isMove = false;
                    }

                    if (!_model.CanMove())
                    {
                        skipButton.Enabled = true;
                        skipButton.Visible = true;
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _isMove = false;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void RemovePiece(RoundButton button)
    {
        if (!_isMill) return;

        try
        {
            if (!_model.Mill(button.TabIndex))
            {
                MessageBox.Show("You clicked on an invalid piece! Please try again!", "Invalid step",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        catch (ArgumentOutOfRangeException ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        button.BackColor = Color.Black;
        _isMill = false;
    }

    public void MovePiece(object? sender, MoveEventArgs e)
    {
        if (e.FromMoveIndex <= 7)
            _outerButtons[e.FromMoveIndex].BackColor = Color.Black;
        else if (e.FromMoveIndex <= 15)
            _middleButtons[e.FromMoveIndex - 8].BackColor = Color.Black;
        else
            _innerButtons[e.FromMoveIndex - 16].BackColor = Color.Black;
        Color playerColor;
        if (e.CurrentPlayer == Values.Player1)
            playerColor = Color.Red;
        else
            playerColor = Color.Blue;


        if (e.ToMoveIndex <= 7)
            _outerButtons[e.ToMoveIndex].BackColor = playerColor;
        else if (e.ToMoveIndex <= 15)
            _middleButtons[e.ToMoveIndex - 8].BackColor = playerColor;
        else
            _innerButtons[e.ToMoveIndex - 16].BackColor = playerColor;
    }


    private void SetButtonSpecs(ref RoundButton button, int counter, int x, int y)
    {
        if (button != null) Controls.Remove(button);

        button = new RoundButton();
        button.Location = new Point(x, y);
        button.Size = new Size(40, 40);
        button.BackColor = Color.Black;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 0;
        button.MouseClick += ButtonClick;
        button.TabIndex = counter;

        Controls.Add(button);
    }
    private void GenerateTable()
    {
        if (skipButton != null) Controls.Remove(skipButton);
        skipButton = new Button();
        skipButton.Location = new Point(100, 290);
        skipButton.Text = "Skip turn";
        skipButton.Visible = false;
        skipButton.Enabled = false;
        skipButton.AutoSize = true;
        skipButton.MouseClick += SkipButtonClick;
        skipButton.Font = new Font(FontFamily.GenericSansSerif, 15);
        Controls.Add(skipButton);
        Paint += PaintTable;
        var counter = 0;
        for (var i = 0; i < 8; i++)
        {
            var y = 45;
            var x = 235 + i * 246;

            if (i >= 4 && i < 7)
            {
                y = 540;
                x = 727 - (i - 4) * 246;
            }
            else if (i == 3)
            {
                y = 290;
                x = 727;
            }
            else if (i == 7)
            {
                y = 290;
                x = 235;
            }

            SetButtonSpecs(ref _outerButtons[i], counter, x, y);
            counter++;
        }

        for (var i = 0; i < 8; i++)
        {
            var y = 103;
            var x = 290 + i * 190;

            if (i >= 4 && i < 7)
            {
                y = 480;
                x = 670 - (i - 4) * 190;
            }
            else if (i == 3)
            {
                y = 290;
                x = 670;
            }
            else if (i == 7)
            {
                y = 290;
                x = 292;
            }

            SetButtonSpecs(ref _middleButtons[i], counter, x, y);
            counter++;
        }

        for (var i = 0; i < 8; i++)
        {
            var y = 160;
            var x = 350 + i * 130;

            if (i >= 4 && i < 7)
            {
                y = 430;
                x = 610 - (i - 4) * 130;
            }
            else if (i == 3)
            {
                y = 290;
                x = 613;
            }
            else if (i == 7)
            {
                y = 290;
                x = 349;
            }

            SetButtonSpecs(ref _innerButtons[i], counter, x, y);
            counter++;
        }
    }

    private void PaintTable(object? sender, PaintEventArgs e)
    {
        var blackColor = Color.Black;
        var blackPenRec = new Pen(blackColor);
        blackPenRec.Width = 5;
        e.Graphics.DrawRectangle(blackPenRec, 250, 60, 500, 500);
        e.Graphics.DrawRectangle(blackPenRec, 308, 120, 380, 380);
        e.Graphics.DrawRectangle(blackPenRec, 365, 180, 270, 270);

        var blackPenLine = new Pen(blackColor);
        blackPenLine.Width = 5;
        e.Graphics.DrawLine(blackPenLine, 500, 60, 500, 180);
        e.Graphics.DrawLine(blackPenLine, 500, 560, 500, 450);
        e.Graphics.DrawLine(blackPenLine, 250, 310, 365, 310);
        e.Graphics.DrawLine(blackPenLine, 750, 310, 635, 310);
    }
}