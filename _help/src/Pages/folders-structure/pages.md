url:        folders-structure/pages.html  
template:   default.hjs

title:      Pages folder structure

---

[back to folders structure](/folders-structure.html)

### Pages

Inside `Pages` folder there will be all page files. Each page file is a markdown markup file with some mandatory metadata at the start of the file.

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

The `url` attribute is mandatory. It shows under which path output html file will be saved. If you want to save in some specific folder, you can do it by providing the following value for `url` attribute:

```markdown
url:    blog/posts/2018/Jan/my-awesome-post.html
```

#### template

The `template` attribute is mandatory. It shows which template should be used to render current page. For now only flat structure is supported, so you can reference templates only from root of `Layout` folder.

You can use other synonimous for `template` attribute which are `TemplateId`, `Layout`, `LayoutId`. All those synonymous are case insensitive.

> More details about templates / layouts can be found in the dedicated [layout](/folders-structure/layout.html) section.

#### title

The `title` attribute is not mandatory, but we recommend to use it for setting html title of your page.


### Custom attributes

If predefined attributes are not enough and you want to add some extra attributes to your page, you can acheive that by using custom attributes section. The name of custom attribute should start with `custom` followed by `.`. After that you should add at least one character that will represent the name of your attribute.

> More advanced details about how to work with custom attributes can be found in [page custom attributes](/folders-structure/pages/attributes.html) section.

### Data model

In case you want to use the same template, but use different data for it you can use page data model. The scenario here is to have different data (in terms of value) with the same structure on different pages. The name of page data model should start with `model` followed by `.`. After that you should add at least one character that will represent the name of your data model. The value of `model` should always point to valid `data` object.

> More advanced details about how to work with data can be found in [page data model](/folders-structure/pages/model.html) section.

### Custom attributes vs data model

The main difference between custom attributes and data model is how they interpreter the values you provide.
- **Custom attributes** always threat all the values like strings.
- **Data model** always threat all the values like reference to global `Data` model. It will copy all the values from referenced `Data` object to current `Model` object.