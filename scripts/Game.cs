using Godot;
using System;

public partial class Game : Node2D
{
	/// creation de la grille	
	 // taille de la grille
	const int SIZE = 3;

	// 0 = vide, 1 = joueur1, 2 = joueur2
	int[,] board = new int[SIZE, SIZE];

	// joueur courant
	int currentPlayer = 1;

	// compteur de tours
	int turn = 0;

	
	public override void _Ready()
	{
        GD.Print("Jeu démarré");
    }

	public void Play(int x, int y)
	{
		// vérifier si la case est vide
		if (board[x, y] != 0)
		{
            GD.Print("Case déjà occupée !");
        }
		else {
				board[x, y] = currentPlayer;							// placer le jeton du joueur courant
				turn++;													// incrémenter le compteur de tours
                currentPlayer = (currentPlayer == 1) ? 2 : 1;           // changer de joueur
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
