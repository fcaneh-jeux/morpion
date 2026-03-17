using System;

public class Game
{
	public const int SIZE = 3;

	// 0 = vide
	// 1 = joueur 1
	// 2 = joueur 2
	public int[,] board = new int[SIZE, SIZE];

	public int currentPlayer = 1;
	public int turn = 0;
	private bool isGameOver = false;
	public bool IsGameOver => isGameOver;

	static readonly (int, int)[] DIRECTIONS =
	{
		(1,0),
		(0,1),
		(1,1),
		(1,-1)
	};

	public enum GameMode
	{
		PlayerVsPlayer,
		PlayerVsAI
	}

	public GameMode Mode = GameMode.PlayerVsPlayer;

	public bool Play(int x, int y)
	{
		// vérifier si la partie est terminée pour éviter de jouer après la fin du jeu
		if (isGameOver) 
			return false;

		// vérifier si la case est déjà occupée
		if (board[x, y] != 0)
			return false;

		// jouer le coup
		board[x, y] = currentPlayer;
		turn++;

		// vérifier victoire
		if (CheckVictory(x, y))
		{
			isGameOver = true;
			return true;
		}

		// vérifier match nul
		if (turn == SIZE * SIZE)
		{
			isGameOver = true;
			return true;
		}

		return true;
	}

	public void NextPlayer()
	{
		currentPlayer = currentPlayer == 1 ? 2 : 1;
	}

	public bool IsDraw()
	{
		return turn == SIZE * SIZE;
	}

	public bool CheckVictory(int x, int y)
	{
		int player = board[x, y];

		foreach (var (dx, dy) in DIRECTIONS)
		{
			int count = 1;

			count += Count(player, x, y, dx, dy);
			count += Count(player, x, y, -dx, -dy);

			if (count >= SIZE)
				return true;
		}

		return false;
	}

	int Count(int player, int x, int y, int dx, int dy)
	{
		int count = 0;

		int cx = x + dx;
		int cy = y + dy;

		while (
			cx >= 0 && cx < SIZE &&
			cy >= 0 && cy < SIZE &&
			board[cx, cy] == player
		)
		{
			count++;

			cx += dx;
			cy += dy;
		}

		return count;
	}

	public void Reset()
	{
		board = new int[SIZE, SIZE];
		currentPlayer = 1;
		turn = 0;
		isGameOver = false;
	}

	public (int, int) GetAIMove()
	{
		for (int x = 0; x < SIZE; x++)
		{
			for (int y = 0; y < SIZE; y++)
			{
				if (board[x, y] == 0)
					return (x, y);
			}
		}

		return (-1, -1); // sécurité en cas de plateau plein ou partie terminée
	}
}
