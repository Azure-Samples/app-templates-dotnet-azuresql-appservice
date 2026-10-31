terraform {
  required_version = "= 1.12.2"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "= 4.81.0"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  features {
    key_vault {
      purge_soft_delete_on_destroy = false
    }
  }

  subscription_id = var.subscription_id
  tenant_id       = var.tenant_id

  storage_use_azuread = true
}

provider "azurerm" {
  alias = "shared"

  features {}

  subscription_id = var.shared_subscription_id
  tenant_id       = var.tenant_id
}