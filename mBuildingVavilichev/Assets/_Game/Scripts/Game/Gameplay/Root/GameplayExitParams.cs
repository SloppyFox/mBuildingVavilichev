namespace SloppyFox
{
	public class GameplayExitParams
	{
		public MainMenuEnterParams MainMenuEnterParams { get; private set; }

		public GameplayExitParams(MainMenuEnterParams mainMenuEnterParams)
		{
			MainMenuEnterParams = mainMenuEnterParams;
		}
	}
}
