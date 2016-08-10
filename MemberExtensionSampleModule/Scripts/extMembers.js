//Call this to register our module to main application
var moduleName = "contactExtMemberModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(
  ['$rootScope', 'virtoCommerce.customerModule.memberTypesResolverService', function ($rootScope, memberTypesResolverService) {
    
      // register member types
  	memberTypesResolverService.registerType({ memberType: 'Supplier', fullTypeName: 'MemberExtensionSampleModule.Web.Model.Supplier', descriptionAdd: 'Supplier description', titleAdd: 'Supplier', icon: 'fa fa-truck', template: 'Modules/$(VirtoCommerce.MemberExtensionSampleModule)/Scripts/blades/supplier-detail.tpl.html' });
    
  }]);