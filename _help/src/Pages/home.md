url:        index.html  
template:   default.hjs

title:      Solidify Project Help

---

### Quickstart guide

- [Create my first website](/quickstart/create-my-first-website.html)

### Reference

- [Folders structure](/folders-structure.html)

### Founders
{{# Data.Authors.Founders }}  

* {{ Name }} {{# Title }} ({{ Title }}) {{/ Title }}  

{{/ Data.Authors.Founders }}

### Contributors
{{# Data.Authors.Contributors }}  

* {{ Name }} {{# Title }} ({{ Title }}) {{/ Title }}  

{{/ Data.Authors.Contributors }}
