resource "azurerm_mssql_server" "app" {
  name                          = "sql-cirrus-dev-patty01"
  resource_group_name           = azurerm_resource_group.app.name
  location                      = azurerm_resource_group.app.location
  version                       = "12.0"
  minimum_tls_version           = "1.2"
  public_network_access_enabled = false

  azuread_administrator {
    login_username              = var.sql_admin_name
    object_id                   = var.sql_admin_object_id
    tenant_id                   = var.tenant_id
    azuread_authentication_only = true
  }

  tags = azurerm_resource_group.app.tags
}

resource "azurerm_mssql_database" "app" {
  name           = "sqldb-cirrus-dev"
  server_id      = azurerm_mssql_server.app.id
  sku_name       = "Basic"
  max_size_gb    = 2
  zone_redundant = false

  tags = azurerm_resource_group.app.tags
}
