﻿namespace CheetahToolbox.Commands;

#region Using Statements
using System;
using System.Reflection;
using System.Runtime.Versioning;
using CheetahToolbox.Utils;
#endregion

internal static class CommandHandler
{
	public static Dictionary<string, Command> Commands { get; } = [];

	private static bool _initialized;

	public static void Initialize()
	{
		if (_initialized) return;
		_initialized = true;

		if (!WindowsUtils.IsRunningAsAdmin())
		{
			Console.WriteLine("WARNING: Not running as administrator. Some commands may not work.");
		}

		Assembly assembly = Assembly.GetExecutingAssembly();
		var types = assembly.GetTypes();
		foreach (var type in types)
		{
			if (type.BaseType == typeof(Command))
			{
				if (type == null || string.IsNullOrEmpty(type.FullName)) continue;
				if (assembly.CreateInstance(type.FullName) is not Command command) continue;
				Commands.Add(command.Name, command);
			}
		}
	}

	[SupportedOSPlatform("windows")]
	public static void HandleCommand(string command, string[] arguments)
	{
		if (!_initialized) Initialize();
		if (string.IsNullOrEmpty(command))
		{
			Console.WriteLine("No command");
			return;
		}

		if (Commands.TryGetValue(command, out var cmd))
		{
			cmd.Execute(arguments);
		}
		else
		{
			Console.WriteLine($"Command \"{command}\" not found");
		}
	}
}
