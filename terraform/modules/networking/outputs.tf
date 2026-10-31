output "vnet_id" {
  description = "ID of the virtual network."
  value       = azurerm_virtual_network.vnet.id
}

output "integration_subnet_id" {
  description = "ID of the App Service integration subnet."
  value       = azurerm_subnet.integration.id
}

output "private_endpoint_subnet_id" {
  description = "ID of the private endpoint subnet."
  value       = azurerm_subnet.private_endpoints.id
}