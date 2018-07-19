url:        folders-structure/pages/model.html  
template:   default.hjs

title:      Pages data model

---

[back to pages folder structure](/folders-structure/pages.html)

### Data model

In case you want to use the same template, but use different data for it, you can use page data model. The scenario here is to have different data (in terms of values) with the same structure on different pages. The name of page data model should start with `model` followed by `.`. After that you should add at least one character that will represent the name of your data model. The value of `model` should always point to valid `data` object.


#### Page object

All data models can be accessible through global `Page` object. That object is accessible from each and every template and it always has a context of current page.

You can access page data models via properties of `Page.Model` object. It will have all the properties defined in page metadata section.

#### Example

If your template file looks like this:

```handlebars
{{ Data.FoldersStructure.Pages.Model.Template01 }}
```

And your page file looks like this:

```markdown
url:                goods1.html  
template:           goods.hjs  
title:              Home  

model.goods:        Data.category1.goods  
model.banner.url:   Data.banners.banner1.url  

---

# Category 1
```

And your data structure looks like this:

```none
Data
    category1.yml
    category2.yml
    banners.yml
```

`category1.yml`
```yaml
goods:
    -
        title: apple
        price: 10
    -
        title: pear
        price: 12
```

`category2.yml`
```yaml
goods:
    -
        title: potato
        price: 6
    -
        title: tomato
        price: 15
```

`banners.yml`
```yaml
banner1:
    url: "http://mywebsite.com/banner1.png"
banner2:
    url: "http://other.com/something.png"
```

Finally, the html rendered by Solidify Engine will look like this:

```html
<img src="http://mywebsite.com/banner1.png"/>

<h1>Category 1</h1>

<table>
    <tr>
        <th>Title</th>
        <th>Price</th>
    </tr>
    <tr>
        <td>apple</td>
        <td>10</td>
    </tr>
        <tr>
        <td>pear</td>
        <td>12</td>
    </tr>
</table>
```


There is one important point to highlight here. `Page.Model` is predifened and case-sensitive, but `goods` was generated dynamicaly from page metadata and it's also case-sensitive.

### Custom attributes vs data model

The main difference between custom attributes and data model is how they interprete the values you provide.
- **Custom attributes** always treats all values as strings.
- **Data model** always treat all values as references to global `Data` model. It will copy all values from referenced `Data` object to current `Model` object.
