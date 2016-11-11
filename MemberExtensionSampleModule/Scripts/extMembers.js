//Call this to register our module to main application
var moduleName = "contactExtMemberModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(
  ['$rootScope', 'virtoCommerce.customerModule.memberTypesResolverService', function ($rootScope, memberTypesResolverService) {

      // add JobTitle field to Contact detail blade
      var contactInfo = memberTypesResolverService.resolve("Contact");
      contactInfo.detailBlade.metaFields.unshift({
          name: 'jobTitle',
          title: "JobTitle",
          valueType: "ShortText"
      });

      // register new Supplier member type
      memberTypesResolverService.registerType({
          memberType: 'Supplier',
          description: 'Supplier description',
          fullTypeName: 'MemberExtensionSampleModule.Web.Model.Supplier',
          icon: 'fa fa-truck',
          detailBlade: {
              template: 'Modules/$(VirtoCommerce.MemberExtensionSampleModule)/Scripts/blades/supplier-detail.tpl.html',
              metaFields: [{
                  name: 'contractNumber',
                  title: "Contract Number",
                  valueType: "ShortText"
              }]
          }
      });

      // registering Supplier as possible child for Organization
      var organizationMetadata = memberTypesResolverService.resolve('Organization');
      organizationMetadata.knownChildrenTypes.push('Supplier');
  }]);