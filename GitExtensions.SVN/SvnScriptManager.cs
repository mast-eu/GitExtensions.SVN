using GitUI;

using System.ComponentModel;

namespace GitExtensions.SVN
{
    sealed class SvnScriptManager
    {
        static private BindingList<GitUI.Script.ScriptInfo> GitExtScriptList = new BindingList<GitUI.Script.ScriptInfo> {};
        static private BindingList<GitUI.Script.ScriptInfo> SvnScriptList = new BindingList<GitUI.Script.ScriptInfo> {};
        static readonly private int FirstHotkeyCommandIdentifier = 10000; // Arbitrary choosen. GE by default starts at 9000 for it's own scripts.

        /// <summary>
        /// Adds a new script, without saving it to GE's settings file.
        /// </summary>
        static public void AddNew(string name, 
                                  string arguments, 
                                  string icon, 
                                  bool enabled = true,
                                  string command = "git",
                                  bool addToRevisionGridContextMenu = true,
                                  GitUI.Script.ScriptEvent onEvent = GitUI.Script.ScriptEvent.ShowInUserMenuBar,
                                  bool askConfirmation = false,
                                  bool runInBackground = false,
                                  bool isPowerShell = false)
        {
            GitExtScriptList = GitUI.Script.ScriptManager.GetScripts();

            GitUI.Script.ScriptInfo newScript = GitExtScriptList.AddNew();
            newScript.Enabled = enabled;
            newScript.Name = name;
            newScript.Command = command;
            newScript.Arguments = arguments;
            newScript.AddToRevisionGridContextMenu = addToRevisionGridContextMenu;
            newScript.OnEvent = onEvent;
            newScript.AskConfirmation = askConfirmation;
            newScript.RunInBackground = runInBackground;
            newScript.IsPowerShell = isPowerShell;
            newScript.HotkeyCommandIdentifier = FirstHotkeyCommandIdentifier + GitExtScriptList.Count;
            newScript.Icon = icon;

            SvnScriptList.Add(newScript);
        }


        /// <summary>
        /// Removes all scripts that have been previously added by AddNew()
        /// </summary>
        static public void RemoveAll()
        {
            GitExtScriptList = GitUI.Script.ScriptManager.GetScripts();
            
            foreach (var script in SvnScriptList)
            { 
                GitExtScriptList.Remove(script);
            }

            SvnScriptList.Clear();
        }
    }
}
