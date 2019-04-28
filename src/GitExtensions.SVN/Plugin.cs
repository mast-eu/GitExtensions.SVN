using GitExtensions.SVN.Properties;
using GitUIPluginInterfaces;
using ResourceManager;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitExtensions.SVN
{
    /// <summary>
    /// Git Extensions SVN plugin implementation.
    /// </summary>
    [Export(typeof(IGitPlugin))]
    public class Plugin : GitPluginBase
    {
        public Plugin()
        {
            Name = "SVN";
            Description = "SVN Plugin";
            Icon = Resources.Icon;
        }

        public override bool Execute(GitUIEventArgs e)
        {
            MessageBox.Show(e.OwnerForm, "Hello from the SVN Plugin.", "Git Extensions");
            return true;
        }
    }
}
