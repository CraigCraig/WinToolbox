﻿namespace CheetahToolbox.Commands;

public abstract class Command(string name, string description)
{
	public string Name = name;
	public string Description = description;
	public abstract void Execute(string[] args);
}