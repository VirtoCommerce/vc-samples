//Call this to register our module to main application
var moduleName = "virtoCommerce.orderModule2";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(
  ['virtoCommerce.orderModule.knownOperations', '$q', '$http', '$compile',
	function (knownOperations, $q, $http, $compile) {

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
	        description: 'Sample Invoice document',
	        treeTemplateUrl: 'invoiceOperation.tpl.html',
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
	        newInstanceFactoryMethod: function (blade) {
	            return $q(function (resolve) {
	                resolve({
	                    createdDate: new Date(),
	                    currency: "EUR",
	                    isApproved: true,
	                    number: "Inv60826-00000",
	                    operationType: "Invoice"
	                });
	            });
	        }
	    };
	    knownOperations.registerOperation(invoiceOperation);

	    $http.get('Modules/$(virtoCommerce.orderModule2)/Scripts/tree-template.html').then(function (response) {
	        // compile the response, which will put stuff into the cache
	        $compile(response.data);
	    });
	}]);