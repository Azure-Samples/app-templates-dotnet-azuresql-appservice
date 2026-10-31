resource "azurerm_resource_group" "app" {
  name     = "rg-cirrus-app-dev"
  location = "uksouth"

  tags = {
    project     = "cirrus-landing-zone"
    environment = "dev"
    purpose     = "application"
  }
}

module "networking" {
  source = "../../modules/networking"

  resource_group_name = azurerm_resource_group.app.name
  location            = azurerm_resource_group.app.location
  vnet_name           = "vnet-cirrus-dev"

  address_space                    = ["10.20.0.0/16"]
  integration_subnet_prefixes      = ["10.20.1.0/24"]
  private_endpoint_subnet_prefixes = ["10.20.2.0/24"]
}