param location string
param resourceToken string

param skuName string = 'S1'
param skuCapacity int = 1

param sqlAdministratorLogin string
@secure()
param sqlAdministratorLoginPassword string

param databaseName string = 'SchoolContext-API'


resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: 'appplan-${resourceToken}'
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

resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: 'app-${resourceToken}'
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

resource apiService 'Microsoft.Web/sites@2020-06-01' = {
  name: 'api-${resourceToken}'
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

resource appServiceLogging 'Microsoft.Web/sites/config@2020-06-01' = {
  name: 'applogs-${resourceToken}'
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
  name: 'apilogs-${resourceToken}'
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
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.properties.InstrumentationKey
  }
  dependsOn: [
    appServiceSiteExtension
  ]
}

resource apiServiceAppSettings 'Microsoft.Web/sites/config@2021-03-01' = {
  name: 'appsettings'
  parent: apiService
  properties: {
    APPINSIGHTS_INSTRUMENTATIONKEY: appInsights.properties.InstrumentationKey
  }
  dependsOn: [
    apiServiceSiteExtension
  ]
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: 'loganalytics-${resourceToken}'
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
  name: 'ai-${resourceToken}'
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

resource appServiceSiteExtension 'Microsoft.Web/sites/siteextensions@2021-03-01' = {
  name: 'Microsoft.ApplicationInsights.AzureWebSites'
  parent: appService
  dependsOn: [
    appInsights
  ]
}

resource apiServiceSiteExtension 'Microsoft.Web/sites/siteextensions@2021-03-01' = {
  name: 'Microsoft.ApplicationInsights.AzureWebSites'
  parent: apiService
  dependsOn: [
    appInsights
  ]
}

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: 'sql-${resourceToken}'
  location: location
  tags: {
    displayName: 'SQL Server'
    ProjectName: 'ContosoUniversity'
  }
  properties: {
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorLoginPassword
    version: '12.0'
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
//  Telemetry Deployment
@description('Enable usage and telemetry feedback to Microsoft.')
param enableTelemetry bool = true
var telemetryId = '69ef933a-eff0-450b-8a46-331cf62e160f-NETWEB-${location}'
resource telemetrydeployment 'Microsoft.Resources/deployments@2021-04-01' = if (enableTelemetry) {
  name: telemetryId
  properties: {
    mode: 'Incremental'
    template: {
      '$schema': 'https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#'
      contentVersion: '1.0.0.0'
      resources: {}
    }
  }
}
