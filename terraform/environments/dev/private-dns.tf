# Creates the private DNS zone used to look up Azure SQL private endpoints. #

resource "azurerm_private_dns_zone" "sql" {

  # Azure's designated private DNS zone name for Azure SQL.
  name = "privatelink.database.windows.net"

  # Places the DNS zone in the Dev application resource group.
  resource_group_name = azurerm_resource_group.app.name

  # Uses the same tags as the resource group designated in main.tf
  tags = azurerm_resource_group.app.tags
}

# Links the private DNS zone to the Dev virtual network.
resource "azurerm_private_dns_zone_virtual_network_link" "sql" {

  # Name of the link in Azure.
  name = "link-cirrus-sql-dev"

  # Places the link in the DNS zone's resource group.
  resource_group_name = azurerm_resource_group.app.name

  # Identifies the DNS zone to link.
  private_dns_zone_name = azurerm_private_dns_zone.sql.name

  # Identifies the VNet that can use this DNS zone.
  virtual_network_id = module.networking.vnet_id

  # Uses the same tags as the resource group.
  tags = azurerm_resource_group.app.tags
}