param location string
param appServicePlanName string
param skuName string
param skuCapacity int

param webSiteName string
param webApiName string

param serverName string
// param sqlAdministratorLogin string
// @secure()
// param sqlAdministratorLoginPassword string
param databaseName string
param appinsightsname string
param loganalyticsname string

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: skuName
    capacity: skuCapacity
  }
  tags: {
    displayName: 'HostingPlan'
    ProjectName: 'ContosoUniversity'
    appTemplateName: 'dotnetweb'
  }
}


resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: webSiteName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  tags: {
    displayName: 'Website'
    ProjectName: 'ContosoUniversity'
  }
  properties: {
    serverFarmId: appServicePlan.id 
    httpsOnly: true
    siteConfig: {
      minTlsVersion: '1.2'
    }
  }
}

// logging resource
resource appServiceLogging 'Microsoft.Web/sites/config@2020-06-01' = {
  name: '${appService.name}/logs'
  properties: {
    applicationLogs: {
      fileSystem: {
        level: 'Warning'
      }
    }
    httpLogs: {
      fileSystem: {
        retentionInMb: 40
        enabled: true
      }
    }
    failedRequestsTracing: {
      enabled: true
    }
    detailedErrorMessages: {
      enabled: true
    }
  }
}
resource appServiceAppSettings 'Microsoft.Web/sites/config@2021-03-01' = {
  name: 'appsettings'
  parent: appService
  properties: {
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.outputs.APPLICATIONINSIGHTS_INSTRUMENTATIONKEY
    APPLICATIONINSIGHTS_CONNECTION_STRING: appInsights.outputs.APPLICATIONINSIGHTS_CONNECTION_STRING
  }
}

resource apiService 'Microsoft.Web/sites@2020-06-01' = {
  name: webApiName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  tags: {
    displayName: 'Website'
    ProjectName: 'ContosoUniversity'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      minTlsVersion: '1.2'
    }
  }
}
resource apiServiceLogging 'Microsoft.Web/sites/config@2020-06-01' = {
    name: '${apiService.name}/logs'
    properties: {
      applicationLogs: {
        fileSystem: {
          level: 'Warning'
        }
      }
      httpLogs: {
        fileSystem: {
          retentionInMb: 40
          enabled: true
        }
      }
      failedRequestsTracing: {
        enabled: true
      }
      detailedErrorMessages: {
        enabled: true
      }
    }
  }
    
  resource apiServiceAppSettings 'Microsoft.Web/sites/config@2021-03-01' = {
    name: 'appsettings'
    parent: apiService
    properties: {
      AZURE_SQL_CONNECTION_STRING: AZURE_SQL_CONNECTION_STRING
      APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.outputs.APPLICATIONINSIGHTS_INSTRUMENTATIONKEY
      APPLICATIONINSIGHTS_CONNECTION_STRING: appInsights.outputs.APPLICATIONINSIGHTS_CONNECTION_STRING
    }
  }

  
module appInsights 'appInsights.bicep' = {
  name: 'applicationinsights-resources'
  params: {
    location: location
    analyticsName:loganalyticsname
    insightName:appinsightsname
  }
}

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: serverName
  location: location
  tags: {
    displayName: 'SQL Server'
    ProjectName: 'ContosoUniversity'
  }
  properties: {
    //administratorLogin: sqlAdministratorLogin
    //administratorLoginPassword: sqlAdministratorLoginPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'User'
      sid: apiService.identity.principalId
      login: 'activedirectoryadmin'
      tenantId: apiService.identity.tenantId
      azureADOnlyAuthentication: true
    }
}
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2021-02-01-preview' = {
  parent: sqlServer
  name: databaseName
  location: location
  tags: {
    displayName: 'Database'
    ProjectName: 'ContosoUniversity'
  }
  sku: {
    name: 'Basic'
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 1073741824
  }
}

// To allow applications hosted inside Azure to connect to your SQL server, Azure connections must be enabled. 
// To enable Azure connections, there must be a firewall rule with starting and ending IP addresses set to 0.0.0.0. 
// This recommended rule is only applicable to Azure SQL Database.
// Ref: https://docs.microsoft.com/en-us/azure/azure-sql/database/firewall-configure?view=azuresql#connections-from-inside-azure
resource symbolicname 'Microsoft.Sql/servers/firewallRules@2021-11-01-preview' = {
  name: 'AllowAllWindowsAzureIps'
  parent: sqlServer
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}
// Defined as a var here because it is used above

var AZURE_SQL_CONNECTION_STRING = 'Server=${sqlServer.properties.fullyQualifiedDomainName}; Authentication=Active Directory Default; Database=${databaseName};'

output APPLICATIONINSIGHTS_CONNECTION_STRING string = appInsights.outputs.APPLICATIONINSIGHTS_CONNECTION_STRING
output sqlconnectionstring string = AZURE_SQL_CONNECTION_STRING
output appServicePlanId string = appServicePlan.id
output WEB_URI string = 'https://${appService.properties.defaultHostName}'
output API_URI string = 'https://${apiService.properties.defaultHostName}'
