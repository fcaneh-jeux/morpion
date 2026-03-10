using Godot;
using System;

public partial class Game : Node2D
{
    // Taille de la grille
    public const int SIZE = 3;

    // 0 = vide, 1 = joueur1, 2 = joueur2
    public int[,] board = new int[SIZE, SIZE];

    // Joueur courant
    public int currentPlayer = 1;

    // Compteur de tours
    public int turn = 0;

    /// <summary>
    /// Essaie de jouer un coup à (x, y).
    /// Retourne true si le coup est valide, false si la case est déjà occupée.
    /// </summary>
    public bool Play(int x, int y)
    {
        if (board[x, y] != 0)
            return false;

        board[x, y] = currentPlayer;
        turn++;
        return true;
    }

    /// <summary>
    /// Passe au joueur suivant.
    /// </summary>
    public void NextPlayer()
    {
        currentPlayer = (currentPlayer == 1) ? 2 : 1;
    }

    /// <summary>
    /// Vérifie si le dernier coup à (x,y) a fait gagner le joueur courant.
    /// </summary>
    public bool CheckVictory(int x, int y)
    {
        int player = board[x, y];

        // Vérifie horizontal, vertical, diagonale principale et anti-diagonale
        if (CheckDirection(player, x, y, 1, 0)) return true;   // horizontal
        if (CheckDirection(player, x, y, 0, 1)) return true;   // vertical
        if (CheckDirection(player, x, y, 1, 1)) return true;   // diagonale \
        if (CheckDirection(player, x, y, 1, -1)) return true;  // diagonale /

        return false;
    }

    /// <summary>
    /// Vérifie une direction (dx, dy) et son opposé pour savoir si SIZE cases sont alignées.
    /// </summary>
    private bool CheckDirection(int player, int x, int y, int dx, int dy)
    {
        int count = 1; // commence par la case jouée

        count += Count(player, x, y, dx, dy);   // sens positif
        count += Count(player, x, y, -dx, -dy); // sens négatif

        return count >= SIZE;
    }

    /// <summary>
    /// Compte le nombre de cases consécutives d’un joueur dans une direction donnée.
    /// </summary>
    private int Count(int player, int x, int y, int dx, int dy)
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

    /// <summary>
    /// Vérifie si le plateau est plein sans vainqueur.
    /// </summary>
    public bool IsDraw()
    {
        return turn >= SIZE * SIZE;
    }

    /// <summary>
    /// Réinitialise la grille et les variables pour une nouvelle partie.
    /// </summary>
    public void Reset()
    {
        board = new int[SIZE, SIZE];
        currentPlayer = 1;
        turn = 0;
    }
}
