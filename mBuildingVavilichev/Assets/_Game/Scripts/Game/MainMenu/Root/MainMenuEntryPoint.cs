using System;
using UnityEngine;

namespace SloppyFox
{
	public class MainMenuEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;

		public event Action GoToGameplaySceneRequested;

		public void Run(UIRootView uiRoot, MainMenuEnterParams enterParams)
		{
			var uiScene = Instantiate(_sceneUIRootPrefab);
			uiRoot.AttachSceneUI(uiScene.gameObject);

			uiScene.GoToGameplayButtonClicked += () =>
			{
				GoToGameplaySceneRequested?.Invoke();
			};

			Debug.Log($"MAIN MENU ENTRY POINT: Run main menu scene. Rsults: {enterParams?.Result}");
		}
	}
}
