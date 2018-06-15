url:        quickstart/create-my-first-website.html  
template:   default.hjs

title:      Create my first website

---

[back to help root](/index.html)

This guide describes how to create your first static website with Solidify Project engine.

### Preparation

Before we start you should create a new empty folder for your website. Let's name that folder `MySite`.

### Get the engine

Solidify Project engine is usualy distributed as a `.zip` archived folder. You should unzip its content to `MySite` folder, so you will have the following stuff on your file system:

```none
MySite
    engine
        ....
    config.yaml
```

In addition to that you will also have either `solidify.sh` or `solidify.bat` inside `MySite` folder (the exact file depends on what operational system you are using). This script file in nothing more than a script wrapper that provides you with easy access to engine executables.

### Bootstrap some website

To bootstrap your first website execute a command from `MySite` folder.

##### Windows
```bash
solidify.bat bootstrap
```

##### MacOS or Linux
```bash
./solidify.sh bootstrap
```

After that you will see a new folder `src` under `MySite` folder. It will look like this:

```none
MySite
    engine
        ....
    src
        ....
    config.yaml
```

### Make some changes

To change default website content, please refer to [Folders structure](/folders-structure.html) section. It will describe what is inside `src` folder and how it affects the final website.

### Render your website

After making changes you can render your website by executing the following command from `MySite` folder.

##### Windows
```bash
solidify.bat render
```

##### MacOS or Linux
```bash
./solidify.sh render
```

After that you will see a new folder `www` under `MySite` folder. It will look like this:

```none
MySite
    engine
        ....
    src
        ....
    www
        ....
    config.yaml
```

You can find pre-rendered minified version of your website inside `www` folder.
