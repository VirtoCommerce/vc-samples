//Call this to register our module to main application
var moduleName = "virtoCommerce.orderModule2";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(
  ['virtoCommerce.orderModule.knownOperations',
	function (knownOperations) {

	    var foundTemplate = knownOperations.getOperation('CustomerOrder');
	    if (foundTemplate) {
	        foundTemplate.detailBlade.metaFields.push(
                {
                    name: 'newField',
                    title: "New field",
                    valueType: "ShortText"
                });

	        foundTemplate.detailBlade.knownChildrenOperations.push('Invoice');
	    }

	    var invoiceOperation = {
	        type: 'Invoice',
	        // treeTemplateUrl: 'orderOperationDefault.tpl.html',
	        detailBlade: {
	            template: 'Modules/$(virtoCommerce.orderModule2)/Scripts/blades/invoice-detail.tpl.html',
	            metaFields: [
                    {
                        name: 'customerId',
                        isRequired: true,
                        title: "CustomerId",
                        valueType: "ShortText"
                    },
                    {
                        name: 'customerName',
                        isReadonly: true,
                        title: "Customer name",
                        valueType: "ShortText"
                    },
                    {
                        name: 'number',
                        isRequired: true,
                        title: "Invoice number",
                        valueType: "ShortText"
                    },
                    {
                        name: 'createdDate',
                        isReadonly: true,
                        title: "created",
                        valueType: "DateTime"
                    }
	            ]
	        },
	        newInstanceMetadata: {
	            name: 'Invoice',
	            descr: 'Sample Invoice document',
	            action: function (blade) {
	                bladeNavigationService.closeBlade(blade);

	                // blade.customerOrder.id
	                var result = {
	                    createdDate: new Date(),
	                    currency: "EUR",
	                    customerId: "0cda0396-43fe-4034-a20e-d0bab4c88c93",
	                    id: "c50acc49-a0ed-4c93-aad2-1428899ce40b",
	                    isApproved: true,
	                    number: "Inv60826-00000",
	                    operationType: "Invoice"
	                }

	                var foundTemplate = knownOperations.getOperation(invoiceOperation.type);
	                var newBlade = angular.copy(foundTemplate.detailBlade);
	                newBlade.currentEntity = result;
	                newBlade.customerOrder = blade.customerOrder;
	                newBlade.isNew = true;
	                var foundField = _.findWhere(newBlade.metaFields, { name: 'createdDate' });
	                if (foundField) {
	                    foundField.isReadonly = false;
	                }

	                bladeNavigationService.showBlade(newBlade, blade.parentBlade);
	            }
	        }
	    };
	    knownOperations.registerOperation(invoiceOperation);

	}]);