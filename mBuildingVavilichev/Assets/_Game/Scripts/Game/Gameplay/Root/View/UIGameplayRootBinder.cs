using System;
using UnityEngine;

namespace SloppyFox
{
	public class UIGameplayRootBinder : MonoBehaviour
	{
		public event Action GoToMainMenuButtonClicked;

		public void HandleGoToMainMenuButtonClick()
		{
			GoToMainMenuButtonClicked?.Invoke();
		}
	}
}
