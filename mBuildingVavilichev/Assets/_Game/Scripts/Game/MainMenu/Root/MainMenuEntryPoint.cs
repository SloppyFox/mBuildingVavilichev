using R3;
using UnityEngine;

namespace SloppyFox
{
	public class MainMenuEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;

		public Observable<MainMenuExitParams> Run(UIRootView uiRoot, MainMenuEnterParams enterParams)
		{
			var uiScene = Instantiate(_sceneUIRootPrefab);
			uiRoot.AttachSceneUI(uiScene.gameObject);

			var exitSceneSignalSubject = new Subject<Unit>();
			uiScene.Bind(exitSceneSignalSubject);

			var gameplayEnterParams = new GameplayEnterParams();
			var mainMenuExitParams =  new MainMenuExitParams(gameplayEnterParams);
			var exitToGameplaySceneSignal = exitSceneSignalSubject.Select(_=>mainMenuExitParams);

			Debug.Log($"MAIN MENU ENTRY POINT: Run main menu scene. Rsults: {enterParams?.Result}");

			return exitToGameplaySceneSignal;
		}
	}
}
