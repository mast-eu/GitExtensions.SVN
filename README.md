# GitExtensions.SVN
This project is for developing a SVN plugin for Git Extensions.


## So is there already a plugin ready to be used?
No. This is work in progress at an very early stage.


## But I need something that works right now!
#### Option 1) Use the `git svn` commands directly from the console
Don't worry, it's not that complicated. You can use either Git Extensions' integrated console or the normal Windows command line, both work the same way.

For `SVN Fetch`, `SVN Rebase` and `SVN DCommit`, respectively type:
```
git svn fetch
git svn rebase
git svn dcommit
```
_Note:_ The revision graph is not automatically refreshed after these commands. You need to do it manually.

For `SVN Clone` type:
```
cd C:\local\dir
git svn clone --stdlayout http://example.com/svn/repository local_repo
```
These commands correspond to the old Git Extensions dialog.
![SVN clone dialog](https://user-images.githubusercontent.com/46861028/52055310-546c1100-255f-11e9-9647-08f13c8f213f.png)

#### Option 2) Downgrade
[Git Extensions version 2.51.05](https://github.com/gitextensions/gitextensions/releases/tag/v2.51.05) is the latest release with integrated SVN support.


## I don't like the above workarounds!
Good, we have something in common. Have a look arround at this project and feel free to contribute.
