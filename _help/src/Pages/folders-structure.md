url:        folders-structure.html  
template:   default.hjs

title:      Folders structure

---

### Root level folders

Each and every website project should have a root folder. Let's assume that name of that folder will be `MySite`. The folder structure should look like:

```
MySite
    engine
        ....
    src
        ....
    www
        ....
    config.yaml
```

Inside `engine` folder there will be all the binaries for Solidify Engine.

Inside `src` folder there will be all the source files for the project. This folder structure is more complicated and will be described in a dedicated section later.

Inside `www` folder there will be all pre-rendered files of website project. Solidify Engine will use this folder as a default output folder.

Inside `config.yaml` file there will be all the configuration for current website project.

### Source files folder

Inside `src` folder there will be all the source files for website project. This folder structure should look like:

```
src
    Assets
        ....
    Data
        ....
    Layout
        ....
        Partials
            ....
    Pages
        ....
```

#### Assets

Inside `Assets` folder there will be all assets (css, js, png, gif, etc.) that will be copied to output without any modifications. Under current output folder engine will create a folder with name `Assets` and copy all files from source assets folder there. Nested folders are supported.

[more details...](folders-structure/assets.html)


#### Data

Inside `Data` folder there will be all data files. For now Solidify Engine can support those data formats:

- json
- csv
- txt
- yaml (yml)

[more details...](folders-structure/data.html)