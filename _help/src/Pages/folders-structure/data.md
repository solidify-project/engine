url:        folders-structure/data.html  
template:   default.hjs

title:      Data folder structure

---

[back to folders structure](/folders-structure.html)

### Data

Inside `Data` folder there will be all data files. For now Solidify Engine can support the following data formats:

- json
- csv
- txt
- yaml (yml)

On views you can use global object `Data` to access actual data that is located on the file system.

##### Example

Let's assume that you have the following folders structure on you file system:

```
Data
    misc
        social.json
```

And inside `social.json` you have the following content:

```
{
    "profiles ": [{
        "url": "http://facebook.com/JohnDoe",
        "icon": "fb.png",
        "name": "facebook"
    }, {
        "url": "http://twitter.com/JohnDoe",
        "icon": "tw.png",
        "name": "twitter"
    }]
}
```

Then our template, that we will use to show the list of social profiles, will look like this:

```
{{ Data.FoldersStructure.data.template01 }}
```

Finally, the html rendered by Solidify Engine will look like this:

```
<ul>
    <li>
        <img src="fb.png"/>
        <a href="http://facebook.com/JohnDoe">
            facebook
        </a>
    </li>
    <li>
        <img src="tw.png"/>
        <a href="http://twitter.com/JohnDoe">
            twitter
        </a>
    </li>
</ul>
```
