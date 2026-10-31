resource "azurerm_resource_group" "monitoring" {
  name     = "rg-cirrus-monitoring"
  location = "uksouth"

  tags = {
    project = "cirrus-landing-zone"
    purpose = "shared-monitoring"
  }
}

resource "azurerm_log_analytics_workspace" "shared" {
  name                = "law-cirrus-shared"
  resource_group_name = azurerm_resource_group.monitoring.name
  location            = azurerm_resource_group.monitoring.location
  sku                 = "PerGB2018"
  retention_in_days   = 30

  tags = azurerm_resource_group.monitoring.tags
}