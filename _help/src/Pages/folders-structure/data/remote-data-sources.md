url:        folders-structure/data/remote-data-sources.html  
template:   default.hjs

title:      Data 101

---

[back to data folder structure](/folders-structure/data.html)

## Table of contents

- [`http`](#http)

---
### <a name="http"></a>`http`

This data source has the following properties:

```yaml
Url:            <url string>
Method:         <http method>
CustomDataType: <data format (type)>
```

This data source will send http request to `<url string>` using http method `<http method>`. It will treat response body as raw content string. It will try to parse raw content using `<data format (type)>` data format parser.

##### Example

Let's assume that you have the following folders structure on you file system:

```none
Data
    misc
        config.http
```

And inside `config.http` you have the following:

```yaml
Url:            https://raw.githubusercontent.com/solidify-project/engine/master/_help/config.yaml
Method:         get
CustomDataType: yaml
```

Then our template will look like this:

```handlebars
{{{ Data.FoldersStructure.Data.DataTemplateConfigs }}}
```

> More details about layout templates can be found in the dedicated [layout](/folders-structure/layout.html) section.

Finally, the html rendered by Solidify Engine will look like this:

```html
<table>
    <tr>
        <th>Engine.Path</th>
        <th>Source.Path</th>
        <th>Output.Path</th>
    </tr>
    <tr>
        <td>engine</td>
        <td>src</td>
        <td>www</td>
    </tr>
</table>


```
