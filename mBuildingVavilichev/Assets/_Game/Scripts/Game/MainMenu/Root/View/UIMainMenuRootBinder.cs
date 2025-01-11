using System;
using UnityEngine;

namespace SloppyFox
{
	public class UIMainMenuRootBinder : MonoBehaviour
	{
		public event Action GoToGameplayButtonClicked;

		public void HandleGoToGameplayButtonClick()
		{
			GoToGameplayButtonClicked?.Invoke();
		}
	}
}
