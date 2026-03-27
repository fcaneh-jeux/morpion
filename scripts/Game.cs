using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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

	/// vérifie si la partie est un match nul, c'est à dire si le plateau est plein et qu'aucun joueur n'a gagné, pour détecter les matchs nuls
	public bool IsDraw()
	{
		return IsBoardFull() && !CheckWinForPlayer(1) && !CheckWinForPlayer(2);
	}

	/// vérifie si le plateau est plein, c'est à dire s'il n'y a plus de cases vides, pour détecter les matchs nuls
	private bool IsBoardFull()
	{
		for (int x = 0; x < SIZE; x++)
		{
			for (int y = 0; y < SIZE; y++)
			{
				if (board[x, y] == 0)
					return false;
			}
		}
		return true;
	}

	/// vérifie si le joueur qui vient de jouer a aligné 3 pions dans une ligne, une colonne ou une diagonale, en comptant le nombre de pions alignés dans les 4 directions possibles à partir du dernier coup joué
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

		// essayer de créer une fourchette pour l'IA
		movePlayed = FindForkMove(currentPlayer);
		if (movePlayed.Item1 != -1)
			return movePlayed;

		// essayer de contrer la fourchette de l'adversaire
		movePlayed = FindForkMove(opponent);
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
	private int CountWinningMoves(int player, int testX, int testY)
	{
		// simuler le coup de l'IA sur la case testée
		board[testX, testY] = player;

		int count = 0;

        int count = 0;

        // Appel de SimulateWinningMove avec un callback qui incrémente le compteur à chaque coup gagnant trouvé
        SimulateWinningMove(player, (x, y) =>
        {
            count++;
        });

        board[testX, testY] = 0;

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

				int winningMoves = CountWinningMoves(player, x, y);

				// si ce coup crée une situation où l'IA a 2 coups gagnants possibles, c'est un coup de fourchette et l'IA doit le jouer
				if (winningMoves >= 2)
				{
					return (x, y);
				}
			}
		}

		return (-1, -1);
	}
	
	///	minimax : algorithme récursif de recherche de coup optimal pour l'IA, en simulant tous les coups possibles et en évaluant le résultat de chaque coup pour choisir le meilleur	
	private int Minimax(bool isMaximizing)
	{
		// 🔚 cas de fin
		if (CheckWinForPlayer(2)) return +1; // IA gagne
		if (CheckWinForPlayer(1)) return -1; // humain gagne
		//if (IsDraw()) return 0;
		if (IsBoardFull()) return 0; // match nul

		if (isMaximizing)
		{
			int bestScore = int.MinValue;

			for (int x = 0; x < SIZE; x++)
			{
				for (int y = 0; y < SIZE; y++)
				{
					if (board[x, y] != 0)
						continue;

					board[x, y] = 2; // IA joue

					int score = Minimax(false);

					board[x, y] = 0; // rollback

					bestScore = Math.Max(bestScore, score);
				}
			}

			return bestScore;
		}
		else
		{
			int bestScore = int.MaxValue;

			for (int x = 0; x < SIZE; x++)
			{
				for (int y = 0; y < SIZE; y++)
				{
					if (board[x, y] != 0)
						continue;

					board[x, y] = 1; // humain joue

					int score = Minimax(true);

					board[x, y] = 0; // rollback

					bestScore = Math.Min(bestScore, score);
				}
			}

			return bestScore;
		}
	}

	private bool CheckWinForPlayer(int player)
	{
		// lignes
		for (int x = 0; x < SIZE; x++)
		{
			if (board[x, 0] == player &&
				board[x, 1] == player &&
				board[x, 2] == player)
				return true;
		}

		// colonnes
		for (int y = 0; y < SIZE; y++)
		{
			if (board[0, y] == player &&
				board[1, y] == player &&
				board[2, y] == player)
				return true;
		}

		// diagonales
		if (board[0, 0] == player &&
			board[1, 1] == player &&
			board[2, 2] == player)
			return true;

		if (board[0, 2] == player &&
			board[1, 1] == player &&
			board[2, 0] == player)
			return true;

		return false;
	}

	public (int, int) GetBestMoveMinimax()
	{
		int bestScore = int.MinValue;
		List<(int, int)> bestMoves = new();

		for (int x = 0; x < SIZE; x++)
		{
			for (int y = 0; y < SIZE; y++)
			{
				if (board[x, y] != 0)
					continue;

				board[x, y] = 2; // IA joue

				int score = Minimax(false);

				board[x, y] = 0; // rollback

				if (score > bestScore)
				{
					bestScore = score;
					bestMoves.Clear();
					bestMoves.Add((x, y));
				}else if (score == bestScore)
				{
					bestMoves.Add((x, y));
				}
			}
		}
		var random = new Random();
		return bestMoves[random.Next(bestMoves.Count)];
	}

}
