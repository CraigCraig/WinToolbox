/// ======================================================================
///		CheetahToolbox: (https://github.com/CraigCraig/CheetahToolbox)
///				Project:  Craig's CheetahToolbox a Swiss Army Knife
///
///
///			Author: Craig Craig (https://github.com/CraigCraig)
///		License:     MIT License (http://opensource.org/licenses/MIT)
/// ======================================================================
#if WINDOWS
namespace CheetahToolbox.Features;
using Commands;
[CommandGroup("Chocolatey", ["Choco"])]
public class Chocolatey
{
    private static readonly List<AppEntry> apps = [];

    //[Command(defaultCommand: true)]
    //public static CommandResult Default() => new(true, "Chocolatey Helper");

    //[CommandGroup]
    public class Find
    {
        //[Command(defaultCommand: true)]
        //public static CommandResult Default()
        //{
        //    Console.WriteLine(Localization.WorkInProgressCommand);
        //    return new(true, "GO GO CHOCO");
        //}

        //[Command]
        //public static CommandResult Package()
        //{
        //    Console.WriteLine(Localization.WorkInProgressCommand);
        //    return new(true, "GO GO CHOCO");
        //}

        //[Command]
        //public static CommandResult Source()
        //{
        //    Console.WriteLine(Localization.WorkInProgressCommand);
        //    return new(true, "GO GO CHOCO");
        //}
    }

    public static string Version => TerminalUtils.Cmd("choco -v")?.Trim() ?? string.Empty;

    public static bool IsInstalled
    {
        get
        {
            try
            {
                string? result = TerminalUtils.Cmd("choco");
                return result switch
                {
                    null => false,
                    _ => true
                };
            }
            catch
            {
                return false;
            }
        }
    }

    public static void Uninstall()
    {
        // TODO: Remove Folders

        // TODO: Remove Registry Entries

        // TODO: Remove Environment Variables
    }

    /// <summary>
    /// Run Choco Updates
    /// </summary>
    public static void Update()
    {
        if (!WindowsUtils.IsRunningAsAdmin())
        {
            Log.Warn("Update Failed: Not running with Administrator privileges.");
            throw new Exceptions.PackageManagerUpdateException();
        }
        string? result = TerminalUtils.Cmd("choco upgrade all -y");
        if (result == null)
        {
            Log.Warn("Failed to update Chocolatey");
            return;
        }

        Log.Write(result);
    }

    public static void Install()
    {
        if (!WindowsUtils.IsRunningAsAdmin())
        {
            Log.Write("Must run this command as admin");
            return;
        }

        if (!IsInstalled)
        {
            //Log.Write("Chocolatey is not installed.");
            //Log.Write("Do you want to install it? Y / N: ");
            //ConsoleKeyInfo input = Console.ReadKey();

            //string? result = TerminalUtils.PowerShell("Get-ExecutionPolicy");
            //if (!string.IsNullOrEmpty(result)) Log.Write(result);

            //if (input.KeyChar is 'Y' or 'y')
            //{
            //    string[] lines = GlobalStrings..InstallCommand.Split(';');

            //    foreach (string line in lines)
            //    {
            //        string cleanLine = line.Trim();
            //        Log.Write(cleanLine);

            //        string? result1 = TerminalUtils.PowerShell(cleanLine);
            //        if (!string.IsNullOrEmpty(result1)) Log.Write(result1);
            //        if (result1 == null)
            //        {
            //            Log.Write("Chocolatey Install Failed..");
            //            return;
            //        }
            //        else
            //        {
            //            if (result1.Contains("Chocolatey (choco.exe) is now ready"))
            //            {
            //                Log.Write("Chocolatey Installed");

            //                string? result2 = TerminalUtils.PowerShell("choco feature enable -n allowGlobalConfirmation");
            //                if (result2 != null)
            //                    Log.Write("choco feature enable -n allowGlobalConfirmation");
            //                return;
            //            }
            //            else
            //            {
            //                Log.Write("Chocolatey Install Failed..");
            //                return;
            //            }
            //        }
            //    }
        }
        else
        {
            Log.Write("Chocolatey Install Skipped..");
            return;
        }
    }
    // Log.Write("Chocolatey already installed..");

    /// <summary>
    /// WIP: Check if Chocolatey result is valid.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public AppEntry? CheckResult(string line)
    {
        List<string> lines = [.. line.Split(' ')];
        if (string.IsNullOrEmpty(lines[0])) return null;
        if (lines.Count != 2) return null;

        return new AppEntry(lines[0], lines[0], null, null, AppEntry.AppSource.CHOCOLATEY);
    }

    /// <summary>
    /// Start the Chocolatey Manager
    /// </summary>
    public void Start()
    {
        apps.Clear();

        string? test = TerminalUtils.PowerShell("choco list");
        if (test == null) return;
        List<string> tempList = [.. test.Split('\n')];
        tempList.RemoveAt(0);

        foreach (string temp in tempList)
        {
            AppEntry? item = CheckResult(temp);
            if (item != null)
                apps.Add(item);
        }
        Log.Write($"Chocolatey has {tempList.Count} packages installed.");
    }
}
#endif