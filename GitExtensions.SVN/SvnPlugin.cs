using GitExtensions.SVN.Properties; 

using GitCommands;
using GitExtUtils;
using GitUI;
using GitUIPluginInterfaces;
using ResourceManager;

using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace GitExtensions.SVN
{
    /// <summary>
    /// Git Extensions SVN plugin implementation.
    /// </summary>
    [Export(typeof(IGitPlugin))]
    public class SvnPlugin : GitPluginBase
    {
        private const string PluginName = "SVN plugin";

        private RepoType repoType = RepoType.Unknown;

        private readonly ArgumentString cmdInfo = new GitArgumentBuilder("svn")
                                                        {
                                                            "info"
                                                        };


        public SvnPlugin()
        {
            SetNameAndDescription(PluginName);
            Icon = Resources.Icon;
        }


        public override void Register(IGitUICommands gitUiCommands)
        {
            if (IsSvnRepo(gitUiCommands))
            {
                AddSvnScripts();
                ForceRefreshGE(gitUiCommands);
            }
        }


        public override void Unregister(IGitUICommands gitUiCommands)
        {
            if (IsSvnRepo(gitUiCommands))
            {
                RemoveSvnScripts();
            }

            repoType = RepoType.Unknown;
        }


        public override bool Execute(GitUIEventArgs gitUiEventArgs)
        {
            if (IsSvnRepo(gitUiEventArgs.GitUICommands))
            {
                MessageBox.Show(gitUiEventArgs.OwnerForm, "The SVN commands are in the toolbar.", PluginName,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            else
            {
                MessageBox.Show(gitUiEventArgs.OwnerForm, "The current repository has no configured SVN remote.", PluginName,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        /// <summary>
        /// Checks if repo has a SVN remote.
        /// </summary>
        /// <returns>
        /// true if has SVN remote; false otherwise.
        /// </returns>
        private bool IsSvnRepo(IGitUICommands gitUiCommands)
        {
            if (repoType == RepoType.Unknown)
            {
                ExecutionResult result = gitUiCommands.GitModule.RunGitCmdResult(cmdInfo);
                repoType = (result.ExitCode == 0) ? RepoType.SVN
                                                  : RepoType.git;
            }

            return (repoType == RepoType.SVN);
        }


        private void AddSvnScripts()
        {
            SvnScriptManager.AddNew("SVN Fetch",   "svn fetch",   "PullFetch");
            SvnScriptManager.AddNew("SVN Rebase",  "svn rebase",  "PullRebase");
            SvnScriptManager.AddNew("SVN DCommit", "svn dcommit", "Push");
            SvnScriptManager.AddNew("SVN Info",    "svn info",    "Information", addToRevisionGridContextMenu: false);
        }


        private void RemoveSvnScripts()
        {
            SvnScriptManager.RemoveAll();
        }


        private void ForceRefreshGE(IGitUICommands gitUiCommands)
        {
            gitUiCommands.RepoChangedNotifier.Notify();
        }
    }
}
