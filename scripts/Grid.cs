using Godot;

public partial class Grid : Node2D
{
    Game game = new Game();

    public override void _Ready()
    {
        GD.Print("Jeu lancé");
    }

    public void OnCellClicked(int x, int y)
    {
        if (!game.Play(x, y))
        {
            GD.Print("Case occupée");
            return;
        }

        UpdateCellVisual(x, y);

        if (game.CheckVictory(x, y))
        {
            GD.Print($"Victoire joueur {game.currentPlayer}");
            return;
        }

        if (game.IsDraw())
        {
            GD.Print("Match nul");
            return;
        }

        game.NextPlayer();
    }

    void UpdateCellVisual(int x, int y)
    {
        var cell = GetNode<ColorRect>($"Cell_{x}_{y}");

        if (game.board[x, y] == 1)
            cell.Color = new Color(1, 0, 0);

        if (game.board[x, y] == 2)
            cell.Color = new Color(0, 0, 1);
    }
}