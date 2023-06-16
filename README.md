# XMLValueReplacer
Replaces values in an XML with the nodes XPath with dashes instead of slashes
Replaces values in an XML with the corresponding nodes XPath


Before
```xml
<root>
  <node>Lorem ipsum</node>
  <node>Lorem ipsum</node>
  <foot>End of file</foot>
 </root>
```
After
```xml
<root>
  <node>[prefix]root-node1</node>
  <node>[prefix]root-node2</node>
  <foot>[prefix]root-foot</foot>
 </root>
```

Creates a template.xml and replacementvalues.txt file after successful run.
