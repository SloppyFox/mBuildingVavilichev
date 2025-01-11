namespace SloppyFox
{
	public class MainMenuExitParams
	{
		public SceneEnterParams TargetSceneEnterParams { get; private set; }

		public MainMenuExitParams(SceneEnterParams targetSceneEnterParams)
		{ 
			TargetSceneEnterParams = targetSceneEnterParams;
		}
	}
}
