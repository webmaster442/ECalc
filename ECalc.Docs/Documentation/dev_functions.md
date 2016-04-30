Engineers calculator python programming guide
---

#Programming

The calculator uses Python2 as a programming language. To learn python programming a good starting point is the python tutorial: https://docs.python.org/2/tutorial/

The built in functions can't be redefined. This means that you can define a function with the name of Sin, but allways the built in version of the function will be called, instead of the redefined user version.

#Developer functions

The developer functions are special functions, that are not displayed on the function list, because they are not needed on the user interface. Available functions:

##Variable management
The Engineers calculator stores variables in a storage system. Only the variables stored in this system are accessible through the user interface.

```
Var(string name)
```

Returns the contents of the variable with the name specified by the argument.

```
Var(string name, object value)
```

Sets the contents of a variable with the name specified by the first argument. The second parameter specifies the value. The value can by one of the following types: string, int, double, Fraction, Set, Matrix, Vector

```
Var(string destination, string source)
```

Copies a variable to another

##Other functions

```
FncList()
```

Returns the list of available functions as a string. Each line contains a function name.

```
RegFunction(string name)
```

Register a function. The calculator's function list only displays registered functions. The RegFunction function puts the specified function name into the User category.