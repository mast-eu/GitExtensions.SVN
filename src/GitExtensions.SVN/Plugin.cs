using GitExtensions.SVN.Properties; 

using GitCommands;
using GitExtUtils;
using GitUI;
using GitUIPluginInterfaces;
using ResourceManager;

using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace GitExtensions.SVN
{
    /// <summary>
    /// Git Extensions SVN plugin implementation.
    /// </summary>
    [Export(typeof(IGitPlugin))]
    public class Plugin : GitPluginBase
    { 
        private const string PluginName = "SVN";

        private enum IsSvnRepo
        {
            /// <summary>
            /// Has a SVN remote.
            /// </summary>
            Yes,

            /// <summary>
            /// Does not have a SVN remote.
            /// </summary>
            No,

            /// <summary>
            /// Not yet checked.
            /// </summary>
            Unknown
        };

        private IsSvnRepo isSvnRepo = IsSvnRepo.Unknown;

        private readonly ArgumentString cmdInfo = new GitArgumentBuilder("svn")
                                                        {
                                                            "info"
                                                        };

        private BindingList<GitUI.Script.ScriptInfo> scriptList;
        private GitUI.Script.ScriptInfo scriptSvnFetch;
        private GitUI.Script.ScriptInfo scriptSvnRebase;
        private GitUI.Script.ScriptInfo scriptSvnDCommit;
        private GitUI.Script.ScriptInfo scriptSvnInfo;

        public Plugin()
        {
            Name = PluginName;
            Description = "SVN Plugin";
            Icon = Resources.Icon;
        }


        public override void Register(IGitUICommands gitUiCommands)
        {
            ForceRemoveAllSvnScripts();

            if (CheckIsSvnRepo(gitUiCommands))
            {
                AddSvnScripts();
                ForceRefreshGE(gitUiCommands);
            }
        }


        public override void Unregister(IGitUICommands gitUiCommands)
        {
            if (CheckIsSvnRepo(gitUiCommands))
            {
                RemoveSvnScripts();
            }

            isSvnRepo = IsSvnRepo.Unknown;
        }


        public override bool Execute(GitUIEventArgs gitUiEventArgs)
        {
            if (!CheckIsSvnRepo(gitUiEventArgs.GitUICommands))
            {
                MessageBox.Show(owner: gitUiEventArgs.OwnerForm, text: "The repository has no SVN remote.", caption: PluginName, buttons: MessageBoxButtons.OK,
                                icon: MessageBoxIcon.Error);

                return false;
            }
            else
            {
                MessageBox.Show(owner: gitUiEventArgs.OwnerForm, text: "The SVN commands are in the toolbar.", caption: PluginName, buttons: MessageBoxButtons.OK,
                                icon: MessageBoxIcon.Information);

                return true;
            }
        }


        private bool CheckIsSvnRepo(IGitUICommands gitUiCommands)
        {
            if (isSvnRepo == IsSvnRepo.Unknown)
            {
                ExecutionResult result = gitUiCommands.GitModule.RunGitCmdResult(cmdInfo);
                isSvnRepo = (result.ExitCode == 0) ? IsSvnRepo.Yes
                                                   : IsSvnRepo.No;
            }

            if (isSvnRepo == IsSvnRepo.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void AddSvnScripts()
        {
            scriptList = GitUI.Script.ScriptManager.GetScripts();
            
            scriptSvnFetch = scriptList.AddNew();
            scriptSvnFetch.Name = "SVN Fetch";
            scriptSvnFetch.Command = "git";
            scriptSvnFetch.Arguments = "svn fetch";
            scriptSvnFetch.AddToRevisionGridContextMenu = true;
            scriptSvnFetch.Enabled = true;
            scriptSvnFetch.RunInBackground = false;
            scriptSvnFetch.IsPowerShell = false;
            scriptSvnFetch.AskConfirmation = false;
            scriptSvnFetch.OnEvent = GitUI.Script.ScriptEvent.ShowInUserMenuBar;
            scriptSvnFetch.Icon = "PullFetch";

            scriptSvnRebase = scriptList.AddNew();
            scriptSvnRebase.Name = "SVN Rebase";
            scriptSvnRebase.Command = "git";
            scriptSvnRebase.Arguments = "svn rebase";
            scriptSvnRebase.AddToRevisionGridContextMenu = true;
            scriptSvnRebase.Enabled = true;
            scriptSvnRebase.RunInBackground = false;
            scriptSvnRebase.IsPowerShell = false;
            scriptSvnRebase.AskConfirmation = false;
            scriptSvnRebase.OnEvent = GitUI.Script.ScriptEvent.ShowInUserMenuBar;
            scriptSvnRebase.Icon = "PullRebase";

            scriptSvnDCommit = scriptList.AddNew();
            scriptSvnDCommit.Name = "SVN DCommit";
            scriptSvnDCommit.Command = "git";
            scriptSvnDCommit.Arguments = "svn dcommit";
            scriptSvnDCommit.AddToRevisionGridContextMenu = true;
            scriptSvnDCommit.Enabled = true;
            scriptSvnDCommit.RunInBackground = false;
            scriptSvnDCommit.IsPowerShell = false;
            scriptSvnDCommit.AskConfirmation = false;
            scriptSvnDCommit.OnEvent = GitUI.Script.ScriptEvent.ShowInUserMenuBar;
            scriptSvnDCommit.Icon = "Push";

            scriptSvnInfo = scriptList.AddNew();
            scriptSvnInfo.Name = "SVN Info";
            scriptSvnInfo.Command = "git";
            scriptSvnInfo.Arguments = "svn info";
            scriptSvnInfo.AddToRevisionGridContextMenu = false;
            scriptSvnInfo.Enabled = true;
            scriptSvnInfo.RunInBackground = false;
            scriptSvnInfo.IsPowerShell = false;
            scriptSvnInfo.AskConfirmation = false;
            scriptSvnInfo.OnEvent = GitUI.Script.ScriptEvent.ShowInUserMenuBar;
            scriptSvnInfo.Icon = "Information";

            AppSettings.OwnScripts = GitUI.Script.ScriptManager.SerializeIntoXml();
        }


        private void RemoveSvnScripts()
        {
            scriptList = GitUI.Script.ScriptManager.GetScripts();

            scriptList.Remove(scriptSvnFetch);
            scriptList.Remove(scriptSvnRebase);
            scriptList.Remove(scriptSvnDCommit);

            AppSettings.OwnScripts = GitUI.Script.ScriptManager.SerializeIntoXml();
        }


        private void ForceRemoveAllSvnScripts()
        {
            scriptList = GitUI.Script.ScriptManager.GetScripts();

            for (int i = scriptList.Count - 1; i >= 0; i--)
            {
                var script = scriptList[i];
                if (script.Arguments.Contains("svn "))
                {
                    scriptList.Remove(script);
                }
            }
        }


        private void ForceRefreshGE(IGitUICommands gitUiCommands)
        {
            gitUiCommands.RepoChangedNotifier.Notify();
        }
    }
}
