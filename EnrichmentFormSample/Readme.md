### Sample UI module enabling to manage address dynamically, using Google Maps JavaScript API
This sample is about building custom UI (module) for data editing.
Editor for address fields was taken as an example. For simplicity's sake, we do not use direct fields of Catalog Product(Item).
That would require creating the backend libraries for just extending existing Entity - a lot to do, out of sample's scope.
Catalog Properties were used instead.

Editor enabling to fill address in two ways:
1. By entering address in arbitrary way in common address input field
2. By setting a location marker on a map directly

![image](https://user-images.githubusercontent.com/2689494/55940404-b8d6db00-5c48-11e9-833f-fbbc2f29ff2a.png)


## Prerequisites
1. Install module (by adding a symbolic link to source code folder)
2. Set Google API key in General Settings (these APIs should be enabled: Geocoding API, Maps JavaScript API, Places API)
3. Add Product Properties (the feature would not work for items missing any of these properties):
* City - short text
* Country - short text
* Position - geo-point
* State - short text
* StreetAddress - long text
* Zip - short text

## Working with source code
Manage the source code by opening this folder in any IDE, e.g. Visual Studio.

## Packaging and releasing
Create a module package by zipping the folder contents (as this is un unmanaged module). Check Virto docs for details https://virtocommerce.com/docs/vc2devguide/development-scenarios/creating-module-packages
