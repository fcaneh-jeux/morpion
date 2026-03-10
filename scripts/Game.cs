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
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
