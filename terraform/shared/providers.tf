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
  features {}

  storage_use_azuread = true

  subscription_id = var.subscription_id
  tenant_id       = var.tenant_id
}