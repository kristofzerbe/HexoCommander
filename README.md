# HexoCommander

HexoCommander is a little C# console for running bundled commands to manage 
a **Hexo blog**, synced via **Dropbox** and published on **GitHub Pages**.

It expects the Hexo Command File to be named ``hexo-commands.txt``, 
located in the same folder or executed with the parameter "/workdir=<Path to your folder>"

It provides the following commands:

**newpost: "&lt;title&gt;"** ... runs

1. ```hexo new draft "<title>"```

Creates a new post.

**newdraft: "&lt;title&gt;"** ... runs

1. ```hexo new draft "<title>"```

Creates a new draft.

**postdraft: "&lt;filename&gt;"** ... runs

1. ```hexo publish "<filename>"```

Makes a post out of a draft.

**regenerate** ... runs

1. ```hexo clean```
2. ```hexo generate```

Wipes all Hexo static pages and generates them new.

**publish** ... runs

1. ```hexo generate```
2. ```git add "source/*" "docs/*"```
3. ```git commit -m "Remote publication via HexoCommander"```
4. ```git push origin master```

Generates Hexo static pages, stage changes on drafts, posts and static pages, 
commits the changes with a generic message and pushes them to the server.

If you want to write comment lines in the 'command.txt', use "//" in front of the line.

See my article about it at [https://kiko.io/categories/Tools/A-New-Blog-Blogging-and-Synching-en-route](https://kiko.io/categories/Tools/A-New-Blog-Blogging-and-Synching-en-route)