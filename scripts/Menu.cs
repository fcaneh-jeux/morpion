using Godot;

public partial class Menu : Control
{
	private CheckButton modeCheckButton;
	private OptionButton difficultyOptionButton;
	private HBoxContainer difficultyLine;
	private Button startButton;
	private Button quitButton;

	public override void _Ready()
	{
		//GD.Print("MENU READY");

		modeCheckButton = GetNode<CheckButton>("CenterContainer/VBoxContainer/HBoxContainer/Mode-CheckButton");
		difficultyLine = GetNode<HBoxContainer>("CenterContainer/VBoxContainer/HBoxContainer2");
		difficultyOptionButton = GetNode<OptionButton>("CenterContainer/VBoxContainer/HBoxContainer2/Difficulty-OptionButton");
		startButton = GetNode<Button>("CenterContainer/VBoxContainer/StartButton");

		startButton.MouseFilter = MouseFilterEnum.Stop;

		startButton.Pressed += OnStartPressed;
	
		//GD.Print($"Start node trouvé : {startButton.Name}");
		//GD.Print($"Start visible : {startButton.Visible}");
		//GD.Print($"Start disabled : {startButton.Disabled}");
		
		quitButton = GetNode<Button>("CenterContainer/VBoxContainer/QuitButton");

		difficultyOptionButton.Clear();
		difficultyOptionButton.AddItem("Dur");
		difficultyOptionButton.AddItem("Ingagnable");
		difficultyOptionButton.Select(0);

		difficultyLine.Visible = false;

		modeCheckButton.Toggled += OnModeToggled;
		startButton.Pressed += OnStartPressed;
		quitButton.Pressed += OnQuitPressed;
	}

	private void OnModeToggled(bool isPressed)
	{
		difficultyLine.Visible = isPressed;
	}

	private void OnStartPressed()
	{
		GD.Print("START PRESSED");

		Game.SelectedMode = modeCheckButton.ButtonPressed
			? Game.GameMode.PlayerVsAI
			: Game.GameMode.PlayerVsPlayer;
		GD.Print($"Selected mode : {Game.SelectedMode}");

		int selectedDifficulty = difficultyOptionButton.Selected;
		if (selectedDifficulty < 0)
			selectedDifficulty = 0; // Default to "Dur" if no selection

		Game.SelectedDifficulty = difficultyOptionButton.Selected == 0
			? Game.AIDifficulty.Hard
			: Game.AIDifficulty.VeryHard;

		string path = "res://scenes/Main.tscn";
		GD.Print($"Scene existe ? {ResourceLoader.Exists(path)}");

		GetTree().ChangeSceneToFile(path);
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}
