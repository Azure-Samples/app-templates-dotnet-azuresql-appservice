targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the the environment which is used to generate a short unique hash used in all resources.')
param name string

@minLength(1)
@description('Primary location for all resources')
param location string

@secure()
@description('SQL Server administrator username')
param sqlServerUsername string

@secure()
@description('SQL Server administrator password')
param sqlServerDatabasePassword string


var resourceToken = toLower(uniqueString(subscription().id, name, location))
var tags = { 'azd-env-name': name }

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: '${name}-rg'
  location: location
  tags: tags
}

module resources 'resources.bicep' = {
  name: 'resources'
  scope: resourceGroup
  params: {
    tags: tags
    location: location
    resourceToken: resourceToken
    sqlAdministratorLogin: sqlServerUsername
    sqlAdministratorLoginPassword: sqlServerDatabasePassword
  }
}

output AZURE_LOCATION string = location

