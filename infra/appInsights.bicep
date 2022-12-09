param insightName string
param analyticsName string
param location string

var appInsightName = toLower(insightName)
var logAnalyticsName = toLower(analyticsName)

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: logAnalyticsName
  location: location
  tags: {
    displayName: 'Log Analytics'
    ProjectName: 'ContosoUniversity'
    appTemplateName: 'dotnetweb'
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
    appTemplateName: 'dotnetweb'
  }
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

output APPLICATIONINSIGHTS_CONNECTION_STRING string = appInsights.properties.ConnectionString
output APPLICATIONINSIGHTS_INSTRUMENTATIONKEY string = appInsights.properties.InstrumentationKey
