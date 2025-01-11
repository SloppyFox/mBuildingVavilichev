using R3;
using UnityEngine;

namespace SloppyFox
{
	public class GameplayEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;

		public Observable<GameplayExitParams> Run(UIRootView uiRoot)
		{
			var uiScene = Instantiate(_sceneUIRootPrefab);
			uiRoot.AttachSceneUI(uiScene.gameObject);

			var exitSceneSignalSubject = new Subject<Unit>();
			uiScene.Bind(exitSceneSignalSubject);

			var mainMenuEnterParams = new MainMenuEnterParams("MainMenuEnterParams");
			var exitParams = new GameplayExitParams(mainMenuEnterParams);
			var exitToMainMenuSceneSignal = exitSceneSignalSubject.Select(_ => exitParams);

			return exitToMainMenuSceneSignal;
		}
	}
}
