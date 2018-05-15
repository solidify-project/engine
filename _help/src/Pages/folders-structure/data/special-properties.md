url:        folders-structure/data/special-properties.html  
template:   default.hjs

title:      Data 101

---

[back to data folder structure](/folders-structure/data.html)

### `__collection` special property

There are a special property `__collection` of each child property of `Data` object. Using this property you can navigate through collection of child properties of current property.

##### Example (simple)

Let's assume that you have the following folders structure on you file system:

```none
Data
    Poland.json
    Ukraine.json
```

And inside each `.json` file in this example you have the following structure:

```json
{
    "details": {
        "flag":    "flag.png",
        "date":    "1st of February",
        "address": "Main street"
    }
}
```

And your template may look like this:

```handlebars
{{ Data.FoldersStructure.Data.DataTemplate01 }}
```

Finally, the html rendered by Solidify Engine will look like this:

```html
<ul>
    <li>
        <img src="poland.png"/>
        On 1st of February at Warsaw
    </li>
    <li>
        <img src="ukraine.png"/>
        On 1st of March at Kyiv
    </li>
</ul>
```

##### Example (nested)

You can also use nested `__collection` properties.

Let's assume that you have the following folders structure on you file system:

```none
Data
    year1999
        Poland.json
        Ukraine.json
    year2001
        Bulgaria.json
        Romania.json
    year2020
        Denmark.json
        Spain.json
```

And inside each `.json` file in this example you have the following structure:

```json
{
    "details": {
        "flag":    "flag.png",
        "date":    "1st of February",
        "address": "Main street"
    }
}
```

And your template may look like this:

```handlebars
{{ Data.FoldersStructure.Data.DataTemplate02 }}
```

Finally, the html rendered by Solidify Engine will look like this:

```html
<ul>
    <li>
        <img src="poland.png"/>
        On 1st of February at Warsaw
    </li>
    <li>
        <img src="ukraine.png"/>
        On 1st of March at Kyiv
    </li>
    <li>
        <img src="bulgaria.png"/>
        On 1st of April at Sofia
    </li>
    <li>
        <img src="romania.png"/>
        On 1st of May at Bucharest
    </li>
    <li>
        <img src="denmark.png"/>
        On 1st of June at Copenhagen
    </li>
    <li>
        <img src="spain.png"/>
        On 1st of July at Madrid
    </li>
</ul>
```