//Call this to register our module to main application
var moduleName = "virtoCommerce.orderModule2";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
.run(
  ['$rootScope', 
	function () {

	    OrderModule_orderDetailBlade.prototype.metaFields.push(
            {
                name: 'newField',
                title: "New field",
                valueType: "ShortText"                
            });

	    //OrderModule_orderDetailBlade.prototype.availableChildrenTypes.push['Invoice'];	 

	    var invoiceOperation = {
	        type: 'Invoice',
	        getDetailBlade: function (operation, blade) {

	            new OrderModule_invoiceDetailBlade(operation);
	        }
	    }
	    OrderModule_knownOperations.push(invoiceOperation);
	}]);

// Invoice detail blade
function OrderModule_invoiceDetailBlade(operation, blade) {
    this.currentEntity = operation;
    this.customerOrder = blade.customerOrder;
    this.title = 'Invoice';
    this.titleValues = { number: operation.number };
    this.subtitle = 'Invoice subtitle';
    this.template = 'Modules/$(VirtoCommerce.Orders)/Scripts/blades/payment-detail.tpl.html';
}

// inherit form base operation
OrderModule_invoiceDetailBlade.prototype = new OrderModule_operationDetailBlade();

OrderModule_invoiceDetailBlade.prototype.metaFields = [
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
    }
];

