Plugin Interface
---

The calculator supports plugins via a simple plugin interface. The plugin interface is defined in the ECalc.Api assembly and namespace. Every plugin must be a descandent of the EcalcModule abstract class, which is defined like this:

```
    public abstract class EcalcModule
    {
        /// <summary>
        /// Module name
        /// </summary>
        public abstract string  ModuleName { get; }

        /// <summary>
        /// Module category
        /// </summary>
        public abstract string ModuleCategory { get; }

        /// <summary>
        /// Get the Control of the module
        /// </summary>
        /// <returns>The module UserControl</returns>
        public abstract UserControl GetControl();

        /// <summary>
        /// Control tile color. Can be chosen from the TileColor enum, or from a user value
        /// </summary>
        public virtual int Color
        {
            get { return (int)TileColor.Default;  }
        }

        /// <summary>
        /// Control tile icon will be used later.
        /// </summary>
        public virtual ImageSource Icon
        {
            get { return null; }
        }
    }
```

A plugin is loadable, when it's placed into an assembly, which targets the same framework version as the program, which is currently 4.5.2, and it has a strong name. Also filling out the version information details is highly recomended.

The loading mechanism only loads assemblies with a filename ending in .module.dll, to reduce unnecessary assembly loading & reflection.
 
##Implementation details

```
string  ModuleName { get; }
```

The name of the module. This name will appear in the title bar and it will appear on the module launcher tile.

```
string ModuleCategory { get; }
```

The category of the module. Can be unique. If your module fits into the existing categories, then please don't reinvent the wheel, use the existing category. The built-in modules are divided into the following categories:

* IT Tools
* Analog Electronics
* Digital Electronics
* Mechanics

```
UserControl GetControl();
```

This function is the main entry point for your module. It must return a System.Windows.Controls.UserControl derived control. In the control don't specify the width and height explicitly, because calculator layout may change in the future. For design purposes set the design width and height to 960x540.

```
int Color { get; }
```

This property can be overridden, but it's not necessary. It specifies the color of the tile, which will be generated for the module. The default tile color is: #CCCCCC (Gray). The TileColor enumeration defines some colors that can be used. The following list contains the names of the colors and their hexadecimal values:

* Default = 0xCCCCCC,
* W8LightGreen = 0x99b433,
* W8Lime = 0xA4C400,
* W8Green = 0x60A917,
* W8DarkGreen = 0x1e7145,
* W8DarkOrange = 0xda532c,
* W8Emerald = 0x008A00,
* W8Teal = 0x00ABA9,
* W8Cyan = 0x1BA1E2,
* W8Cobalt = 0x0050EF,
* W8Indigo = 0x6A00FF,
* W8Violet = 0xAA00FF,
* W8Pink = 0xF472D0,
* W8Magenta = 0xD80073,
* W8Crimson = 0xA20025,
* W8Red = 0xE51400,
* W8Orange = 0xFA6800,
* W8GAmber = 0xF0A30A,
* W8Yellow = 0xE3C800,
* W8Brown = 0x825A2C,
* W8Olive = 0x6D8764,
* W8Steel = 0x647687,
* W8Mauve = 0x76608A,
* FlatTurquoise = 0x1abc9c,
* FlatEmerald = 0x2ecc71,
* FlatPeterRiver = 0x3498db,
* FlatAmethyst = 0x9b59b6,
* FlatWetAsphalt = 0x34495e,
* FlatGeenSea = 0x16a085,
* FlatNephritis = 0x27ae60,
* FlatBelizeHole = 0x2980b9,
* FlatWisteria = 0x8e44ad,
* FlatMidnightBlue = 0x2c3e50,
* FlatSunFlower = 0xf1c40f,
* FlatCarrot = 0xe67e22,
* FlatAlizarin = 0xe74c3c,
* FlatClouds = 0xecf0f1,
* FlatConcrete = 0x95a5a6,
* FlatOrange = 0xf39c12,
* FlatPumpkin = 0xd35400,
* FlatPomegrande = 0xc0392b,
* FlatSilver = 0xbdc3c7,
* FlatAsbestos = 0x7f8c8d

```
ImageSource Icon
```

Also can be overridden, but it's not necessary. It specifies the icon on the tile for the module. This should return a 100x100 pixel PNG image with transparent background and white foreground. The program uses icons from icons8. When there is an appropriate icon in the icons8 library, then it should be used. Icons8: https://icons8.com/ 