url:        folders-structure/pages.html  
template:   default.hjs

title:      Pages folder structure

---

[back to folders structure](/folders-structure.html)

### Pages

Inside `Pages` folder will be all page files. Each page file is a markdown markup file with some mandatory metadata at the start of the file.

Page file looks like this:

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

The `url` attribute is mandatory. It shows under which path output html file will be saved. If you want to save it in some specific folder, you can do it by providing the following value for `url` attribute:

```markdown
url:    blog/posts/2018/Jan/my-awesome-post.html
```

#### template

The `template` attribute is mandatory. It shows which template should be used to render current page. For now only flat structure is supported, so you can reference templates only from root of `Layout` folder.

You can use other synonims for `template` attribute which are `TemplateId`, `Layout`, `LayoutId`. All those synonyms are case-insensitive.

> More details about templates / layouts can be found in the dedicated [layout](/folders-structure/layout.html) section.

#### title

The `title` attribute is not mandatory, but we recommend to use it for setting html title of your page.


### Custom attributes

If predefined attributes are not enough and you want to add some extra attributes to your page, you can acheive that by using custom attributes section. The name of custom attribute should start with `custom` followed by `.`. After that you should add at least one character that will represent the name of your attribute.

> More advanced details about how to work with custom attributes can be found in [page custom attributes](/folders-structure/pages/attributes.html) section.


### Data model

In case you want to use the same template, but use different data for it, you can use page data model. The scenario here is to have different data (in terms of values) with the same structure on different pages. The name of page data model should start with `model` followed by `.`. After that you should add at least one character that will represent the name of your data model. The value of `model` should always point to valid `data` object.

> More advanced details about how to work with data can be found in [page data model](/folders-structure/pages/model.html) section.

### Custom attributes vs data model

The main difference between custom attributes and data model is how they interprete the values you provide.
- **Custom attributes** always treat the values as strings.
- **Data model** always treats the values as references to global `Data` model. It will copy all values from referenced `Data` object to current `Model` object.


### Feeds

There are cases when you may need to display a collection of similar items, byt the regular repeater may not be enough. For example - if you need to show the list of phones to call and make an order in your online shop repeater will work, but for showing the list of all items available for purchase it may not. The reason why repeater may not be good enough is because of volume of items within the collection. For longer lists everyone will expect to have them splitted onto few pages.

**Feeds** are a way to split a big collection of items to smaller sub-parts and render them on your website.

Feeds adopts push model of putting items to the feed. It means that you need to explicetly push a page to the feed.

#### Adding a page to the feed

There are dedicated attributes to add a page to the feed:

```markdown
FeedDestination:         blogPosts
FeedDestinationOrder:    2019-12-21
```

`FeedDestination` has a string type and represents the name of a feed that page will be added to. If you need to add a few pages to the same feed this attribute should be the same for all of them.

`FeedDestinationOrder` has a string type and serves the purpose of ordering items within the feed. **IMPORTANT!** - because `FeedDestinationOrder` is a string field it means that rules of string comparison will be applied to determine the order of items in a feed. For example, following items will be ordered like this: `09`, `1`, `10`, `2`, `3`. Chose the values for `FeedDestinationOrder` wisely.


#### Consuming pages feed



