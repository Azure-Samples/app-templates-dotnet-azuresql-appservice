resource "azurerm_role_assignment" "patty_dev_state" {
  scope                = azurerm_storage_container.state["dev"].id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = var.state_admin_object_id
  principal_type       = "User"
}

resource "azurerm_role_assignment" "patty_shared_state" {
  scope                = azurerm_storage_container.state["shared"].id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = var.state_admin_object_id
  principal_type       = "User"
}