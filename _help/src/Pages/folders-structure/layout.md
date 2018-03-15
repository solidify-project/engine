url:        folders-structure/layout.html  
template:   default.hjs

title:      Layout folder structure

---

[back to folders structure](/folders-structure.html)

### Layout

Inside `Layout` folder there will be all layout files. For now Solidify Engine can support those layout formats: 

- mustache ([more details...](http://mustache.github.io/mustache.5.html))

Inside `Layout` folder we expect to see the following structure:

```none
Layout
    ...
    Partials
        ...
```

All files inside both `Layout` and `Partials` folders should have extension `.hjs`.

### Partials

`Partials` is a special folder which should have a flat structure (should not contain any subfolders). All partials should be placed here.

#### Example

Folders structure should be like this:

```none
Layout
    default.hjs
    Partials
        footer.hjs
```

Template file can look like this:

```handlebars
{{ Data.FoldersStructure.LayoutTemplate01 }}
```

Partials file can look like this:

```html
<div>
    All rights reserved.
</div>
```

Page file will look like:
```markdown
url:        index.html  
template:   default.hjs
title:      Home
---
# Welcome to my website!
```

> More details about pages can be found in the dedicated [pages](/folders-structure/pages.html) section.

Finally, the html rendered by Solidify Engine will look like this:

```html
<html>
    <head>
        <title>Home</title>
    </head>
    <body>
        <div>
            <h1>Welcome to my website!</h1>
        </div>

        <div>
            All rights reserved.
        </div>
    </body>
</html>
```
