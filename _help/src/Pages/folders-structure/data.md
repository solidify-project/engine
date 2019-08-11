url:        folders-structure/data.html  
template:   default.hjs

title:      Data folder structure

---

[back to folders structure](/folders-structure.html)

### Data

Inside `Data` folder there will be all data files. For now Solidify Engine can support the following local data formats:

- json
- csv
- txt
- yaml (yml)

In addition to that Solidify Engine can support the following remote data sources:

- http

When content is downloaded from remote data source Solidify Engine will treat it as one of the local data formats mentioned above. More details can be found in [remote data sources](/folders-structure/data/remote-data-sources.html  ) section.

Solidify Engine will look for exact file extension from the list provided above. In case it is unable to find a match, the engine will blow up. In case there is a match, it will apply a parser based on exact extension to transform the file content into in-memory data object.

On views and pages you can use global object `Data` to access actual data that is located on the file system.

All properties of `Data` object are case-sensitive.

> More advanced details about how to work with data can be found in [data special properties](/folders-structure/data/special-properties.html) section.

##### Example

Let's assume that you have the following folders structure on you file system:

```none
Data
    misc
        social.json
```

And inside `social.json` you have the following content:

```json
{
    "profiles": [{
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

Then our template that we will use to show the list of social profiles will look like this:

```handlebars
{{ Data.FoldersStructure.DataTemplate01 }}
```

> More details about layout templates can be found in the dedicated [layout](/folders-structure/layout.html) section.

Finally, the html rendered by Solidify Engine will look like this:

```html
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
