# The Dev VPN is an optional administrative access path, so its subnet
# is defined here rather than in the reusable networking module.

resource "azurerm_subnet" "gateway" {
  name                 = "GatewaySubnet"
  resource_group_name  = azurerm_resource_group.app.name
  virtual_network_name = "vnet-cirrus-dev"
  address_prefixes     = ["10.20.254.0/27"]
}

# Public address that VPN clients use to reach the gateway.
/*
resource "azurerm_public_ip" "vpn" {
  name                = "pip-cirrus-vpn-dev"
  resource_group_name = azurerm_resource_group.app.name
  location            = azurerm_resource_group.app.location
  allocation_method   = "Static"
  sku                 = "Standard"

  tags = azurerm_resource_group.app.tags
}
*/

# The virtual network gateway is the VPN endpoint for clients to connect to the VNet.
# Creates a temporary point-to-site VPN gateway so an administrator can
# connect to the Dev VNet and access private resources such as Azure SQL.
# VPN clients authenticate using a certificate and receive an IP address
# from the 172.20.50.0/24 address pool.

/*
resource "azurerm_virtual_network_gateway" "dev" {
  name                = "vng-cirrus-dev"
  location            = azurerm_resource_group.app.location
  resource_group_name = azurerm_resource_group.app.name

  type          = "Vpn"
  vpn_type      = "RouteBased"
  sku           = "VpnGw1AZ"
  generation    = "Generation1"
  active_active = false

  ip_configuration {
    name                          = "gateway-ip-config"
    public_ip_address_id          = azurerm_public_ip.vpn.id
    private_ip_address_allocation = "Dynamic"
    subnet_id                     = azurerm_subnet.gateway.id
  }

  vpn_client_configuration {
    address_space        = ["172.20.50.0/24"]
    vpn_client_protocols = ["IkeV2"]
    vpn_auth_types       = ["Certificate"]

    root_certificate {
      name             = "CirrusDevVpnRoot"
      public_cert_data = filebase64("${path.module}/CirrusDevVpnRoot.cer")
    }
  }

  tags = azurerm_resource_group.app.tags
}

*/