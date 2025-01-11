using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SloppyFox
{
	public class GameEntryPoint
	{
		private static GameEntryPoint _instance;

		private Coroutines _coroutines;
		private UIRootView _uiRootView;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void AutostartGame()
		{
			_instance = new GameEntryPoint();
			_instance.RunGame();
		}

		private GameEntryPoint()
		{
			CreateUtils();
			CreateUIRootView();
		}

		private void CreateUtils()
		{
			_coroutines = new GameObject("[Corutines]").AddComponent<Coroutines>();
			Object.DontDestroyOnLoad(_coroutines.gameObject);
		}

		private void CreateUIRootView()
		{
			var prefabUIRootView = Resources.Load<UIRootView>("UIRoot");
			_uiRootView = Object.Instantiate(prefabUIRootView);
			Object.DontDestroyOnLoad(_uiRootView.gameObject);
		}

		private void RunGame()
		{
#if UNITY_EDITOR
			string sceneName = SceneManager.GetActiveScene().name;

			if (sceneName == Scenes.GAMEPLAY)
			{
				_coroutines.StartCoroutine(LoadAndStartGameplay());
				return;
			}
			
			if (sceneName == Scenes.MAIN_MENU)
			{
				_coroutines.StartCoroutine(LoadAndStartMainMenu());
				return;
			}

			if (sceneName != Scenes.BOOT)
				return;
#endif

			_coroutines.StartCoroutine(LoadAndStartGameplay());
		}

		private IEnumerator LoadAndStartGameplay()
		{
			_uiRootView.ShowLoadingScreen();

			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.GAMEPLAY);

			yield return new WaitForSeconds(2);

			var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
			sceneEntryPoint.Run(_uiRootView);

			// !!!
			sceneEntryPoint.GoToMainMenuSceneRequested += () =>
			{
				_coroutines.StartCoroutine(LoadAndStartMainMenu());
			};
			// !!!

			_uiRootView.HideLoadingScreen();
		}
		
		private IEnumerator LoadAndStartMainMenu()
		{
			_uiRootView.ShowLoadingScreen();

			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.MAIN_MENU);

			yield return new WaitForSeconds(2);

			var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
			sceneEntryPoint.Run(_uiRootView);

			// !!!
			sceneEntryPoint.GoToGameplaySceneRequested += () =>
			{
				_coroutines.StartCoroutine(LoadAndStartGameplay());
			};
			// !!!

			_uiRootView.HideLoadingScreen();
		}

		private IEnumerator LoadScene(string sceneName)
		{
			yield return SceneManager.LoadSceneAsync(sceneName);
		}
	}
}
