url:        folders-structure/pages/attributes.html  
template:   default.hjs

title:      Pages custom attributes

---

[back to pages folder structure](/folders-structure/pages.html)

### Custom attributes

If predefined attributes are not enough and you want to add some extra attributes to your page, you can acheive that by using custom attributes section. The name of custom attribute should start with `custom` followed by `.`. After that you should add at least one character that will represent the name of your attribute.


#### Page object

All attributes can be accessible through global `Page` object. That object is accessible from each and every template and it always has a context of current page.

You can access predefined page attributes via `Page.Title`, `Page.Url`, `Page.TemplateId` and `Page.Content` for page html content. All properties of global `Page` object are case sensitive.

You can access custom page attributes via properties of `Page.Custom` object. It will have all the properties defined in page metadata section.

#### Example

If your page file looks like this:

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

You can access logo url using the following statement `Page.Custom.logo.url`.

There is one important point to highlight here. `Page.Custom` is predifened and case sensitive, but `logo.url` was generated dynamicaly from page metadata and it's also case sensitive.

### Custom attributes vs data model

The main difference between custom attributes and data model is how they interpreter the values you provide.
- **Custom attributes** always threat all the values like strings.
- **Data model** always threat all the values like reference to global `Data` model. It will copy all the values from referenced `Data` object to current `Model` object.