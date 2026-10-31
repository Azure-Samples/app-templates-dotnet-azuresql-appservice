data "azurerm_log_analytics_workspace" "shared" {
  provider = azurerm.shared

  name                = "law-cirrus-shared"
  resource_group_name = "rg-cirrus-monitoring"
}

resource "azurerm_application_insights" "dev" {
  name                = "appi-cirrus-dev"
  location            = azurerm_resource_group.app.location
  resource_group_name = azurerm_resource_group.app.name
  application_type    = "web"
  workspace_id        = data.azurerm_log_analytics_workspace.shared.id

  tags = azurerm_resource_group.app.tags
}