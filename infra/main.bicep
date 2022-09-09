targetScope = 'subscription'
// teste https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.web/web-app-loganalytics/main.bicep

param skuName string = 'S1'
param skuCapacity int = 1
param servicePlan string ='contosoasp'

param appName string ='contosoapp'
param apiName string ='contosoapi'
param insightName string ='contosoai'
param analyticsName string ='contosola'
param location string = 'eastus' //resourceGroup().location

param serverName string ='contosodbserver'
// param sqlAdministratorLogin string
// @secure()
// param sqlAdministratorLoginPassword string
param databaseName string ='contosodb'

var appServicePlanName = toLower(servicePlan)
var webSiteName = toLower(appName)
var webApiName = toLower(apiName)
var resourceGroupName ='rgContosoDemo'
var appInsightName = toLower(insightName)
var logAnalyticsName = toLower(analyticsName)
var sqlserverName = toLower(serverName)

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name:resourceGroupName
  location: location
}

module resources 'resources.bicep' = {
  name:'resources'
  scope: resourceGroup
  params: {
    location: location
    appServicePlanName:appServicePlanName
    skuCapacity:skuCapacity
    skuName:skuName
    databaseName:databaseName
    webSiteName:webSiteName
    webApiName:webApiName
    serverName:sqlserverName
    appinsightsname:appInsightName
    loganalyticsname:logAnalyticsName
  }
}

output AZURE_SQL_CONNECTION_STRING string = resources.outputs.sqlconnectionstring
output APPLICATIONINSIGHTS_CONNECTION_STRING string = resources.outputs.APPLICATIONINSIGHTS_CONNECTION_STRING
output WEB_BASE_URL string = resources.outputs.WEB_URI
output API_BASE_URL string = resources.outputs.API_URI
output AZURE_LOCATION string = location
