url:        folders-structure/pages.html  
template:   default.hjs

title:      Pages folder structure

---

[back to folders structure](/folders-structure.html)

### Pages

Inside `Pages` folder there will be all pages files. Each page file is a markdown markup file with some mandatory metadata at the start of the file.

Page file look like this:

```markdown
url:                index.html  
template:           default.hjs  
title:              Home  

custom.header:      header  
custom.description: this is my home page  
custom.logo.url:    http://my.com/logo.png  

---

# Welcome to my website!
```

### Predefined attributes

#### url

The `url` attribute is mandatory. It shows under which path output html file will be saved. If you want to save in some specific folder you can do it by providing this kind of value for `url` attribute:

```markdown
url:    blog/posts/2018/Jan/my-awesome-post.html
```

#### template

The `template` attribute is mandatory. It shows which template should be used to render current page. For now only flat structure is supported, so you can reference templates only from root of `Layout` folder.

You can use other synonimous for `template` attribute which are `TemplateId`, `Layout`, `LayoutId`. All those synonymous are case insensitive.

> More details about templates / layouts can be found in the dedicated [layout](/folders-structure/layout.html) section.

#### title

The `title` attribute is not mandatory, but we reccond to use it for setting html title of your page.


### Custom attributes

If predefined attributes are not enough and you want to add some extra attributes to your page, you can acheive that by using custom attributes section. The name of custom attribute should start with `custom` and be followed by `.`. After that you should add at least one character that will represent the name of your attribute.


#### Page object

All attributes can be accessible through global `Page` object. That object is accessible from each and every template and it always have a context of current page.

You can access predefined page attributes via `Page.Title`, `Page.Url`, `Page.TemplateId` and `Page.Content` for page html content. All properties of global `Page` object are case sensitive.

You can access custom page attributes via properties of `Page.Custom` object. It will have all the properties defibed in page metadata secrion.

#### Example

If your page file look like this:

```markdown
url:                index.html  
template:           default.hjs  
title:              Home  

custom.header:      header  
custom.description: this is my home page  
custom.logo.url:    http://my.com/logo.png  

---

# Welcome to my website!
```

You can access logo url using following statement `Page.Custom.logo.url`.

There is one important point to highlight here. `Page.Custom` is predifened and case sensitive, but `logo.url` was generated dynamicaly from page metadata and it's also case sensitive.