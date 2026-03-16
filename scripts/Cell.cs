using Godot;

public partial class Cell : ColorRect
{
	public int X;
	public int Y;

	Grid grid;

	public override void _Ready()
	{
		// Utilise GetParent() et cast en Grid
		grid = GetParent() as Grid;
		if (grid == null)
		{
			GD.PrintErr("Impossible de trouver le nœud Grid !");
			return;
		}

		// Extraction des coordonnées depuis le nom
		string[] parts = Name.ToString().Split('_');
		if (parts.Length == 3 && int.TryParse(parts[1], out int x) && int.TryParse(parts[2], out int y))
		{
			X = x;
			Y = y;
		}
		else
		{
			GD.PrintErr($"Nom de cellule invalide : {Name}");
		}
	}


	public override void _GuiInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				grid.OnCellClicked(X, Y);
			}
		}
	}

	public Color Reset()
	{
		return Color = new Color(0.69f, 0.69f, 0.69f); // Couleur grise de départ
	}
}
