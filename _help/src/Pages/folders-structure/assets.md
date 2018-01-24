url:        folders-structure/assets.html  
template:   default.hjs

title:      Assets folder structure

---

[back to folders structure](/folders-structure.html )

#### Assets

Inside `Assets` folder there will be all assets (css, js, png, gif, etc.) that will be copied to output without any modifications. Under current output folder engine will create a folder with name `Assets` and copy all files from source assets folder there. Nested folders are supported.

##### Example

If you want to have a website logo, you should do the following:

- Put logo under `Assets` source folder. Let's assume that name of the file will be `logo.png`.

- Create an image html tag on a website.
```
<img src="/Assets/logo.png"/>
```

- Re-render website project. File `logo.png` should appear in output `Assets` folder. Check re-rendered html page with the logo in output folder.