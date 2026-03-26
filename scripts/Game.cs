using Godot;
using System;
using System.ComponentModel;

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

	private static readonly (int, int)[] FAVORITE_MOVES =
	{
		(1,1),
		(0,0), (0,2), (2,0), (2,2)
	};

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
		(int, int) movePlayed;
		
		// essayer de gagner
		movePlayed = TestIAMove(currentPlayer);
		if (movePlayed.Item1 != - 1)
			return movePlayed;

		// essayer de contrer l'adversaire
        var opponent = currentPlayer == 1 ? 2 : 1;

        movePlayed = TestIAMove(opponent);
        if (movePlayed.Item1 != -1)
            return movePlayed;

        // essayer de créer une fourchette
        movePlayed = FindForkMove(currentPlayer);
		if (movePlayed.Item1 != -1)
			return movePlayed;

        // essayer de jouer une case favorite
        movePlayed = IAFavoriteMoves();
		if (movePlayed.Item1 != -1)
			return movePlayed;

        // coup random sur la première case vide trouvée
        for (int x = 0; x < SIZE; x++)
		{
			for (int y = 0; y < SIZE; y++)
			{
				if (board[x, y] == 0)
					return (x, y);
			}
		}

		return (-1, -1);
	}

    private (int, int) TestIAMove(int playerOnTest)
    {
        (int, int) winningMove = (-1, -1);

        // Appel de SimulateWinningMove avec un callback qui capture winningMove
        SimulateWinningMove(playerOnTest, (x, y) =>
        {
			if (winningMove.Item1 == -1)
            {
                winningMove = (x, y);
            }            
        });
        return winningMove;
    }


    private (int,int) IAFavoriteMoves()
	{

		foreach (var (x,y) in FAVORITE_MOVES)
		{
			if (board[x,y] == 0)
				return (x,y);
		}
		return (-1, -1);
    }

    private void SimulateWinningMove(int player, Action<int, int> onWinningMove)
    {
        for (int x = 0; x < SIZE; x++)
        {
            for (int y = 0; y < SIZE; y++)
            {
                if (board[x, y] != 0)
                    continue;
                board[x, y] = player;

                bool isWinning = CheckVictory(x, y);

                board[x, y] = 0;

                if (isWinning)
                {
                    onWinningMove(x, y);
                }
            }
        }
    }

	/// préparation de fourchettes pour l'IA, comptage du nombre de coups gagnants possibles 
	private int CountWinningMoves(int player)
	{
		int count = 0;

		// Appel de SimulateWinningMove avec un callback qui incrémente le compteur à chaque coup gagnant trouvé
		SimulateWinningMove(player, (x, y) =>
		{
			count++;
		});

		return count;
	}

	/// détection de fouchette : si l'IA peut créer une situation où elle a deux coups gagnants possibles au prochain tour, elle doit jouer ce coup
	private (int, int) FindForkMove(int player)
    {
		// Parcours de toutes les cases du plateau pour simuler un coup de l'IA et compter le nombre de coups gagnants possibles après ce coup
		for (int x = 0; x < SIZE; x++)
		{
			for (int y = 0; y < SIZE; y++)
			{
				if (board[x, y] != 0)
					continue;

				// simule le coup
				board[x, y] = player;

				int winningMoves = CountWinningMoves(player);

				// rollback
				board[x, y] = 0;
				// si ce coup crée une situation où l'IA a 2 coups gagnants possibles, c'est un coup de fourchette et l'IA doit le jouer
                if (winningMoves >= 2)
                {
                    return (x, y);
                }
            }
        }

        return (-1, -1);
    }


}
