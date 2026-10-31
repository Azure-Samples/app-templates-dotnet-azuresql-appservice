resource "azurerm_resource_group" "tfstate" {
  name     = "rg-cirrus-tfstate"
  location = "uksouth"

  tags = {
    project = "cirrus-landing-zone"
    purpose = "terraform-state"
  }
}

resource "azurerm_storage_account" "tfstate" {
  name                     = var.storage_account_name
  resource_group_name      = azurerm_resource_group.tfstate.name
  location                 = azurerm_resource_group.tfstate.location
  account_kind             = "StorageV2"
  account_tier             = "Standard"
  account_replication_type = "LRS"

  https_traffic_only_enabled      = true
  min_tls_version                 = "TLS1_2"
  allow_nested_items_to_be_public = false
  shared_access_key_enabled       = false
  default_to_oauth_authentication = true
  public_network_access_enabled   = true

  network_rules {
    default_action = "Deny"
    bypass         = ["None"]
    ip_rules       = [var.allowed_public_ip]
  }

  blob_properties {
    versioning_enabled = true

    delete_retention_policy {
      days = 30
    }

    container_delete_retention_policy {
      days = 30
    }
  }

  tags = azurerm_resource_group.tfstate.tags

  lifecycle {
    prevent_destroy = true
  }
}

resource "azurerm_storage_container" "state" {
  for_each = toset(["bootstrap", "shared", "dev", "prod"])
  # the above creates four containers in the storage account, one for each environment #

  name                  = each.value
  storage_account_id    = azurerm_storage_account.tfstate.id
  container_access_type = "private"
}