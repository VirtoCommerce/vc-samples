### Sample UI module which allow you edit dynamic address properties using Google Map API
This is the example how to build custom extension to edit fields.
Editor for address fields was taken as an example. To do not make example more difficult we do not use fields of Product(Item).
This will require to create a backend part with extension of exist Entity - a lot to do.
Instead we use dynamic fields.

Editor allow to fill all address fields using two possibilities
1. By enter address in arbitrary way in common address field
2. By setting marker on the map

![image](https://user-images.githubusercontent.com/2689494/55940404-b8d6db00-5c48-11e9-833f-fbbc2f29ff2a.png)


## Prerequisites
1. Install module
2. Set Google API key in General Settins (these API should be switched on: Geocoding API, Maps JavaScript API, Places API)
3. Add Product Properties (for items which would not have all these properties the feature would not work)
* City - short text
* Country - short text
* Position - geo-point
* State - short text
* StreetAddress - long text
* Zip - short text
## Work with
To edit/view module content just open folder in Visual Studio

As it unmanaged module to create package just zip the folder https://virtocommerce.com/docs/vc2devguide/development-scenarios/creating-module-packages

