url:        folders-structure.html  
template:   default.hjs

title:      Folders structure

---

[back to help root](/index.html)

### Root level folders

Each and every website project should have a root folder. Let's assume that name of that folder will be `MySite`. The folder structure should look like:

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

Inside `engine` folder there will be all binaries for Solidify Engine.

Inside `src` folder there will be all source files for the project. This folder structure is more complicated and will be described in a dedicated section later.

Inside `www` folder there will be all pre-rendered files of website project. Solidify Engine will use this folder as a default output folder.

Inside `config.yaml` file there will be all the configuration for current website project.


### Ignored files

There is a list of files that will be ignored by engine to load and process. These files are:

- `README.md`
- `.DS_Store`

These files will be ignored, if found, in any folders and subfolders.


### Source files folder

Inside `src` folder there will be all the source files for website project. This folder structure should look like:

```none
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


#### Layout

Inside `Layout` folder there will be all layout files. For now Solidify Engine can support those layout formats: 

- mustache

[more details...](folders-structure/layout.html)


#### Pages

Inside `Pages` folder there will be all pages files. Each page file is a markdown markup file with some mandatory metadata at the start of the file.

[more details...](folders-structure/pages.html)
