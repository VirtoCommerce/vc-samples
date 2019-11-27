//Call this to register our module to main application
var moduleName = "virtoCommerce.dummy";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    ;
