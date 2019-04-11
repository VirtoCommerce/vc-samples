//Call this to register our module to main application
var moduleTemplateName = "enrichmentFormSample";

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, []).run(['platformWebApp.widgetService',
    function (widgetService) {
        var enrichmentFormWidget = {
            controller: 'enrichmentFormSample.widgetController',
            template: 'Modules/$(EnrichmentFormSample)/Scripts/widgets/enrichmentFormWidget.tpl.html'
        };
        widgetService.registerWidget(enrichmentFormWidget, 'itemDetail');
    }]);
