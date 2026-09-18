using BepInEx.Configuration;

namespace ServerSync;

public abstract class OwnConfigEntryBase
{
	public object? LocalBaseValue;

	public bool SynchronizedConfig = true;

	public abstract ConfigEntryBase BaseConfig { get; }
}
