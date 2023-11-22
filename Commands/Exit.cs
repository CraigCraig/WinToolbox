﻿namespace CheetahToolbox.Commands;

using CheetahApp.Commands;

public class Exit() : Command("exit", "quit the program")
{
	public override void Execute(string[] args)
	{
		Program.App?.Close();
	}
}