using System;
using System.Collections.Generic;

namespace SloppyFox.DI
{
	public class DIContainer
	{
		private readonly DIContainer _parentDIContainer;
		private readonly Dictionary<(string, Type), DIRegistration> _registrations = new();
		private readonly HashSet<(string, Type)> _resolutions = new();

		public DIContainer(DIContainer parentDIContainer = null) => _parentDIContainer = parentDIContainer;

		public void RegisterSingleton<T>(Func<DIContainer, T> factory) => RegisterSingleton(null, factory);

		public void RegisterSingleton<T>(string tag, Func<DIContainer, T> factory) => Register((tag, typeof(T)), factory, true);

		public void RegisterTransient<T>(Func<DIContainer, T> factory) => RegisterTransient(null, factory);

		public void RegisterTransient<T>(string tag, Func<DIContainer, T> factory) => Register((tag, typeof(T)), factory, false);

		public void RegisterInstance<T>(T instance) => RegisterInstance(null, instance);

		public void RegisterInstance<T>(string tag, T instance)
		{
			var key = (tag, typeof(T));

			if (_registrations.ContainsKey(key))
				throw new Exception($"DI: Factory with tag {key.Item1} and Type {key.Item2.FullName} has already registered");

			_registrations[key] = new DIRegistration
			{
				Instance = instance,
				IsSingleton = true
			};
		}

		public T Resolve<T>(string tag = null)
		{
			var key = (tag, typeof(T));

			if (_resolutions.Contains(key))
			{
				throw new Exception($"Cyclic dependency for tag {key.tag} and type {key.Item2.FullName}");
			}

			_resolutions.Add(key);

			try
			{
				if (_registrations.TryGetValue(key, out var registration))
				{
					if (registration.IsSingleton)
					{
						if (registration.Instance == null && registration.Factory != null)
						{
							registration.Instance = registration.Factory(this);
						}

						return (T)registration.Instance;
					}

					return (T)registration.Factory(this);
				}

				if (_parentDIContainer != null)
					return _parentDIContainer.Resolve<T>(tag);
			}
			finally
			{
				_resolutions.Remove(key);
			}

			throw new Exception($"Couldn't find dependency for tag {tag} and type {key.Item2.FullName}");
		}

		public void Register<T>((string, Type) key, Func<DIContainer, T> factory, bool isSingleton)
		{
			if (_registrations.ContainsKey(key))
				throw new Exception($"DI: Factory with tag {key.Item1} and Type {key.Item2.FullName} has already registered");

			_registrations[key] = new DIRegistration
			{
				Factory = c => factory(c),
				IsSingleton = isSingleton
			};
		}
	}
}
