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
		if (board[x, y] != 0){
            GD.Print("Case déjà occupée !");
        }
		else{
			board[x, y] = currentPlayer;							// placer le jeton du joueur courant
			turn++;                                                 // incrémenter le compteur de tours
                                                                    
            if (CheckVictory(x, y)){                                // vérifier victoire
                GD.Print("Victoire du joueur " + currentPlayer + " !");
                return; // fin du jeu
            }
            else if (turn == SIZE * SIZE){
                GD.Print("Match nul !");
                return; // fin du jeu
            }

            currentPlayer = (currentPlayer == 1) ? 2 : 1;           // changer de joueur
        }
    }

    bool CheckVictory(int x, int y)
    {
        int player = board[x, y];

        // vérifie horizontal, vertical, diagonale et anti-diagonale
        if (CheckDirection(player, x, y, 1, 0)) return true;   // horizontal
        if (CheckDirection(player, x, y, 0, 1)) return true;   // vertical
        if (CheckDirection(player, x, y, 1, 1)) return true;   // diagonale \
        if (CheckDirection(player, x, y, 1, -1)) return true;  // diagonale /

        return false;
    }

    bool CheckDirection(int player, int x, int y, int dx, int dy)
    {
        int count = 1; // commence par la case jouée

        count += Count(player, x, y, dx, dy);   // sens positif (+1)
        count += Count(player, x, y, -dx, -dy); // sens négatif (-1)

        return count >= SIZE;
    }

    int Count(int player, int x, int y, int dx, int dy)
    {
        int count = 0;
        int cx = x + dx;
        int cy = y + dy;

        while (cx >= 0 && cx < SIZE && cy >= 0 && cy < SIZE && board[cx, cy] == player)
        {
            count++;
            cx += dx;
            cy += dy;
        }

        return count;
    }



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
