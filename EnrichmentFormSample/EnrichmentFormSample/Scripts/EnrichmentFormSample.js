//Call this to register our module to main application
var moduleTemplateName = "EnrichmentFormSample";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleTemplateName);
}

angular.module(moduleTemplateName, []).run(['platformWebApp.widgetService',
    function (widgetService) {
        var enrichmentFormWidget = {
            controller: 'EnrichmentFormSample.WidgetController',
            template: 'Modules/$(EnrichmentFormSample)/Scripts/widgets/enrichmentFormWidget.tpl.html'
        };
        widgetService.registerWidget(enrichmentFormWidget, 'itemDetail');
    }]);
