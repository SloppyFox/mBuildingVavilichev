using System;
using UnityEngine;

namespace SloppyFox
{
	public class MainMenuEntryPoint : MonoBehaviour
	{
		[SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;

		public event Action GoToGameplaySceneRequested;

		public void Run(UIRootView uiRoot)
		{
			var uiScene = Instantiate(_sceneUIRootPrefab);
			uiRoot.AttachSceneUI(uiScene.gameObject);

			uiScene.GoToGameplayButtonClicked += () =>
			{
				GoToGameplaySceneRequested?.Invoke();
			};
		}
	}
}
