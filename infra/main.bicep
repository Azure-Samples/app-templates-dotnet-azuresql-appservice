// teste https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.web/web-app-loganalytics/main.bicep

param skuName string = 'S1'
param skuCapacity int = 1
param servicePlan string

param appName string
param apiName string
param insightName string
param analyticsName string
param location string = resourceGroup().location

param serverName string
//Used so we can setup an AAD login to the database using the Managed Identity of the App Service
param sqlAdministratorLogin string
@secure()
param sqlAdministratorLoginPassword string
param databaseName string
@description('Name of the Service Principal that runs deployments')
param sqlAdministratorAadName string
@description('ClientId of the Service Principal that runs deployments')
param sqlAdministratorAadClientId string

var appServicePlanName = toLower(servicePlan)
var webSiteName = toLower(appName)
var webApiName = toLower(apiName)
var appInsightName = toLower(insightName)
var logAnalyticsName = toLower(analyticsName)
var sqlserverName = toLower(serverName)

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
  }
}

resource appService 'Microsoft.Web/sites@2022-03-01' = {
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
      netFrameworkVersion: 'v6.0'
      appSettings: [
        {
          name: 'Api:Address'
          value: 'https://${apiService.properties.hostNames[0]}/'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~2'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'recommended'
        }
        {
          name: 'InstrumentationEngine_EXTENSION_VERSION'
          value: '~1'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_BaseExtensions'
          value: '~1'
        }      
      ]
    }
  }
}

resource apiService 'Microsoft.Web/sites@2022-03-01' = {
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
      netFrameworkVersion: 'v6.0'
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~2'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'recommended'
        }
        {
          name: 'InstrumentationEngine_EXTENSION_VERSION'
          value: '~1'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_BaseExtensions'
          value: '~1'
        }
      ]
      connectionStrings: [
        {
          name: 'ContosoUniversityAPIContext'
          connectionString: 'Server=tcp:${sqlServer.name}${environment().suffixes.sqlServerHostname},1433;Authentication=Active Directory Managed Identity;Database=${sqlDatabase.name}'
          type: 'SQLAzure'
        }
      ]
    }
  }
}

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

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: logAnalyticsName
  location: location
  tags: {
    displayName: 'Log Analytics'
    ProjectName: 'ContosoUniversity'
  }
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 120
  }
}

resource appInsights 'microsoft.insights/components@2020-02-02-preview' = {
  name: appInsightName
  location: location
  kind: 'string'
  tags: {
    displayName: 'AppInsight'
    ProjectName: 'ContosoUniversity'
  }
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: sqlserverName
  location: location
  tags: {
    displayName: 'SQL Server'
    ProjectName: 'ContosoUniversity'
  }
  properties: {
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorLoginPassword    
    version: '12.0'
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: false
      principalType: 'Application'
      tenantId: subscription().tenantId
      sid: sqlAdministratorAadClientId
      login: sqlAdministratorAadName
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

output apiIdentityPrincipalId string = apiService.identity.principalId
