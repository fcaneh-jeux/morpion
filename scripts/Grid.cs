using Godot;
using static Game;

public partial class Grid : Control
{
	public PackedScene CellScene;
	public Game game = new Game();
 	private Label turnLabel; // Variable pour le label
	private Button resetButton; // Variable pour le bouton de réinitialisation


	public override void _Ready()
	{
// Charger la scène modèle Cell.tscn
		CellScene = GD.Load<PackedScene>("res://scenes/Cell.tscn");
		if (CellScene == null)
		{
			GD.Print("Erreur : CellScene introuvable !");
			return;
		}

		#region création des cellules
		float cellSize = 64; // correspond à Rect.Size de Cell.tscn
		GD.Print("GRID READY");
		// Génération automatique des cellules
		for (int x = 0; x < Game.SIZE; x++)
		{
			for (int y = 0; y < Game.SIZE; y++)
			{
				var cell = (Cell)CellScene.Instantiate();
				cell.Position = new Vector2(x * cellSize, y * cellSize);
				cell.Size = new Vector2(cellSize, cellSize);
				cell.Name = $"Cell_{x}_{y}";
				AddChild(cell);
			}
		}
		#endregion création des cellules

		#region génration des lignes de démarcation 
		// Ajouter des lignes verticales
		for (int x = 1; x < Game.SIZE; x++)
		{
			var line = new Line2D();
			line.Width = 2; // Épaisseur de la ligne
			line.AddPoint(new Vector2(0, 0)); // Point de départ (en haut)
			line.AddPoint(new Vector2(0, Game.SIZE * cellSize)); // Point d'arrivée (en bas)
			line.DefaultColor = new Color(0, 0, 0); // Noir
			line.Position = new Vector2(x * cellSize, 0); // Positionne la ligne verticale
			AddChild(line);
		}

		// Ajouter des lignes horizontales
		for (int y = 1; y < Game.SIZE; y++)
		{
			var line = new Line2D();
			line.Width = 2; // Épaisseur de la ligne
			line.AddPoint(new Vector2(0, 0)); // Point de départ (à gauche)
			line.AddPoint(new Vector2(Game.SIZE * cellSize, 0)); // Point d'arrivée (à droite)
			line.DefaultColor = new Color(0, 0, 0); // Noir
			line.Position = new Vector2(0, y * cellSize); // Positionne la ligne horizontale
			AddChild(line);
		}
		#endregion génration des lignes de démarcation
		
		#region affichage joueur en cours
		turnLabel = GetNode<Label>("TurnLabel");
		if (turnLabel == null)
		{
			GD.PrintErr("Erreur : TurnLabel introuvable !");
		}
		else
		{
			UpdateTurnLabel(); // Met à jour le label au démarrage
		}
		#endregion affichage joueur en cours

		#region button de réinitialisation
		resetButton = GetNode<Button>("ResetButton");
		resetButton.Pressed += OnResetPressed;
		#endregion button de réinitialisation

		game.Mode = GameMode.PlayerVsAI;

		GD.Print("Grille générée automatiquement !");
	}

	// Méthode appelée par Cell quand on clique dessus
	public void OnCellClicked(int x, int y)
	{
		if (game.IsGameOver)
			return;

		bool isAITurn = (game.Mode == GameMode.PlayerVsAI && game.currentPlayer == 2);

		if (!game.Play(x, y))
			return;

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
		UpdateTurnLabel();

		// Si c'est le tour de l'IA, elle joue automatiquement
		if (!isAITurn && game.Mode == GameMode.PlayerVsAI && game.currentPlayer == 2)
		{
			GD.Print("🤖 Tour IA");

			var (aiX, aiY) = game.GetAIMove();

			if (aiX != -1)
			{
				OnCellClicked(aiX, aiY);
			}
		}
	}
	
	private void UpdateCellVisual(int x, int y)
	{
		var cell = GetNode<Cell>($"Cell_{x}_{y}");
		if (game.board[x, y] == 1)
			cell.Color = new Color(1, 0, 0); // rouge joueur 1
		else if (game.board[x, y] == 2)
			cell.Color = new Color(0, 0, 1); // bleu joueur 2
	}
	
	private void UpdateTurnLabel()
	{	
		if (turnLabel != null)
		{
			turnLabel.Text = $"Tour du joueur : {game.currentPlayer}";
		}
	}
 
	private void OnResetPressed()
	{
		game.Reset(); // Réinitialise le jeu
		foreach (var child in GetChildren())
		{
			if (child is Cell cell)
			{
				cell.Color = cell.Reset();
			}
		}
		UpdateTurnLabel(); // Met à jour le label du tour
	}
}
