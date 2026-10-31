resource "azurerm_private_endpoint" "sql" {
  name                = "pe-cirrus-sql-dev"
  location            = azurerm_resource_group.app.location
  resource_group_name = azurerm_resource_group.app.name
  subnet_id           = module.networking.private_endpoint_subnet_id

  private_service_connection {
    name                           = "psc-cirrus-sql-dev"
    private_connection_resource_id = azurerm_mssql_server.app.id
    subresource_names              = ["sqlServer"]
    is_manual_connection           = false
  }

  # We add this block to link the private endpoint to the private DNS zone we created in private-dns.tf. This allows the private endpoint to be resolved using the private DNS zone.
  private_dns_zone_group {
    name                 = "default"
    private_dns_zone_ids = [azurerm_private_dns_zone.sql.id]
  }

  tags = azurerm_resource_group.app.tags
}



