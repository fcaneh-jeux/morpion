using Godot;
using System;

public partial class Grid : Node2D
{
    // Instance du jeu
    public Game game;

    public override void _Ready()
    {
        // Initialisation de la logique du jeu
        game = new Game();

        GD.Print("Grille prête, jeu démarré !");
    }

    /// <summary>
    /// Méthode connectée au signal input_event de chaque Area2D / ColorRect
    /// </summary>
    /// <param name="viewport"></param>
    /// <param name="@event"></param>
    /// <param name="shape_idx"></param>
    /// <param name="X">Coordonnée X exportée de la case</param>
    /// <param name="Y">Coordonnée Y exportée de la case</param>
    public void _OnCellInput(Node viewport, InputEvent @event, int shape_idx, int X, int Y)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            // Essayer de jouer le coup
            if (game.Play(X, Y))
            {
                // Met à jour l'affichage
                UpdateCellVisual(X, Y);

                // Vérifie victoire
                if (game.CheckVictory(X, Y))
                {
                    GD.Print($"Victoire du joueur {game.currentPlayer} !");
                    // Optionnel : bloquer les clics ou proposer reset
                }
                // Vérifie match nul
                else if (game.IsDraw())
                {
                    GD.Print("Match nul !");
                }
                else
                {
                    // Passer au joueur suivant
                    game.NextPlayer();
                }
            }
            else
            {
                GD.Print("Case déjà occupée !");
            }
        }
    }

    /// <summary>
    /// Met à jour la couleur de la case après un coup
    /// </summary>
    void UpdateCellVisual(int x, int y)
    {
        // Récupère la ColorRect correspondante
        var cell = GetNode<ColorRect>($"Cell_{x}_{y}");

        if (game.board[x, y] == 1)
            cell.Color = new Color(1, 0, 0);   // rouge = joueur 1
        else if (game.board[x, y] == 2)
            cell.Color = new Color(0, 0, 1);   // bleu = joueur 2
    }

    /// <summary>
    /// Permet de réinitialiser le plateau et les couleurs
    /// </summary>
    public void ResetGrid()
    {
        game.Reset();

        for (int x = 0; x < Game.SIZE; x++)
        {
            for (int y = 0; y < Game.SIZE; y++)
            {
                var cell = GetNode<ColorRect>($"Cell_{x}_{y}");
                cell.Color = new Color(0.8f, 0.8f, 0.8f); // couleur neutre
            }
        }
    }
}

