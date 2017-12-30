url:        index.html  
template:   default.hjs

title:      Static Site Generator - Home

---
# Welcome to Static Site Generator!

#### Authors

##### Founders
{{#Data.misc.Authors.Founders}}  

* {{ Name }} ({{ Title }})  

{{/Data.misc.Authors.Founders}}

##### Contributors
{{#Data.misc.Authors.Contributors}}  

* {{ Name }} ({{ Title }})  

{{/Data.misc.Authors.Contributors}}

**Powered by .NET Core 2.0**