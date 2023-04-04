// teste https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.web/web-app-loganalytics/main.bicep
targetScope = 'subscription'

param environmentName string
param resourceGroupName string
param location string 

@secure()
param sqlAdministratorLoginPassword string

var tags = { 'azd-env-name': environmentName}

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: empty(resourceGroupName) ? 'rg-${environmentName}' : resourceGroupName
  location: location
  tags: union(tags, { apptemplate: 'IntegrationSample' })
}

module resources 'resources.bicep' = {
  scope: rg
  name: 'resources'
  params: {
    environmentName: environmentName
    location: location
    sqlAdministratorLoginPassword: sqlAdministratorLoginPassword
  }
}

output AZURE_RESOURCE_GROUP string = rg.name
output AZURE_CONTOSO_CONN_STRING string = resources.outputs.connectionStringKey
output databaseName string = resources.outputs.databaseName
output AZURE_CONTOSO_APP_INSIGHTS string = resources.outputs.appInsightConnectionString
output AZ_API_NAME string = resources.outputs.apiName
output AZURE_API_URI string = resources.outputs.apiUri
