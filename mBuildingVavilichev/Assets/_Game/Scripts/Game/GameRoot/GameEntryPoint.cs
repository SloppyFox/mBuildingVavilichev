using System.Collections;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

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
				_coroutines.StartCoroutine(LoadAndStartGameplay(new GameplayEnterParams()));
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

			_coroutines.StartCoroutine(LoadAndStartMainMenu());
		}

		private IEnumerator LoadAndStartGameplay(GameplayEnterParams enterParams)
		{
			_uiRootView.ShowLoadingScreen();

			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.GAMEPLAY);

			yield return new WaitForSecondsRealtime(0.5f);

			var sceneEntryPoint = Object.FindFirstObjectByType<GameplayEntryPoint>();
			sceneEntryPoint.Run(_uiRootView, enterParams).Subscribe(exitParams =>
			{
				_coroutines.StartCoroutine(LoadAndStartMainMenu(exitParams.MainMenuEnterParams));
			});

			_uiRootView.HideLoadingScreen();
		}

		private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
		{
			_uiRootView.ShowLoadingScreen();

			yield return LoadScene(Scenes.BOOT);
			yield return LoadScene(Scenes.MAIN_MENU);

			yield return new WaitForSecondsRealtime(0.5f);

			var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
			sceneEntryPoint.Run(_uiRootView, enterParams).Subscribe(mainMenuExitParams =>
			{
				if (mainMenuExitParams.TargetSceneEnterParams.SceneName == Scenes.GAMEPLAY)
					_coroutines.StartCoroutine(LoadAndStartGameplay(mainMenuExitParams.TargetSceneEnterParams.As<GameplayEnterParams>()));

			});

			_uiRootView.HideLoadingScreen();
		}

		private IEnumerator LoadScene(string sceneName)
		{
			yield return SceneManager.LoadSceneAsync(sceneName);
		}
	}
}
