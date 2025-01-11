using System;
using UnityEngine;

namespace SloppyFox
{
	public class GameplayEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIGameplayRootBinder _sceneUIRootPrefab;

		public event Action GoToMainMenuSceneRequested;

		public void Run(UIRootView uiRoot)
		{
			var uiScene = Instantiate(_sceneUIRootPrefab);
			uiRoot.AttachSceneUI(uiScene.gameObject);

			uiScene.GoToMainMenuButtonClicked += () =>
			{
				GoToMainMenuSceneRequested?.Invoke();
			};
		}
	}
}
