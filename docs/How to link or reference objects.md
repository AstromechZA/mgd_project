I found it quite tricky to find the correct way to link objects. Unity has some funny concepts for the scene graph.

Note: ```Find``` is not very efficient, so only use this in the ```Start()``` or ```Awake()``` methods.

### To access a object in the scene
```
GameObject.Find("Main Camera");
```

returns the actual GameObject.

### To access a child object
```
transform.Find("child object").gameObject;
```

this seems weird, but works, ```transform.Find("x")``` returns a Transform Component.

### To access a script instance or component on another GameObject
```
gameObject.GetComponent<Type>();
```

Type is the class name. Or a component like 'Transform' or 'MeshCollider'.



#### Useful links
![GameObject.GetComponent](http://docs.unity3d.com/Documentation/ScriptReference/GameObject.GetComponent.html)

![GameObject](http://docs.unity3d.com/Documentation/ScriptReference/GameObject.html)

![Component](http://docs.unity3d.com/Documentation/ScriptReference/Component.html)
